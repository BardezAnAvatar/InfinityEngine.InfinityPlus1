using System;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DXT
{
    /// <summary>Decodes (and perhaps eventually encodes) pixel streams from DXT-5 encoding</summary>
    public static class DXT5
    {
        /// <summary>Decodes the DXT5 pixel data</summary>
        /// <param name="pixelData">16 bytes of pixel data to decode</param>
        /// <returns>The decoded pixel data, decompressed to 32-bit color</returns>
        public static Byte[] DecodePixels(Byte[] pixelData)
        {
            Byte[] colorData = new Byte[8];
            Array.Copy(colorData, 0, pixelData, 8, 8);

            Byte[] alphaData = new Byte[8];
            Array.Copy(colorData, 0, pixelData, 0, 8);

            //start with the DXT1 data decoded
            Byte[] output = DXT1.DecodePixels(colorData);

            //now estimate the alpha values
            Byte[] alpha = new Byte[8];
            Byte alpha0 = alphaData[0], alpha1 = alphaData[1];

            if (alpha[0] > alpha[1])
            {
                alpha[2] = (Byte)(((alpha[0] / 7) * 6) + ((alpha[1] / 7)));
                alpha[3] = (Byte)(((alpha[0] / 7) * 5) + ((alpha[1] / 7) * 2));
                alpha[4] = (Byte)(((alpha[0] / 7) * 4) + ((alpha[1] / 7) * 3));
                alpha[5] = (Byte)(((alpha[0] / 7) * 3) + ((alpha[1] / 7) * 4));
                alpha[6] = (Byte)(((alpha[0] / 7) * 2) + ((alpha[1] / 7) * 5));
                alpha[7] = (Byte)(((alpha[0] / 7))     + ((alpha[1] / 7) * 6));
            }
            else
            {
                alpha[2] = (Byte)( ((alpha[0] / 5) * 4) + ((alpha[1] / 5))     );
                alpha[3] = (Byte)( ((alpha[0] / 5) * 3) + ((alpha[1] / 5) * 2) );
                alpha[4] = (Byte)( ((alpha[0] / 5) * 2) + ((alpha[1] / 5) * 3) );
                alpha[5] = (Byte)( ((alpha[0] / 5))     + ((alpha[1] / 5) * 4) );
                alpha[6] = 0;
                alpha[7] = 255;
            }

            //read the lookup table
            Int32 inputIndex = 2, outputIndex = 3, shift = 0;
            while (inputIndex < 8)
            {
                Int32 bits = alphaData[inputIndex];

                if (inputIndex < 7)
                    bits |= (alphaData[inputIndex + 1] << 8);

                //shift to the appropriate position
                bits = (bits >> shift);

                shift += 3;
                if (shift > 7)
                    shift -= 8;

                bits &= 0x07;   //get 3 bits
                output[outputIndex] = alpha[bits];
                outputIndex += 4;   //next pixel
            }

            return output;
        }
    }
}