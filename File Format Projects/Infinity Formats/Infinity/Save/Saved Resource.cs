using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Save
{
    /// <summary>Represents a *.SAV resource in standard Infinity Engine games</summary>
    public class SavedResource : IInfinityFormat
    {
        #region Fields
        /// <summary>Represents the length of the file name</summary>
        protected Int32 resourceNameLength;

        /// <summary>Represents the name of the saved resource</summary>
        protected ZString resourceName;

        /// <summary>Represents the length of data once decompressed</summary>
        public Int32 DecompressedDataSize { get; set; }

        /// <summary>Represents the length of data once compressed</summary>
        protected Int32 compressedDataSize;

        /// <summary>The compressed data within the save file</summary>
        protected Byte[] compressedData;
        #endregion


        #region Properties
        /// <summary>Exposes the length of the file name</summary>
        public Int32 ResourceNameLength
        {
            get { return this.resourceNameLength; }
        }

        /// <summary>Exposes the resource name and updates the length of the resource name</summary>
        public String ResourceName
        {
            get { return this.resourceName.Value; }
            set
            {
                this.resourceName.Source = value;
                this.resourceNameLength = this.resourceName.Source.Length;
            }
        }

        /// <summary>Exposes the length of data once compressed</summary>
        public Int32 CompressedDataSize
        {
            get { return this.compressedDataSize; }
        }

        /// <summary>Exposes the compressed data array</summary>
        public Byte[] CompressedData
        {
            get { return this.compressedData; }
            set
            {
                this.compressedData = value;
                this.compressedDataSize = this.compressedData.Length;
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.resourceName = new ZString();
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
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 4);
            this.resourceNameLength = ReusableIO.ReadInt32FromArray(buffer, 0);

            buffer = ReusableIO.BinaryRead(input, this.resourceNameLength);
            this.resourceName.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, buffer.Length);

            buffer = ReusableIO.BinaryRead(input, 8);
            this.DecompressedDataSize = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.compressedDataSize = ReusableIO.ReadInt32FromArray(buffer, 4);

            this.CompressedData = ReusableIO.BinaryRead(input, this.compressedDataSize);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.resourceNameLength, output);
            ReusableIO.WriteStringToStream(this.resourceName.Source, output, CultureConstants.CultureCodeEnglish, true);
            ReusableIO.WriteInt32ToStream(this.DecompressedDataSize, output);
            ReusableIO.WriteInt32ToStream(this.compressedDataSize, output);
            output.Write(this.CompressedData, 0, this.compressedData.Length);
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
            return StringFormat.ReturnAndIndent(String.Format("Saved resource entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Saved resource entry:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Resource name length"));
            builder.Append(this.resourceNameLength);
            builder.Append(StringFormat.ToStringAlignment("Resource name"));
            builder.Append(String.Format("'{0}'", this.resourceName.Value));
            builder.Append(StringFormat.ToStringAlignment("Decompressed Data length"));
            builder.Append(this.DecompressedDataSize);
            builder.Append(StringFormat.ToStringAlignment("Compressed Data length"));
            builder.Append(this.compressedDataSize);
            builder.Append(StringFormat.ToStringAlignment("Compressed Data"));
            builder.Append(StringFormat.ByteArrayToHexString(this.compressedData));

            return builder.ToString();
        }
        #endregion
    }
}