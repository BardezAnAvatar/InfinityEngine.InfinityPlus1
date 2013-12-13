using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Audio Stream Silence opcode</summary>
    public class AudioStreamSilence : AudioStream
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 6;
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] data = ReusableIO.BinaryRead(input, AudioStreamSilence.OpcodeSize);

            this.SequenceIndex = ReusableIO.ReadUInt16FromArray(data, 0);
            this.StreamMask = ReusableIO.ReadUInt16FromArray(data, 2);
            this.UncompressedSampleDataLength = ReusableIO.ReadUInt16FromArray(data, 4);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.GetStringRepresentation());
            builder.Append(StringFormat.ToStringAlignment("Sequence Index"));
            builder.Append(this.SequenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Stream Mask"));
            builder.Append(this.StreamMask);
            builder.Append(StringFormat.ToStringAlignment("Data Legth"));
            builder.Append(this.UncompressedSampleDataLength);

            return builder.ToString();
        }
        #endregion
    }
}