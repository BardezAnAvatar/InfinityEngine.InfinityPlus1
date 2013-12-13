using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation.Component
{
    /// <summary>Represents a BAM palette entry</summary>
    /// <remarks>
    ///     BAM's transparency is not quite straightforward, so I am inheriting to add
    ///     a property for the alpha value.
    /// </remarks>
    public class BamPaletteEntry : PaletteEntry
    {
        #region Properties
        /// <summary>Exposes the first index in the palette of RGB(0, 255, 0)</summary>
        /// <remarks>
        /// Per IESDP: The transparency index is set to the first occurence of RGB(0,255,0). If RGB(0,255,0) does not exist in the palette then transparency index is set to 0
        /// </remarks>
        protected Int32 FirstTransparentGreenIndex
        {
            get
            {
                Int32 index = -1;
                for (Int32 i = 0; i < this.Colors.Length; ++i)
                {
                    ColorEntry color = this.Colors[i];
                    //trying to optimize the short-circuit logic with green, red, blue
                    if (color.Green == 255 && color.Red == 0 & color.Blue == 0)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
        }

        /// <summary>Exposes the transparency index of the palette</summary>
        public Int32 TransparencyIndex
        {
            get { return this.FirstTransparentGreenIndex > -1 ? this.FirstTransparentGreenIndex : 0; }
        }

        /// <summary>Exposure of the Colors array with alpha transparency.</summary>
        /// <remarks>This is built dynamically, and if going to be referenced rapidly, should be stored temporarily.</remarks>
        public TransparencyColorEntry[] ColorsAlpha
        {
            get
            {
                Int32 transparent = this.TransparencyIndex;
                TransparencyColorEntry[] retArr = new TransparencyColorEntry[base.Colors.Length];   //256?
                for (Int32 i = 0; i < retArr.Length; ++i)
                    retArr[i] = new TransparencyColorEntry(base.Colors[i], i == transparent ? Byte.MinValue : byte.MaxValue);

                return retArr;
            }
            set { base.Colors = value; }
        }
        #endregion
    }
}