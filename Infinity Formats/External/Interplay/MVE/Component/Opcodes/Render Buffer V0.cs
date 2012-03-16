using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Render Video Buffer opcode</summary>
    public class RenderBuffer0 : OpcodeData
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        private const Int32 OpcodeSize = 4;
        #endregion


        #region Fields
        /// <summary>Represents start index of the palette to install/copy</summary>
        public UInt16 PaletteStartIndex { get; set; }

        /// <summary>Represents count of the palette entries to install/copy</summary>
        public UInt16 PaletteIndexCount{ get; set; }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, RenderBuffer0.OpcodeSize);

            this.PaletteStartIndex = ReusableIO.ReadUInt16FromArray(data, 0);
            this.PaletteIndexCount = ReusableIO.ReadUInt16FromArray(data, 2);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.PaletteStartIndex, output);
            ReusableIO.WriteUInt16ToStream(this.PaletteIndexCount, output);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Palette Start Index"));
            builder.Append(this.PaletteStartIndex);
            builder.Append(StringFormat.ToStringAlignment("Palette Index Count"));
            builder.Append(this.PaletteIndexCount);

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
                if (obj != null && obj is RenderBuffer0)
                {
                    RenderBuffer0 compare = obj as RenderBuffer0;

                    Boolean structureEquality = base.Equals(compare);
                    structureEquality &= (this.PaletteStartIndex == compare.PaletteStartIndex);
                    structureEquality &= (this.PaletteIndexCount == compare.PaletteIndexCount);

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
            hash ^= this.PaletteStartIndex.GetHashCode();
            hash ^= this.PaletteIndexCount.GetHashCode();

            return hash;
        }
        #endregion
    }
}