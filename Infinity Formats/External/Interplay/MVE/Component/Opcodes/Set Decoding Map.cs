using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents the decoding map opcode, which appears to be 1:1 with video data (is each video data a frame? TODO:RENAME video data enums and opcodes if so)</summary>
    /// <remarks>Data is stored low/high, low/high for 8x8 blocks (like J/MPEG!)</remarks>
    public class SetDecodingMap : OpcodeData
    {
        #region Fields
        /// <summary>Represents the length of data to read</summary>
        public Int32 DataLength { get; set; }

        /// <summary>Represents the decoding map raw data</summary>
        public Byte[] Data { get; set; }

        /// <summary>Represents the collection of block encoding in the data stream</summary>
        public BlockEncodingMethod[] BlockEncoding { get; set; }
        #endregion


        #region Construction
        /// <summary>Partial definition construcor</summary>
        /// <param name="dataSize">Size of the data to read</param>
        public SetDecodingMap(Int32 dataSize)
        {
            this.DataLength = dataSize;
        }
        #endregion


        public virtual void DecodeMap()
        {
            Int32 len = this.Data.Length;
            this.BlockEncoding = new BlockEncodingMethod[(len * 2)];

            for (Int32 i = 0; i < len; ++i)
            {
                Byte datum = this.Data[i];
                Int32 index = i * 2;
                this.BlockEncoding[index] = (BlockEncodingMethod)(datum & 0x0F);
                this.BlockEncoding[index + 1] = (BlockEncodingMethod)(datum >> 4);
            }
        }


        #region IO method implemetations
        /// <summary>This public method reads opcode binary data, but not stored data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadData(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.OffsetData);
            this.Data = ReusableIO.BinaryRead(input, this.DataLength);
        }

        /// <summary>This public method reads the data offset from the input stream's position, or sets to -1 if not applicable.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReacordDataOffset(Stream input)
        {
            this.OffsetData = input.Position;
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            Byte[] data = new Byte[this.DataLength];

            for (Int32 i = 0; i < this.BlockEncoding.Length; i += 2)
            {
                Byte datum = (Byte)this.BlockEncoding[i];
                datum |= Convert.ToByte((Byte)this.BlockEncoding[i + 1] << 4);
                data[i / 2] = datum;
            }

            output.Write(data, 0, this.DataLength);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            for (Int32 i = 0; i < this.BlockEncoding.Length; ++i)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Encoding Method # {0,5}:", i)));
                builder.Append(this.BlockEncoding[i].GetDescription());
            }

            return builder.ToString();
        }
        #endregion
    }
}