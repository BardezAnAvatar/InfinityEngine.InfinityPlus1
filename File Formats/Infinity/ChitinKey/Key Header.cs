using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey
{
    /// <summary>This class is the header to a chitin.key file</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset      Size (data type)  	Description
    ///     0x0000 	    4 (char array) 	    Signature ('KEY ')
    ///     0x0004 	    4 (char array) 	    Version ('V1 ')
    ///     0x0008 	    4 (dword) 	        Count of BIF entries
    ///     0x000c 	    4 (dword) 	        Count of resource1 entries
    ///     0x0010 	    4 (dword) 	        Offset (from start of file) to BIF entries
    ///     0x0014 	    4 (dword) 	        Offset (from start of file) to resource1 entries
    /// </remarks>
    public class ChitinKeyHeader : IDeepCloneable
    {
        #region Protected Members
        /// <summary>This member contains the signature of the file. In all Infinity Engine cases it should be 'KEY '.</summary>
        protected String signature;

        /// <summary>This member contains the version of the file format. In all Infinity Engine cases it should be 'V1  '.</summary>
        protected String version;

        /// <summary>This four-byte value indicates the count of BIF entries in the key file.</summary>
        protected UInt32 countBif;

        /// <summary>This four-byte value indicates the count of resource1 (file) entries in the key file.</summary>
        protected UInt32 countResource;

        /// <summary>This four-byte value indicates the offset to the BIF entries in the key file.</summary>
        protected UInt32 offsetBif;

        /// <summary>This four-byte value indicates the offset to the resource1 entries in the key file.</summary>
        protected UInt32 offsetResource;
        #endregion

        #region Properties
        /// <summary>This property represents the signature of the file. In all Infinity Engine cases it should be 'KEY '.</summary>
        public String Signature
        {
            get { return this.signature; }
            set { this.signature = value; }
        }

        /// <summary>This property represents the version of the file format. In all Infinity Engine cases it should be 'V1  '.</summary>
        public String Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        /// <summary>This two-byte value represents the count of BIF entries in the key file.</summary>
        public UInt32 CountBif
        {
            get { return this.countBif; }
            set { this.countBif = value; }
        }

        /// <summary>This four-byte value represents the count of resource1 (file) entries in the key file.</summary>
        public UInt32 CountResource
        {
            get { return this.countResource; }
            set { this.countResource = value; }
        }

        /// <summary>This four-byte value represents the offset to the BIF entries in the key file.</summary>
        public UInt32 OffsetBif
        {
            get { return this.offsetBif; }
            set { this.offsetBif = value; }
        }

        /// <summary>This four-byte value represents the offset to the resource1 entries in the key file.</summary>
        public UInt32 OffsetResource
        {
            get { return this.offsetResource; }
            set { this.offsetResource = value; }
        } 
        #endregion

        #region Public Methods
        /// <summary>This public method reads the 18-byte header into the header record</summary>
        /// <param name="input">Stream object from which to read from</param>
        public void Read(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 24);   //header buffer
            //Encoding encoding = new ASCIIEncoding();

            //signature
            this.signature = ReusableIO.ReadStringFromByteArray(buffer, 0, Constants.CultureCodeEnglish, 4);

            //version
            this.version = ReusableIO.ReadStringFromByteArray(buffer, 4, Constants.CultureCodeEnglish, 4);

            //Count of BIF entries
            this.countBif = ReusableIO.ReadUInt32FromArray(buffer, 0x8);

            //Count of resource1 entries
            this.countResource = ReusableIO.ReadUInt32FromArray(buffer, 0xC);

            //BIF entries offset
            this.offsetBif = ReusableIO.ReadUInt32FromArray(buffer, 0x10);

            //Resource entries offset
            this.offsetResource = ReusableIO.ReadUInt32FromArray( buffer, 0x14);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            Byte[] writeBytes;

            //signature
            writeBytes = ReusableIO.WriteStringToByteArray(this.signature, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //version
            writeBytes = ReusableIO.WriteStringToByteArray(this.version, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Count of BIF entries
            writeBytes = BitConverter.GetBytes(this.countBif);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Count of resource1 entries
            writeBytes = BitConverter.GetBytes(this.countResource);
            output.Write(writeBytes, 0, writeBytes.Length);

            //BIF entries offset
            writeBytes = BitConverter.GetBytes(this.offsetBif);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Resource entries offset
            writeBytes = BitConverter.GetBytes(this.offsetResource);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ChitinKeyHeader:");
            builder.Append("\n    Signature:        '");
            builder.Append(this.signature);
            builder.Append("'\n    Version:          '");
            builder.Append(this.version);
            builder.Append("'\n    BIF Count:        ");
            builder.Append(this.countBif);
            builder.Append("\n    Resource count:   ");
            builder.Append(this.countResource);
            builder.Append("\n    BIF Offset:       ");
            builder.Append(this.offsetBif);
            builder.Append("\n    Resource Offset:  ");
            builder.Append(this.offsetResource);
            builder.Append("\n\n");

            return builder.ToString();
        }

        /// <summary>Provides a clone (new instance) of the existing class.</summary>
        /// <returns>A new instance of ChitinKeyHeader class with identical data.</returns>
        /// <remarks>No Deep Copy provided since object data is Strings and Integers, and strings are immutable.</remarks>
        public IDeepCloneable Clone()
        {
            return this.MemberwiseClone() as ChitinKeyHeader; 
        }
        #endregion
    }
}