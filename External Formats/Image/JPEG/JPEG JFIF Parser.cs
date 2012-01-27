using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Static class that will return a JPEG JFIF Interchange from an input stream.</summary>
    public static class JpegJfifParser
    {
        /// <summary>Reads a JPEG image from the input stream</summary>
        /// <param name="input">Input stream to read from</param>
        /// <returns>A fully populated JPEG Interchange object</returns>
        /// <exception cref="System.ApplicationException">Thrown when there has been an error in the stream.</exception>
        public static JpegJfifInterchange ParseJpegFromStream(Stream input, ResizeDelegateInteger resizeDelegate)
        {
            UInt16 marker = JpegParser.ReadMarker(input);
            if (marker != JpegInterchangeMarkerConstants.StartOfImage)
                throw new ApplicationException("File does not start with Start of Image marker.");

            JpegJfifInterchange jpeg = new JpegJfifInterchange(resizeDelegate);
            JpegJfifParser.ReadFrame(jpeg, input);

            marker = JpegParser.ReadMarker(input);
            if (marker != JpegInterchangeMarkerConstants.EndOfImage)
                throw new ApplicationException("File does not end with End of Image marker.");

            return jpeg;
        }

        #region Read methods
        /// <summary>Reads the "Frame" object of the JPG specification from the input stream</summary>
        /// <param name="jpeg">JPEG Interchange to populate</param>
        /// <param name="input">Input stream to read from</param>
        private static void ReadFrame(JpegJfifInterchange jpeg, Stream input)
        {
            JpegFrame frame = new JpegFrame();

            //Since this is JFIF, expect an App0 segment immediately.
            JpegJfifParser.ReadFrameJfifHeader(input, frame);

            //read any other tables, segments before the Start of Frame
            RestartInterval interval = frame.Restart;   //copy the interval reference
            UInt16 marker = JpegParser.ReadTablesAndMiscellaneous(input, frame.Miscellaneous, frame.QuantizationTables, frame.DcCodingTables, frame.AcCodingTables, ref interval);
            frame.Restart = interval;                   //re-assign the interval

            //I should now have a Start of the Frame marker.
            JpegFrameHeader header = new JpegFrameHeader(marker);
            header.Read(input);
            frame.Header = header;
            frame.ScanLines = frame.Header.Height;  //can be set to 0.

            //attach the frame
            jpeg.Frame = frame;

            //set up component data
            jpeg.PopulateComponents();

            //Read first scan
            JpegParser.ReadScan(jpeg, frame, input);

            //after reading one, there's an optional DNL. If not DNL, either more scans or end of image
            switch (JpegParser.ReadMarker(input))
            {
                //read DNL marker
                case JpegInterchangeMarkerConstants.DefineNumberOfLines:
                    JpegParser.ReadNumberOfLines(frame, input);
                    break;
                default:
                    ReusableIO.SeekIfAble(input, -2, SeekOrigin.Current);   //back up and read another scan; if EOI, it will be caught in the read loop.
                    break;
            }

            Boolean keepReading = true;

            //read additional scans
            while (keepReading)
            {
                switch (JpegParser.ReadMarker(input))
                {
                    case JpegInterchangeMarkerConstants.EndOfImage:
                        ReusableIO.SeekIfAble(input, -2, SeekOrigin.Current);   //back up return; EOI will be caught in the read JPEG method.
                        keepReading = false; //end of stream
                        break;
                    default:
                        ReusableIO.SeekIfAble(input, -2, SeekOrigin.Current);   //back up and read another scan; if EOI, it will be caught in the read loop.
                        JpegParser.ReadScan(jpeg, frame, input);  //inappropriate markers will be read and caught inside the ReadScan code.
                        break;
                }
            }
        }

        /// <summary>Reas the JFIF header for a JPEG Frame and stores it</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="frame">JPEG Frame to add data to</param>
        private static void ReadFrameJfifHeader(Stream input, JpegFrame frame)
        {
            UInt16 marker;

            try
            {
                marker = JpegParser.ReadMarker(input);
                if (marker != JpegInterchangeMarkerConstants.ApplicationSegment00)
                    throw new ApplicationException("Decoder was expecting the APP0 substream for a JFIF header.");

                //we have an App0 marker
                JfifHeader header = new JfifHeader();
                header.Marker = marker;

                //get size
                UInt16 size = JpegParser.ReadParameter16(input);
                header.Length = size;

                //read the rest
                header.Read(input, size);

                //assign the JFIF header to the Frame
                frame.Miscellaneous.Add(header);
                frame.HeaderJFIF = header;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was an issue while reading the JFIF APP0 substream.", ex);
            }
        }
        #endregion
    }
}