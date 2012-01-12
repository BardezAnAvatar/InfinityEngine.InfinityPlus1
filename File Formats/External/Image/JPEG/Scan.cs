using System;
using System.Collections.Generic;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
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

        /// <summary>Exposes the entropy-decoded data length of a single MCU.</summary>
        [Obsolete("block for DCT, fix!")]
        public Int32 MinimumCodedUnitSize
        {
            get
            {
                //if there is exactly one component, it is non-interleaved. Otherwise, it is interleaved.
                Boolean interleaved = this.Header.ComponentNumbers > 1;
                Int32 mcuSize = 64; //non-interleaved

                if (interleaved)
                {
                    mcuSize = 0;    //reset

                    foreach (ScanComponentData data in this.components)
                        mcuSize += data.McuDataSize;
                }

                return mcuSize;
            }
        }
        #endregion
    }
}