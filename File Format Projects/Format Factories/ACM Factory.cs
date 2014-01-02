using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>Creates and returns an ACM instance based on an input stream</summary>
    public static class AcmFactory
    {
        /// <summary>Builds an ACM instance from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>The built ACM instance</returns>
        public static AcmAudioFile BuildAcm(Stream input)
        {
            AcmAudioFile acm = null;

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
                    Int32 peekSize = 4;

                    if (availableSize < peekSize)  //magic number size
                        throw new InvalidDataException("The input Stream does not have sufficient data to read.");

                    UInt32 id = ReusableIO.ReadUInt32FromStream(input);
                    ReusableIO.SeekIfAble(input, position);
                    if (id != AcmHeader.MagicNumber)
                        throw new InvalidDataException(String.Format("The first four bytes of the decrypted data (0x{0:X8}) did not indicate an ACM file.", id));

                    acm = new AcmAudioFile();
                    acm.Read(input, true);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encountered an error while trying to build an ACM instance from the input stream.", ex);
            }

            return acm;
        }
    }
}