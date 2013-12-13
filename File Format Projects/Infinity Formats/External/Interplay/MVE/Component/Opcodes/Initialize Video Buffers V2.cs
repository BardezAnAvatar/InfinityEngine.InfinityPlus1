using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Intialize Video Buffers opcode</summary>
    /// <remarks>Video is apparently 16 bits per pixel or 24? 32?</remarks>
    public class InitializeVideoBuffers2 : InitializeVideoBuffers1
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public new const Int32 OpcodeSize = 8;
        #endregion


        #region Fields
        /// <summary>Represents the true color parameter</summary>
        protected UInt16 trueColor;
        #endregion


        #region Properties
        /// <summary>Exposes the size of samples in the audio stream</summary>
        public override UInt16 HighColor
        {
            get { return this.trueColor; }
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, InitializeVideoBuffers2.OpcodeSize);

            this.Width = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Height = ReusableIO.ReadUInt16FromArray(data, 2);
            this.bufferLength = ReusableIO.ReadUInt16FromArray(data, 4);
            this.trueColor = ReusableIO.ReadUInt16FromArray(data, 6);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);
            ReusableIO.WriteUInt16ToStream(this.trueColor, output);
        }
        #endregion
    }
}