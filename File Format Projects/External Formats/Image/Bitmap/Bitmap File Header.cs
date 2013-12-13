using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap
{
    /// <summary>Contains information about the type, size, and layout of a file that contains a DIB.</summary>
    public class BitmapFileHeader
    {
        #region Fields
        /// <summary>Constant indicating the structure size on disk</summary>
        public const UInt32 StructSize = 14U;
        #endregion


        #region Properties
        /// <summary>The file type.</summary>
        /// <value>Must be 'BM', which is: 19778 or 0x4D42</value>
        public Int16 Type { get; set; }

        /// <summary>The size, in bytes, of the bitmap file.</summary>
        public UInt32 Size { get; set; }

        /// <summary>Reserved.</summary>
        /// <value>Must be zero.</value>
        public Int16 Reserved1 { get; set; }

        /// <summary>Reserved.</summary>
        /// <value>Must be zero.</value>
        public Int16 Reserved2 { get; set; }

        /// <summary>The offset, in bytes, from the beginning of the file to the bitmap bits.</summary>
        public UInt32 BitmapDataOffset { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public BitmapFileHeader() { }

        /// <summary>Definition constructor</summary>
        /// <param name="type">The file type</param>
        /// <param name="size">The size, in bytes, of the bitmap file</param>
        /// <param name="reserved1">Reserved</param>
        /// <param name="reserved2">Reserved</param>
        /// <param name="dataOffset">offset, in bytes, from the beginning of the file to the bitmap bits.</param>
        public BitmapFileHeader(Int16 type, UInt32 size, Int16 reserved1, Int16 reserved2, UInt32 dataOffset)
        {
            this.Type = type;
            this.Size = size;
            this.Reserved1 = reserved1;
            this.Reserved2 = reserved2;
            this.BitmapDataOffset = dataOffset;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="type">The file type</param>
        /// <param name="size">The size, in bytes, of the bitmap file</param>
        /// <param name="dataOffset">offset, in bytes, from the beginning of the file to the bitmap bits.</param>
        public BitmapFileHeader(Int16 type, UInt32 size, UInt32 dataOffset) : this (type, size, 0, 0, dataOffset) { }
        #endregion


        #region Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, 14);

            this.Type = ReusableIO.ReadInt16FromArray(data, 0);
            this.Size = ReusableIO.ReadUInt32FromArray(data, 2);
            this.Reserved1 = ReusableIO.ReadInt16FromArray(data, 6);
            this.Reserved2 = ReusableIO.ReadInt16FromArray(data, 8);
            this.BitmapDataOffset = ReusableIO.ReadUInt32FromArray(data, 10);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteInt16ToStream(this.Type, output);
            ReusableIO.WriteUInt32ToStream(this.Size, output);
            ReusableIO.WriteInt16ToStream(this.Reserved1, output);
            ReusableIO.WriteInt16ToStream(this.Reserved2, output);
            ReusableIO.WriteUInt32ToStream(this.BitmapDataOffset, output);
        }
        #endregion
    }
}