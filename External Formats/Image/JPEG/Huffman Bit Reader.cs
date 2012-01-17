using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a Bit Stream to </summary>
    public class HuffmanBitReader
    {
        #region Fields
        /// <summary>Represents the source Stream to read from</summary>
        protected Stream source;

        /// <summary>Represents the number of bits consumed</summary>
        protected Int32 bitBytePosition;

        /// <summary>Byte buffer</summary>
        protected Byte buffer;
        #endregion


        #region Properties
        /// <summary></summary>
        protected Int32 ByteBitsAvailable
        {
            get { return 8 - this.bitBytePosition; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public HuffmanBitReader()
        {
            this.buffer = 0;
            this.bitBytePosition = 0;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="source">Source stream to consume</param>
        public HuffmanBitReader(Stream source) : this()
        {
            this.source = source;
        }
        #endregion


        #region Methods
        /// <summary>Gets an unsigned short (UInt16) of bits from the Huffman stream</summary>
        /// <param name="bitCount">Count of bits to read, less than 17</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is to be discarded,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>up to 16 bits, shifted to the least significant bit as the least significant bit in the return value</returns>
        public virtual UInt16 GetBits(Int32 bitCount, ref Boolean halt)
        {
            Int32 remainingBits = bitCount;
            UInt16 value = 0;

            do
            {
                if (this.bitBytePosition == 0)  //bit position is 0, need a new byte
                {
                    Int32 byteTemp = this.source.ReadByte();

                    if (byteTemp == -1)
                        throw new EndOfStreamException(String.Format("Could not read additional byte. Position: {0}", this.source.Position));

                    //FF becomes FF00, so consume 00
                    if (byteTemp == 0xFF)
                    {
                        byteTemp = this.source.ReadByte();

                        if (byteTemp == -1)
                            throw new EndOfStreamException(String.Format("Could not read additional byte. Position: {0}", this.source.Position));
                        // This reader read a Byte, but the byte was not an escape of 0x00; it was a legitimate marker.
                        else if (byteTemp != 0)
                        {
                            // I part of me wants to use an exception, but it's dirty to rely on, and it's not a problem, per se. Also, architecturally, it would have to be caught,
                            // rethrown after a return and just make things even dirtier. So, return a 0xFFFF and declare it as discardable in the API.
                            ReusableIO.SeekIfAble(this.source, -2, SeekOrigin.Current);    //back up two bytes so that the marker is going to be the next two bytes.

                            //I dislike multiple return points, but this one is as close to the exception model as I'm comfortable with,
                            //and less imposing on the rest of the code; lesser of two evils, I guess.
                            return UInt16.MaxValue;
                        }

                        byteTemp = 0xFF;
                    }
                    this.buffer = (Byte)byteTemp;
                }

                //read bits in byte.
                Int32 bitsToRead = remainingBits < this.ByteBitsAvailable ? remainingBits : this.ByteBitsAvailable;
                Int32 shift = 8 + this.bitBytePosition;

                //isolate current bits remaining
                UInt16 tempValue = this.buffer;
                tempValue <<= shift;
                tempValue >>= (16 - bitsToRead);

                //shift the value read so far to the right.
                value <<= bitsToRead;

                //OR the shift with the bits read so far
                value |= tempValue;

                this.bitBytePosition += bitsToRead;     //increment the internal bit pointer.
                remainingBits -= bitsToRead;            //bits left to read goes down

                //reset the Byte bit position
                if (this.bitBytePosition > 7)
                    this.bitBytePosition = 0;
            }
            while (remainingBits > 0);

            return value;
        }

        /// <summary>Resets the bitstream's buffer to having no bits read.</summary>
        /// <remarks>Used for when a seek has been performed outside the bitstream and reading resumes at a new binary index</remarks>
        public virtual void ResetBuffer()
        {
            this.bitBytePosition = 0;
        }
        #endregion
    }
}