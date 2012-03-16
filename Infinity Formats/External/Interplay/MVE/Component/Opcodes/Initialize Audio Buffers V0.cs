using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Intialize Audio Buffers opcode</summary>
    public class InitializeAudioBuffers0 : InitializeAudioBuffers
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 8;
        #endregion


        #region Fields
        /// <summary>Represents the buffer length of the audio buffer</summary>
        /// <remarks>Private so as to hide</remarks>
        private UInt16 bufferLength;
        #endregion


        #region Properties
        /// <summary>Exposes the intended buffer length</summary>
        public override UInt32 BufferLength
        {
            get { return this.bufferLength; }
        }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public override Boolean Compressed
        {
            get { return false; }
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, InitializeAudioBuffers0.OpcodeSize);

            this.Unknown = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Flags = ReusableIO.ReadUInt16FromArray(data, 2);
            this.SampleRate = ReusableIO.ReadUInt16FromArray(data, 4);
            this.bufferLength = ReusableIO.ReadUInt16FromArray(data, 6);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.Unknown, output);
            ReusableIO.WriteUInt16ToStream(this.Flags, output);
            ReusableIO.WriteUInt16ToStream(this.SampleRate, output);
            ReusableIO.WriteUInt16ToStream(this.bufferLength, output);
        }
        #endregion


        #region ToString() methods
        /// <summary>This gets a human-readable representation of the flags.</summary>
        /// <returns>A string, formatted largely for console, that describes the flags' values.</returns>
        protected override String GetFlagStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Channels"));
            builder.Append(this.Channels);
            builder.Append(StringFormat.ToStringAlignment("Sample Size"));
            builder.Append(this.SampleSize);

            return builder.ToString();
        }
        #endregion
    }
}