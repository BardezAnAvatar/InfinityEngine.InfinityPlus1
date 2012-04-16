using System;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Biff1
{
    /// <summary>This class is a representation of the BIFF version 1 file format.</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset 	Size    (data type) 	Description
    ///     0x0000 	4       (char array) 	Signature ('BIFF')
    ///     0x0004 	4       (char array) 	Version ('V1 ')
    ///     0x0008 	4       (dword) 	    Count of file entries
    ///     0x000c 	4       (dword) 	    Count of tileset entries
    ///     0x0010 	4       (dword) 	    Offset (from start of file) to file entries. Tileset entries, if any, immediately follow the file entries.
    /// </remarks>
    public class Biff1Header
    {
        #region Members
        /// <summary>This member contains the signature of the file. In all Infinity Engine cases it should be 'BIFF'.</summary>
        protected String signature;

        /// <summary>This member contains the version of the file format. In all Infinity Engine cases it should be 'V1  '.</summary>
        protected String version;

        /// <summary>This member contains the count of non-tileset resources in this archive.</summary>
        protected UInt32 countResource;

        /// <summary>This member contains the count of tileset resources in this archive.</summary>
        protected UInt32 countTileset;

        /// <summary>This member contains the offset from the beginning of the file to the non-tileset resources entries in this archive.</summary>
        protected UInt32 offsetResource;
        #endregion

        #region Properties
        /// <summary>This proerty exposes the signature of the file. In all Infinity Engine cases it should be 'BIFF'.</summary>
        public String Signature
        {
            get { return this.signature; }
            set { this.signature = value; }
        }

        /// <summary>This proerty exposes the version of the file format. In all Infinity Engine cases it should be 'V1  '.</summary>
        public String Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        /// <summary>This proerty exposes the count of non-tileset resources in this archive.</summary>
        public UInt32 CountResource
        {
            get { return this.countResource; }
            set { this.countResource = value; }
        }

        /// <summary>This proerty exposes the count of tileset resources in this archive.</summary>
        public UInt32 CountTileset
        {
            get { return this.countTileset; }
            set { this.countTileset = value; }
        }

        /// <summary>This proerty exposes the offset from the beginning of the file to the non-tileset resources entries in this archive.</summary>
        public UInt32 OffsetResource
        {
            get { return this.offsetResource; }
            set { this.offsetResource = value; }
        }

        /// <summary>This proerty exposes the offset from the beginning of the file to the non-tileset resources entries in this archive.</summary>
        public UInt32 OffsetTileset
        {
            get { return this.offsetResource + this.countResource * 16U; }
        }
        #endregion

        #region Public Methods
        /// <summary>This public method reads the 20-byte header into the header record</summary>
        /// <param name="input">Stream object into which to write to</param>
        public void Read(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 20);   //header buffer
            Byte[] temp = new Byte[4];
            Encoding encoding = new ASCIIEncoding();

            //signature
            Array.Copy(buffer, temp, 4);
            this.signature = encoding.GetString(temp);

            //version
            Array.Copy(buffer, 4, temp, 0, 4);
            this.version = encoding.GetString(temp);

            //Number of resource1 entries
            this.countResource = ReusableIO.ReadUInt32FromArray(buffer, 0x08);

            //Number of tileset entries
            this.countTileset = ReusableIO.ReadUInt32FromArray(buffer, 0x0C);

            //Offset to resource1 entries
            this.offsetResource = ReusableIO.ReadUInt32FromArray(buffer, 0x10);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            Byte[] writeBytes;

            //signature
            writeBytes = ReusableIO.GetStringByteArray(this.signature, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //version
            writeBytes = ReusableIO.GetStringByteArray(this.version, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Number of resource1 entries
            writeBytes = BitConverter.GetBytes(this.countResource);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Number of tileset entries
            writeBytes = BitConverter.GetBytes(this.countTileset);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Offset to resource1 entries
            writeBytes = BitConverter.GetBytes(this.offsetResource);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Biff1Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append("'");
            builder.Append(this.signature);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append("'");
            builder.Append(this.version);
            builder.Append("'");
            builder.Append(StringFormat.ToStringAlignment("Resource Count"));
            builder.Append(this.countResource);
            builder.Append(StringFormat.ToStringAlignment("Tileset Count"));
            builder.Append(this.countTileset);
            builder.Append(StringFormat.ToStringAlignment("Resource Count"));
            builder.Append(this.offsetResource);
            builder.Append(StringFormat.ToStringAlignment("Tileset Count"));
            builder.Append(this.OffsetTileset);

            return builder.ToString();
        }
        #endregion
    }
}