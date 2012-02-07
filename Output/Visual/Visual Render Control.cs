using System;
using System.Windows.Forms;

using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;

namespace Bardez.Projects.InfinityPlus1.Output.Visual
{
    /// <summary>
    ///     Represents an abstract User Control that allows a PixelData class to be sent to it for
    ///     Visual rendering.
    /// </summary>
    public abstract class VisualRenderControl : UserControl
    {
        /// <summary>Sets the frame to be rendered to the User Control</summary>
        /// <param name="data">Frame to add</param>
        public abstract void SetRenderFrame(Int32 key);

        /// <summary>Sets the frame to be rendered to the User Control</summary>
        /// <param name="data">Frame to add</param>
        /// <param name="originX">X coordinate to start drawing from</param>
        /// <param name="originY">Y coordinate to start drawing from</param>
        public abstract void SetRenderFrame(Int32 key, Int64 originX, Int64 originY);

        /// <summary>Adds a renderable frame to be resources manager for the User Control</summary>
        /// <param name="data">Frame to add</param>
        /// <returns>A unique key for the frame data to be referenced</returns>
        public abstract Int32 AddFrameResource(Frame data);
    }
}