using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareAnimation;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>Creates and returns a BAM instance based on an input stream</summary>
    public static class BamFactory
    {
        /// <summary>Builds a 2DA from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>The built area</returns>
        public static Object BuildBam(Stream input)
        {
            Object bam = null;

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
                    Int32 peekLen = 8;

                    if (availableSize < peekLen)
                        throw new InvalidDataException("The input Stream does not have sufficient data to read.");

                    //read 8 bytes to get the type and version
                    Byte[] peek = ReusableIO.BinaryRead(input, peekLen);
                    ReusableIO.SeekIfAble(input, position);

                    String type = ReusableIO.ReadStringFromByteArray(peek, 0, CultureConstants.CultureCodeEnglish, 4);
                    String version = ReusableIO.ReadStringFromByteArray(peek, 4, CultureConstants.CultureCodeEnglish, 4);
                    
                    if (version != "V1  ")
                        throw new InvalidDataException(String.Format("The second four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate a known BAM version (\"V1  \").", peek[4], peek[5], peek[6], peek[7]));

                    if (type == "BAM ")
                    {
                        BioWareAnimation1 bam1 = new BioWareAnimation1();
                        bam1.Initialize();
                        bam1.Read(input, true);
                        bam = bam1;
                    }
                    else if (type == "BAMC")
                    {
                        BioWareAnimationCompressed_v1 bamc = new BioWareAnimationCompressed_v1();
                        bamc.Initialize();
                        bamc.Read(input, true);
                        bam = bamc;
                    }
                    else
                        throw new InvalidDataException(String.Format("The first four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate a BAM.", peek[0], peek[1], peek[2], peek[3]));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encountered an error while trying to build a BAM instance from the input stream.", ex);
            }

            return bam;
        }
    }
}