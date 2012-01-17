using System;
using System.Collections.Generic;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a single pass through the data for one or more of the components in an image.</summary>
    public class JpegScan : ContainerSegment
    {
        #region Fields
        /// <summary>Represents the can header within this scan segment</summary>
        public JpegScanHeader Header { get; set; }

        /// <summary>Represents the collection of entroy-coded segments.</summary>
        protected List<EntropyCodedSegment> entropySegments;

        /// <summary>Represents the list of restart intervals</summary>
        protected List<RestartMarker> restarts;

        /// <summary>Represents the collection of entropy-decoded components</summary>
        protected List<ScanComponentData> components;
        #endregion


        #region Construction
        /// <summary>Initializes Lists</summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.entropySegments = new List<EntropyCodedSegment>();
            this.restarts = new List<RestartMarker>();
            this.components = new List<ScanComponentData>();
        }
        #endregion


        #region Properties
        /// <summary>Exposes the collection of entroy-coded segments.</summary>
        public List<EntropyCodedSegment> EntropySegments
        {
            get { return this.entropySegments; }
        }

        /// <summary>Exposes the list of restart intervals</summary>
        public List<RestartMarker> Restarts
        {
            get { return this.restarts; }
        }

        /// <summary>Exposes the collection of entropy-decoded components</summary>
        public List<ScanComponentData> Components
        {
            get { return this.components; }
        }
        #endregion
    }
}