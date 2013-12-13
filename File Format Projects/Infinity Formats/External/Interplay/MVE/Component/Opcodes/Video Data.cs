using System;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents a set of video data, probably at the frame level</summary>
    /// <remarks>Version expected is version 3, the only one observed in MVE files</remarks>
    public class VideoData : OpcodeData
    {
        #region Constants
        /// <summary>Constant representing the length of leading data bytes</summary>
        private const Int32 ParameterLength = 14;
        #endregion


        #region Fields
        /// <summary>The Byte stream of block data, linked to the set palette entries</summary>
        public Byte[] Data { get; set; }

        /// <summary>Size of data to read</summary>
        public Int32 DataLength { get; set; }

        /// <summary>Exposes the current frame number associated with this video data</summary>
        public UInt16 FrameNumberCurrent { get; set; }

        /// <summary>Exposes the current frame number associated with this video data's previous frame</summary>
        public UInt16 FrameNumberPrevious { get; set; }

        /// <summary>Exposes the first unknown value, observed to constantly be 0</summary>
        /// <remarks>Labelled as X offset in MVE code bundled with documentation</remarks>
        public Int16 Unknown1 { get; set; }

        /// <summary>Exposes the second unknown value, observed to constantly be 0</summary>
        /// <remarks>Labelled as Y offset in MVE code bundled with documentation</remarks>
        public Int16 Unknown2 { get; set; }

        /// <summary>Exposes width -in 8x8 blocks- of video data</summary>
        public UInt16 BlockWidth { get; set; }

        /// <summary>Exposes height -in 8x8 blocks- of video data</summary>
        public UInt16 BlockHeight { get; set; }

        /// <summary>Flags set. Only the first bit (delta frame) is known</summary>
        public UInt16 Flags { get; set; }
        #endregion


        #region Properties
        /// <summary>Indicates whether this frame is a delta frame of the previous frame</summary>
        public Boolean DeltaFrame
        {
            get { return (this.Flags & 0x01) == 1; }
            set { this.Flags |= 0x01; }
        }
        #endregion


        #region Construction
        /// <summary>Partial definition construcor</summary>
        /// <param name="dataSize">Size of the data to read</param>
        public VideoData(Int32 dataSize)
        {
            this.DataLength = dataSize;
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode binary data, but not stored data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadData(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.OffsetData);
            Byte[] temp = ReusableIO.BinaryRead(input, VideoData.ParameterLength);

            this.FrameNumberCurrent = ReusableIO.ReadUInt16FromArray(temp, 0);
            this.FrameNumberPrevious = ReusableIO.ReadUInt16FromArray(temp, 2);
            this.Unknown1 = ReusableIO.ReadInt16FromArray(temp, 4);
            this.Unknown2 = ReusableIO.ReadInt16FromArray(temp, 6);
            this.BlockWidth = ReusableIO.ReadUInt16FromArray(temp, 8);
            this.BlockHeight = ReusableIO.ReadUInt16FromArray(temp, 10);
            this.Flags = ReusableIO.ReadUInt16FromArray(temp, 12);

            if (this.DataLength > VideoData.ParameterLength)
                this.Data = ReusableIO.BinaryRead(input, this.DataLength - VideoData.ParameterLength);
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
            output.Write(this.Data, 0, this.Data.Length);
        }
        #endregion
    }
}