using System;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DXT
{
    /// <summary>Decodes (and perhaps eventually encodes) pixel streams from DXT-1 encoding</summary>
    public static class DXT1
    {
        /// <summary>Decodes the DXT1 pixel data</summary>
        /// <param name="pixelData">8 bytes of pixel data to decode</param>
        /// <returns>The decoded pixel data, decompressed to 32-bit color</returns>
        public static Byte[] DecodePixels(Byte[] pixelData)
        {
            UInt16 color0 = ReusableIO.ReadUInt16FromArray(pixelData, 0);
            UInt16 color1 = ReusableIO.ReadUInt16FromArray(pixelData, 2);

            RgbQuad c0 = DXT1.Get32bitColor(color0), c1 = DXT1.Get32bitColor(color1);
            RgbQuad c2 = null, c3 = null;

            /*
             *  Colors are now multiplied and divided. To get a proper value, the components need to be split out and multiplied by channel, then recomposited. *
             *  i.e.: RGB 74,56,33 * 2/3 => RGB 49,37,22, composited back into a color. It APPEARS that the proper color should be fully expanded to 32 bit     *
             *  to get the intermediate value.                                                                                                                  *
             *  Additionally, it appears that division should occur prior to multiplication. Thanks to Charles Miller, Jr. for spot checking the math for me.   *
             */

            if (color0 > color1)
            {
                c2 = new RgbQuad( ((c0.Red / 3) * 2) + ((c1.Red / 3)),     ((c0.Green / 3) * 2) + ((c1.Green / 3)),     ((c0.Blue / 3) * 2) + ((c1.Blue / 3)),     255);   // 2/3 color0 + 1/3 color1
                c3 = new RgbQuad( ((c0.Red / 3))     + ((c1.Red / 3) * 2), ((c0.Green / 3))     + ((c1.Green / 3) * 2), ((c0.Blue / 3))     + ((c1.Blue / 3) * 2), 255);   // 1/3 color0 + 2/3 color1
            }
            else
            {
                c2 = new RgbQuad( (c0.Red / 2) + (c1.Red / 2),  (c0.Green / 2) + (c1.Green / 2),    (c0.Blue / 2) + (c1.Blue / 2),  255);// 1/2 color0 + 1/2 color1
                c3 = new RgbQuad(0, 0, 0, 0);   //black, premultiplied alpha
            }

            Byte[] pixels = new Byte[64];   //16 pixels * 4 bytes = 64 pxielbytes :)

            //read the lookup table
            Int32 inputIndex = 4, outputIndex = 0;
            while (inputIndex < 8)
            {
                DXT1.PopulateDataArray(DXT1.PickColor(c0, c1, c2, c3, ( pixelData[inputIndex]      & 0x03)), pixels, ref outputIndex);
                DXT1.PopulateDataArray(DXT1.PickColor(c0, c1, c2, c3, ((pixelData[inputIndex]>> 2) & 0x03)), pixels, ref outputIndex);
                DXT1.PopulateDataArray(DXT1.PickColor(c0, c1, c2, c3, ((pixelData[inputIndex]>> 4) & 0x03)), pixels, ref outputIndex);
                DXT1.PopulateDataArray(DXT1.PickColor(c0, c1, c2, c3, ((pixelData[inputIndex]>> 6) & 0x03)), pixels, ref outputIndex);

                ++inputIndex;
            }

            return pixels;
        }

        /// <summary>Picks the decoded output color</summary>
        /// <param name="color0">First possible color</param>
        /// <param name="color1">Second possible color</param>
        /// <param name="color2">Third possible color</param>
        /// <param name="color3">Fourth possible color</param>
        /// <param name="value">Value to decode</param>
        /// <returns>The picked color</returns>
        private static RgbQuad PickColor(RgbQuad color0, RgbQuad color1, RgbQuad color2, RgbQuad color3, Int32 value)
        {
            if (value == 0)
                return color0;
            else if (value == 1)
                return color1;
            else if (value == 2)
                return color2;
            else if (value == 3)
                return color3;
            else
                throw new InvalidOperationException("The value exceeded the acceptible decoding size.");
        }

        /// <summary>Converts a 16-bit color to a 32-bit color</summary>
        /// <param name="color16">16-bit color to convert</param>
        /// <returns>An RGB quad value representing the expanded color</returns>
        private static RgbQuad Get32bitColor(UInt16 color16)
        {
            RgbQuad color32 = null;

            Int32 red = (color16 >> 11) & 0x1F;
            Int32 green = (color16 >> 5) & 0x3F;
            Int32 blue = color16 & 0x1F;

            red = (red << 3) | (red >> 2);
            green = (green << 2) | (green >> 4);
            blue = (blue << 3) | (blue >> 2);

            color32 = new RgbQuad(red, green, blue, 255);

            return color32;
        }

        /// <summary>Populates the data array with the color in the specified index</summary>
        /// <param name="color">Color to populate</param>
        /// <param name="data">Data to populate</param>
        /// <param name="index">Index at which to write to the array</param>
        private static void PopulateDataArray(RgbQuad color, Byte[] data, ref Int32 index)
        {
            data[index++] = color.Blue;
            data[index++] = color.Green;
            data[index++] = color.Red;
            data[index++] = color.Alpha;
        }
    }
}