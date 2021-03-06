using System;
using System.Windows.Forms;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;

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
        public abstract Int32 AddFrameResource(IMultimediaImageFrame data);

        /// <summary>Frees a Bitmap resource in the resource manager and Disposes of it.</summary>
        /// <param name="frameKey">Direct2D Bitmap key to be Disposed.</param>
        public abstract void FreeFrameResource(Int32 frameKey);
    }
}