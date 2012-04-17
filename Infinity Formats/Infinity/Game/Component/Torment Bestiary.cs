using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    public class TormentBestiary : IInfinityFormat
    {
        #region Constants
        /// <summary>Representst the size of this structure on disk</summary>
        public const Int32 StructureSize = 260;
        #endregion


        #region Fields
        /// <summary>Represents the collection of beasts</summary>
        public Byte[] Beasts { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize() { }
        #endregion


        #region Methods
        /// <summary>Gets a value indicating whether a creature with the given bestiary index has been encountered</summary>
        /// <param name="index">Index into the Bestiary array</param>
        /// <returns>Boolean indicating whether or not the creature has been encountered</returns>
        public Boolean BeastiaryIndexEncountered(Int32 index)
        {
            return Convert.ToBoolean(this.Beasts[index]);
        }

        /// <summary>Sets a value indicating whether a creature with the given bestiary index has been encountered</summary>
        /// <param name="index">Index into the Bestiary array</param>
        public void SetBeastiaryIndexHasBeenEncountered(Int32 index, Boolean encountered)
        {
            this.Beasts[index] = Convert.ToByte(encountered);
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();
            this.Beasts = ReusableIO.BinaryRead(input, TormentBestiary.StructureSize);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            output.Write(this.Beasts, 0, TormentBestiary.StructureSize);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (Int32 index = 0; index < TormentBestiary.StructureSize; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Encountered beast entry # ", index)));
                builder.Append(this.Beasts[index]);
            }

            return builder.ToString();
        }
        #endregion
    }
}