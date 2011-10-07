using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.TwoDimensionalArray
{
    public static class InfinityXorEncryption
    {
        /// <summary>64-Byte (512-bit) encryption key used for encoding text resources.</summary>
        public static readonly Byte[] Key = new Byte[]
        {
            0x88, 0xA8, 0x8F, 0xBA, 0x8A, 0xD3, 0xB9, 0xF5,
            0xED, 0xB1, 0xCF, 0xEA, 0xAA, 0xE4, 0xB5, 0xFB,
            0xEB, 0x82, 0xF9, 0x90, 0xCA, 0xC9, 0xB5, 0xE7,
            0xDC, 0x8E, 0xB7, 0xAC, 0xEE, 0xF7, 0xE0, 0xCA,
            0x8E, 0xEA, 0xCA, 0x80, 0xCE, 0xC5, 0xAD, 0xB7,
            0xC4, 0xD0, 0x84, 0x93, 0xD5, 0xF0, 0xEB, 0xC8,
            0xB4, 0x9D, 0xCC, 0xAF, 0xA5, 0x95, 0xBA, 0x99,
            0x87, 0xD2, 0x9D, 0xE3, 0x91, 0xBA, 0x90, 0xCA
        };

        public static Byte[] Decrypt(Byte[] input)
        {
            Byte[] decrypted;

            //return the original data if it could not be encrypted
            if (input.Length < 2 || (input[0] != 0 && input[1] != 0))  //if first two bytes are not 0 or it is less than 2 bytes long
                decrypted = input;
            else
            {
                //instantiate new byte array to write to
                decrypted = new Byte[input.Length - 2];

                Int32 keyIndex = 0;
                for (Int32 index = 2; index < input.Length; ++index)
                {
                    keyIndex %= Key.Length;    //wrap the index back to 0
                    decrypted[index - 2] = (Byte)(input[index] ^ Key[keyIndex]);
                    ++keyIndex;
                }
            }

            return decrypted;
        }

        /// <summary>
        ///     Reads the stream to a binary array and either decrypts it or passes it through,
        ///     returning it as a MemoryStream.
        /// </summary>
        /// <param name="input">Stream to read from.</param>
        /// <returns>A MemoryStream containing the decrypted (or passed through) binary data</returns>
        public static MemoryStream Decrypt(Stream input)
        {
            //I need to peek at the leading two bytes to see if it is encrypted. However, it could be a filestream, memory, whatever...
            Byte[] inputRead = ReusableIO.BinaryRead(input, input.Length);

            Byte[] decrypted;

            //return the original data if it could not be encrypted
            if (inputRead.Length < 2 || (inputRead[0] != 0xFF && inputRead[1] != 0xFF))  //if first two bytes are not 0xFF or it is less than 2 bytes long
                decrypted = inputRead;
            else
            {
                //instantiate new byte array to write to
                decrypted = new Byte[inputRead.Length - 2];

                Int32 keyIndex = 0;
                for (Int32 index = 2; index < input.Length; ++index)
                {
                    keyIndex %= Key.Length;    //wrap the index back to 0
                    decrypted[index - 2] = (Byte)(inputRead[index] ^ Key[keyIndex]);
                    ++keyIndex;
                }
            }

            return new MemoryStream(decrypted);
        }
    }
}