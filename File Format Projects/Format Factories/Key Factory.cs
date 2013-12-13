using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.KeyTable;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Logic.Infinity.Factories
{
    /// <summary>Creates and returns a KeyTable instance based on an input stream</summary>
    public static class KeyFactory
    {
        /// <summary>
        ///     Reads identifying information from the BIFF data stream and then creates and returns an IBiff
        ///     instance basedoff of its identifying data
        /// </summary>
        /// <param name="input">Input stream to peek at data from</param>
        /// <returns>An instance of the IBiff</returns>
        public static KeyTable BuildKey(Stream input)
        {
            KeyTable key = null;

            //since the entire point of this is to peek to see which version to use, I need to be able to backtrack
            Int64 initialPosition = input.Position;

            try
            {
                //read the header
                Byte[] header = ReusableIO.BinaryRead(input, 20);
                String type = ReusableIO.ReadStringFromByteArray(header, 0, "en-US", 4);
                String version = ReusableIO.ReadStringFromByteArray(header, 4, "en-US", 4);
                UInt32 offset = ReusableIO.ReadUInt32FromArray(header, 16);

                //seek back to the initial location
                ReusableIO.SeekIfAble(input, initialPosition);

                //general validation
                if (type != "KEY " || version == "V1  ")    //This is the test case for both Neverwinter Nights and IE.
                    throw new InvalidOperationException(String.Format("The stream had a type of \"{0}\" and a version of \"{1}\", while expecting a type of \"KEY \" and a version of \"V1  \".", type, version));

                //NWN has additional fields that do not exist in IE; the version and type are the same, so I have to guess a bit.
                if (offset == 64)   //specific case for NWN
                    throw new NotImplementedException("I do not yet have NWN key support written.");
                else //I believe I can assume IE
                {
                    key = new KeyTable(true);
                    key.Read(input, true);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encountered an error while trying to read the KEY file from Stream.", ex);
            }

            return key;
        }
    }
}