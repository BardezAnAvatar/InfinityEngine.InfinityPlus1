using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Audio Stream opcode base class, for silence</summary>
    public class AudioStream : OpcodeData
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 6;
        #endregion


        #region Fields
        /// <summary>Sequential index of the audio chunk (opcode?)</summary>
        public UInt16 SequenceIndex { get; set; }

        /// <summary>Stream mask of the audio chunk (opcode?)</summary>
        public UInt16 StreamMask { get; set; }

        /// <summary>Length of uncompressed audio sample data in this opcode</summary>
        public UInt16 UncompressedSampleDataLength { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes audio stream membership</summary>
        public AudioStreamChannels StreamChannel
        {
            get { return (AudioStreamChannels)this.StreamMask; }
        }

        /// <summary>Exposes the audio stream data samples</summary>
        public virtual Byte[] Samples
        {
            get { return new Byte[this.UncompressedSampleDataLength]; }
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, AudioStreamData.OpcodeSize);

            this.SequenceIndex = ReusableIO.ReadUInt16FromArray(data, 0);
            this.StreamMask = ReusableIO.ReadUInt16FromArray(data, 2);
            this.UncompressedSampleDataLength = ReusableIO.ReadUInt16FromArray(data, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.SequenceIndex, output);
            ReusableIO.WriteUInt16ToStream(this.StreamMask, output);
            ReusableIO.WriteUInt16ToStream(this.UncompressedSampleDataLength, output);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Sequence Index"));
            builder.Append(this.SequenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Stream Mask"));
            builder.Append(this.StreamMask);
            builder.Append(StringFormat.ToStringAlignment("Data Legth"));
            builder.Append(this.UncompressedSampleDataLength);

            return builder.ToString();
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is AudioStream)
                {
                    AudioStream compare = obj as AudioStream;

                    Boolean structureEquality = base.Equals(compare);
                    structureEquality &= (this.SequenceIndex == compare.SequenceIndex);
                    structureEquality &= (this.StreamMask == compare.StreamMask);
                    structureEquality &= (this.UncompressedSampleDataLength == compare.UncompressedSampleDataLength);

                    //offsets are unimportant when it comes to data value equivalence/equality
                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = base.GetHashCode();
            hash ^= this.SequenceIndex.GetHashCode();
            hash ^= this.StreamMask.GetHashCode();
            hash ^= this.UncompressedSampleDataLength.GetHashCode();

            return hash;
        }
        #endregion
    }
}