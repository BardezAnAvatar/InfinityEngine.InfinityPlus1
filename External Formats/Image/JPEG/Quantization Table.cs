using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents a single quantization table</summary>
    public class QuantizationTable : GenericTable
    {
        #region Fields
        /// <summary>Specifies the precision of the quantization values</summary>
        /// <value>0 means 8-bit; 1 means 16-bit</value>
        public Byte Precision { get; set; }

        /// <summary>Represents the 64 quantization table elements.</summary>
        /// <remarks>Follows the pattern of indecies 0 ... k ... 63 with k being in the zig-zag pattern</remarks>
        /// <value>Can be either Byte or UInt16, depending on <see cref="Precision" /></value>
        protected UInt16[] elements;
        #endregion


        #region Properties
        /// <summary>Represents the 64 quantization table elements.</summary>
        /// <remarks>Follows the pattern of indecies 0 ... k ... 63 with k being in the zig-zag pattern</remarks>
        /// <value>Can be either Byte or UInt16, depending on <see cref="Precision" /></value>
        public UInt16[] Elements
        {
            get { return this.elements; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public QuantizationTable() { }

        /// <summary>Definition constructor</summary>
        /// <param name="precision">0 for 8 bits, 1 for 16 bits</param>
        /// <param name="tableDestination">indicates decoding table destination</param>
        public QuantizationTable(Byte precision, Byte tableDestination)
        {
            this.Precision = precision;
            this.TableDestinationIndex = tableDestination;
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void Read(Stream input)
        {
            //read parameters
            Byte precision, table;
            JpegParser.ReadParameters4(input, out precision, out table);
            this.Precision = precision;
            this.TableDestinationIndex = table;

            //read table contents
            this.elements = new UInt16[64];
            Byte[] data;
            Int32 index;
            if (this.Precision == 0)
            {
                data = ReusableIO.BinaryRead(input, 64);
                for (index = 0; index < 64; ++index)
                    this.Elements[index] = data[index];
            }
            else if (this.Precision == 1)
            {
                data = ReusableIO.BinaryRead(input, 128);
                for (index = 0; index < 128; index += 2)
                    this.Elements[index] = ReusableIO.ReadUInt16FromArray(data, index, Endianness.BigEndian);
            }
            else
                throw new ApplicationException(String.Format("Unexpected table bit precision. Expected 0 or 1, found {0}.", this.Precision));
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            //write parameters
            Byte temp = (Byte)(this.Precision << 4);
            temp &= (Byte)(this.TableDestinationIndex & 0x0F);
            output.WriteByte(temp);

            // write table
            for (Int32 index = 0; index < 64; ++index)
            {
                if (this.Precision == 0)
                    output.WriteByte((Byte)this.Elements[index]);
                else if (this.Precision == 1)
                    ReusableIO.WriteUInt16ToStream(this.Elements[index], output, Endianness.BigEndian);
            }
        }
        #endregion
    }
}