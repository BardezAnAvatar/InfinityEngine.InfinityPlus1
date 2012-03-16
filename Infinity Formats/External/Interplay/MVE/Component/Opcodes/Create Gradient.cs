using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents a Create Gradient opcode</summary>
    public class CreateGradient : OpcodeData
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 6;
        #endregion


        #region Fields
        /// <summary>Represents palette start index for Red/Blue entries</summary>
        public Byte IndexRedBlue { get; set; }

        /// <summary>Represents count of Red/Blue Red gradient progression</summary>
        public Byte RedBlue_RedCount { get; set; }

        /// <summary>Represents count of Red/Blue Blue gradient progression</summary>
        public Byte RedBlue_BlueCount { get; set; }

        /// <summary>Represents palette start index for Red/Green entries</summary>
        public Byte IndexRedGreen { get; set; }

        /// <summary>Represents count of Red/Green Red gradient progression</summary>
        public Byte RedGreen_RedCount { get; set; }

        /// <summary>Represents count of Red/Green Green gradient progression</summary>
        public Byte RedGreen_GreenCount { get; set; }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] data = ReusableIO.BinaryRead(input, CreateGradient.OpcodeSize);

            this.IndexRedBlue = data[0];
            this.RedBlue_RedCount = data[1];
            this.RedBlue_BlueCount = data[2];
            this.IndexRedGreen = data[3];
            this.RedGreen_RedCount = data[4];
            this.RedGreen_GreenCount = data[5];
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            output.WriteByte(this.IndexRedBlue);
            output.WriteByte(this.RedBlue_RedCount);
            output.WriteByte(this.RedBlue_BlueCount);
            output.WriteByte(this.IndexRedGreen);
            output.WriteByte(this.RedGreen_RedCount);
            output.WriteByte(this.RedGreen_GreenCount);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Index Red/Blue"));
            builder.Append(this.IndexRedBlue);
            builder.Append(StringFormat.ToStringAlignment("Red/Blue Red Count"));
            builder.Append(this.RedBlue_RedCount);
            builder.Append(StringFormat.ToStringAlignment("Red/Blue Blue Count"));
            builder.Append(this.RedBlue_BlueCount);
            builder.Append(StringFormat.ToStringAlignment("Index Red/Green"));
            builder.Append(this.IndexRedGreen);
            builder.Append(StringFormat.ToStringAlignment("Red/Green Red Count"));
            builder.Append(this.RedGreen_RedCount);
            builder.Append(StringFormat.ToStringAlignment("Red/Green Green Count"));
            builder.Append(this.RedGreen_GreenCount);

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
                if (obj != null && obj is CreateGradient)
                {
                    CreateGradient compare = obj as CreateGradient;

                    Boolean structureEquality;
                    structureEquality = base.Equals(compare);
                    structureEquality &= (this.IndexRedBlue == compare.IndexRedBlue);
                    structureEquality &= (this.RedBlue_RedCount == compare.RedBlue_RedCount);
                    structureEquality &= (this.RedBlue_BlueCount == compare.RedBlue_BlueCount);
                    structureEquality &= (this.IndexRedGreen == compare.IndexRedGreen);
                    structureEquality &= (this.RedGreen_RedCount == compare.RedGreen_RedCount);
                    structureEquality &= (this.RedGreen_GreenCount == compare.RedGreen_GreenCount);

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
            hash ^= this.IndexRedBlue.GetHashCode();
            hash ^= this.RedBlue_RedCount.GetHashCode();
            hash ^= this.RedBlue_BlueCount.GetHashCode();
            hash ^= this.IndexRedGreen.GetHashCode();
            hash ^= this.RedGreen_RedCount.GetHashCode();
            hash ^= this.RedGreen_GreenCount.GetHashCode();

            return hash;
        }
        #endregion
    }
}