using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Interface;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Version;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Factories
{
    /// <summary>Creates and returns an instance of an IBiff based on an input stream</summary>
    public static class BiffFactory
    {
        /// <summary>
        ///     Reads identifying information from the BIFF data stream and then creates and returns an IBiff
        ///     instance basedoff of its identifying data
        /// </summary>
        /// <param name="input">Inptu stream to peek at data from</param>
        /// <returns>An instance of the IBiff</returns>
        public static IBiff GetBiff(Stream input)
        {
            IBiff instance = null;
            Int64 initialPosition = input.Position;

            if ((input.Length - input.Position) > 7)
            {
                //read the header
                Byte[] header = ReusableIO.BinaryRead(input, 8);
                String type = ReusableIO.ReadStringFromByteArray(header, 0, "en-US", 4);
                String version = ReusableIO.ReadStringFromByteArray(header, 4, "en-US", 4);

                //seek back to the initial location
                ReusableIO.SeekIfAble(input, initialPosition);

                if (type == "BIFF" && version == "V1  ")        //uncompressed
                    instance = new Biff1Archive(true);
                else if (type == "BIF " && version == "V1.0")   //sav-compressed
                    instance = new CBiffArchive();
                else if (type == "BIFC" && version == "V1.0")   //zlib-compressed
                    throw new NotImplementedException("BifC s not yet implemented");

                instance.Read(input);
            }

            return instance;
        }
    }
}