using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Interface;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Version;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>Creates and returns an instance of an IBiff based on an input stream</summary>
    public static class BiffFactory
    {
        /// <summary>
        ///     Reads identifying information from the BIFF data stream and then creates and returns an IBiff
        ///     instance basedoff of its identifying data
        /// </summary>
        /// <param name="input">Input stream to peek at data from</param>
        /// <returns>An instance of the IBiff</returns>
        public static IBiff BuildBiff(Stream input)
        {
            IBiff instance = null;

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

                    //read the header
                    Byte[] peek = ReusableIO.BinaryRead(input, 8);
                    ReusableIO.SeekIfAble(input, position); //seek back to the initial location

                    String type = ReusableIO.ReadStringFromByteArray(peek, 0, CultureConstants.CultureCodeEnglish, 4);
                    String version = ReusableIO.ReadStringFromByteArray(peek, 4, CultureConstants.CultureCodeEnglish, 4);

                    if (type == "BIFF")
                    {
                        if (version == "V1  ")
                            instance = new Biff1Archive(true);
                        else
                            throw new InvalidDataException(String.Format("The first four bytes indicated an uncompressed BIFF, but second four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate a known uncompressed BIFF version (\"V1  \").", peek[4], peek[5], peek[6], peek[7]));
                    }
                    else if (type == "BIF ")
                    {
                        if (version == "V1.0")
                            instance = new CBiffArchive();
                        else
                            throw new InvalidDataException(String.Format("The first four bytes indicated an save-compressed BIFF, but second four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate a known save-compressed BIFF version (\"V1.0\").", peek[4], peek[5], peek[6], peek[7]));
                    }
                    else if (type == "BIFC")
                    {
                        if (version == "V1.0")
                            instance = new CBiffArchive();
                        else
                            throw new InvalidDataException(String.Format("The first four bytes indicated an zlib-compressed BIFF, but second four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate a known zlib-compressed BIFF version (\"V1.0\").", peek[4], peek[5], peek[6], peek[7]));
                    }
                    else
                        throw new InvalidDataException(String.Format("The first four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate a BIFF archive.", peek[0], peek[1], peek[2], peek[3]));

                    instance.Initialize();
                    instance.Read(input, true);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encountered an error while trying to read the BIFF archive from Stream.", ex);
            }

            return instance;
        }
    }
}