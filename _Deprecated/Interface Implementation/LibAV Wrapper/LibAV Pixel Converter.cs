using System;
using System.IO;

using Bardez.Projects.MultiMedia.LibAV;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.MultiMedia.MediaBase.Video.Pixels;

namespace Bardez.Projects.Multimedia.LibAV.Wrapper
{
    /// <summary>Pixel converter that utilizes SWScale from LibAV</summary>
    public class LibAVPixelConverter : IPixelConverter
    {
        /// <summary>Converts data from the current format to another specified format</summary>
        /// <param name="data">Decompressed data to convert</param>
        /// <param name="sourceFormat">Format to convert from</param>
        /// <param name="destinationFormat">Format to convert to</param>
        /// <param name="horizontalPacking">Row packing</param>
        /// <param name="verticalPacking">Row count to align to</param>
        /// <param name="sourceWidth">Indicates the source width of the image data</param>
        /// <param name="sourceHeight">Indicates the source height of the image data</param>
        /// <param name="decodedBitDepth">The bits per pixel once decoded</param>
        /// <returns>New byte data</returns>
        public MemoryStream ConvertData(MemoryStream data, PixelFormat sourceFormat, PixelFormat destinationFormat, Int32 horizontalPacking, Int32 verticalPacking, Int32 sourceWidth, Int32 sourceHeight, Int32 decodedBitDepth)
        {
            MemoryStream result = null;

            //translate pixel formats
            LibAVPixelFormat sourceFormatLibAV = sourceFormat.ToLibAVPixelFormat();
            LibAVPixelFormat destFormatLibAV = destinationFormat.ToLibAVPixelFormat();

            using (SWScale scale = new SWScale(
                        LibAVPictureDetail.Build(sourceWidth, sourceHeight, sourceFormatLibAV),
                        LibAVPictureDetail.Build(sourceWidth, sourceHeight, destFormatLibAV),
                        ResizeOptions.Lanczos))
            {
                MemoryStream outStream = null;
                scale.Transform(data, out outStream);
                result = outStream;
            }

            return result;
        }
    }
}