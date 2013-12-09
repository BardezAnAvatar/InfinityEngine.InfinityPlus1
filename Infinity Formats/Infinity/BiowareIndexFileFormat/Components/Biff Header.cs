using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Components
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
    public class Biff1Header : InfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 20;
        #endregion


        #region Fields
        /// <summary>This property exposes the count of non-tileset resources in this archive.</summary>
        public Int32 CountResource { get; set; }

        /// <summary>This property exposes the count of tileset resources in this archive.</summary>
        public Int32 CountTileset { get; set; }

        /// <summary>This property exposes the offset from the beginning of the file to the non-tileset resources entries in this archive.</summary>
        public Int32 OffsetResource { get; set; }
        #endregion

        
        #region Properties
        /// <summary>This proerty exposes the offset from the beginning of the file to the non-tileset resources entries in this archive.</summary>
        public Int32 OffsetTileset
        {
            get { return this.OffsetResource + this.CountResource * 16; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region IInfinityFormat I/O methods
        /// <summary>This public method reads the 20-byte header into the header record</summary>
        /// <param name="input">Stream object into which to write to</param>
        public override void ReadBody(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 12);   //header buffer

            this.CountResource = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.CountTileset = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.OffsetResource = ReusableIO.ReadInt32FromArray(buffer, 8);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            ReusableIO.WriteInt32ToStream(this.CountResource, output);
            ReusableIO.WriteInt32ToStream(this.CountTileset, output);
            ReusableIO.WriteInt32ToStream(this.OffsetResource, output);
        }
        #endregion


        #region ToString() override
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Biff Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Signature)));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Version)));
            builder.Append(StringFormat.ToStringAlignment("Resource Count"));
            builder.Append(this.CountResource);
            builder.Append(StringFormat.ToStringAlignment("Tileset Count"));
            builder.Append(this.CountTileset);
            builder.Append(StringFormat.ToStringAlignment("Resource Count"));
            builder.Append(this.OffsetResource);
            builder.Append(StringFormat.ToStringAlignment("Tileset Count"));
            builder.Append(this.OffsetTileset);

            return builder.ToString();
        }
        #endregion
    }
}