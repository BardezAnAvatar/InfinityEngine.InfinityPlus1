using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a JPEG specification Frame, coded as JFIF interchange</summary>
    /// <remarks>MUST Contain a JFIF App0 first in table/markers or else it is not JFIF.</remarks>
    public class JpegFrame : ContainerSegment
    {
        #region Fields
        /// <summary>The JFIF App0 header</summary>
        public JfifHeader HeaderJFIF { get; set; }

        /// <summary>The JPEG Frame header</summary>
        public JpegFrameHeader Header { get; set; }

        /// <summary>The installation location of quantization tables</summary>
        protected QuantizationTable[] quantizationTables;

        /// <summary>The installation location of DC coefficient coding tables</summary>
        protected GenericCodingTable[] dcCodingTables;

        /// <summary>The installation location of AC coefficient coding tables</summary>
        protected GenericCodingTable[] acCodingTables;

        /// <summary>The collection of scans</summary>
        protected List<JpegScan> scans;

        /// <summary>The number of lines in the frame</summary>
        public UInt16 ScanLines { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the installation location of quantization tables</summary>
        public QuantizationTable[] QuantizationTables
        {
            get { return this.quantizationTables; }
        }

        /// <summary>Exposes installation location of DC coefficient coding tables</summary>
        public GenericCodingTable[] DcCodingTables
        {
            get { return this.dcCodingTables; }
        }

        /// <summary>Exposes installation location of AC coefficient coding tables</summary>
        public GenericCodingTable[] AcCodingTables
        {
            get { return this.acCodingTables; }
        }

        /// <summary>Exposes the collection of scans</summary>
        public List<JpegScan> Scans
        {
            get { return this.scans; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public JpegFrame()
        {
            this.Initialize();
            this.HeaderJFIF = null;
        }

        /// <summary>Initializes Lists</summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.scans = new List<JpegScan>();

            //per spec, there can be 4 tables, and each one is redefinable
            this.quantizationTables = new QuantizationTable[] { null, null, null, null };
            this.dcCodingTables = new GenericCodingTable[] { null, null, null, null };
            this.acCodingTables = new GenericCodingTable[] { null, null, null, null };
        }
        #endregion

        //contains the Number of lines 
    }
}