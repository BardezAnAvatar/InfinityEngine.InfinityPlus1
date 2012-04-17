using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkOverride
{
    /// <summary>Represents an entry within the talk override table that </summary>
    public class TalkOverrideTableEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 0x20C;
        #endregion


        #region Fields
        /// <summary>Offset tothe next free region.</summary>
        /// <value>
        ///     GemRB does not provide muh detail as to what this is for, aside from interal memory management.
        ///     From what I have observed, this field is -1 in the first entry, otherwise 0.
        /// </value>
        public Int32 NextFreeRegion { get; set; }

        /// <summary>Offset to the previous block of text in this string. Usually -1, but in the case of longer strings, it is an offset to the previous string.</summary>
        /// <remarks>It is not an offset to the first, but the previous.</remarks>
        public Int32 PreviousOffset { get; set; }

        /// <summary>Block of 512 bytes of string data.</summary>
        public Byte[] Text { get; set; }

        /// <summary>Offset to next block, if the string overflows past this block</summary>
        public Int32 ContinuedOffset { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize() { }
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
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 8);
            this.NextFreeRegion = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.PreviousOffset = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.Text = ReusableIO.BinaryRead(input, 512);
            buffer = ReusableIO.BinaryRead(input, 4);
            this.ContinuedOffset = ReusableIO.ReadInt32FromArray(buffer, 0);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.NextFreeRegion, output);
            ReusableIO.WriteInt32ToStream(this.PreviousOffset, output);
            output.Write(this.Text, 0, 512);
            ReusableIO.WriteInt32ToStream(this.ContinuedOffset, output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Talk Override Table Entry:");
            builder.Append(StringFormat.ToStringAlignment("Next Free Region"));
            builder.Append(this.NextFreeRegion);
            builder.Append(StringFormat.ToStringAlignment("Offset to previous connected entry"));
            builder.Append(this.PreviousOffset);
            builder.Append(StringFormat.ToStringAlignment("Text"));
            builder.Append(ZString.GetZString(ReusableIO.ReadStringFromByteArray(this.Text, 0, CultureConstants.CultureCodeEnglish, 512)));
            builder.Append(StringFormat.ToStringAlignment("Offset to next entry, where text is continued"));
            builder.Append(this.ContinuedOffset);
            builder.AppendLine(String.Empty);

            return builder.ToString();
        }
        #endregion
    }
}