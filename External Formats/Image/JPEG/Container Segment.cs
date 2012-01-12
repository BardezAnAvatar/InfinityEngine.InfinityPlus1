using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Base class for JpegFrame and JpegScan container marker segment types</summary>
    public abstract class ContainerSegment
    {
        #region Fields
        /// <summary>An in-order list of Frame miscellaneous marker segments.</summary>
        public List<MarkerSegment> Miscellaneous { get; set; }

        /// <summary>The MCU interval at which restarts should appear</summary>
        public RestartInterval Restart { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ContainerSegment()
        {
            this.Initialize();
        }

        /// <summary>Initializes Lists</summary>
        protected virtual void Initialize()
        {
            this.Miscellaneous = new List<MarkerSegment>();
        }
        #endregion
    }
}