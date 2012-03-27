using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Audio Stream Data opcode</summary>
    public class AudioStreamData : AudioStream
    {
        #region Fields
        /// <summary>Length of stored data for this op code to read</summary>
        public UInt16 StoredDataLength { get; set; }

        /// <summary>Binary sample data</summary>
        public Byte[] Data { get; set; }
        #endregion


        #region Construction
        /// <summary>Partial definition constructor</summary>
        /// <param name="length">Length of data sub-stream</param>
        public AudioStreamData(UInt16 length)
        {
            this.StoredDataLength = length;
        }

        /// <summary>Exposes the audio stream data samples</summary>
        public override Byte[] Samples
        {
            get { return new Byte[this.UncompressedSampleDataLength]; }
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode binary data, but not stored data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadData(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.OffsetData);
            this.Data = ReusableIO.BinaryRead(input, this.StoredDataLength - AudioStreamData.OpcodeSize);
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
            base.Write(output);
            output.Write(this.Data, 0, this.Data.Length);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.GetStringRepresentation());
            builder.Append(StringFormat.ToStringAlignment("Data"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Data));

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
                if (obj != null && obj is AudioStreamData)
                {
                    AudioStreamData compare = obj as AudioStreamData;

                    Boolean structureEquality = base.Equals(compare);
                    structureEquality &= (this.SequenceIndex == compare.SequenceIndex);
                    structureEquality &= (this.StreamMask == compare.StreamMask);
                    structureEquality &= (this.UncompressedSampleDataLength == compare.UncompressedSampleDataLength);
                    structureEquality &= (this.Data.Equals<Byte>(compare.Data));

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
            hash ^= this.Data.GetHashCode<Byte>();

            return hash;
        }
        #endregion
    }
}