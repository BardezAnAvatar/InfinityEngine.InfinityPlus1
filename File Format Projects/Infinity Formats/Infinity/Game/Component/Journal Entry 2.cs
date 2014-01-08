using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents a journal entry for BG2</summary>
    public class JournalEntry_v2 : JournalEntryBase
    {
        #region Fields
        /// <summary>Represents the chapter number when this journal entry occurred.</summary>
        public Byte Chapter { get; set; }

        /// <summary>Which characters have not read this entry; bit flags?</summary>
        /// <value>I've seen FF, C0</value>
        public Byte ReadByCharacter { get; set; }

        /// <summary>Represents the type of journal entry</summary>
        public JournalEntry EntryType { get; set; }

        /// <summary>Represents where the data for the StrRef resides</summary>
        public JournalLocation EntryDataLocation { get; set; }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            base.ReadBody(input);
            Byte[] buffer = ReusableIO.BinaryRead(input, 4);
            this.Chapter = buffer[0];
            this.ReadByCharacter = buffer[1];
            this.EntryType = (JournalEntry)buffer[2];
            this.EntryDataLocation = (JournalLocation)buffer[3];
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);
            output.WriteByte(this.Chapter);
            output.WriteByte(this.ReadByCharacter);
            output.WriteByte((Byte)this.EntryType);
            output.WriteByte((Byte)this.EntryDataLocation);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndex">Known spells entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Journal entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Journal entry:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());
            builder.Append(StringFormat.ToStringAlignment("Chapter # entered"));
            builder.Append(this.Chapter);
            builder.Append(StringFormat.ToStringAlignment("Unknown"));
            builder.Append(this.ReadByCharacter);
            builder.Append(StringFormat.ToStringAlignment("Entry Type (value)"));
            builder.Append((Byte)this.EntryType);
            builder.Append(StringFormat.ToStringAlignment("Entry Type (description)"));
            builder.Append(this.EntryType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Entry Location (value)"));
            builder.Append((Byte)this.EntryDataLocation);
            builder.Append(StringFormat.ToStringAlignment("Entry Location (description)"));
            builder.Append(this.EntryDataLocation.GetDescription());

            return builder.ToString();
        }
        #endregion
    }
}