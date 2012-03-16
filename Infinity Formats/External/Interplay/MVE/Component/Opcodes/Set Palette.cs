using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Represents a Set Palette opcode</summary>
    public class SetPalette : OpcodeData
    {
        #region Constants
        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 4;

        /// <summary>Represents the size of this opcode in an MVE chunk</summary>
        public const Int32 ColorSize = 3;
        #endregion


        #region Fields
        /// <summary>Represents palette start index for entries</summary>
        public UInt16 StartIndex { get; set; }

        /// <summary>Represents count of palette entries</summary>
        public UInt16 ColorCount { get; set; }

        /// <summary>Represents the palette data to be set</summary>
        public Byte[] Data { get; set; }
        #endregion


        #region MediaBase Palette Method(s)
        /// <summary>Generates a new palette from the existing palette passed in</summary>
        /// <param name="existing">Existing palette to adjust</param>
        /// <returns>A MediaBase Palette reference instance with palette data</returns>
        public virtual Palette GeneratePalette(Palette existing)
        {
            if (existing == null)
                existing = this.CreateBlankPalette();

            //palette data
            Int32 index = this.StartIndex;
            for (Int32 i = 0; i < this.Data.Length; i += 3)
            {
                existing.Pixels[index] = new RgbTriplet((Byte)(this.Data[i] << 2), (Byte)(this.Data[i + 1] << 2), (Byte)(this.Data[i + 2] << 2));
                ++index;
            }
            existing.GeneratePixelData();

            return existing;
        }

        /// <summary>Creates a blank palette, for when a no existing palette is passed in</summary>
        /// <returns>A blanked, new palette</returns>
        protected virtual Palette CreateBlankPalette()
        {
            IList<PixelBase> pixels = new PixelBase[256];

            //palette blank start
            for (Int32 i = 0; i < this.StartIndex; ++i)
                pixels[i] = new RgbTriplet();

            //palette blank end
            for (Int32 i = this.StartIndex + this.ColorCount; i < 256; ++i)
                pixels[i] = new RgbTriplet();

            return new Palette(24, pixels);
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadParameters(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, SetPalette.OpcodeSize);

            this.StartIndex = ReusableIO.ReadUInt16FromArray(data, 0);
            this.ColorCount = ReusableIO.ReadUInt16FromArray(data, 2);
        }

        /// <summary>This public method reads the data offset from the input stream's position, or sets to -1 if not applicable.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReacordDataOffset(Stream input)
        {
            this.OffsetData = input.Position;
        }

        /// <summary>This public method reads opcode binary data, but not stored data.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadData(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.OffsetData);
            this.Data = ReusableIO.BinaryRead(input, this.ColorCount * SetPalette.ColorSize);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {            
            ReusableIO.WriteUInt16ToStream(this.StartIndex, output);
            ReusableIO.WriteUInt16ToStream(this.ColorCount, output);
            output.Write(this.Data, 0, this.Data.Length);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Palette Start Index"));
            builder.Append(this.StartIndex);
            builder.Append(StringFormat.ToStringAlignment("Palette Entry Count"));
            builder.Append(this.ColorCount);
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
                if (obj != null && obj is SetPalette)
                {
                    SetPalette compare = obj as SetPalette;

                    Boolean structureEquality = base.Equals(compare);
                    structureEquality &= (this.StartIndex == compare.StartIndex);
                    structureEquality &= (this.ColorCount == compare.ColorCount);
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
            hash ^= this.StartIndex.GetHashCode();
            hash ^= this.ColorCount.GetHashCode();
            hash ^= this.Data.GetHashCode<Byte>();

            return hash;
        }
        #endregion
    }
}