using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents a journal entry for more Infinity Engine games</summary>
    public class JournalEntryBase : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 12;
        #endregion


        #region Fields
        /// <summary>Reference to the String index for the Journal entry text</summary>
        public StringReference JournalText { get; set; }

        /// <summary>Represents when the jounral entry was added</summary>
        /// <remarks>In seconds (game time?)</remarks>
        public UInt32 Time { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.JournalText = new StringReference();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, JournalEntryBase.StructSize - 4);

            this.JournalText.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.Time = ReusableIO.ReadUInt32FromArray(buffer, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.JournalText.StringReferenceIndex, output);
            ReusableIO.WriteUInt32ToStream(this.Time, output);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Journal Entry StrRef index"));
            builder.Append(this.JournalText.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Time entered (seconds)"));
            builder.Append(this.Time);

            return builder.ToString();
        }
        #endregion
    }
}