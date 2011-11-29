using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Dialog.Component
{
    /// <summary>Represents a scripting block of the dialog file, be it a set of actions, state triggers or transition triggers</summary>
    public class DialogScript : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 8;

        #region Fields
        /// <summary>Offset from start of file to the Scripting Trigger</summary>
        public UInt32 OffsetScript { get; set; }

        /// <summary>Length of the scripting Trigger</summary>
        public UInt32 LengthScript { get; set; }

        /// <summary>Actual string representing the scripting trigger</summary>
        /// <remarks>Not directly referenced as part of the struct; inferred member</remarks>
        public ZString Script { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public DialogScript() { }
 
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Script = new ZString();
        }
        #endregion

        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the output stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] triggerData = ReusableIO.BinaryRead(input, StructSize);

            this.OffsetScript = ReusableIO.ReadUInt32FromArray(triggerData, 0);
            this.LengthScript = ReusableIO.ReadUInt32FromArray(triggerData, 4);
        }

        /// <summary>This public method reads the underlying trigger string from the inout stream, after the offsets have been read</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadScript(Stream input)
        {
            this.Initialize();

            ReusableIO.SeekIfAble(input, this.OffsetScript, SeekOrigin.Begin);
            Byte[] triggerData = ReusableIO.BinaryRead(input, this.LengthScript);

            this.Script.Source = ReusableIO.ReadStringFromByteArray(triggerData, 0, Constants.CultureCodeEnglish, triggerData.Length);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.OffsetScript, output);
            ReusableIO.WriteUInt32ToStream(this.LengthScript, output);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void WriteScript(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.OffsetScript);
            ReusableIO.WriteStringToStream(this.Script.Source, output, Constants.CultureCodeEnglish, true);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = "Dialog trigger:";

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
            return StringFormat.ReturnAndIndent(String.Format("Dialog trigger # {0}", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Offset"));
            builder.Append(this.OffsetScript);
            builder.Append(StringFormat.ToStringAlignment("Length"));
            builder.Append(this.LengthScript);
            builder.AppendLine(StringFormat.ToStringAlignment("Trigger"));
            builder.Append(String.Format("'{0}'", this.Script.Value));

            return builder.ToString();
        }
        #endregion

        /// <summary>Maintains the data integrity of the instance</summary>
        public void MaintainMinimalDataIntegrity()
        {
            Byte[] data = ReusableIO.WriteStringToByteArray(this.Script.Source, Constants.CultureCodeEnglish);
            this.LengthScript = Convert.ToUInt32(data.Length);
        }
    }
}