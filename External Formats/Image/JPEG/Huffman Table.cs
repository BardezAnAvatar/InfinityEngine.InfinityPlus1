using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a single JPEG Huffman table</summary>
    /// <remarks>See T.81, page 40-41, §B.2.4.2, JPEG Spec</remarks>
    public class HuffmanTable : GenericCodingTable
    {
        #region Fields
        /// <summary>Represents the number of Huffman codes for each of the 16 possible lengths (BITS) allowed by the JPEG spec.</summary>
        protected Byte[] bitsList;

        /// <summary>Represents the Huffman code values (HUFFVAL)</summary>
        protected Byte[] huffVal;

        /// <summary>The table of Huffman code sizes</summary>
        protected List<Byte> huffSize;

        /// <summary>The table of Huffman code sizes</summary>
        protected List<UInt16> huffCode;
        #endregion


        #region Properties
        /// <summary>Exposes the maximum length in any CodeLengthCount index</summary>
        [Obsolete("Was made during a test run that didn't pan out.")]
        protected Int32 MaxLength
        {
            get
            {
                //Get maxium length
                Int32 max = 0;
                for (Int32 index = 1; index < 17; ++index)
                    max = this.bitsList[index] > max ? this.bitsList[index] : max;

                return max;
            }
        }

        /// <summary>Exposes the sum of values in <see cref="bitsList"/> (BITS).</summary>
        protected Int32 BitsSum
        {
            get
            {
                //Get total length
                Int32 sum = 0;
                for (Int32 index = 1; index < 17; ++index)
                    sum += this.bitsList[index];

                return sum;
            }
        }

        /// <summary>Exposes the number of Huffman codes for each of the 16 possible lengths (BITS) allowed by the JPEG spec.</summary>
        /// <remarks>Spec indexes it from 1 to 16. I find it increasingly easier to just have a 0-16 indexed array, discarding index 0</remarks>
        public Byte[] BitsList
        {
            get { return this.bitsList; }
        }

        /// <summary>Exposes the Huffman code values</summary>
        public Byte[] HuffVal
        {
            get { return this.huffVal; }
        }

        /// <summary>Exposes the table of Huffman code sizes</summary>
        public List<Byte> HuffSizeTable
        {
            get { return this.huffSize; }
        }

        /// <summary>Exposes the table of Huffman codes</summary>
        public List<UInt16> HuffCodeTable
        {
            get { return this.huffCode; }
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
        {
            //read parameters
            Byte class_, tableIndex;
            JpegParser.ReadParameters4(input, out class_, out tableIndex);
            this.TableClass = class_;
            this.TableDestinationIndex = tableIndex;

            //read table codes counts
            this.bitsList = new Byte[17];
            Byte[] temp = ReusableIO.BinaryRead(input, 16);
            Array.Copy(temp, 0, this.bitsList, 1, 16);

            //instantiate 2D array
            this.huffVal = ReusableIO.BinaryRead(input, this.BitsSum);

            //build secondary tables
            this.BuildTables();
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            //write parameters
            Byte temp = (Byte)(this.TableClass << 4);
            temp &= (Byte)(this.TableDestinationIndex & 0x0F);
            output.WriteByte(temp);

            //write code length counts
            output.Write(this.bitsList, 1, this.bitsList.Length - 1);

            // write table
            output.Write(this.huffVal, 0, this.huffVal.Length);
        }
        #endregion


        #region Table Building Methods
        /// <summary>Implements the size table building function specified in JPEG § C.2</summary>
        protected void BuildHuffSize()
        {
            this.huffSize = new List<Byte>();

            //spec variables defined
            Byte i = 1, j = 1;

            while (i < 17)
            {
                if (j > this.bitsList[i]) //BITS ranges from 1-16, but indices are 0-15
                {
                    ++i;
                    j = 1;
                }
                else
                {
                    this.huffSize.Add(i);
                    ++j;
                }
            }

            this.huffSize.Add(0);
        }

        /// <summary>Implements the code table building function specified in JPEG § C.2</summary>
        protected void BuildHuffCode()
        {
            this.huffCode = new List<UInt16>();

            UInt16 code = 0, si = this.huffSize[0];
            Int32 k = 0;

            //I don't like while...true, but it seem the most literal interpretation of the flow chart
            while (true)
            {
                while (true)
                {
                    this.huffCode.Add(code);
                    ++code;
                    ++k;

                    if (this.huffSize[k] == si)
                        continue;
                    else
                        break;
                }

                if (this.huffSize[k] == 0)
                    break;
                else
                {
                    while (true)
                    {
                        code <<= 1;
                        ++si;

                        if (this.huffSize[k] == si)
                            break;
                    }
                }
            }
        }

        /// <summary>Calls the table building functions of JPEG §C.2</summary>
        public void BuildTables()
        {
            this.BuildHuffSize();
            this.BuildHuffCode();
        }
        #endregion
    }
}