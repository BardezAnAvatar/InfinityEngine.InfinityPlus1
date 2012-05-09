using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Components
{
    public class FrameData
    {
        #region Fields
        /// <summary>Audio of the frame</summary>
        public AudioPacket Audio { get; set; }

        /// <summary>Video packet of the frame</summary>
        public VideoPacket Video { get; set; }
        #endregion
    }
}
