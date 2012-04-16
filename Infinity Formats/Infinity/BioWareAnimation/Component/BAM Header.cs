using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BlackIsleAnimation.Component
{
    /// <summary>Represents a BAM file's header</summary>
    public class BamHeader : InfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 24;
        #endregion


        #region Fields
        /// <summary>Count of individual frames within this BAM file</summary>
        public UInt16 CountFrames { get; set; }

        /// <summary>Count of animations within this BAM file</summary>
        /// <remarks>Animations in NI, Cycles in IESDP</remarks>
        public Byte CountAnimations { get; set; }

        /// <summary>Palette entry index that may or may not be RLE encoded</summary>
        public Byte RlePaletteIndex { get; set; }

        /// <summary>Offset to th frame entry structures</summary>
        /// <remarks>Animation entries immediately follow, apparently</remarks>
        public UInt32 OffsetFrameEntries { get; set; }

        /// <summary>Offset to the palette</summary>
        public UInt32 OffsetPalette { get; set; }

        /// <summary>Offset to the frame lookup table</summary>
        public UInt32 OffsetFrameLookupTable { get; set; }
        #endregion


        #region Properties
        /// <summary>Calculated offset to Animation entries</summary>
        public Int64 OffsetAnimationEntries
        {
            get { return this.OffsetFrameEntries + (FrameEntry.StructSize * this.CountFrames); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, BamHeader.StructSize - 8);   //header buffer

            this.CountFrames = ReusableIO.ReadUInt16FromArray(buffer, 0);
            this.CountAnimations = buffer[2];
            this.RlePaletteIndex = buffer[3];
            this.OffsetFrameEntries = ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.OffsetPalette = ReusableIO.ReadUInt32FromArray(buffer, 8);
            this.OffsetFrameLookupTable = ReusableIO.ReadUInt32FromArray(buffer, 12);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt16ToStream(this.CountFrames, output);
            output.WriteByte(CountAnimations);
            output.WriteByte(RlePaletteIndex);
            ReusableIO.WriteUInt32ToStream(this.OffsetFrameEntries, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetPalette, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetFrameLookupTable, output);
        }
        #endregion


        #region Public Methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        /// <remarks>TIS header signatures can be null-terminated rather than have whitespace</remarks>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("BAM Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Frame Count"));
            builder.Append(this.CountFrames);
            builder.Append(StringFormat.ToStringAlignment("Animation Count"));
            builder.Append(this.CountAnimations);
            builder.Append(StringFormat.ToStringAlignment("RLI index"));
            builder.Append(this.RlePaletteIndex);
            builder.Append(StringFormat.ToStringAlignment("Frame Entries Offset"));
            builder.Append(this.OffsetFrameEntries);
            builder.Append(StringFormat.ToStringAlignment("Animation Entries Offset"));
            builder.Append(this.OffsetAnimationEntries);
            builder.Append(StringFormat.ToStringAlignment("Palette Offset"));
            builder.Append(this.OffsetPalette);
            builder.Append(StringFormat.ToStringAlignment("Frame Lookup Table Offset"));
            builder.Append(this.OffsetFrameLookupTable);

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
                if (obj != null && obj is BamHeader)
                {
                    BamHeader compare = obj as BamHeader;

                    Boolean structureEquality = (this.CountFrames == compare.CountFrames);
                    structureEquality &= (this.CountAnimations == compare.CountAnimations);
                    structureEquality &= (this.RlePaletteIndex == compare.RlePaletteIndex);

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
            hash ^= this.CountFrames.GetHashCode();
            hash ^= this.CountAnimations.GetHashCode();
            hash ^= this.RlePaletteIndex.GetHashCode();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}