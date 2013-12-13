using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Variables
{
    /// <summary>Represents a single variable entry</summary>
    public class VariableEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 44;
        #endregion


        #region Fields
        /// <summary>Scope of this variable</summary>
        public ZString Scope { get; set; }

        /// <summary>Name of the variable</summary>
        public ZString Name { get; set; }

        /// <summary>Value of the variable</summary>
        public Int32 Value { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Scope = new ZString();
            this.Name = new ZString();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, VariableEntry.StructSize);
            this.Scope.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 8, CultureConstants.CultureCodeEnglish, 32);
            this.Value = ReusableIO.ReadInt32FromArray(buffer, 40);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Scope.Source, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteInt32ToStream(this.Value, output);
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
            String header = "Variable:";

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
            return StringFormat.ReturnAndIndent(String.Format("Variable # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Scope"));
            builder.Append(String.Format("'{0}'", this.Scope.Value));
            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Value"));
            builder.Append(this.Value);
            builder.AppendLine(String.Empty);

            return builder.ToString();
        }
        #endregion
    }
}