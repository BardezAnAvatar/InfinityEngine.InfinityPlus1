using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents a create timer opcode to set the frame rate</summary>
    public class CreateTimer : OpcodeData
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 6;

        /// <summary>Constant multiplier of the frame rate</summary>
        public const UInt32 Multiplier = 1000000U;
        #endregion


        #region Fields
        /// <summary>Represents the main rate of the denominator</summary>
        public UInt32 Rate { get; set; }

        /// <summary>Represents the secondary denominator</summary>
        public UInt16 Subdivisor { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the numertor of the frame rate</summary>
        public virtual UInt32 Numerator
        {
            get { return CreateTimer.Multiplier; }
        }

        /// <summary>Exposes the denominator of the frame rate</summary>
        public virtual UInt32 Denominator
        {
            get { return this.Rate * this.Subdivisor; }
        }

        /// <summary>Exposes the frame rate, as a Double</summary>
        public virtual Double FrameRateDouble
        {
            get { return Convert.ToDouble(this.Numerator) / Convert.ToDouble(this.Denominator); }
        }

        /// <summary>Exposes the frame rate, as a Decimal</summary>
        public virtual Decimal FrameRateDecimal
        {
            get { return Convert.ToDecimal(this.Numerator) / Convert.ToDecimal(this.Denominator); }
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, CreateTimer.OpcodeSize);

            this.Rate = ReusableIO.ReadUInt32FromArray(data, 0);
            this.Subdivisor = ReusableIO.ReadUInt16FromArray(data, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.Rate, output);
            ReusableIO.WriteUInt16ToStream(this.Subdivisor, output);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Rate"));
            builder.Append(this.Rate);
            builder.Append(StringFormat.ToStringAlignment("Subdivisor"));
            builder.Append(this.Subdivisor);
            builder.Append(StringFormat.ToStringAlignment("Numerator"));
            builder.Append(this.Numerator);
            builder.Append(StringFormat.ToStringAlignment("Denominator"));
            builder.Append(this.Denominator);
            builder.Append(StringFormat.ToStringAlignment("Frame Rate"));
            builder.Append(this.FrameRateDecimal);

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
                if (obj != null && obj is CreateTimer)
                {
                    CreateTimer compare = obj as CreateTimer;

                    Boolean structureEquality;
                    structureEquality = base.Equals(compare);
                    structureEquality &= (this.Rate == compare.Rate);
                    structureEquality &= (this.Subdivisor == compare.Subdivisor);

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
            hash ^= this.Rate.GetHashCode();
            hash ^= this.Subdivisor.GetHashCode();

            return hash;
        }
        #endregion
    }
}