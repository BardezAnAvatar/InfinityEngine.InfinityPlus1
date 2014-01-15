using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TwoDimensionalArray._2DA1;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>Creates and returns an Area instance based on an input stream</summary>
    public static class TwoDimensionalArrayFactory
    {
        /// <summary>Builds a 2DA from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>The built area</returns>
        public static TwoDimensionalArray1 Build2DA(Stream input)
        {
            TwoDimensionalArray1 _2da = null;

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
                    Int32 peekLen = 2;

                    if (availableSize < peekLen)
                        throw new InvalidDataException("The input Stream does not have sufficient data to read.");

                    //read 2 bytes to check the encryption
                    Byte[] peek = ReusableIO.BinaryRead(input, peekLen);
                    ReusableIO.SeekIfAble(input, position);

                    if (peek[0] == Byte.MaxValue && peek[1] == Byte.MaxValue)
                        peekLen = 10;
                    else
                        peekLen = 8;

                    if (availableSize < peekLen)
                        throw new InvalidDataException("The input Stream does not have sufficient data to read.");
                    
                    //read the header
                    peek = ReusableIO.BinaryRead(input, peekLen);
                    ReusableIO.SeekIfAble(input, position);     //seek back to the beginning
                    peek = InfinityXorEncryption.Decrypt(peek); //decrypt

                    //check the version
                    String type = ReusableIO.ReadStringFromByteArray(peek, 0, CultureConstants.CultureCodeEnglish, 4);
                    String version = ReusableIO.ReadStringFromByteArray(peek, 4, CultureConstants.CultureCodeEnglish, 4);

                    if (type != "2DA ")
                        throw new InvalidDataException(String.Format("The first four bytes of the decrypted data ([{0}, {1}, {2}, {3}]) did not indicate a 2DA file.", peek[0], peek[1], peek[2], peek[3]));
                    else if (version != "V1.0")
                        throw new InvalidDataException(String.Format("The second four bytes of the decrypted data ([{0}, {1}, {2}, {3}]) did not indicate a known 2DA version (V1.0).", peek[4], peek[5], peek[6], peek[7]));

                    //perform the read
                    _2da = new TwoDimensionalArray1();
                    _2da.Initialize();
                    _2da.Read(input, true);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encountered an error while trying to build a 2DA instance from the input stream.", ex);
            }

            return _2da;
        }
    }
}