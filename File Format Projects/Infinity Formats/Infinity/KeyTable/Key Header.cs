using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable
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
    ///     
    ///     One astonishingly interesting gem I came across while reading the Aurora Engine devlopers' documents is that the Aurora Engine
    ///     insists that it also uses Key version 1. The reason that this is interesting is that BioWare claims this format is consistent.
    ///     Well, with Infinity, the value at 0x8 is always 0x18, immediately after this header. Aurora's key indicates that at this offset, there is an
    ///     addtional 40 bytes, 4 for the build years since 1900, 4 for the days since january 1, and 32 reserved bytes; if this format was
    ///     in fact the same, then they just overwrote these values at 0x18 when generating the key file, [or BioWare could not version anything properly ;)]
    /// </remarks>
    public class ChitinKeyHeader : InfinityFormat, IDeepCloneable
    {
        #region Fields
        /// <summary>This two-byte value represents the count of BIF entries in the key file.</summary>
        public UInt32 CountBif { get; set; }

        /// <summary>This four-byte value represents the count of resource1 (file) entries in the key file.</summary>
        public UInt32 CountResource { get; set; }

        /// <summary>This four-byte value represents the offset to the BIF entries in the key file.</summary>
        public UInt32 OffsetBif { get; set; }

        /// <summary>This four-byte value represents the offset to the resource1 entries in the key file.</summary>
        public UInt32 OffsetResource { get; set; }
        #endregion

        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion

        #region Public Methods
        /// <summary>This public method reads the 18-byte header into the header record</summary>
        /// <param name="input">Stream object from which to read from</param>
        public override void ReadBody(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 16);   //header buffer

            this.CountBif = ReusableIO.ReadUInt32FromArray(buffer, 0x0);
            this.CountResource = ReusableIO.ReadUInt32FromArray(buffer, 0x4);
            this.OffsetBif = ReusableIO.ReadUInt32FromArray(buffer, 0x8);
            this.OffsetResource = ReusableIO.ReadUInt32FromArray( buffer, 12);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            Byte[] writeBytes;

            //Signature
            writeBytes = ReusableIO.GetStringByteArray(this.Signature, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Version
            writeBytes = ReusableIO.GetStringByteArray(this.Version, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Count of BIF entries
            writeBytes = BitConverter.GetBytes(this.CountBif);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Count of resource1 entries
            writeBytes = BitConverter.GetBytes(this.CountResource);
            output.Write(writeBytes, 0, writeBytes.Length);

            //BIF entries offset
            writeBytes = BitConverter.GetBytes(this.OffsetBif);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Resource entries offset
            writeBytes = BitConverter.GetBytes(this.OffsetResource);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ReturnAndIndent("ChitinKeyHeader:", 0));
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.Signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.Version));
            builder.Append(StringFormat.ToStringAlignment("BIFF Count"));
            builder.Append(this.CountBif);
            builder.Append(StringFormat.ToStringAlignment("Resource count"));
            builder.Append(this.CountResource);
            builder.Append(StringFormat.ToStringAlignment("BIFF Offset"));
            builder.Append(this.OffsetBif);
            builder.Append(StringFormat.ToStringAlignment("Resource Offset"));
            builder.Append(this.OffsetResource);

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