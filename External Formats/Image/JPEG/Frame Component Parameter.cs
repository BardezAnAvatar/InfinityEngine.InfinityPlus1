using System;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents an individual Component parameter in a frame</summary>
    public class FrameComponentParameter
    {
        #region Fields
        /// <summary>A unique label to the n-th component in the sequence of frame component specification parameters.</summary>
        /// <remarks>These values shall be used in the scan headers to identify the components in the scan.</remarks>
        public Byte Identifier { get; set; }

        /// <summary>Specifies the relationship between the component horizontal dimension and maximum image dimension X</summary>
        /// <remarks>Also specifies the number of horizontal data units of component C in each MCU, when more than one component is encoded in a scan.</remarks>
        /// <value>1 to 4</value>
        public Byte HorizontalSamplingFactor { get; set; }

        /// <summary>Specifies the relationship between the component vertical dimension and maximum image dimension Y</summary>
        /// <remarks>Also specifies the number of vertical data units of component C in each MCU, when more than one component is encoded in a scan.</remarks>
        /// <value>1 to 4</value>
        public Byte VerticalSamplingFactor { get; set; }

        /// <summary>Specifies one of four possible quantization table destinations from which the quantization table to use for dequantization of DCT coefficients of component C is retrieved</summary>
        public Byte QuantizationTableIndex { get; set; }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.Identifier = (Byte)input.ReadByte();

            //read sampling factors
            Byte horizontal, vertical;
            JpegParser.ReadParameters4(input, out horizontal, out vertical);
            this.HorizontalSamplingFactor = horizontal;
            this.VerticalSamplingFactor = vertical;

            //read quantization destination
            this.QuantizationTableIndex = (Byte)input.ReadByte();
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            output.WriteByte(this.Identifier);

            //write sampling factors
            Byte temp = (Byte)(this.HorizontalSamplingFactor << 4);
            temp &= (Byte)(this.VerticalSamplingFactor & 0x0F);
            output.WriteByte(temp);

            //read quantization destination
            output.WriteByte(this.QuantizationTableIndex);
        }
        #endregion
    }
}