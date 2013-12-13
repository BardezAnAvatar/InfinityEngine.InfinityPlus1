using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Intialize Video Buffers opcode</summary>
    /// <remarks>Video is apparently 16 bits per pixel</remarks>
    public class InitializeVideoBuffers0 : InitializeVideoBuffers
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 4;

        /// <summary>Represents teh byte width of 16 bits per pixel</summary>
        public const UInt16 SixteenColorSize = 2;
        #endregion


        #region Properties
        /// <summary>Exposes the intended buffer length</summary>
        public override UInt16 BufferLength
        {
            get { return (UInt16)(this.Width * this.Height * InitializeVideoBuffers0.SixteenColorSize); }
        }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public override UInt16 HighColor
        {
            get { return 0; }
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, InitializeVideoBuffers0.OpcodeSize);

            this.Width = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(data, 2);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Width, output);
            ReusableIO.WriteUInt16ToStream(this.Height, output);
        }
        #endregion
    }
}