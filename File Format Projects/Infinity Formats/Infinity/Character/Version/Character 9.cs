using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature9;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Version
{
    /// <summary>Character 9.0, used in Icewind Dale</summary>
    public class Character9 : IInfinityFormat
    {
        #region Members
        /// <summary>Character file header</summary>
        protected Character1Header header;

        /// <summary>Main data of the character</summary>
        protected Creature9 creature;
        #endregion

        #region Properties
        /// <summary>Character file header</summary>
        public Character1Header Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        /// <summary>Main data of the character</summary>
        public Creature9 Creature
        {
            get { return this.creature; }
            set { this.creature = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Character9()
        {
            this.header = null;
            this.creature = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.header = new Character1Header();
            this.creature = new Creature9();
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.header = new Character1Header();
                this.header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            this.header.Read(input);

            //The creature files offsets are treated as its own substream, with the offsets being relative to its start point, not the character start point
            // (this is probably a good thing). So, read the header's offset and length into a substream for the creature's stream
            ReusableIO.SeekIfAble(input, this.header.OffsetCreature, SeekOrigin.Begin);
            Byte[] creatureBinaryArray = ReusableIO.BinaryRead(input, this.header.LengthCreature);
            using (MemoryStream subStream = new MemoryStream(creatureBinaryArray))
                this.creature.Read(subStream);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            //Moving data integrity to after the subarray is gathered. Gathering it first, since the header will need to be updated if it is wrong with ofsets and length.

            //The creature will maintain its own minimal data integrity; as such, if I pass in the output stream, it will not
            //  account for a start of file index. Simply givig it a substream, them merging them should work just fine.
            MemoryStream subStream = new MemoryStream();
            this.creature.Write(subStream);
            Byte[] buffer = subStream.ToArray();

            //Minimal data integrity
            if (buffer.Length != this.header.LengthCreature || this.header.OffsetCreature < this.header.StructSize)
            {
                this.header.OffsetCreature = Convert.ToUInt32(this.header.StructSize);
                this.header.LengthCreature = Convert.ToUInt32(buffer.Length);
            }

            this.header.Write(output);

            ReusableIO.SeekIfAble(output, this.header.OffsetCreature, SeekOrigin.Begin);
            output.Write(buffer, 0, buffer.Length);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Character version 9.0");
            builder.AppendLine(this.header.ToString());
            builder.AppendLine(this.creature.ToString());

            return builder.ToString();
        }
        #endregion
    }
}