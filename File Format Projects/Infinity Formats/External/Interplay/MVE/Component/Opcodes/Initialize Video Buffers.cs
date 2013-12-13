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
    public abstract class InitializeVideoBuffers : OpcodeData
    {
        #region Fields
        /// <summary>Represents the width of the video frames</summary>
        public UInt16 Width { get; set; }

        /// <summary>Represents the height of the video frames</summary>
        public UInt16 Height { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the buffer length</summary>
        public abstract UInt16 BufferLength { get; }

        /// <summary>Exposes the flag indicating high (16-bit) color</summary>
        public abstract UInt16 HighColor { get; }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Buffer Length"));
            builder.Append(this.BufferLength);
            builder.Append(StringFormat.ToStringAlignment("True Color"));
            builder.Append(this.HighColor);

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
                if (obj != null && obj is InitializeVideoBuffers)
                {
                    InitializeVideoBuffers compare = obj as InitializeVideoBuffers;

                    Boolean structureEquality = base.Equals(compare);
                    structureEquality &= (this.Width == compare.Width);
                    structureEquality &= (this.Height == compare.Height);
                    structureEquality &= (this.BufferLength == compare.BufferLength);
                    structureEquality &= (this.HighColor == compare.HighColor);

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
            hash ^= this.Width.GetHashCode();
            hash ^= this.Height.GetHashCode();
            hash ^= this.HighColor.GetHashCode();
            hash ^= this.BufferLength.GetHashCode();

            return hash;
        }
        #endregion
    }
}