using System;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM
{
    /// <summary>Declaration of a delegate type for the filler functions associated with ACM file format</summary>
    /// <param name="block">Block to fill</param>
    /// <param name="bits">Number of bits to consume for the filler function</param>
    /// <param name="column">Column of the PackedBlock to affect.</param>
    internal delegate void PackedBlockFiller(PackedBlock block, Int32 bits, Int32 column);

    /// <summary>Class representing a BitBlock of Amplitude-encodeded modulation file.</summary>
    /// <remarks>This class should be used for nearly single operation, and as such is implmented as a Singleton (therefore sealed)</remarks>
    /// <example>
    ///     BitBlock block = BitBlock.GetInstance();
    ///     block.SetReferences(Header header, BitStream stream);
    ///     this.PackedBlocks.Add(block.Unpack());
    ///     this.PackedBlocks.Add(block.Unpack());
    ///     this.PackedBlocks.Add(block.Unpack());
    ///     ...
    /// </example>
    public sealed class BitBlock
    {
        #region Fields
        /// <summary>The collction of packed block filler functions.</summary>
        internal readonly PackedBlockFiller[] fillers;

        /// <summary>Since this is single instance, used for locking on public methods</summary>
        internal static Object lockable = new Object();

        /// <summary>Singleton instance</summary>
        internal static BitBlock singleton;

        /// <summary>Amplitude buffer to be created and populated</summary>
        private AmplitudeBuffer ampBuffer;
        
        /// <summary>Decoding buffer for all packed blocks. Shared when processing a single file</summary>
        /// <remarks>
        ///     A new PackedBlock gets values still in buffer from previous block. Also, larger than documentation:
        ///     
        ///     It appears to be the decBuff from documentation, with size of 2 * columns - 2
        ///     compare with 3 * (columns/2) - 2 (documentation)
        ///     colums:2  doc: 1 berlios:2
        ///     colums:8  doc: 10 berlios:14
        ///     colums:32 doc: 46 berlios:62
        ///
        ///     ...berlios comes out larger
        /// </remarks>
        private Int32[] decodeBuffer;

        #endregion

        #region Properties
        public AcmHeader AcmHeader { get; set; }

        /// <summary>BitStream to populate </summary>
        public BitStream BitDataStream { get; set; }

        /// <summary>Accessor to singleton BitBlock instance</summary>
        public static BitBlock Instance
        {
            get
            {
                lock (BitBlock.lockable)
                    if (singleton == null)
                        BitBlock.singleton = new BitBlock();

                return BitBlock.singleton;
            }
        }

        #endregion
        
        #region Construction
        /// <summary>Default constructor</summary>
        private BitBlock()
        {
            this.fillers = new PackedBlockFiller[32]
            {
                this.FillerColumnZero, this.FillerDiscard, this.FillerDiscard,
                this.FillerColumnLinear, this.FillerColumnLinear, this.FillerColumnLinear,
                this.FillerColumnLinear, this.FillerColumnLinear, this.FillerColumnLinear,
                this.FillerColumnLinear, this.FillerColumnLinear, this.FillerColumnLinear,
                this.FillerColumnLinear, this.FillerColumnLinear, this.FillerColumnLinear,
                this.FillerColumnLinear, this.FillerColumnLinear,
                this.FillerColumnAmplitude1Bits3, this.FillerColumnAmplitude1Bits2, this.FillerColumnBase3,
                this.FillerColumnAmplitude2Bits4, this.FillerColumnAmplitude2Bits3, this.FillerColumnBase5,
                this.FillerColumnAmplitude3Bits5, this.FillerColumnAmplitude3Bits4, this.FillerDiscard,
                this.FillerColumnAmplitude4Bits5, this.FillerColumnAmplitude4Bits4, this.FillerDiscard,
                this.FillerColumnBase11, this.FillerDiscard, this.FillerDiscard
            };
        }
        #endregion

        /// <summary>Decodes a bitblock from the bit data stream</summary>
        /// <returns>A new packed block for usage</returns>
        public PackedBlock Decode()
        {
            lock (BitBlock.lockable)
            {
                PackedBlock block = new PackedBlock(this.AcmHeader.PackingColumns, this.AcmHeader.PackingRows, this.AcmHeader.PackingLevels);

                //read leading 20 bytes
                Int16 power = this.BitDataStream.ReadInt16(4);
                Int16 value = this.BitDataStream.ReadInt16(16);

                this.ampBuffer = new AmplitudeBuffer(power, value);

                for (Int32 column = 0; column < this.AcmHeader.PackingColumns; ++column)
                {
                    Int32 functionIndex = this.BitDataStream.ReadInt16(5);
                    this.fillers[functionIndex](block, functionIndex, column);
                }

                //juggle as is necessary; the determinate code is inside the method, so just call directly
                this.JuggleBlock(block);

                return block;
            }
        }

        #region Padded Block filler methods
        /// <summary>Fills the packed block with 0s</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        private void FillerColumnZero(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
                block[column, i] = 0;
        }

        /// <summary>Breaks the filling of the current packed block and discards its contents.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        private void FillerDiscard(PackedBlock block, Int32 bits, Int32 column)
        {
            throw new AcmReturnException("Discode operator found");
        }

        /// <summary>Breaks the filling of the current packed block and discards its contents.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        private void FillerColumnLinear(PackedBlock block, Int32 bits, Int32 column)
        {
            //value will be read, but needs to be treated as being signed. So, do some 2's compliment math.
            Int32 signedAdjustment = 1 << (bits - 1);

            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = BitDataStream.ReadInt32(bits);
                block[column, i] = this.ampBuffer[value - signedAdjustment];
            }
        }

        /// <summary>Fills up to two rows by reading up to three bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k13</remarks>
        /// <value>
        ///     Uses variable count of bits (up to 3) from BitBlock to fill the column:
        ///
        ///     bit-sequence | action
        ///     (in order of |
        ///      appearance) |
        ///    --------------+-------------
        ///      0           | PB[i][n] = 0; PB[++i][n] = 0
        ///      1, 0        | PB[i][n] = 0
        ///      1, 1, 0     | PB[i][n] = Buff_Middle [-1]
        ///      1, 1, 1     | PB[i][n] = Buff_Middle [+1]
        /// </value>
        private void FillerColumnAmplitude1Bits3(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(1, 3);
                switch (value)
                {
                    case 0: //dual zero
                        block[column, i++] = 0;
                        if (i < block.Rows)
                            block[column, i] = 0;
                        break;
                    case 1: //single zero
                        block[column, i] = 0;
                        break;
                    case 3: //middle - 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 7: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }

        /// <summary>Fills one row by reading up to two bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k12</remarks>
        /// <value>
        ///     Up to 2 bits:
        ///
        ///     bit-seq. | value
        ///    ----------+-----------
        ///      0       | 0
        ///      1, 0    | Buff_Middle [-1]
        ///      1, 1    | Buff_Middle [+1]
        /// </value>
        private void FillerColumnAmplitude1Bits2(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(1, 2);
                switch (value)
                {
                    case 0: //single zero
                        block[column, i] = 0;
                        break;
                    case 1: //middle - 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 3: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }

        /// <summary>Fills up to two rows by reading up to four bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k24</remarks>
        /// <value>
        ///     Up to 4 bits:
        ///
        ///     bit-seq. | value(s)
        ///    ----------+-----------
        ///      0       | 0, 0
        ///      1,0     | 0
        ///      1,1,0,0 | Buff_Middle [-2]
        ///      1,1,1,0 | Buff_Middle [-1]
        ///      1,1,0,1 | Buff_Middle [+1]
        ///      1,1,1,1 | Buff_Middle [+2]
        /// </value>
        private void FillerColumnAmplitude2Bits4(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(2, 4);
                switch (value)
                {
                    case 0: //dual zero
                        block[column, i++] = 0;
                        if (i < block.Rows)
                            block[column, i] = 0;
                        break;
                    case 1: //single zero
                        block[column, i] = 0;
                        break;
                    case 3: //middle - 2
                        block[column, i] = this.ampBuffer[-2];
                        break;
                    case 7: //middle - 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 11: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    case 15: //middle + 2
                        block[column, i] = this.ampBuffer[2];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }

        /// <summary>Fills up to one row by reading up to three bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k24</remarks>
        /// <value>
        ///     Up to 3 bits:
        ///
        ///     bit-seq. | value(s)
        ///    ----------+-----------
        ///      0       | 0
        ///      1,0,0   | Buff_Middle [-2]
        ///      1,1,0   | Buff_Middle [-1]
        ///      1,0,1   | Buff_Middle [+1]
        ///      1,1,1   | Buff_Middle [+2]
        /// </value>
        private void FillerColumnAmplitude2Bits3(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(2, 3);
                switch (value)
                {
                    case 0: //single zero
                        block[column, i] = 0;
                        break;
                    case 1: //middle - 2
                        block[column, i] = this.ampBuffer[-2];
                        break;
                    case 3: //middle - 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 5: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    case 7: //middle + 2
                        block[column, i] = this.ampBuffer[2];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }
        
        /// <summary>Fills two rows by reading up to five bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k35</remarks>
        /// <value>
        ///     Up to 5 bits: 
        ///
        ///     bit-seq.     | value(s)            /---->  2bits | B_M index
        ///    --------------+-----------          |      -------+-----------
        ///     0            | 0, 0                |        0,0  | -3
        ///     1,0          | 0                   |        1,0  | -2
        ///     1,1,0,0      | Buff_Middle [-1]    |        0,1  | +2
        ///     1,1,0,1      | Buff_Middle [+1]    |        1,1  | +3
        ///     1,1,1, 2bits |    (*) -------------/        /// </value>
        private void FillerColumnAmplitude3Bits5(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(3, 5);
                switch (value)
                {
                    case 0: //dual zero
                        block[column, i++] = 0;
                        if (i < block.Rows)
                            block[column, i] = 0;
                        break;
                    case 1: //single zero
                        block[column, i] = 0;
                        break;
                    case 3: //middle - 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 11: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    case 7: //middle -3
                        block[column, i] = this.ampBuffer[-3];
                        break;
                    case 15: //middle - 2
                        block[column, i] = this.ampBuffer[-2];
                        break;
                    case 23: //middle + 2
                        block[column, i] = this.ampBuffer[2];
                        break;
                    case 31: //middle + 3
                        block[column, i] = this.ampBuffer[3];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }
        
        /// <summary>Fills one row by reading up to four bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k34</remarks>
        /// <value>
        ///      Up to 4 bits: 
        ///
        ///      bit-seq.     | value(s)            /---->  2bits | B_M index
        ///     --------------+-----------          |      -------+-----------
        ///      0            | 0                   |        0,0  | -3
        ///      1,0,0        | Buff_Middle [-1]    |        1,0  | -2
        ///      1,0,1        | Buff_Middle [+1]    |        0,1  | +2
        ///      1,1, 2bits   |    (*) -------------/        1,1  | +3
        ///      
        /// Note: pretty sure 1,0,1 (5) is a typo. Both 1 and 5 used to be -1
        /// </value>
        private void FillerColumnAmplitude3Bits4(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(3, 4);
                switch (value)
                {
                    case 0: //single zero
                        block[column, i] = 0;
                        break;
                    case 1: //middle - 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 5: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    case 3: //middle -3
                        block[column, i] = this.ampBuffer[-3];
                        break;
                    case 7: //middle - 2
                        block[column, i] = this.ampBuffer[-2];
                        break;
                    case 11: //middle + 2
                        block[column, i] = this.ampBuffer[2];
                        break;
                    case 15: //middle + 3
                        block[column, i] = this.ampBuffer[3];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }

        /// <summary>Fills one row by reading up to four bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k44</remarks>
        /// <value>
        ///      Up to 4 bits: 
        ///
        ///      bit-seq. | value
        ///     ----------+----------- 
        ///      0        | 0         
        ///      1, 3bits | 3bits->index:  000-> -4, 100-> -3, 010-> -2, 110-> -1
        ///                                001-> +1, 101-> +2, 011-> +3, 111-> +4
        /// Note: pretty sure 1,0,1 (5) is a typo. Both 1 and 5 used to be -1
        /// </value>
        private void FillerColumnAmplitude4Bits4(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(4, 4);
                switch (value)
                {
                    case 0: //single zero
                        block[column, i] = 0;
                        break;
                    case 1: //middle - 4
                        block[column, i] = this.ampBuffer[-4];
                        break;
                    case 3: //middle -3
                        block[column, i] = this.ampBuffer[-3];
                        break;
                    case 5: //middle - 2
                        block[column, i] = this.ampBuffer[-2];
                        break;
                    case 7: //middle + 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 9: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    case 11: //middle + 2
                        block[column, i] = this.ampBuffer[2];
                        break;
                    case 13: //middle + 3
                        block[column, i] = this.ampBuffer[3];
                        break;
                    case 15: //middle + 4
                        block[column, i] = this.ampBuffer[4];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }

        /// <summary>Fills two rows by reading up to five bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function k45</remarks>
        /// <value>
        ///     Up to 5 bits: 
        ///
        ///     bit-seq.    | value
        ///    -------------+-----------
        ///      0          | 0, 0
        ///      1,0        | 0
        ///      1,1, 3bits | 3bits->B_M index:  000-> -4, 100-> -3, 010-> -2, 110-> -1
        ///                                      001-> +1, 101-> +2, 011-> +3, 111-> +4
        /// </value>
        private void FillerColumnAmplitude4Bits5(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = this.ReadAmplitudeBits(4, 5);
                switch (value)
                {
                    case 0: //dual zero
                        block[column, i++] = 0;
                        if (i < block.Rows)
                            block[column, i] = 0;
                        break;
                    case 1: //single zero
                        block[column, i] = 0;
                        break;
                    case 3: //middle - 4
                        block[column, i] = this.ampBuffer[-4];
                        break;
                    case 7: //middle -3
                        block[column, i] = this.ampBuffer[-3];
                        break;
                    case 11: //middle - 2
                        block[column, i] = this.ampBuffer[-2];
                        break;
                    case 15: //middle - 1
                        block[column, i] = this.ampBuffer[-1];
                        break;
                    case 19: //middle + 1
                        block[column, i] = this.ampBuffer[1];
                        break;
                    case 23: //middle + 2
                        block[column, i] = this.ampBuffer[2];
                        break;
                    case 27: //middle + 3
                        block[column, i] = this.ampBuffer[3];
                        break;
                    case 31: //middle + 4
                        block[column, i] = this.ampBuffer[4];
                        break;
                    default:
                        throw new NotSupportedException(String.Format("The value {0} is not supported", value));
                }
            }
        }

        /****************************************************************************************************************************************************
        *   Base shifting Filler functions                                                                                                                  *
        *                                                                                                                                                   *
        *   Notes: These methods take a constant number of bits and re-evaluate the value in another base.                                                  *
        *      These re-evaluations logically shift the base and separate the digits out into an appropriate number of values.                              *
        *      These digits also represent a signed value. So, Base 3 is -1,0, and 1; -not- 0,1,2.                                                          *
        *      Similary, base 5 ranges from -2 to 2 and base 11 ranges from -5 to 5.                                                                        *
        *                                                                                                                                                   *
        *      Personally, I'm not sure how the evaluation of a digit -should- look when considering it to be signed, but I know that, despite              *
        *      the lack of documentation out there, BerliOS just subtracts half (round down) from the intended value, which flies in the face of            *
        *      2's compliment computing, where the absolute value of the negative most value is one greater than that of the positive most value posible.   *
        *      Finally of note, the digits are consumed from a least-significant-first approach. So, 11 == 102 base 3 ends up --shifted-- 1 then -1 then 0  *
        ****************************************************************************************************************************************************/

        /// <summary>Fills three rows by reading five bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function t15</remarks>
        /// <value>
        ///     Takes 5 bits; values 26 or below are the only valid values. BerliOS would throw an error on 27 or greater; I will do modulus
        ///     The digits are signed. So, take the resulting digit and subtract 1 for the signed value. (i.e.: 0 = -1; 1 = 0; 2 = 1)
        /// </value>
        private void FillerColumnBase3(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = BitDataStream.ReadInt32(5);
                Int32 digit1, digit2, digit3;   //convert to base 3
                digit1 = value % 3;         //least significant
                digit2 = (value / 3) % 3;
                digit3 = (value / 9) % 3;   //most significant

                //first
                block[column, i++] = this.ampBuffer[digit1 - 1];

                //second
                if (i < block.Rows)
                    block[column, i++] = this.ampBuffer[digit2 - 1];

                //third
                if (i < block.Rows)
                    block[column, i] = this.ampBuffer[digit3 - 1];
            }
        }

        /// <summary>Fills three rows by reading seven bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function t27</remarks>
        /// <value>
        ///     Takes 7 bits; values 124 or below are the only valid values. BerliOS would throw an error on 125 or greater; I will do modulus
        ///     The digits are signed. So, take the resulting digit and subtract 2 for the signed value. (i.e.: 0 = -2; 1 = -1; 4 = 2)
        /// </value>
        private void FillerColumnBase5(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = BitDataStream.ReadInt32(7);
                Int32 digit1, digit2, digit3;   //convert to base 5
                digit1 = value % 5;         //least significant
                digit2 = (value / 5) % 5;
                digit3 = (value / 25) % 5;   //most significant

                //first
                block[column, i++] = this.ampBuffer[digit1 - 2];

                //second
                if (i < block.Rows)
                    block[column, i++] = this.ampBuffer[digit2 - 2];

                //third
                if (i < block.Rows)
                    block[column, i] = this.ampBuffer[digit3 - 2];
            }
        }

        /// <summary>Fills two rows by reading seven bits.</summary>
        /// <param name="block">Block to fill</param>
        /// <param name="bits">Number of bits to consume for the filler function</param>
        /// <param name="column">Column of the PackedBlock to affect.</param>
        /// <remarks>function t37</remarks>
        /// <value>
        ///     Takes 7 bits; values 120 or below are the only valid values. BerliOS would throw an error on 121 or greater; I will do modulus
        ///     The digits are signed. So, take the resulting digit and subtract 5 for the signed value. (i.e.: 0 = -5; 10 = 5; 4 = -1)
        /// </value>
        private void FillerColumnBase11(PackedBlock block, Int32 bits, Int32 column)
        {
            for (Int32 i = 0; i < block.Rows; ++i)
            {
                Int32 value = BitDataStream.ReadInt32(7);
                Int32 digit1, digit2;   //convert to base 5
                digit1 = value % 11;         //least significant
                digit2 = (value / 11) % 11;   //most significant

                //first
                block[column, i++] = this.ampBuffer[digit1 - 5];

                //second
                if (i < block.Rows)
                    block[column, i] = this.ampBuffer[digit2 - 5];
            }
        }

        /// <summary>Reads variable number of bits based on the max number requested and the amplitude</summary>
        /// <param name="amplitude">Maximum amplitude</param>
        /// <param name="maxBits">Maximum bits to read</param>
        /// <returns>A int32 representing bitsread</returns>
        /// <remarks>
        ///     This will be long-winded. All the amplitude reads have similar properties.
        ///     The first bit of zero indicates a return of zero.
        ///     If the read is a wide read (i.e.: when max bits is 2 more than bits needed), then 0 means two zero rows, and 1 means a single zero row.
        ///     if the amplitude is 3, there is the option for amplitude 1 being 2 bits instead of 3, so an interim read is performed.
        /// </remarks>
        public Int32 ReadAmplitudeBits(Int32 amplitude, Int32 maxBits)
        {
            //two choices: read bit by bit, or all bits and then back up
            //Going with bit by bit for now

            Int32 bitsNeeded = (amplitude == 1) ? 1 : (amplitude == 2 ? 2 : 3);
            Boolean wideRead = (maxBits - bitsNeeded == 2);
            Int32 value = BitDataStream.ReadInt32(1);  //zero indicator bit
            Int32 bitsRead = 1;

            if (value == 1) //not an initial zero bit
            {
                Int32 temp = -1;    //initially invalid for below test
                if (wideRead)       //extra bit exists for single zero
                {
                    temp = BitDataStream.ReadInt32(1);
                    value |= (temp << bitsRead);
                    bitsRead++;
                }

                if (temp != 0)     //we did not read a zero-indicative bit
                {
                    //read amplitude
                    if (amplitude == 3)   // possibly read 2 bits instead of 3
                    {
                        temp = BitDataStream.ReadInt32(1);
                        value |= (temp << bitsRead);
                        bitsRead++;
                    }
                        
                    if (temp == 0)     //we did not read amplitude 3 bit of 0
                        temp = BitDataStream.ReadInt32(1);
                    else //we did not read amplitude 3 bit of 0
                        temp = BitDataStream.ReadInt32(maxBits - bitsRead);
                    value |= (temp << bitsRead);
                    //no need to incrementbits read, now
                }
            }

            return value;
        }
        #endregion

        #region Levels juggling
        /// <summary>This method 'juggles' the packed block data, treating it as blocks rather than columns and rows.</summary>
        /// <param name="packedBlockData">Data array of the packed block</param>
        /// <param name="packedBlockIndex">Starting index into Data array of the packed block</param>
        /// <param name="decodeBuffer">Buffer in which to decode and juggle the data</param>
        /// <param name="decodeBufferIndex">Index in the decoding buffer from which to start</param>
        /// <param name="blockLength">length of a given decoding block</param>
        /// <param name="blockCount">Count of decoding blocks</param>
        private void Juggle(Int32[] packedBlockData, Int32 packedBlockIndex, Int32[] decodeBuffer, Int32 decodeBufferIndex, Int32 blockLength, Int32 blockCount)
        {
            Int32 packedBlockAccessorIndex;
            
            //entry values are 
            Int32 entryFirst, entrySecond, valueFirst, valueSecond;

            for (Int32 outerLoop = 0; outerLoop < blockLength; ++outerLoop)  //could also be something like blockIndex = 0, which is more meaningful, but also more confusing
            {
                packedBlockAccessorIndex = packedBlockIndex;    //set it to the current position

                //get first two data points
                entryFirst = decodeBuffer[decodeBufferIndex];
                entrySecond = decodeBuffer[decodeBufferIndex + 1];

                for (Int32 blockNumber = 0; blockNumber < blockCount; blockNumber += 2)
                {
                    //take twice the second entry, and add to it the first value and the first entry (this data point). Then move to the next 'block', storing the original data for later use
                    valueFirst = packedBlockData[packedBlockAccessorIndex];
                    packedBlockData[packedBlockAccessorIndex] = (entrySecond * 2) + (entryFirst + valueFirst);
                    packedBlockAccessorIndex += blockLength;

                    //take twice the first value (previous data point), and subtract from it the second value and the second entry (this data point). Then move to the next 'block', storing the original data for later use
                    valueSecond = packedBlockData[packedBlockAccessorIndex];
                    packedBlockData[packedBlockAccessorIndex] = (valueFirst * 2) - (entrySecond + valueSecond);
                    packedBlockAccessorIndex += blockLength;

                    //set the 'first entry and second entry to the value data points, for subsequent use and later setting outsid this loop
                    entryFirst = valueFirst;
                    entrySecond = valueSecond;
                }

                //set entry values
                decodeBuffer[decodeBufferIndex] = entryFirst;
                decodeBuffer[decodeBufferIndex + 1] = entrySecond;

                //increment indecies
                decodeBufferIndex += 2;
                ++packedBlockIndex;
            }
        }

        /// <summary>Does the 'Juggling' of data based on </summary>
        /// <param name="block">Packed Block being gnerated</param>
        /// <remarks>Contrary to documentation, this is not recursive, but instead looped.</remarks>
        private void JuggleBlock(PackedBlock block)
        {
            if (this.AcmHeader.PackingLevels > 0)
            {
                //get the step count; BerliOS says 2048 >> levels, and if levels exceeds, then 1. Docs say packAttrs2, or Rows.
                Int32 stepCount = this.AcmHeader.PackingLevels > 9 ? 1 : ((2048 >> this.AcmHeader.PackingLevels) - 2);
                Int32 todo = this.AcmHeader.PackingRows;

                //need to make an initial adjustment for the first two statements in the do loop since they were moved and it alters execution.
                todo += stepCount;
                Int32 paddedBlockIndex = -(stepCount << this.AcmHeader.PackingLevels);

#if DEBUG
                Int32 loopCount = 0;
#endif

                do
                {
                    //moved from end to beginning.
                    todo -= stepCount;
                    paddedBlockIndex += (stepCount << this.AcmHeader.PackingLevels);

                    Int32 blockCount = stepCount > todo ? todo : stepCount;

                    //reset the decoding buffer's index
                    Int32 decodingBufferIndex = 0;
                    Int32 blockLength = (Int32)(this.AcmHeader.PackingColumns) / 2;
                    blockCount *= 2;

                    //there is a bit of a prime-and-read, an initial Juggle,
                    //followed by incrementing a lot of block values. This is probably the incremental for-loop in documentation.
                    this.Juggle(block.Data, paddedBlockIndex, decodeBuffer, decodingBufferIndex, blockLength, blockCount);

                    for (Int32 i = 0, blockIndex = paddedBlockIndex; i < blockCount; ++i)
                    {
                        block.Data[blockIndex]++;
                        blockIndex += blockLength;
                    }

                    //the remainder of the Juggle looping
                    while (blockLength > 1)
                    {
                        decodingBufferIndex += blockLength * 2; //moved inside loop to avoid duplicate code.

                        //do the block shifting, per the documentation. This is the recursion level with the values being constant stuff.
                        blockLength /= 2;
                        blockCount *= 2;
                        //Juggle operation
                        this.Juggle(block.Data, paddedBlockIndex, decodeBuffer, decodingBufferIndex, blockLength, blockCount);
                    }
#if DEBUG
                    loopCount++;
#endif

                    //the evaluation logic is succeeded by two statements. Moving these to the top, which will also affect expected incoming values.
                } while (todo > stepCount);
            }
        }
        #endregion

        /// <summary>Sets the fields of the ACM file to this BitBlock.</summary>
        /// <param name="acmHeader">ACM Header instance to reference</param>
        /// <param name="dataStream">BitStream to read from</param>
        public void SetFields(AcmHeader acmHeader, BitStream dataStream)
        {
            lock (BitBlock.lockable)
            {
                this.AcmHeader = acmHeader;
                this.BitDataStream = dataStream;
                this.InstantiateDecodeBuffer();
            }
        }

        /// <summary>Creates and assigns a new instance of the decodeBuffer. Requires AcmHeader to be set.</summary>
        private void InstantiateDecodeBuffer()
        {
            /************************************************************************************
            *   About the decoding buffer:                                                      *
            *     the decode buffer ALSO appears to be reused between                           *
            *     each PackedBlock. I need to move its declaration and instantiation outside of *
            *     the juggle routine.                                                           *
            ************************************************************************************/
            this.decodeBuffer = new Int32[(2 * this.AcmHeader.PackingColumns) - 2];
        }
    }
}