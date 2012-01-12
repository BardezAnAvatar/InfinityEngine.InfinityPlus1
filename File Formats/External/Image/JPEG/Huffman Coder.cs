using System;
using System.Collections.Generic;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents a class to encode and decode a Huffman stream of bits</summary>
    public class HuffmanCoder : ICoder
    {
        #region Fields
        /// <summary>Symbol-value ordered code table</summary>
        protected List<Byte> encodeHuffCodeTable;

        /// <summary>Symbol-value ordered size table</summary>
        protected List<Byte> encodeHuffSizeTable;

        /// <summary>Represents the table with minimum value of Huffman code for each code length.</summary>
        /// <value>Per §F.2.2.3: The values in MINCODE and MAXCODE are signed 16-bit integers</value>
        protected Int32[] minCode;

        /// <summary>Represents the table with maximum value of Huffman code for each code length.</summary>
        /// <value>Per §F.2.2.3: The values in MINCODE and MAXCODE are signed 16-bit integers</value>
        protected Int32[] maxCode;

        /// <summary>Represents the list of indices for first value in HUFFVAL for each code length.</summary>
        protected Byte[] valPtr;

        /// <summary>Reference copy of the Huffman table definitions's value array</summary>
        protected Byte[] huffVal;

        /// <summary>Reads bits from an input stream</summary>
        /// <remarks>In practice, this should be shared through all HuffmanCoders</remarks>
        protected HuffmanBitReader huffmanBitReader;
        #endregion


        #region Properties
        /// <summary>Symbol-value ordered code table</summary>
        public List<Byte> EncodeHuffCodeTable
        {
            get { return this.encodeHuffCodeTable; }
        }

        /// <summary>Symbol-value ordered size table</summary>
        public List<Byte> EncodeHuffSizeTable
        {
            get { return this.encodeHuffSizeTable; }
        }
        #endregion


        #region Construction
        /// <summary>Gets a new Huffman decoder object</summary>
        /// <param name="reader">Huffman bit reader to read from</param>
        /// <param name="table">Huffman table to build decoder from</param>
        /// <returns>The built HuffmanDecoder</returns>
        public static HuffmanCoder BuildHuffmanDecoder(HuffmanBitReader reader, HuffmanTable table)
        {
            HuffmanCoder huff = new HuffmanCoder();
            huff.InitializeDecoder(reader, table);

            return huff;
        }
        #endregion


        #region Table Building Methods
        /// <summary>Builds encoder tabkes in symbol value order</summary>
        protected void BuildEncodeHuffTables(List<Byte> huffCode, List<Byte> huffSize, Byte[] huffVal)
        {
            //duplicate and reorder is what this does
            this.encodeHuffCodeTable = new List<Byte>(huffCode.Count);
            this.encodeHuffSizeTable = new List<Byte>(huffSize.Count);

            Byte k = 0, i;

            while (k < huffSize.Count)
            {
                i = huffVal[k];
                this.EncodeHuffCodeTable[i] = huffCode[k];
                this.EncodeHuffSizeTable[i] = huffSize[k];
                ++k;
            }
        }

        /// <summary>Builds the three decoding tables described in JPEG §F.2.2.3</summary>
        protected void BuildDecodeHuffTables(Byte[] huffBits, List<UInt16> huffCode)
        {
            //instantiate the tables; 17 to match the discarded 0 index of huffBits
            this.minCode = new Int32[17];
            this.maxCode = new Int32[17];
            this.valPtr = new Byte[17];

            //variable declarations
            Byte j = 0;

            //build tables
            for (Byte i = 1; i < 17; ++i)
            {
                if (huffBits[i] == 0)
                    this.maxCode[i] = -1;
                else
                {
                    this.valPtr[i] = j;
                    this.minCode[i] = huffCode[j];
                    j += (Byte)(huffBits[i] - 1);
                    this.maxCode[i] = huffCode[j];
                    ++j;
                }
            }
        }
        #endregion


        #region Decoding
        /// <summary>Initializes decoder members</summary>
        /// <param name="reader">Huffman bit reader to read from</param>
        /// <param name="table">Huffman table to reference for building the decoder</param>
        public virtual void InitializeDecoder(HuffmanBitReader reader, HuffmanTable table)
        {
            this.BuildDecodeHuffTables(table.BitsList, table.HuffCodeTable);
            this.huffmanBitReader = reader;
            this.huffVal = table.HuffVal;
        }

        /// <summary>Decodes a single byte from the input stream.</summary>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>The value stored in huffVal at the apropriate location</returns>
        public Byte Decode(ref Boolean halt)
        {
            Byte value;
            Int32 i = 0;
            UInt16 code = 0;
            do
            {
                ++i;

                //CODE = (SLL CODE 1) + NEXTBIT
                UInt16 bit = this.huffmanBitReader.GetBits(1, ref halt);
                if (halt)   //exception condition
                    break;

                code <<= 1;
                code += bit;
            }
            while (code > this.maxCode[i]);


            Int32 j = this.valPtr[i];
            j += (code - this.minCode[i]);
            value = this.huffVal[j];

            return value;
        }

        /// <summary>Places the next SSSS bits of the entropy-coded segment into the low order bits of the returned value</summary>
        /// <param name="ssss">Number of bits to place. Should be 0 to 11.</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>The received value</returns>
        /// <remarks>Isn't this just getbits(x)?</remarks>
        protected Int16 Receive(Int32 ssss, ref Boolean halt)
        {
            Int16 v = 0;

            for (Int32 i = 0; i < ssss; ++i)
            {
                v <<= 1;
                Int16 nextBit = (Int16)this.huffmanBitReader.GetBits(1, ref halt);

                if (halt)   //exception condition
                    return v;

                v += nextBit;
            }

            return v;
        }

        /// <summary>Extends the sign bit of a decoded value in v</summary>
        /// <param name="v">Passed in value</param>
        /// <param name="t">Bits to shift. Theoreticaly ranges from 0 to 255. Realistically, should be 0 to 11.</param>
        /// <returns>The signed value</returns>
        protected Int32 Extend(Int32 v, Int32 t)
        {
            Int32 temp = 1 << (t - 1);

            if (v < temp)
            {
                temp = (-1 << t) + 1;
                v += temp;
            }

            return v;
        }

        /// <summary>Runs the Decode procedures on a DC coefficient</summary>
        /// <param name="pred">Prediction of the DC</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>The DC coefficient differential</returns>
        public Int32 DecodeDC(ref Int32 pred, ref Boolean halt)
        {
            Byte temp = this.Decode(ref halt);
            if (halt)
                return -1;

            Int32 Diff = this.Receive(temp, ref halt);
            if (halt)
                return -1;

            Diff = this.Extend(Diff, temp);
            Int32 prevPred = pred;

            //TODO: do I set the prediction to the difference + the previous prediction [ZZ(0)], or do I set it to the decoded Diff?
            pred = Diff + prevPred;

            return pred;
        }

        /// <summary>Decodes a single AC coefficient from the Huffman bit stream</summary>
        /// <param name="zz">Initialized coefficient block</param>
        /// <param name="index">Index to decode</param>
        /// <param name="ssss">Bits to shift</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate up the stack,
        ///     returning work done so far, but ultimately terminating the scan
        /// </param>
        protected void DecodeZZ(Int32[] zz, Int32 index, Int32 ssss, ref Boolean halt)
        {
            Int32 receive = this.Receive(ssss, ref halt);
            if (halt)   ///escape condition
                return;

            zz[index] = receive;
            zz[index] = this.Extend(zz[index], ssss);
        }

        /// <summary>Decodes AC coefficients from the Huffman bit stream</summary>
        /// <param name="emptyBlock">Initialized coefficient block</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate up the stack,
        ///     returning work done so far, but ultimately terminating the scan
        /// </param>
        public void DecodeSequentialACs(Int32[] emptyBlock, ref Boolean halt)
        {
            Int32 k = 1;

            while (true)
            {
                Int32 rs = this.Decode(ref halt);
                if (halt)   //escape condition
                    return;

                Int32 ssss = rs % 16;
                Int32 rrrr = rs >> 4;
                Int32 r = rrrr;

                if (ssss == 0)
                {
                    if (r == 15)
                        k += 16;
                    else
                        break;
                }
                else
                {
                    k += r;
                    this.DecodeZZ(emptyBlock, k, ssss, ref halt);
                    if (halt)   //escape condition
                        return;

                    if (k == 63)
                        break;
                    else
                        ++k;
                }
            }
        }

        /// <summary>Decodes AC coefficients from the Huffman bit stream</summary>
        /// <param name="emptyBlock">Initialized coefficient block</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate up the stack,
        ///     returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <param name="count">Count of coefficients to read</param>
        /// <returns>The count of 0-runs following this block</returns>
        public Int32 DecodeProgressiveACs(Int32[] emptyBlock, ref Boolean halt, Int32 count)
        {
            Int32 k = 0, endOfBand = 0;

            while (true)
            {
                Int32 rs = this.Decode(ref halt);
                if (halt)   //escape condition
                    return 0;

                Int32 ssss = rs % 16;
                Int32 rrrr = rs >> 4;
                Int32 r = rrrr;

                if (ssss == 0)
                {
                    if (r == 15)
                        k += 16;
                    else    //This is, I think, where the End-of-Band comes into play
                    {
                        endOfBand = this.ReceiveEndOfBand(rrrr, ref halt) - 1;
                        if (halt)   //escape condition
                            return 0;

                        break;
                    }
                }
                else
                {
                    k += r;
                    this.DecodeZZ(emptyBlock, k, ssss, ref halt);
                    if (halt)   //escape condition
                        return 0;

                    if (k >= (count - 1))
                        break;
                    else
                        ++k;
                }
            }

            return endOfBand;
        }

        /// <summary>Places the next RRRR bits of the entropy-coded segment into the low order bits of the returned value</summary>
        /// <param name="rrrr">Number of bits to read. Should be 0 to 14.</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <returns>The received value</returns>
        /// <remarks>It starts with 0 == 1. So, start with 1 and, it looks like, shift x bits and read x bits</remarks>
        protected Int32 ReceiveEndOfBand(Int32 rrrr, ref Boolean halt)
        {
            Int32 receive = 1;
            if (rrrr > 0)
            {
                receive <<= rrrr;   //(0 - 14 bits)
                receive += this.huffmanBitReader.GetBits(rrrr, ref halt);
            }
            return receive;
        }
        #endregion


        #region Helper methods
        /// <summary>Splits a value, decode, into two 4-bit values, rrrr and ssss based</summary>
        /// <param name="rrrr">The four most significant bits, shifted to the least significant slot of the returned Byte</param>
        /// <param name="ssss">The four least significant bits, shifted to the least significant slot of the returned Byte</param>
        /// <param name="decode">Byte to be split</param>
        protected void SplitDecode(out Byte rrrr, out Byte ssss, Byte decode)
        {
            rrrr = (Byte)(decode & 0xF0);
            ssss = (Byte)(decode & 0x0F);
        }

        /// <summary>Calculates the DC coefficient difference's SSSS value</summary>
        /// <param name="value">DIFF value used to calculate the SSSS</param>
        /// <returns>Number of bits based off of the</returns>
        /// <remarks>See §F.1.2.1.1</remarks>
        protected Int32 GetDcCategory(Int32 value)
        {
            /*
                SSSS    Signed DIFF value ranges
                        Negative        Positive
                0                   0
                1             –1          1
                2       –3...–2         2...3
                3       –7...–4         4...7
                4       –15...–8        8...15
                5       –31...–16       16...31
                6       –63...–32       32...63
                7       –127...–64      64...127
                8       –255...–128     128...255
                9       –511...–256     256...511
                10      –1023...–512    512...1023
                11      –2047...–1024   1024...2047
            */
            Int32 ssss = 0;

            if (value == 0)
                ssss = 0;
            else
            {
                value = Math.Abs(value);
                Int32 threshold = 1;
                for (Int32 category = 1; category < 12; ++category)
                {
                    if (value <= ((threshold * 2) - 1))
                    {
                        ssss = category;
                        break;
                    }
                    else
                        threshold *= 2;
                }
            }

            return ssss;
        }
        #endregion

        /// <summary>Resets the coding bitstream to 0 bits read</summary>
        public void ResetCodingStream()
        {
            this.huffmanBitReader.ResetBuffer();
        }
    }
}