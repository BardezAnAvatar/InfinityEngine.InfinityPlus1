using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation.Component
{
    /// <summary>Represents a BAM v2 file's header</summary>
    public class BamHeader_v2 : InfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 32;
        #endregion


        #region Fields
        /// <summary>Count of individual frames within this BAM file</summary>
        public UInt32 CountFrames { get; set; }

        /// <summary>Count of animations within this BAM file</summary>
        /// <remarks>Animations in NI, Cycles in IESDP</remarks>
        public UInt32 CountAnimations { get; set; }

        /// <summary>Count of blocks</summary>
        public Int32 CountDataBlocks { get; set; }

        /// <summary>Offset to the frame entry structures</summary>
        public UInt32 OffsetFrameEntries { get; set; }

        /// <summary>Offset to the animation entry structures</summary>
        public UInt32 OffsetAnimationEntries { get; set; }

        /// <summary>Offset to the PVRZ data blocks</summary>
        public UInt32 OffsetDataBlocks { get; set; }
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

            this.CountFrames = ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.CountAnimations = ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.CountDataBlocks = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.OffsetFrameEntries = ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.OffsetAnimationEntries = ReusableIO.ReadUInt32FromArray(buffer, 16);
            this.OffsetDataBlocks = ReusableIO.ReadUInt32FromArray(buffer, 20);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt32ToStream(this.CountFrames, output);
            ReusableIO.WriteUInt32ToStream(this.CountAnimations, output);
            ReusableIO.WriteInt32ToStream(this.CountDataBlocks, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetFrameEntries, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetAnimationEntries, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetDataBlocks, output);
        }
        #endregion


        #region Public Methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        /// <remarks>TIS header signatures can be null-terminated rather than have whitespace</remarks>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("BAM v2 Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Frame Count"));
            builder.Append(this.CountFrames);
            builder.Append(StringFormat.ToStringAlignment("Animation Count"));
            builder.Append(this.CountAnimations);
            builder.Append(StringFormat.ToStringAlignment("Data Block Count"));
            builder.Append(this.CountDataBlocks);
            builder.Append(StringFormat.ToStringAlignment("Frame Entries Offset"));
            builder.Append(this.OffsetFrameEntries);
            builder.Append(StringFormat.ToStringAlignment("Animation Entries Offset"));
            builder.Append(this.OffsetAnimationEntries);
            builder.Append(StringFormat.ToStringAlignment("Data Block Offset"));
            builder.Append(this.OffsetDataBlocks);

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
                if (obj != null && obj is BamHeader_v2)
                {
                    BamHeader_v2 compare = obj as BamHeader_v2;

                    Boolean structureEquality = (this.CountFrames == compare.CountFrames);
                    structureEquality &= (this.CountAnimations == compare.CountAnimations);
                    structureEquality &= (this.CountDataBlocks == compare.CountDataBlocks);

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
            hash ^= this.CountDataBlocks.GetHashCode();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}