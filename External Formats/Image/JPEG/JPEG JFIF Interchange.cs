using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Pixels;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a raster graphics image file format used to store digital images, independently of the display device (such as a graphics adapter), compling to JPEG specifications.</summary>
    /// <remarks>
    ///     See ISO/IEC 10918-1 : 1993(E) for JPEG Part 1
    ///     See JPEG File Interchange Format, Version 1.02 for JFIF 1.02
    ///     Does not work with JPEG/EXIF!
    ///     Kind of works with JPEG/JFIF/EXIF.
    /// </remarks>
    public class JpegJfifInterchange : IJpegInterchange
    {
        #region Constants
        /// <summary>Constant to pre-multiply the values by, then divide by, to approximate the decimal multiplcation.</summary>
        /// <remarks>The average data loss of this approach is 0.015</remarks>
        private const Int32 YCbCrShift = 14500;
        private const Int32 CbGreen = 4990;     //0.34414 * 14500 = 4990.03
        private const Int32 CbBLue = 25694;     //1.772 * 14500 =   25694
        private const Int32 CrRed = 20329;      //1.402 * 14500 =   20329
        private const Int32 CrGreen = 10355;    //0.71414 * 14500 = 10355.03
        #endregion


        #region Fields
        /// <summary>Represents the Frame segment of the JPEG stream</summary>
        public JpegFrame Frame { get; set; }

        /// <summary>Represents the component data of the interchange, compiled from scans</summary>
        public ComponentDataInteger[] ComponentData { get; set; }
        #endregion


        #region Frame methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public Frame GetFrame()
        {
            Frame frame = new Frame();
            frame.OriginX = 0UL;
            frame.OriginY = 0UL;
            frame.Pixels = this.GetPixelData();

            return frame;
        }

        /// <summary>Generates a Pixel data object containing the pixel data for this JPEG interchange</summary>
        /// <returns>A PixelData instance</returns>
        protected PixelData GetPixelData()
        {
            Byte[] data = this.MergeComponents();

            PixelData pd = new PixelData(data, Image.Enums.ScanLineOrder.TopDown, Pixels.Enums.PixelFormat.RGB_B8G8R8, this.Frame.ScanLines, this.Frame.Header.Width, 0, 0, 24);

            return pd;
        }

        /// <summary>Merges the various components into RGB24 pixel data</summary>
        /// <returns>An interwoven Byte array of pixel data</returns>
        protected Byte[] MergeComponents()
        {
            ////sort by component number
            //scanComponents.Sort((a, b) => a.Identifier.CompareTo(b.Identifier));

            //resample all components to output size
            List<IList<Int32>> scaledData = new List<IList<Int32>>();
            foreach (ComponentDataInteger component in this.ComponentData)
                if (component != null)
                {
                    Int32[] sampleData = component.GetSampleData();
                    IList<Int32> resized = Resize.BilinearResampleInteger(sampleData, component.Height, component.Width, component.Height, component.Width, this.Frame.ScanLines, this.Frame.Header.Width);
                    scaledData.Add(resized);
                }

            //the scaleData array should contain Y and probably Cb and Cr, in that order, all of the same scale.
            
            Int32 dataSize = this.Frame.ScanLines * this.Frame.Header.Width;

            //so, instantiate the data output
            Int32 outputPixelSize = dataSize;
            return MergeSampleData(scaledData, outputPixelSize);
        }

        /// <summary>Merges the Lists of component sample data into a byte array of pixels</summary>
        /// <param name="scaledData">List of List of Int32-value samples</param>
        /// <param name="outputPixelSize">Height * width of the input data</param>
        /// <returns>Byte array of newly merged data</returns>
        private static Byte[] MergeSampleData(IList<IList<Int32>> scaledData, Int32 outputPixelSize)
        {
            Byte[] data = new Byte[3 * outputPixelSize];    //outputting to RGB, regardless of component count

            //for each destination pixel, resample the source components together
            for (Int32 i = 0; i < outputPixelSize; ++i)
            {
                Int32 indexBase = i * 3;    //OUTPUT components

                //now, convert Y, Cb and Cr
                Int32 red, green, blue;
                red = green = blue = scaledData[0][i] * JpegJfifInterchange.YCbCrShift; //Y

                if (scaledData.Count > 1)
                {
                    Int32 Cb = (scaledData[1][i] - 128);
                    green -= Cb * JpegJfifInterchange.CbGreen;  //Cb
                    blue += Cb * JpegJfifInterchange.CbBLue;    //Cb
                }

                if (scaledData.Count > 2)
                {
                    Int32 Cr = (scaledData[2][i] - 128);
                    red += Cr * JpegJfifInterchange.CrRed;      //Cr
                    green -= Cr * JpegJfifInterchange.CrGreen;  //Cr
                }

                data[indexBase] = JpegJfifInterchange.ConvertSampleToByte(blue);
                data[indexBase + 1] = JpegJfifInterchange.ConvertSampleToByte(green);
                data[indexBase + 2] = JpegJfifInterchange.ConvertSampleToByte(red);
            }

            return data;
        }

        /// <summary>Merges the Lists of component sample data into a byte array of pixels</summary>
        /// <param name="scaledData">List of List of Double-value samples</param>
        /// <param name="outputPixelSize">Height * width of the input data</param>
        /// <returns>Byte array of newly merged data</returns>
        private static Byte[] MergeSampleData(IList<IList<Double>> scaledData, Int32 outputPixelSize)
        {
            Byte[] data = new Byte[3 * outputPixelSize];    //outputting to RGB, regardless of component count

            //for each destination pixel, resample the source components together
            for (Int32 i = 0; i < outputPixelSize; ++i)
            {
                Int32 indexBase = i * 3;    //OUTPUT components

                //now, convert Y, Cb and Cr
                Double red, green, blue;
                red = green = blue = scaledData[0][i]; //Y

                if (scaledData.Count > 1)
                {
                    Double Cb = scaledData[1][i] - 128;
                    green -= Cb * 0.34414;  //Cb
                    blue += Cb * 1.772;     //Cb
                }

                if (scaledData.Count > 2)
                {
                    Double Cr = scaledData[2][i] - 128;
                    red += Cr * 1.402;      //Cr
                    green -= Cr * 0.71414;  //Cr
                }

                data[indexBase] = JpegJfifInterchange.ConvertSampleToByte(blue);
                data[indexBase + 1] = JpegJfifInterchange.ConvertSampleToByte(green);
                data[indexBase + 2] = JpegJfifInterchange.ConvertSampleToByte(red);
            }

            return data;
        }
        #endregion


        #region Component data merge methods
        /// <summary>Merges the MCU data read in into the individual components collection.</summary>
        /// <param name="frame">JPEG frame containing reference data</param>
        /// <param name="scan">JPEG scan to merge the data of</param>
        /// <remarks>
        ///     If components are spread throughout scans, merges them into one collection.
        ///     If progressive, merges multiple scans' component data into one collection.
        /// </remarks>
        public void MergeScanData(JpegFrame frame, JpegScan scan)
        {
            if (this.IsFirstOrderScan(frame, scan))
                this.MergeFirstOrderMcuData(scan);
        }

        /// <summary>Loops through scans' MCUs and compiles their data into a singular location</summary>
        public void MergeFirstOrderMcuData(JpegScan scan)
        {
            //prepare a location for the arrays
            List<Int32[]>[] components = new List<Int32[]>[5];  //4 maximum, what is the problem with a few null references?
            foreach (ScanComponentData component in scan.Components)
                components[component.Identifier] = new List<Int32[]>();


            //source data should not exist in any measure, extract it from these MCUs
            foreach (EntropyCodedSegment ecs in scan.EntropySegments)
            {
                foreach (MimimumCodedUnit mcu in ecs.MimimumCodedUnits)
                {
                    Int32 dataUnitIndex = 0;

                    foreach (ScanComponentData component in scan.Components)
                    {
                        //read each component's data
                        for (Int32 componentDataUnit = 0; componentDataUnit < component.McuDataSize; ++componentDataUnit)
                        {
                            components[component.Identifier].Add((mcu as DctMcu).DataUnits[dataUnitIndex]);
                            ++dataUnitIndex;
                        }
                    }
                }
            }

            //reorder the coefficient blocks
            foreach (ScanComponentData component in scan.Components)
            {
                //The way data comes in, 8x8 blocks are stored in a sampling zig-zag.
                //See JPEG specification, §A.2.3.
                Int32[,][] unzigged = new Int32[component.ContiguousBlockCountHorizontal, component.ContiguousBlockCountVertical][];
                Int32 x = 0, y = 0;

                for (Int32 blockIndex = 0; blockIndex < component.ContiguousBlockCount; blockIndex += component.McuDataSize)
                {
                    for (Int32 mcuVerticalIndex = 0; mcuVerticalIndex < component.VerticalSamplingFactor; ++mcuVerticalIndex)
                    {
                        for (Int32 mcuHorizontalIndex = 0; mcuHorizontalIndex < component.HorizontalSamplingFactor; ++mcuHorizontalIndex)
                        {
                            Int32 dataIndex = (blockIndex + (mcuVerticalIndex * component.HorizontalSamplingFactor) + mcuHorizontalIndex);
                            
                            //copy array reference
                            unzigged[(x + mcuHorizontalIndex), (y + mcuVerticalIndex)] = components[component.Identifier][dataIndex];
                        }
                    }

                    //increment my baseline indecies
                    x += component.HorizontalSamplingFactor;

                    if (x == component.ContiguousBlockCountHorizontal)
                    {
                        x = 0;
                        y += component.VerticalSamplingFactor;
                    }
                }

                //After the MCU order has been unshuffled, there remains the issue that data is stored in 8x8 blocks,
                //rather than in sample order. We need to further loop through the data
                //and re-arrange samples into the actual sample order. This has to be done *after* all block processing, however.
                //In the interim, persist the reorganized blocks.
                this.ComponentData[component.Identifier].SourceData = unzigged;
            }
        }
        #endregion


        #region Helper methods
        /// <summary>Returns a Byte value from the sample provided</summary>
        /// <param name="sample">Datum to convert</param>
        /// <returns>Byte-clamped value of the sample provided</returns>
        protected static Byte ConvertSampleToByte(Double sample)
        {
            return sample < 0.0 ? Byte.MinValue : sample > 255.0 ? Byte.MaxValue : Convert.ToByte(sample);
        }

        /// <summary>Returns a Byte value from the sample provided</summary>
        /// <param name="sample">Datum to convert</param>
        /// <returns>Byte-clamped value of the sample provided</returns>
        protected static Byte ConvertSampleToByte(Int32 sample)
        {
            Int32 shiftSample = sample / JpegJfifInterchange.YCbCrShift;
            return shiftSample < 0 ? Byte.MinValue : shiftSample > 255 ? Byte.MaxValue : Convert.ToByte(shiftSample);
        }

        /// <summary>Sets up and populates the non-sample data of the components</summary>
        public void PopulateComponents()
        {
            //set up component data
            this.ComponentData = new ComponentDataInteger[this.Frame.Header.ComponentNumbers + 1];
            for (Int32 index = 1; index < this.Frame.Header.ComponentNumbers + 1; ++index)
                this.ComponentData[index] = new ComponentDataInteger();

            Int32 hMax = this.Frame.Header.MaxHorizontalSamplingFactor, vMax = this.Frame.Header.MaxVerticalSamplingFactor;

            for (Int32 componentIndex = 1; componentIndex < this.Frame.Header.ComponentNumbers + 1; ++componentIndex)
            {
                FrameComponentParameter fcp = this.MatchComponentPrameter(componentIndex);

                if (fcp != null)
                {
                    this.ComponentData[fcp.Identifier].Identifier = fcp.Identifier;
                    this.ComponentData[fcp.Identifier].HorizontalSamplingFactor = fcp.HorizontalSamplingFactor;
                    this.ComponentData[fcp.Identifier].VerticalSamplingFactor = fcp.VerticalSamplingFactor;
                    this.ComponentData[fcp.Identifier].QuantizationTableIndex = fcp.QuantizationTableIndex;

                    //width; JPEG spec. §A.1.1
                    Decimal factor = Convert.ToDecimal(fcp.HorizontalSamplingFactor) / Convert.ToDecimal(hMax);
                    factor = Convert.ToDecimal(this.Frame.Header.Width) * factor;
                    this.ComponentData[fcp.Identifier].Width = Convert.ToInt32(Math.Ceiling(factor));

                    //height; JPEG spec. §A.1.1
                    factor = Convert.ToDecimal(fcp.VerticalSamplingFactor) / Convert.ToDecimal(vMax);
                    factor = Convert.ToDecimal(this.Frame.Header.Height) * factor;
                    this.ComponentData[fcp.Identifier].Height = Convert.ToInt32(Math.Ceiling(factor));
                }
            }
        }

        /// <summary>Retrieves the Frame component parameter matching the component Identifier.</summary>
        /// <param name="componentId">Component Identifier to match.</param>
        /// <returns>The matching frame component parameter, or null.</returns>
        protected FrameComponentParameter MatchComponentPrameter(Int32 componentId)
        {
            FrameComponentParameter component = null;

            foreach (FrameComponentParameter fc in this.Frame.Header.Components)
            {
                if (fc.Identifier == componentId)
                {
                    component = fc;
                    break;
                }
            }

            return component;
        }

        /// <summary>Indicates whether the scan is of first order data or not (Progressive DCs or Sequential scan)</summary>
        /// <param name="frame">Frame containing the type of image sequential or progressive DCT)</param>
        /// <param name="scan">Scan to be examined</param>
        /// <returns>True if it is a FO scan, false if it is a successive scan</returns>
        protected Boolean IsFirstOrderScan(JpegFrame frame, JpegScan scan)
        {
            Boolean firstOrder = false;

            //first, error conditions
            switch (frame.Header.Marker)
            {
                //sequential
                case JpegInterchangeMarkerConstants.StartOfFrameHuffmanBaselineDCT:
                case JpegInterchangeMarkerConstants.StartOfFrameHuffmanExtendedSequentialDCT:
                case JpegInterchangeMarkerConstants.StartOfFrameArithmeticExtendedSequentialDCT:
                    firstOrder = true;
                    break;
                case JpegInterchangeMarkerConstants.StartOfFrameHuffmanProgressiveDCT:
                case JpegInterchangeMarkerConstants.StartOfFrameArithmeticProgressiveDCT:
                    if (scan.Header.StartSelector == 0 && scan.Header.SuccessiveApproximationHigh == 0)
                        firstOrder = true;
                    break;
                default:
                    throw new NotImplementedException("Decoding measures beside progressive and sequential are not currently implemented.");
            }

            return firstOrder;
        }

        /// <summary>Performs primary decoding on component data</summary>
        public void Decode()
        {
            foreach (ComponentDataInteger component in this.ComponentData)
                if (component != null)
                    component.DecodeData(this.Frame.QuantizationTables[component.QuantizationTableIndex], 8);
        }
        #endregion
    }
}