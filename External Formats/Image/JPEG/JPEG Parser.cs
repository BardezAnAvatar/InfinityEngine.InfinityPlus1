using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Static class that reads JPEG data from an input stream.</summary>
    public static class JpegParser
    {
        #region Read methods
        /// <summary>Reads the tables an miscelaneous sub streams before a larger segment</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="miscellaneous">Miscellaneous Marker Segments collection to add to</param>
        /// <param name="quantizationTables">Quantization tables to store to</param>
        /// <param name="dcTables">DC coefficient coding tables installation location</param>
        /// <param name="acTables">AC coefficient coding tables installation location</param>
        /// <param name="interval">Restart interval, in case defined during read.</param>
        /// <returns>The latest start of Frame </returns>
        public static UInt16 ReadTablesAndMiscellaneous(Stream input, List<MarkerSegment> miscellaneous, QuantizationTable[] quantizationTables, GenericCodingTable[] dcTables, GenericCodingTable[] acTables, ref RestartInterval interval)
        {
            UInt16 marker = 0;

            try
            {
                Boolean readMarkers = true;     //read any further Miscellany valid marker segments
                while (readMarkers)
                {
                    marker = JpegParser.ReadMarker(input);
                    UInt16 length;
                    MarkerSegmentSubStream stream;

                    switch (marker)
                    {
                        //App segments
                        case JpegInterchangeMarkerConstants.ApplicationSegment00:   //App0 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment01:   //App1 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment02:   //App2 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment03:   //App3 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment04:   //App4 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment05:   //App5 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment06:   //App6 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment07:   //App7 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment08:   //App8 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment09:   //App9 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment10:   //App10 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment11:   //App11 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment12:   //App12 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment13:   //App13 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment14:   //App14 segment
                        case JpegInterchangeMarkerConstants.ApplicationSegment15:   //App15 segment
                            length = JpegParser.ReadParameter16(input);
                            stream = new MarkerSegmentSubStream(marker, length);
                            stream.Read(input);
                            miscellaneous.Add(stream);
                            break;
                        case JpegInterchangeMarkerConstants.Comment:                //Comment
                            length = JpegParser.ReadParameter16(input);
                            stream = new MarkerSegmentSubStream(marker, length);
                            stream.Read(input);
                            miscellaneous.Add(stream);
                            break;
                        case JpegInterchangeMarkerConstants.DefineRestartInterval:  //Define restart interval
                            length = JpegParser.ReadParameter16(input);
                            UInt16 rstInterval = JpegParser.ReadParameter16(input);
                            interval = new RestartInterval(marker, length, rstInterval);
                            miscellaneous.Add(interval);
                            break;
                        case JpegInterchangeMarkerConstants.DefineQuantizationTable:
                            length = JpegParser.ReadParameter16(input);
                            GenericTableSegment<QuantizationTable> qtSegment = new GenericTableSegment<QuantizationTable>(marker, length);
                            qtSegment.Read(input, quantizationTables);  //reads and sets the quantization table(s)
                            miscellaneous.Add(qtSegment);               //attach quantization table
                            break;
                        case JpegInterchangeMarkerConstants.DefineHuffmanTable:
                            length = JpegParser.ReadParameter16(input);
                            GenericCodingTableSegment<HuffmanTable> huffmanSegment = new GenericCodingTableSegment<HuffmanTable>(marker, length);
                            huffmanSegment.Read(input, dcTables, acTables);  //reads and sets the Huffman table(s)
                            miscellaneous.Add(huffmanSegment);          //attach quantization table
                            break;
                        case JpegInterchangeMarkerConstants.DefineArithmeticCodingConditioning:
                            throw new NotImplementedException("Not widely used in JPEG/JFIF files.");
                        case JpegInterchangeMarkerConstants.StartOfFrameHuffmanBaselineDCT:                     //SOF-0
                        case JpegInterchangeMarkerConstants.StartOfFrameHuffmanExtendedSequentialDCT:           //SOF-1
                        case JpegInterchangeMarkerConstants.StartOfFrameHuffmanProgressiveDCT:                  //SOF-2
                        case JpegInterchangeMarkerConstants.StartOfFrameHuffmanLossless:                        //SOF-3
                        case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialSequentialDCT:       //SOF-5
                        case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialProgressiveDCT:      //SOF-6
                        case JpegInterchangeMarkerConstants.StartOfFrameHuffmanDifferentialLossless:            //SOF-7
                        case JpegInterchangeMarkerConstants.StartOfFrameArithmeticJpegExtension:                //JPG
                        case JpegInterchangeMarkerConstants.StartOfFrameArithmeticExtendedSequentialDCT:        //SOF-9
                        case JpegInterchangeMarkerConstants.StartOfFrameArithmeticProgressiveDCT:               //SOF-10
                        case JpegInterchangeMarkerConstants.StartOfFrameArithmeticLossless:                     //SOF-11
                        case JpegInterchangeMarkerConstants.StartOfFrameArithmeticDifferentialSequentialDCT:    //SOF-13
                        case JpegInterchangeMarkerConstants.StartOfFrameArithmeticDifferentialProgressiveDCT:   //SOF-14
                        case JpegInterchangeMarkerConstants.StartOfFrameArithmeticDifferentialLossless:         //SOF-15
                        case JpegInterchangeMarkerConstants.StartOfScan:                                        //SOS
                            readMarkers = false;
                            break;
                        default:
                            throw new ApplicationException("The marker read was not a valid marker.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was an issue while reading the miscellaneous marker segments.", ex);
            }

            //pass back the latest marker found
            return marker;
        }

        /// <summary>Reads a minimum coded unit from the coded stream specified in scan's components</summary>
        /// <param name="usesDCT">Boolean indicating if the compression is a DCT approach</param>
        /// <param name="isProgressive">Flag indicating whether the MCU will be for a progressive process</param>
        /// <param name="scan">Scan to add/read from</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        /// <param name="zeroRunEndOfBand">Count of zero-run End of Band data units/MCUs to append to the data stream</param>
        /// <returns>The MinumumCodedUnit read</returns>
        public static MimimumCodedUnit ReadMinimumCodedUnit(Boolean usesDCT, Boolean isProgressive, JpegScan scan, ref Boolean halt, out Int32 zeroRunEndOfBand)
        {
            MimimumCodedUnit mcu = null;
            zeroRunEndOfBand = 0;

            if (usesDCT) //read an interleave of blocks
            {
                DctMcu dct = new DctMcu();

                foreach (ScanComponentData component in scan.Components)
                {
                    //read each component's data
                    for (Int32 componentDataUnit = 0; componentDataUnit < component.McuDataSize; ++componentDataUnit)
                    {
                        Int32[] dataUnit;
                        if (isProgressive)
                            dataUnit = component.DecodeProgressiveBlock(ref halt, scan.Header.StartSelector, scan.Header.EndSelector, out zeroRunEndOfBand);
                        else
                            dataUnit = component.DecodeSequentialBlock(ref halt);

                        dct.DataUnits.Add(dataUnit);
                        //component.ComponentData.AddRange(dataUnit);

                        if (halt)   //escape condition
                            return dct;
                    }
                }

                mcu = dct;
            }
            else                //read an interleave of samples
            {
                throw new NotImplementedException("DCT is the only currently supported method of JPEG coding.");
            }

            return mcu;
        }
        
        /// <summary>Reads the tables an miscelaneous sub streams before a larger segment</summary>
        /// <param name="frame">JPEG Frame to add data to</param>
        /// <param name="input">Input stream to read from</param>
        public static void ReadScan(JpegFrame frame, Stream input)
        {
            //create a new scan
            JpegScan scan = new JpegScan();

            //read any miscellaneous tables, &ct.
            RestartInterval restart = scan.Restart;     //copy the interval reference
            UInt16 marker = JpegParser.ReadTablesAndMiscellaneous(input, scan.Miscellaneous, frame.QuantizationTables, frame.DcCodingTables, frame.AcCodingTables, ref restart);
            frame.Restart = restart;                    //re-assign the interval

            //miscellaneous now read. Should have a Start of Scan (SOS) marker, now. This would be the scan header.
            JpegScanHeader header = new JpegScanHeader(marker);
            header.Read(input);
            scan.Header = header;

            //Read the entropy-coded segments.
            //If a restart interval is defined, inter-read the ECSs and restart intervals. Otherwise, read one very large entropy-coded segment
            Int32 mcuCount = JpegParser.MinumumCodedUnitCount(header, frame.Header);
            Int32 intervalCount = mcuCount > 0 ? mcuCount : Int32.MaxValue; //default value for ECS size

            //Retrieve the interval count
            if (frame.Restart != null)
                intervalCount = frame.Restart.Interval;

            if (scan.Restart != null)
                intervalCount = scan.Restart.Interval;

            //set up the entropy-decoding byte buffers per channel/component
            foreach (ScanComponentParameter scanParam in header.Components)
            {
                ScanComponentData componentData = new ScanComponentData();
                componentData.PopulateFields(scanParam, JpegParser.MatchScanComponent(scanParam, frame.Header.Components), frame.Header);
                scan.Components.Add(componentData);
            }

            //reset decoders.
            JpegParser.BuildDecoders(input, frame, scan);

            //read the entropy-coded segments
            JpegParser.ReadScanDecodeEntropySegments(scan, intervalCount, frame.Header.UsesDCT, frame.Header.UsesProgressive, input);
            frame.Scans.Add(scan);
        }

        /// <summary>Reads a scan's Entropy-coded segments</summary>
        /// <param name="scan">Scan to add to</param>
        /// <param name="intervalCount">Entropy-coded segment interval</param>
        /// <param name="isDct">Flag indicating whether the coding process was DCT</param>
        /// <param name="isProgressive">Flag indicating whether the MCU will be for a progressive process</param>
        /// <param name="input">Input stream to read from</param>
        public static void ReadScanDecodeEntropySegments(JpegScan scan, Int32 intervalCount, Boolean isDct, Boolean isProgressive, Stream input)
        {
            Boolean halt = false;   //shall we stop reading the scan?

            //prime and read
            JpegParser.ReadEntropyCodedSegment(scan, intervalCount, isDct, isProgressive, ref halt);

            while (!halt)
            {
                //Read the restart marker
                JpegParser.ReadScanRestartMarker(scan.Restarts, input, ref halt);
                if (halt)
                    break;

                //read ECS
                JpegParser.ReadEntropyCodedSegment(scan, intervalCount, isDct, isProgressive, ref halt);
            }
        }

        /// <summary>Reads a single entropy-coded segment from the scan and adds its contents to the various scan destinations</summary>
        /// <param name="scan">Scan to add the data to</param>
        /// <param name="intervalCount">count of MCUs in the entropy-coded segment</param>
        /// <param name="isDct">Flag indicating whether the MCU will be for a DCT process</param>
        /// <param name="isProgressive">Flag indicating whether the MCU will be for a progressive process</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate up the stack,
        ///     returning work done so far, but ultimately terminating the scan
        /// </param>
        public static void ReadEntropyCodedSegment(JpegScan scan, Int32 intervalCount, Boolean isDct, Boolean isProgressive, ref Boolean halt)
        {
            EntropyCodedSegment ecs = new EntropyCodedSegment();

            //reset component decoding DC predictions
            JpegParser.ResetScanComponentDecoderValues(scan);

            Int32 zeroRun = 0;  //debug variable

            //read an entropy-coded segment
            for (Int32 intervalIndex = 0; intervalIndex < intervalCount; ++intervalIndex)
            {
                //read an MCU
                Int32 zeroRunEndOfBand = 0;
                MimimumCodedUnit mcu = JpegParser.ReadMinimumCodedUnit(isDct, isProgressive, scan, ref halt, out zeroRunEndOfBand);

                ecs.MimimumCodedUnits.Add(mcu);

                if (halt)   //escape condition
                {
                    //add segment
                    scan.EntropySegments.Add(ecs);
                    return;
                }

                zeroRun += zeroRunEndOfBand;

                //if we encountered EOB > 0... it has to be an AC progressive by definition.
                if (zeroRunEndOfBand > 0 && isProgressive && scan.Components.Count == 1 && scan.Header.StartSelector != 0)
                {
                    Int32 remainingMCUs = intervalCount - (intervalIndex + 1); //plus one because we just read one.
                    zeroRunEndOfBand = zeroRunEndOfBand > remainingMCUs ? remainingMCUs : zeroRunEndOfBand; //Don't throw in too many MCUs; respect any upcoming restarts

                    //add this many blank MCUs to the Entropy Coded Segment.
                    for (Int32 runIndex = 0; runIndex < zeroRunEndOfBand; ++runIndex)
                    {
                        if (isDct)
                            ecs.MimimumCodedUnits.Add(new DctMcu(scan.Header.StartSelector, scan.Header.EndSelector));
                        else
                            throw new ApplicationException("Not sure what to do in a non-DCT situation.");
                    }

                    intervalIndex += zeroRunEndOfBand;
                }
            }

            //add segment
            scan.EntropySegments.Add(ecs);
        }

        /// <summary>Reads a restart marker (16 bits) from the input stream</summary>
        /// <param name="restartMarkers">List of Restart markers to add to</param>
        /// <param name="input">Input stream to read from</param>
        /// <param name="halt">
        ///     Reference flag set to false that, if set in this method, will percolate upwards indicating that the return value is possibly dirty,
        ///     and the stack to be traversed upward, returning work done so far, but ultimately terminating the scan
        /// </param>
        public static void ReadScanRestartMarker(List<RestartMarker> restartMarkers, Stream input, ref Boolean halt)
        {
            UInt16 marker = JpegParser.ReadMarker(input);

            switch (marker)
            {
                case JpegInterchangeMarkerConstants.RestartInterval0:
                case JpegInterchangeMarkerConstants.RestartInterval1:
                case JpegInterchangeMarkerConstants.RestartInterval2:
                case JpegInterchangeMarkerConstants.RestartInterval3:
                case JpegInterchangeMarkerConstants.RestartInterval4:
                case JpegInterchangeMarkerConstants.RestartInterval5:
                case JpegInterchangeMarkerConstants.RestartInterval6:
                case JpegInterchangeMarkerConstants.RestartInterval7:
                    break;
                default:
                    ReusableIO.SeekIfAble(input, -2, SeekOrigin.Current);   //back up to re-read the marker
                    halt = true;    //escape condition
                    return;
            }

            RestartMarker restart = new RestartMarker(marker);
            restartMarkers.Add(restart);
        }

        /// <summary>Reads the DNL marker parameters from the input stream.</summary>
        /// <param name="frame">Frame whose scan line count to set</param>
        /// <param name="input">Input stream to read from</param>
        public static void ReadNumberOfLines(JpegFrame frame, Stream input)
        {
            UInt16 length = JpegParser.ReadParameter16(input);
            UInt16 lines = JpegParser.ReadParameter16(input);
            frame.ScanLines = lines;
        }

        /// <summary>Resets JPEG scan coding information for all components</summary>
        /// <param name="scan">Scan for whose components to reset the coder values for</param>
        public static void ResetScanComponentDecoderValues(JpegScan scan)
        {
            if (scan.Components != null && scan.Components.Count > 0)
                scan.Components[0].DcCoder.ResetCodingStream();  //since the coding stream is shared, one call will do.

            foreach (ScanComponentData component in scan.Components)
                component.DcPrediction = 0;
        }
        #endregion


        #region IO methods
        /// <summary>Reads a JPEG 'marker' from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <returns>The marker, coded as a UInt16</returns>
        /// <remarks>
        ///     JPEG specifies, in Annex B.1.1.2:
        ///     "Any marker may optionally be preceded by any number of fill bytes, which are bytes assigned code [0xFF]."
        ///     This means that if the first byte and the second byte are both 0xFF, discard the first byte, recursing on the result.
        ///     
        ///     Also, this method should be called when the application expects a marker. This could be if an 0xFF escape sequence
        ///     was already read (since this method will prepend 0xFF) or it is expected to be the next byte.
        /// </remarks>
        public static UInt16 ReadMarker(Stream input)
        {
            Int32 marker = 0xFF;

            while (marker == 0xFF)
                marker = input.ReadByte();

            //in case reading failed for some reason
            if (marker < 0)
                throw new ApplicationException("Expected a value to read!");

            Byte[] data = new Byte[] { 0xFF, Convert.ToByte(marker) };

            return ReusableIO.ReadUInt16FromArray(data, 0, Endianness.BigEndian);
        }

        /// <summary>Reads a 16-bit Big Endian parameter from the input Stream</summary>
        /// <param name="input">Input stream to read from</param>
        /// <returns>The parameter as a UInt16</returns>
        public static UInt16 ReadParameter16(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, 2);
            return ReusableIO.ReadUInt16FromArray(data, 0, Endianness.BigEndian);
        }

        /// <summary>Reads an 8-bit Big Endian parameter from the input Stream</summary>
        /// <param name="input">Input stream to read from</param>
        /// <returns>The parameter as a Byte</returns>
        public static Byte ReadParameter8(Stream input)
        {
            Byte[] data = ReusableIO.BinaryRead(input, 1);
            return data[0];
        }

        /// <summary>Reads two 4-bit Big Endian parameters from the input Stream</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="param1">First output parameter</param>
        /// <param name="param2">Second output parameter</param>
        /// <returns>The parameter as a Byte</returns>
        public static void ReadParameters4(Stream input, out Byte param1, out Byte param2)
        {
            Byte[] data = ReusableIO.BinaryRead(input, 1);
            param1 = (Byte)((data[0] & 0xF0) >> 4);
            param2 = (Byte)(data[0] & 0x0F);
        }
        #endregion


        #region Helper Methods
        /// <summary>Gets the number of MCUs in a given scan.</summary>
        /// <param name="scanHeader">Scan's header</param>
        /// <param name="frameHeader">Scan's frame header</param>
        /// <returns>The MCU count within the sca</returns>
        private static Int32 MinumumCodedUnitCount(JpegScanHeader scanHeader, JpegFrameHeader frameHeader)
        {
            //if there is exactly one component, it is non-interleaved. Otherwise, it is interleaved.
            Boolean interleaved = scanHeader.ComponentNumbers > 1;
            Int32 mcuCount = 0;

            if (interleaved)
            {
                Int32 maxDensity = JpegParser.MaxSamplingFactorDensity(scanHeader, frameHeader);

                if (frameHeader.UsesDCT)
                    mcuCount = (frameHeader.ContiguousBlockCount / maxDensity) + ((frameHeader.ContiguousBlockCount % maxDensity) == 0 ? 0 : 1);
                else    //lossless
                {
                    //determine the ratio of channel sample precision to one another. This will give an MCU afterward.
                    //Refer to §A.2.3...

                    //basically, get the multiplied sampling factor. the highest multiple will be the most pixels per MCU, which will determine the MCU count
                    Int32 mcuPerBlock = (64 / maxDensity) + ((64 % maxDensity) == 0 ? 0 : 1);
                    mcuCount = mcuPerBlock * frameHeader.ContiguousBlockCount;
                }
            }
            else    //non-interleaved; 8x8 grid is one MCU.
                mcuCount = frameHeader.ContiguousBlockCountHorizontal * frameHeader.ContiguousBlockCountVertical;

            return mcuCount;
        }

        /// <summary>Gets the maximum sampling factor density of the frame (horizontal sampling factor times vertical sampling factor)</summary>
        /// <param name="scanHeader">Scan header</param>
        /// <param name="frameHeader">Scan's frame header</param>
        private static Int32 MaxSamplingFactorDensity(JpegScanHeader scanHeader, JpegFrameHeader frameHeader)
        {
            Int32 density = 0;

            //loop through scan components
            foreach (ScanComponentParameter component in scanHeader.Components)
            {
                //get matching frame component
                FrameComponentParameter frameComponent = JpegParser.MatchScanComponent(component, frameHeader.Components);
                Int32 newDensity = frameComponent.HorizontalSamplingFactor * frameComponent.VerticalSamplingFactor;
                if (newDensity > density)
                    density = newDensity;
            }

            return density;
        }

        /// <summary>Gets the maximum sampling factor density of the frame (horizontal sampling factor times vertical sampling factor)</summary>
        /// <param name="scanComponent">Scan component</param>
        /// <param name="frameComponents">Frame components list</param>
        private static FrameComponentParameter MatchScanComponent(ScanComponentParameter scanComponent, List<FrameComponentParameter> frameComponents)
        {
            FrameComponentParameter component = null;

            foreach (FrameComponentParameter fc in frameComponents)
            {
                if (fc.Identifier == scanComponent.Identifier)
                {
                    component = fc;
                    break;
                }
            }

            return component;
        }

        /// <summary>Builds stream decoders for each scan component</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="frame">JpegFrame to reference tables from</param>
        /// <param name="scan">Scan to reference from</param>
        private static void BuildDecoders(Stream input, JpegFrame frame, JpegScan scan)
        {
            if (frame.Header.UsesHuffmanCoding)
            {
                HuffmanBitReader reader = new HuffmanBitReader(input);

                for (Int32 index = 0; index < scan.Components.Count; ++index)
                {
                    Int32 dcTableIndex = scan.Components[index].IndexDC, acTableIndex = scan.Components[index].IndexAC;
                    scan.Components[index].DcCoder = HuffmanCoder.BuildHuffmanDecoder(reader, frame.DcCodingTables[dcTableIndex] as HuffmanTable);
                    scan.Components[index].AcCoder = HuffmanCoder.BuildHuffmanDecoder(reader, frame.AcCodingTables[acTableIndex] as HuffmanTable);
                }
            }
            else
            {
                throw new NotImplementedException("Arithmetic entropy decoding not yet supported.");
            }
        }
        #endregion
    }
}