using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Contains a list of JPEG constants</summary>
    /// <remarks>Taken from Table B.1 of the JPEG specification</remarks>
    public static class JpegInterchangeMarkerConstants
    {
        /// <summary>Represents a marker for baseline DCT encoding</summary>
        /// <remarks>Known as SOF-sub-0 in JPEG spec</remarks>
        public const UInt16 StartOfFrameHuffmanBaselineDCT                      = 0xFFC0;

        /// <summary>Represents a marker for extended sequential DCT encoding</summary>
        /// <remarks>Known as SOF-sub-1 in JPEG spec</remarks>
        public const UInt16 StartOfFrameHuffmanExtendedSequentialDCT            = 0xFFC1;

        /// <summary>Represents a marker for progressive DCT encoding</summary>
        /// <remarks>Known as SOF-sub-2 in JPEG spec</remarks>
        public const UInt16 StartOfFrameHuffmanProgressiveDCT                   = 0xFFC2;

        /// <summary>Represents a marker for Lossless encoding</summary>
        /// <remarks>Known as SOF-sub-3 in JPEG spec</remarks>
        public const UInt16 StartOfFrameHuffmanLossless                         = 0xFFC3;

        //There is no SOF-sub-4. It is Define Huffman Table(s), really.

        /// <summary>Represents a marker for differential sequential DCT</summary>
        /// <remarks>Known as SOF-sub-5 in JPEG spec</remarks>
        public const UInt16 StartOfFrameHuffmanDifferentialSequentialDCT        = 0xFFC5;

        /// <summary>Represents a marker for differential progressive DCT encoding</summary>
        /// <remarks>Known as SOF-sub-6 in JPEG spec</remarks>
        public const UInt16 StartOfFrameHuffmanDifferentialProgressiveDCT       = 0xFFC6;

        /// <summary>Represents a marker for differential lossless encoding</summary>
        /// <remarks>Known as SOF-sub-7 in JPEG spec</remarks>
        public const UInt16 StartOfFrameHuffmanDifferentialLossless             = 0xFFC7;

        /// <summary>Represents a marker for JPEG extensions</summary>
        /// <remarks>Known as JPG in JPEG spec</remarks>
        public const UInt16 StartOfFrameArithmeticJpegExtension                 = 0xFFC8;

        /// <summary>Represents a marker for extended sequential DCT</summary>
        /// <remarks>Known as SOF-sub-9 in JPEG spec</remarks>
        public const UInt16 StartOfFrameArithmeticExtendedSequentialDCT         = 0xFFC9;

        /// <summary>Represents a marker for extended progressive DCT encoding</summary>
        /// <remarks>Known as SOF-sub-10 in JPEG spec</remarks>
        public const UInt16 StartOfFrameArithmeticProgressiveDCT                = 0xFFCA;

        /// <summary>Represents a marker for lossless encoding</summary>
        /// <remarks>Known as SOF-sub-11 in JPEG spec</remarks>
        public const UInt16 StartOfFrameArithmeticLossless                      = 0xFFCB;

        /// <summary>Represents a marker for differential sequential DCT</summary>
        /// <remarks>Known as SOF-sub-13 in JPEG spec</remarks>
        public const UInt16 StartOfFrameArithmeticDifferentialSequentialDCT     = 0xFFCD;

        /// <summary>Represents a marker for differential progressive DCT encoding</summary>
        /// <remarks>Known as SOF-sub-14 in JPEG spec</remarks>
        public const UInt16 StartOfFrameArithmeticDifferentialProgressiveDCT    = 0xFFCE;

        /// <summary>Represents a marker for differential lossless encoding</summary>
        /// <remarks>Known as SOF-sub-15 in JPEG spec</remarks>
        public const UInt16 StartOfFrameArithmeticDifferentialLossless          = 0xFFCF;


        /// <summary>Represents a marker for defining Huffman table(s)</summary>
        /// <remarks>Known as DHT in JPEG spec</remarks>
        public const UInt16 DefineHuffmanTable                                  = 0xFFC4;

        /// <summary>Represents a marker for defining arithmetic coding conditioning(s)</summary>
        /// <remarks>Known as DAC in JPEG spec</remarks>
        public const UInt16 DefineArithmeticCodingConditioning                  = 0xFFCC;


        /// <summary>Represents a marker for restarting an interval with modulo count of 0</summary>
        /// <remarks>Known as RST-sub-0 in JPEG spec</remarks>
        public const UInt16 RestartInterval0                                    = 0xFFD0;

        /// <summary>Represents a marker for restarting an interval with modulo count of 1</summary>
        /// <remarks>Known as RST-sub-1 in JPEG spec</remarks>
        public const UInt16 RestartInterval1                                    = 0xFFD1;

        /// <summary>Represents a marker for restarting an interval with modulo count of 2</summary>
        /// <remarks>Known as RST-sub-2 in JPEG spec</remarks>
        public const UInt16 RestartInterval2                                    = 0xFFD2;

        /// <summary>Represents a marker for restarting an interval with modulo count of 3</summary>
        /// <remarks>Known as RST-sub-3 in JPEG spec</remarks>
        public const UInt16 RestartInterval3                                    = 0xFFD3;

        /// <summary>Represents a marker for restarting an interval with modulo count of 4</summary>
        /// <remarks>Known as RST-sub-4 in JPEG spec</remarks>
        public const UInt16 RestartInterval4                                    = 0xFFD4;

        /// <summary>Represents a marker for restarting an interval with modulo count of 5</summary>
        /// <remarks>Known as RST-sub-5 in JPEG spec</remarks>
        public const UInt16 RestartInterval5                                    = 0xFFD5;

        /// <summary>Represents a marker for restarting an interval with modulo count of 6</summary>
        /// <remarks>Known as RST-sub-6 in JPEG spec</remarks>
        public const UInt16 RestartInterval6                                    = 0xFFD6;

        /// <summary>Represents a marker for restarting an interval with modulo count of 7</summary>
        /// <remarks>Known as RST-sub-7 in JPEG spec</remarks>
        public const UInt16 RestartInterval7                                    = 0xFFD7;

        /// <summary>Represents a marker for the start of the image</summary>
        /// <remarks>Known as SOI in JPEG spec</remarks>
        public const UInt16 StartOfImage                                        = 0xFFD8;

        /// <summary>Represents a marker for the end of the image</summary>
        /// <remarks>Known as EOI in JPEG spec</remarks>
        public const UInt16 EndOfImage                                          = 0xFFD9;

        /// <summary>Represents a marker for the start of a scan</summary>
        /// <remarks>Known as SOS in JPEG spec</remarks>
        public const UInt16 StartOfScan                                         = 0xFFDA;

        /// <summary>Represents a marker for defining quantization table(s)</summary>
        /// <remarks>Known as DQT in JPEG spec</remarks>
        public const UInt16 DefineQuantizationTable                             = 0xFFDB;

        /// <summary>Represents a marker for defining the number of lines</summary>
        /// <remarks>Known as DNL in JPEG spec</remarks>
        public const UInt16 DefineNumberOfLines                                 = 0xFFDC;

        /// <summary>Represents a marker for defining a restart interval</summary>
        /// <remarks>Known as DNL in JPEG spec</remarks>
        public const UInt16 DefineRestartInterval                               = 0xFFDD;

        /// <summary>Represents a marker for defining hierarchical progression</summary>
        /// <remarks>Known as DHP in JPEG spec</remarks>
        public const UInt16 DefineHierarchicalProgression                       = 0xFFDE;

        /// <summary>Represents a marker for expanding reference component(s)</summary>
        /// <remarks>Known as EXP in JPEG spec</remarks>
        public const UInt16 ExpandReferenceComponent                            = 0xFFDF;

        /// <summary>Represents a marker for application segment 0</summary>
        /// <remarks>Known as APP-sub-0 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment00                                = 0xFFE0;

        /// <summary>Represents a marker for application segment 1</summary>
        /// <remarks>Known as APP-sub-1 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment01                                = 0xFFE1;

        /// <summary>Represents a marker for application segment 2</summary>
        /// <remarks>Known as APP-sub-2 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment02                                = 0xFFE2;

        /// <summary>Represents a marker for application segment 3</summary>
        /// <remarks>Known as APP-sub-3 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment03                                = 0xFFE3;

        /// <summary>Represents a marker for application segment 4</summary>
        /// <remarks>Known as APP-sub-4 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment04                                = 0xFFE4;

        /// <summary>Represents a marker for application segment 5</summary>
        /// <remarks>Known as APP-sub-5 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment05                                = 0xFFE5;

        /// <summary>Represents a marker for application segment 6</summary>
        /// <remarks>Known as APP-sub-6 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment06                                = 0xFFE6;

        /// <summary>Represents a marker for application segment 7</summary>
        /// <remarks>Known as APP-sub-7 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment07                                = 0xFFE7;

        /// <summary>Represents a marker for application segment 8</summary>
        /// <remarks>Known as APP-sub-8 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment08                                = 0xFFE8;

        /// <summary>Represents a marker for application segment 9</summary>
        /// <remarks>Known as APP-sub-9 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment09                                = 0xFFE9;

        /// <summary>Represents a marker for application segment 10</summary>
        /// <remarks>Known as APP-sub-10 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment10                                = 0xFFEA;

        /// <summary>Represents a marker for application segment 11</summary>
        /// <remarks>Known as APP-sub-11 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment11                                = 0xFFEB;

        /// <summary>Represents a marker for application segment 12</summary>
        /// <remarks>Known as APP-sub-12 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment12                                = 0xFFEC;

        /// <summary>Represents a marker for application segment 13</summary>
        /// <remarks>Known as APP-sub-13 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment13                                = 0xFFED;

        /// <summary>Represents a marker for application segment 14</summary>
        /// <remarks>Known as APP-sub-14 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment14                                = 0xFFEE;

        /// <summary>Represents a marker for application segment 15</summary>
        /// <remarks>Known as APP-sub-15 in JPEG spec</remarks>
        public const UInt16 ApplicationSegment15                                = 0xFFEF;

        /// <summary>Represents a marker for JPEG extension 0</summary>
        /// <remarks>Known as JPG-sub-0 in JPEG spec</remarks>
        public const UInt16 JpegExtension00                                     = 0xFFF0;

        /// <summary>Represents a marker for JPEG extension 1</summary>
        /// <remarks>Known as JPG-sub-1 in JPEG spec</remarks>
        public const UInt16 JpegExtension01                                     = 0xFFF1;

        /// <summary>Represents a marker for JPEG extension 2</summary>
        /// <remarks>Known as JPG-sub-2 in JPEG spec</remarks>
        public const UInt16 JpegExtension02                                     = 0xFFF2;

        /// <summary>Represents a marker for JPEG extension 3</summary>
        /// <remarks>Known as JPG-sub-3 in JPEG spec</remarks>
        public const UInt16 JpegExtension03                                     = 0xFFF3;

        /// <summary>Represents a marker for JPEG extension 4</summary>
        /// <remarks>Known as JPG-sub-4 in JPEG spec</remarks>
        public const UInt16 JpegExtension04                                     = 0xFFF4;

        /// <summary>Represents a marker for JPEG extension 5</summary>
        /// <remarks>Known as JPG-sub-5 in JPEG spec</remarks>
        public const UInt16 JpegExtension05                                     = 0xFFF5;

        /// <summary>Represents a marker for JPEG extension 6</summary>
        /// <remarks>Known as JPG-sub-6 in JPEG spec</remarks>
        public const UInt16 JpegExtension06                                     = 0xFFF6;

        /// <summary>Represents a marker for JPEG extension 7</summary>
        /// <remarks>Known as JPG-sub-7 in JPEG spec</remarks>
        public const UInt16 JpegExtension07                                     = 0xFFF7;

        /// <summary>Represents a marker for JPEG extension 8</summary>
        /// <remarks>Known as JPG-sub-8 in JPEG spec</remarks>
        public const UInt16 JpegExtension08                                     = 0xFFF8;

        /// <summary>Represents a marker for JPEG extension 9</summary>
        /// <remarks>Known as JPG-sub-9 in JPEG spec</remarks>
        public const UInt16 JpegExtension09                                     = 0xFFF9;

        /// <summary>Represents a marker for JPEG extension 10</summary>
        /// <remarks>Known as JPG-sub-10 in JPEG spec</remarks>
        public const UInt16 JpegExtension10                                     = 0xFFFA;

        /// <summary>Represents a marker for JPEG extension 11</summary>
        /// <remarks>Known as JPG-sub-11 in JPEG spec</remarks>
        public const UInt16 JpegExtension11                                     = 0xFFFB;

        /// <summary>Represents a marker for JPEG extension 12</summary>
        /// <remarks>Known as JPG-sub-12 in JPEG spec</remarks>
        public const UInt16 JpegExtension12                                     = 0xFFFC;

        /// <summary>Represents a marker for JPEG extension 13</summary>
        /// <remarks>Known as JPG-sub-13 in JPEG spec</remarks>
        public const UInt16 JpegExtension13                                     = 0xFFFD;

        /// <summary>Represents a marker for comment(s)</summary>
        /// <remarks>Known as COM in JPEG spec</remarks>
        public const UInt16 Comment                                             = 0xFFFE;
    }
}