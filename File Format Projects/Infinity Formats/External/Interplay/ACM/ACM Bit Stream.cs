using System;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM
{
    /// <summary>ACM implementation of a Bitstream. Will allow for reading past the end of file, returning 0s</summary>
    public class AcmBitStream : BitStream
    {
        /// <summary>Default constructor</summary>
        public AcmBitStream(Byte[] dataSource)
        {
            this.Data = dataSource;
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A UInt64 containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>
        ///     Internally, uses UInt64 due to CLR restrictions on the shift operators.
        ///     Protected, as the return value is intended to be casted between various types.
        ///     Does no error checking, as this method is meant to be wrapped by others.
        /// </remarks>
        protected override UInt64 GetBits(Int32 bitCount)
        {
            UInt64 value = 0UL; //in case there are no bits left

            if (this.BitsRemaining > 0L)
                value = base.GetBits(this.BitsRemaining < bitCount ? Convert.ToInt32(this.BitsRemaining) : bitCount);

            return value;
        }
    }
}