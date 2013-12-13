using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents an Render Video Buffer opcode</summary>
    public class RenderBuffer1 : RenderBuffer0
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        private const Int32 OpcodeSize = 6;
        #endregion


        #region Fields
        /// <summary>Represents unkown third UInt16</summary>
        /// <remarks>Maybe param #2 (PaletteIndexCount) is a 4-byte DWORD?</remarks>
        public UInt16 Unknown { get; set; }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, RenderBuffer1.OpcodeSize);

            this.PaletteStartIndex = ReusableIO.ReadUInt16FromArray(data, 0);
            this.PaletteIndexCount = ReusableIO.ReadUInt16FromArray(data, 2);
            this.Unknown = ReusableIO.ReadUInt16FromArray(data, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.PaletteStartIndex, output);
            ReusableIO.WriteUInt16ToStream(this.PaletteIndexCount, output);
            ReusableIO.WriteUInt16ToStream(this.Unknown, output);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.GetStringRepresentation());
            builder.Append(StringFormat.ToStringAlignment("Unknown"));
            builder.Append(this.Unknown);

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
                if (obj != null && obj is RenderBuffer1)
                {
                    RenderBuffer1 compare = obj as RenderBuffer1;

                    Boolean structureEquality = base.Equals(compare);
                    structureEquality &= (this.Unknown == compare.Unknown);

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
            hash ^= this.Unknown.GetHashCode();

            return hash;
        }
        #endregion
    }
}