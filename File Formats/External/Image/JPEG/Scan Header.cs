using System;
using System.Collections.Generic;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents the JPEG scan header, §B.2.3</summary>
    public class JpegScanHeader : MarkerSegment
    {
        #region Fields
        /// <summary>Number of image components (colorspace channels, hierarchy overlays, etc.) in frame.</summary>
        /// <value>1-4</value>
        public Byte ComponentNumbers { get; set; }

        /// <summary>Represents the component-specification parameters collection</summary>
        protected List<ScanComponentParameter> components;

        /// <summary>Specifies the first DCT coefficient in each block in zig-zag order which shall be coded in the scan.</summary>
        public Byte StartSelector { get; set; }

        /// <summary>Specifies the last DCT coefficient in each block in zig-zag order which shall be coded in the scan.</summary>
        public Byte EndSelector { get; set; }

        /// <summary>
        ///     This parameter specifies the point transform used in the preceding scan (i.e. successive approximation bit position low in the preceding scan)
        ///     for the band of coefficients specified by <see cref="StartSelector" /> and <see cref="EndSelector" />.
        /// </summary>
        /// <value>In the lossless mode of operations this parameter has no meaning. It shall be set to zero.</value>
        public Byte SuccessiveApproximationHigh { get; set; }

        /// <summary>Specifies the point transform, i.e. bit position low, used before coding the band of coefficients specified by <see cref="StartSelector" /> and <see cref="EndSelector" />.</summary>
        /// <value>This parameter shall be set to zero for the sequential DCT processes. In the lossless mode of operations, this parameter specifies the point transform, Pt.</value>
        public Byte SuccessiveApproximationLow { get; set; }
        #endregion


        #region Properties
        /// <summary>Represents the component-specification parameters collection</summary>
        public List<ScanComponentParameter> Components
        {
            get { return this.components; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public JpegScanHeader() { }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        public JpegScanHeader(UInt16 marker) : base(marker) { }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Length of this substream</param>
        public JpegScanHeader(UInt16 marker, UInt16 length) : base(marker, length) { }

        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.components = new List<ScanComponentParameter>();
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.Initialize();

            this.Length = JpegParser.ReadParameter16(input);

            this.ComponentNumbers = (Byte)input.ReadByte();

            for (Int32 index = 0; index < this.ComponentNumbers; ++index)
            {
                ScanComponentParameter component = new ScanComponentParameter();
                component.Read(input);
                this.components.Add(component);
            }

            this.StartSelector = (Byte)input.ReadByte();
            this.EndSelector = (Byte)input.ReadByte();

            //read sampling factors
            Byte high, low;
            JpegParser.ReadParameters4(input, out high, out low);
            this.SuccessiveApproximationHigh = high;
            this.SuccessiveApproximationLow = low;
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            output.WriteByte(this.ComponentNumbers);

            for (Int32 index = 0; index < this.ComponentNumbers; ++index)
                this.components[index].Write(output);

            output.WriteByte(this.StartSelector);
            output.WriteByte(this.EndSelector);

            //write sampling factors
            Byte temp = (Byte)(this.SuccessiveApproximationHigh << 4);
            temp &= (Byte)(this.SuccessiveApproximationLow & 0x0F);
            output.WriteByte(temp);
        }
        #endregion
    }
}