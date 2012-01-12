using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.External.Image.Mathematics;
using Bardez.Projects.InfinityPlus1.Files.External.Image.Pixels;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents a raster graphics image file format used to store digital images, independently of the display device (such as a graphics adapter), compling to JPEG specifications.</summary>
    /// <remarks>
    ///     See ISO/IEC 10918-1 : 1993(E) for JPEG Part 1
    ///     See JPEG File Interchange Format, Version 1.02 for JFIF 1.02
    ///     Does not work with JPEG/EXIF!
    ///     Kind of works with JPEG/JFIF/EXIF.
    /// </remarks>
    public class JpegJfifInterchange
    {
        #region Fields
        /// <summary>Represents the Frame segment of the JPEG stream</summary>
        public JpegFrame Frame { get; set; }

        protected ComponentDataFloat[] ComponentData { get; set; }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            throw new NotImplementedException();
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Frame methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public Frame GetFrameFloat()
        {
            Frame frame = new Frame();
            frame.OriginX = 0UL;
            frame.OriginY = 0UL;
            frame.Pixels = this.GetPixelDataFloat();

            return frame;
        }

        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public Frame GetFrameInteger()
        {
            Frame frame = new Frame();
            frame.OriginX = 0UL;
            frame.OriginY = 0UL;
            frame.Pixels = this.GetPixelDataInteger();

            return frame;
        }

        protected PixelData GetPixelDataFloat()
        {
            Byte[] data = this.MergeComponentsFloat();

            PixelData pd = new PixelData(data, Image.Enums.ScanLineOrder.TopDown, Pixels.Enums.PixelFormat.RGB_B8G8R8, this.Frame.ScanLines, this.Frame.Header.Width, 0, 0, 24);

            return pd;
        }

        protected PixelData GetPixelDataInteger()
        {
            Byte[] data = this.MergeComponentsInteger();

            PixelData pd = new PixelData(data, Image.Enums.ScanLineOrder.TopDown, Pixels.Enums.PixelFormat.RGB_B8G8R8, this.Frame.ScanLines, this.Frame.Header.Width, 0, 0, 24);

            return pd;
        }

        protected Byte[] MergeComponentsFloat()
        {
            ////gather components
            //List<ScanComponentData> scanComponents = new List<ScanComponentData>();
            //foreach (JpegScan scan in this.Frame.Scans)
            //{
            //    foreach (ScanComponentData component in scan.Components)
            //        scanComponents.Add(component);
            //}

            ////sort by component number
            //scanComponents.Sort((a, b) => a.Identifier.CompareTo(b.Identifier));

            ////resample all components to output size
            //List<List<Double>> scaledData = new List<List<Double>>();
            //foreach (ScanComponentData component in scanComponents)
            //    scaledData.Add(Resize.BilinearResampleFloat(component.ComponentDecodedDataFloat, component.Height, component.Width, component.ContiguousBlockHeight, component.ContiguousBlockWidth, this.Frame.ScanLines, this.Frame.Header.Width));

            List<List<Double>> scaledData = new List<List<Double>>();
            foreach (ComponentDataFloat component in this.ComponentData)
                scaledData.Add(Resize.BilinearResampleFloat(component.SampleData, component.Height, component.Width, component.ContiguousBlockHeight, component.ContiguousBlockWidth, this.Frame.ScanLines, this.Frame.Header.Width));

            ////the scaleData array should contain Y and probably Cb and Cr, in that order, all of the same scale.

            
            Int32 dataSize = this.Frame.ScanLines * this.Frame.Header.Width;

            //so, instantiate the data output
            Int32 outputPixelSize = dataSize;
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

        protected Byte[] MergeComponentsInteger()
        {
            //gather components
            List<ScanComponentData> scanComponents = new List<ScanComponentData>();
            foreach (JpegScan scan in this.Frame.Scans)
            {
                foreach (ScanComponentData component in scan.Components)
                    scanComponents.Add(component);
            }

            //sort by component number
            scanComponents.Sort((a, b) => a.Identifier.CompareTo(b.Identifier));

            //resample all components to output size
            List<List<Int32>> scaledData = new List<List<Int32>>();
            foreach (ScanComponentData component in scanComponents)
                scaledData.Add(Resize.BilinearResampleInteger(component.ComponentDecodedDataInteger, component.Height, component.Width, component.ContiguousBlockHeight, component.ContiguousBlockWidth, this.Frame.ScanLines, this.Frame.Header.Width));
            //the scaleData array should contain Y and probably Cb and Cr, in that order, all of the same scale.

            Int32 dataSize = this.Frame.ScanLines * this.Frame.Header.Width;


            //so, instantiate the data output
            Int32 outputPixelSize = dataSize;
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


        /// <summary>Gets pixel data from the Y component only, no transformations of colorspace, as RGBA</summary>
        /// <returns>Crap data</returns>
        [Obsolete("This was used as testing; may be useful in some conditions, but likely not.")]
        protected PixelData GetCrapPixelData()
        {
            //to see the bizarre result, I will return a frame with the first component tripled
            Byte[] pixelData = new Byte[this.Frame.Header.ContiguousBlockHeight * this.Frame.Header.ContiguousBlockWidth];

            Int32 sourceDataIndex = 0;

            //for each line of contiguous blocks
            for (Int32 y = 0; y < this.Frame.Header.ContiguousBlockCountVertical; ++y)
            {
                Int32 baseY = y * 8;

                for (Int32 x = 0; x < this.Frame.Header.ContiguousBlockCountHorizontal; ++x)
                {
                    Int32 baseX = x * 8;

                    for (Int32 index = 0; index < 64; ++index)
                    {
                        Int32 column = index % 8, row = index / 8;
                        Int32 pixelIndex = ((baseY + row) * this.Frame.Header.ContiguousBlockWidth * 4) + (baseX + column);

                        pixelData[pixelIndex] = (Byte)(this.Frame.Scans[0].Components[0].ComponentData[sourceDataIndex]);
                        pixelData[pixelIndex + 1] = (Byte)(this.Frame.Scans[0].Components[0].ComponentData[sourceDataIndex]);
                        pixelData[pixelIndex + 2] = (Byte)(this.Frame.Scans[0].Components[0].ComponentData[sourceDataIndex]);
                        pixelData[pixelIndex + 3] = 255;

                    }
                }
            }

            PixelData pd = new PixelData(pixelData, Image.Enums.ScanLineOrder.TopDown, Pixels.Enums.PixelFormat.RGBA_R8G8B8A8, this.Frame.ScanLines, this.Frame.Header.Width, 8, 8, 32);

            return pd;
        }
        #endregion


        #region Decoding methods
        #region Common decoding

        /// <summary>Merges the MCU data read in into the individual components collection.</summary>
        /// <remarks>
        ///     If components are spread throughout scans, merges them into one collection.
        ///     If progressive, merges multiple scans' component data into one collection.
        /// </remarks>
        private void MergeScanData()
        {
            //first, error conditions
            switch (Frame.Header.Marker)
            {
                case JpegInterchangeMarkerConstants.StartOfFrameHuffmanBaselineDCT:
                case JpegInterchangeMarkerConstants.StartOfFrameHuffmanExtendedSequentialDCT:
                case JpegInterchangeMarkerConstants.StartOfFrameArithmeticExtendedSequentialDCT:
                    this.MergeSequentialComponents();
                    break;
                case JpegInterchangeMarkerConstants.StartOfFrameHuffmanProgressiveDCT:
                case JpegInterchangeMarkerConstants.StartOfFrameArithmeticProgressiveDCT:
                    this.MergeProgressiveComponents();
                    break;
                default:
                    throw new NotImplementedException("Decoding measures beside progressive and sequential are not currently implemented.");
            }
        }

        /// <summary>Loops through scans' MCUs and compiles their data into a singular location</summary>
        protected void MergeSequentialComponents()
        {
            //set up component data
            this.PopulateComponents();

            foreach (JpegScan scan in this.Frame.Scans)
            {
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
                                this.ComponentData[component.Identifier - 1].SourceData.AddRange((mcu as DctMcu).DataUnits[dataUnitIndex]);
                                ++dataUnitIndex;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Loops through scans' MCUs and compiles their data into a singular location</summary>
        protected void MergeProgressiveComponents()
        {
            //set up component data
            this.PopulateComponents();

            //set up temp
            Int32[][][] componentsTempData = new Int32[this.Frame.Header.ComponentNumbers][][];
            for (Int32 componentNumber = 0; componentNumber < this.Frame.Header.ComponentNumbers; ++componentNumber)
            {
                Int32 blocks = this.ComponentData[componentNumber].ContiguousBlockCount;
                componentsTempData[componentNumber] = new Int32[blocks][];

                for (Int32 blockNumber = 0; blockNumber < blocks; ++blockNumber)
                    componentsTempData[componentNumber][blockNumber] = new Int32[64];   //size of one block;
            }

            //Merge the data
            foreach (JpegScan scan in this.Frame.Scans)
            {
                Int32[] blockIndecies = new Int32[this.Frame.Header.ComponentNumbers + 1];  //start at 0, who cares about 1 unused 0 index?

                for (Int32 entropyCodedSegmentIndex = 0; entropyCodedSegmentIndex < scan.EntropySegments.Count; ++entropyCodedSegmentIndex)
                {
                    EntropyCodedSegment ecs = scan.EntropySegments[entropyCodedSegmentIndex];

                    for (Int32 mcuIndex = 0; mcuIndex < ecs.MimimumCodedUnits.Count; ++mcuIndex)
                    {
                        DctMcu mcu = ecs.MimimumCodedUnits[mcuIndex] as DctMcu;
                        Int32 dataUnitIndex = 0;

                        foreach (ScanComponentData component in scan.Components)
                        {
                            //read each component's data
                            for (Int32 componentDataUnit = 0; componentDataUnit < component.McuDataSize; ++componentDataUnit)
                            {
                                Int32 count = scan.Header.EndSelector - scan.Header.StartSelector + 1;

                                //do not use Array.Copy; there will likely be some Successive Approximation needed.
                                for (Int32 index = 0; index < count; ++index)
                                {
                                    Int32 value = mcu.DataUnits[dataUnitIndex][index];
                                    
                                    //restore image
                                    value <<= scan.Header.SuccessiveApproximationLow;   //AL

                                    //component.Identifier is 1-indexed, not 0-indexed
                                    componentsTempData[component.Identifier - 1][blockIndecies[component.Identifier]][scan.Header.StartSelector + index] = value;
                                }
                                //Array.Copy(mcu.DataUnits[dataUnitIndex], 0, componentsTempData[component.Identifier - 1][blockIndecies[component.Identifier]], scan.Header.StartSelector, count);

                                ++dataUnitIndex;
                                blockIndecies[component.Identifier] += component.McuDataSize;
                            }
                        }
                    }
                }
            }

            //generate the output source data
            for (Int32 componentIndex = 0; componentIndex < this.ComponentData.Length; ++componentIndex)
            {
                for (Int32 blockIndex = 0; blockIndex < componentsTempData[componentIndex].Length; ++blockIndex)
                    this.ComponentData[componentIndex].SourceData.AddRange(componentsTempData[componentIndex][blockIndex]); //add each block
            }
        }


        /// <summary>Reverses the zig-zag sampling order of JPEG blocks</summary>
        /// <param name="zigZagList">Source list to undo the zig-zag of.</param>
        private void UnZigZag(List<Int32> zigZagList)
        {
            Int32[] temp = new Int32[64];

            for (Int32 blockStart = 0; blockStart < zigZagList.Count; blockStart += 64)
            {
                Int32 x = 0, y = 0;
                Boolean incrementX = true;
                for (Int32 index = 0; index < 64; ++index)
                {
                    temp[(8 * y) + x] = zigZagList[blockStart + index];

                    if (incrementX)
                    {
                        if (y > 0 && x < 7)
                            --y;
                        else
                            incrementX = false;

                        if (x == 7)
                        {
                            ++y;
                            incrementX = false;
                        }
                        else
                            ++x;
                    }
                    else
                    {
                        if (x > 0 && y < 7)
                            --x;
                        else
                            incrementX = true;

                        if (y == 7)
                        {
                            ++x;
                            incrementX = true;
                        }
                        else
                            ++y;
                    }
                }

                //copy back
                for (Int32 i = 0; i < 64; ++i)
                    zigZagList[blockStart + i] = temp[i];
            }
        }

        /// <summary>Reorders the component data 8x8 blocks based on multiple vertical sampling levels</summary>
        /// <param name="componentData">Component to unshuffle</param>
        private static void ReorderBlockData(ScanComponentData component)
        {
            //The way data comes in, 8x8 blocks are stored in a sampling zig-zag.
            //See JPEG specification, §A.2.3.
            List<Int32>[,] unzigged = new List<Int32>[component.ContiguousBlockCountHorizontal, component.ContiguousBlockCountVertical];
            Int32 x = 0, y = 0;

            for (Int32 blockIndex = 0; blockIndex < component.ContiguousBlockCount; blockIndex += component.McuDataSize)
            {
                for (Int32 mcuVerticalIndex = 0; mcuVerticalIndex < component.VerticalSamplingFactor; ++mcuVerticalIndex)
                {
                    for (Int32 mcuHorizontalIndex = 0; mcuHorizontalIndex < component.HorizontalSamplingFactor; ++mcuHorizontalIndex)
                    {
                        Int32 dataIndex = (blockIndex + (mcuVerticalIndex * component.HorizontalSamplingFactor) + mcuHorizontalIndex) * 64;
                        List<Int32> block = component.ComponentData.GetRange(dataIndex, 64);

                        unzigged[(x + mcuHorizontalIndex), (y + mcuVerticalIndex)] = block;
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
            List<Int32> output = new List<Int32>();

            for (y = 0; y < component.ContiguousBlockCountVertical; ++y)
                for (x = 0; x < component.ContiguousBlockCountHorizontal; ++x)
                    output.AddRange(unzigged[x, y]);

            component.ComponentData = output;
        }

        /// <summary>Dequantizes the component data list in-place</summary>
        /// <param name="componentData">List of component data to de-quantize</param>
        /// <param name="quantizationElements">Quantization table elements to dequantize against</param>
        private void Dequantize(List<Int32> componentData, Int32 quantizationTableIndex)
        {
            QuantizationTable qt = this.Frame.QuantizationTables[quantizationTableIndex];

            for (Int32 qDctIndex = 0; qDctIndex < componentData.Count; ++qDctIndex)
            {
                Int32 dDct = componentData[qDctIndex] * qt.Elements[qDctIndex % 64];
                componentData[qDctIndex] = dDct;
            }
        }

        /// <summary>Attempts to implement JPEG specification §K.2.8.1</summary>
        /// <param name="component">Component to smooth</param>
        /// <remarks>
        ///     My thanks to the following, indicating another divide by 8 (see the IDCT fast implementation). I need to just start dividing by when I get funny results.
        ///     Adaptive AC-Coefficient Prediction for Image Compression and Blind Watermarking
        ///     (K. Veeraswamy, S. Srinivas Kumar)
        ///     http://www.academypublisher.com/jmm/vol03/no01/jmm03011622.pdf
        /// </remarks>
        private void SmoothingPrediction(ScanComponentData component)
        {
            AmplitudeCoefficientPredictiveSmoothing.SmoothingPrediction(component.ContiguousBlockCountVertical, component.ContiguousBlockCountHorizontal, component.ComponentDecodedDataFloat);
        }
        #endregion

        #region Double-precision floating point decoding
        /// <summary>Decodes the data using double-precision floating point between the IDCT and the RGB color space conversion</summary>
        public void DecodeFloat()
        {
            //Dequantize the DC, AC coefficients
            //this.DecodeJpegFrameDataFloat();

            this.MergeScanData();

            foreach (ComponentData<Double> cd in this.ComponentData)
                cd.DecodeData(this.Frame.QuantizationTables[cd.QuantizationTableIndex], this.Frame.Header.SamplePrecision);
        }

        /// <summary>Performs the mathematical transforms after de-coding from the input stream</summary>
        private void DecodeJpegFrameDataFloat()
        {
            foreach (JpegScan scan in this.Frame.Scans)
            {
                foreach (ScanComponentData component in scan.Components)
                {
                    //re-order the blocks to fit the image grid
                    JpegJfifInterchange.ReorderBlockData(component);

                    //De-quantize the coefficients to reconstruct an approximate collection of forward DCT coefficients
                    this.Dequantize(component.ComponentData, component.QuantizationTableIndex);

                    //Undo the zig-zag coefficient between dequantizing and IDCT. Block order does not matter.
                    this.UnZigZag(component.ComponentData);

                    //convert to floating-point data
                    JpegJfifInterchange.ConvertToFloatData(component);

                    //smooth blocks
                    //this.SmoothingPrediction(component);

                    //perform the IDCT
                    this.InverseDiscreteCosineTransformFloat(component);

                    //undo the 0-center shifting to restore back to 0 - 255 unsigned value range
                    JpegJfifInterchange.UndoLevelShift(this.Frame.Header.SamplePrecision, component.ComponentDecodedDataFloat);

                    //re-order the samples to form a true top-down image
                    this.ReorderBlockSampleDataFloat(component);
                }
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>
        ///     See JPEG specification §A.3.3
        ///     
        ///     Captures a floating-point list of data, to preserve data when generating RGB values from YCbCr data.
        /// </remarks>
        private void InverseDiscreteCosineTransformFloat(ScanComponentData component)
        {
            //component.ComponentDecodedDataFloat = DiscreteCosineTransformation.InverseDiscreteCosineTransformSlowFloat(component.ComponentDecodedDataFloat);
            component.ComponentDecodedDataFloat = DiscreteCosineTransformation.InverseDiscreteCosineTransformFastFloat(component.ComponentDecodedDataFloat);
        }

        /// <summary>Performs an unshift by adding the shift level to </summary>
        /// <param name="level">Sample bit precision</param>
        /// <param name="data">List of samples to unshift</param>
        private static void UndoLevelShift(Int32 level, List<Double> data)
        {
            Int32 shift;
            switch (level)
            {
                case 8:
                    shift = 128;
                    break;
                case 12:
                    shift = 2048;
                    break;
                default:
                    throw new ApplicationException(String.Format("Unexpected level shift size of {0}.", level));
            }

            for (Int32 index = 0; index < data.Count; ++index)
                data[index] += shift;
        }

        /// <summary>Reorders the component data 8x8 blocks based on multiple vertical sampling levels</summary>
        /// <param name="componentData">Component to unshuffle</param>
        private void ReorderBlockDataFloat(ScanComponentData component)
        {
            //The way data comes in, 8x8 blocks are stored in a sampling zig-zag.
            //See JPEG specification, §A.2.3.

            List<Double>[,] unzigged = new List<Double>[component.ContiguousBlockCountHorizontal, component.ContiguousBlockCountVertical];
            Int32 x = 0, y = 0;

            for (Int32 blockIndex = 0; blockIndex < component.ContiguousBlockCount; blockIndex += component.McuDataSize)
            {
                for (Int32 mcuVerticalIndex = 0; mcuVerticalIndex < component.VerticalSamplingFactor; ++mcuVerticalIndex)
                {
                    for (Int32 mcuHorizontalIndex = 0; mcuHorizontalIndex < component.HorizontalSamplingFactor; ++mcuHorizontalIndex)
                    {
                        Int32 dataIndex = (blockIndex + (mcuVerticalIndex * component.HorizontalSamplingFactor) + mcuHorizontalIndex) * 64;
                        List<Double> block = component.ComponentDecodedDataFloat.GetRange(dataIndex, 64);

                        unzigged[(x + mcuHorizontalIndex), (y + mcuVerticalIndex)] = block;
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
            //and re-arrange samples into the actual sample order.

            //have the unzigged data. Now I shall unzag. (No jaggedness. Really just appending horizontally, but it fits the diction)
            List<Double> unzagged = new List<Double>();

            for (y = 0; y < component.ContiguousBlockCountVertical; ++y)
                for (Int32 blockScanline = 0; blockScanline < 8; ++blockScanline)
                    for (x = 0; x < component.ContiguousBlockCountHorizontal; ++x)
                        unzagged.AddRange(unzigged[x, y].GetRange(blockScanline * 8, 8));

            component.ComponentDecodedDataFloat = unzagged;
        }

        /// <summary>Reorders the component data 8x8 blocks based on multiple vertical sampling levels</summary>
        /// <param name="componentData">Component to unshuffle</param>
        private void ReorderBlockSampleDataFloat(ScanComponentData component)
        {
            List<Double>[,] blocks = new List<Double>[component.ContiguousBlockCountHorizontal, component.ContiguousBlockCountVertical];
            Int32 indexer = 0;
            for (Int32 y = 0; y < component.ContiguousBlockCountVertical; ++y)
            {
                for (Int32 x = 0; x < component.ContiguousBlockCountHorizontal; ++x)
                {
                    blocks[x, y] = component.ComponentDecodedDataFloat.GetRange(indexer, 64);
                    indexer += 64;
                }
            }

            //Application has the unzigged data. Now it shall un-block the samples.
            List<Double> unblocked = new List<Double>();

            for (Int32 y = 0; y < component.ContiguousBlockCountVertical; ++y)
                for (Int32 blockScanline = 0; blockScanline < 8; ++blockScanline)
                    for (Int32 x = 0; x < component.ContiguousBlockCountHorizontal; ++x)
                        unblocked.AddRange(blocks[x, y].GetRange(blockScanline * 8, 8));

            component.ComponentDecodedDataFloat = unblocked;
        }

        /// <summary>Converts the Integer component data to floating point data</summary>
        /// <param name="component">Component to convert</param>
        private static void ConvertToFloatData(ScanComponentData component)
        {
            //convert to floating-point precision numbers
            List<Double> floatData = new List<Double>();
            for (Int32 i = 0; i < component.ComponentData.Count; ++i)
                floatData.Add(Convert.ToDouble(component.ComponentData[i]));

            component.ComponentDecodedDataFloat = floatData;
        }
        #endregion

        #region Integer decoding
        /// <summary>Decodes the data using integer representation between the IDCT and the RGB color space conversion</summary>
        public void DecodeInteger()
        {
            //Dequantize the DC, AC coefficients
            this.DecodeJpegFrameDataInteger();
        }

        /// <summary>Performs the mathematical transforms after de-coding from the input stream</summary>
        private void DecodeJpegFrameDataInteger()
        {
            foreach (JpegScan scan in this.Frame.Scans)
            {
                foreach (ScanComponentData component in scan.Components)
                {
                    //Undo the zig-zag coefficient before dequantizing and IDCT. Block order does not matter.
                    this.UnZigZag(component.ComponentData);

                    //re-order the blocks to fit the image grid
                    JpegJfifInterchange.ReorderBlockData(component);

                    //De-quantize the coefficients to reconstruct an approximate collection of forward DCT coefficients
                    this.Dequantize(component.ComponentData, component.QuantizationTableIndex);

                    //perform the IDCT
                    this.InverseDiscreteCosineTransformInteger(component);

                    //undo the 0-center shifting to restore back to 0 - 255 unsigned value range
                    JpegJfifInterchange.UndoLevelShift(this.Frame.Header.SamplePrecision, component.ComponentDecodedDataInteger);

                    //re-order the samples to form a true top-down image
                    this.ReorderBlockSampleDataInteger(component);
                }
            }
        }

        /// <summary>Performs the inverse DCT on the list of FDCT values</summary>
        /// <param name="fDctList">List of integer Forward DCT values</param>
        /// <remarks>See JPEG specification §A.3.3</remarks>
        private void InverseDiscreteCosineTransformInteger(ScanComponentData component)
        {
            //full mathmatical summation
            //component.ComponentDecodedDataInteger = DiscreteCosineTransformation.InverseDiscreteCosineTransformSlowInteger(component.ComponentData);
            component.ComponentDecodedDataInteger = DiscreteCosineTransformation.InverseDiscreteCosineTransformFastInteger(component.ComponentData);
        }

        /// <summary>Performs an unshift by adding the shift level to </summary>
        /// <param name="level">Sample bit precision</param>
        /// <param name="data">List of samples to unshift</param>
        private static void UndoLevelShift(Int32 level, List<Int32> data)
        {
            Int32 shift;
            switch (level)
            {
                case 8:
                    shift = 128;
                    break;
                case 12:
                    shift = 2048;
                    break;
                default:
                    throw new ApplicationException(String.Format("Unexpected level shift size of {0}.", level));
            }

            for (Int32 index = 0; index < data.Count; ++index)
                data[index] += shift;
        }

        /// <summary>Reorders the component data 8x8 blocks based on multiple vertical sampling levels</summary>
        /// <param name="componentData">Component to unshuffle</param>
        private void ReorderBlockSampleDataInteger(ScanComponentData component)
        {
            List<Int32>[,] blocks = new List<Int32>[component.ContiguousBlockCountHorizontal, component.ContiguousBlockCountVertical];
            Int32 indexer = 0;
            for (Int32 y = 0; y < component.ContiguousBlockCountVertical; ++y)
            {
                for (Int32 x = 0; x < component.ContiguousBlockCountHorizontal; ++x)
                {
                    blocks[x, y] = component.ComponentDecodedDataInteger.GetRange(indexer, 64);
                    indexer += 64;
                }
            }

            //Application has the unzigged data. Now it shall un-block the samples.
            List<Int32> unblocked = new List<Int32>();

            for (Int32 y = 0; y < component.ContiguousBlockCountVertical; ++y)
                for (Int32 blockScanline = 0; blockScanline < 8; ++blockScanline)
                    for (Int32 x = 0; x < component.ContiguousBlockCountHorizontal; ++x)
                        unblocked.AddRange(blocks[x, y].GetRange(blockScanline * 8, 8));

            component.ComponentDecodedDataInteger = unblocked;
        }
        #endregion
        #endregion


        /// <summary>Returns a Byte value from the sample provided</summary>
        /// <param name="sample">Datum to convert</param>
        /// <returns>Byte-clamped value of the sample provided</returns>
        protected static Byte ConvertSampleToByte(Double sample)
        {
            return sample < 0.0 ? (Byte)0 : sample > 255.0 ? (Byte)255 : Convert.ToByte(sample);
        }

        /// <summary>Sets up and populates the non-sample data of the components</summary>
        protected void PopulateComponents()
        {
            //set up component data
            this.ComponentData = new ComponentDataFloat[this.Frame.Header.ComponentNumbers];
            for (Int32 index = 0; index < this.Frame.Header.ComponentNumbers; ++index)
                this.ComponentData[index] = new ComponentDataFloat();

            Int32 hMax = this.Frame.Header.MaxHorizontalSamplingFactor, vMax = this.Frame.Header.MaxVerticalSamplingFactor;

            for (Int32 componentIndex = 0; componentIndex < this.Frame.Header.ComponentNumbers; ++componentIndex)
            {
                FrameComponentParameter fcp = this.MatchComponentPrameter(componentIndex + 1);

                if (fcp != null)
                {
                    this.ComponentData[componentIndex].Identifier = fcp.Identifier;
                    this.ComponentData[componentIndex].HorizontalSamplingFactor = fcp.HorizontalSamplingFactor;
                    this.ComponentData[componentIndex].VerticalSamplingFactor = fcp.VerticalSamplingFactor;

                    //width; JPEG spec. §A.1.1
                    Decimal factor = Convert.ToDecimal(fcp.HorizontalSamplingFactor) / Convert.ToDecimal(hMax);
                    factor = Convert.ToDecimal(this.Frame.Header.Width) * factor;
                    this.ComponentData[componentIndex].Width = Convert.ToInt32(Math.Ceiling(factor));

                    //height; JPEG spec. §A.1.1
                    factor = Convert.ToDecimal(fcp.VerticalSamplingFactor) / Convert.ToDecimal(vMax);
                    factor = Convert.ToDecimal(this.Frame.Header.Height) * factor;
                    this.ComponentData[componentIndex].Height = Convert.ToInt32(Math.Ceiling(factor));
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
    }
}