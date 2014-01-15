using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Bitmap;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>Creates and returns a device-independent bitmap instance based on an input stream</summary>
    public static class BitmapFactory
    {
        /// <summary>Builds a device-independent bitmap instance from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>The built DIB instance</returns>
        public static DeviceIndependentBitmap BuildBitmap(Stream input)
        {
            DeviceIndependentBitmap bmp = null;
            try
            {
                lock (input)
                {
                    //validation
                    if (!input.CanRead)
                        throw new InvalidOperationException("The input Stream cannot be read from.");
                    else if (!input.CanSeek)
                        throw new InvalidOperationException("The input Stream cannot seek, which is required.");

                    Int64 position = input.Position;
                    Int64 availableSize = input.Length - input.Position;
                    Int32 peekSize = 2;

                    if (availableSize < peekSize)  //magic number size
                        throw new InvalidDataException("The input Stream does not have sufficient data to read.");

                    Byte[] peek = ReusableIO.BinaryRead(input, peekSize);
                    ReusableIO.SeekIfAble(input, position);
                    String type = ReusableIO.ReadStringFromByteArray(peek, 0, CultureConstants.CultureCodeEnglish, peekSize);
                    if (type != "BM")
                        throw new InvalidDataException(String.Format("The first two bytes of the data ([{0}, {1}]) did not indicate a BMP file.", peek[0], peek[1]));

                    bmp = new DeviceIndependentBitmap();
                    bmp.Initialize();
                    bmp.Read(input);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encountered an error while trying to build a BMP instance from the input stream.", ex);
            }

            return bmp;
        }
    }
}