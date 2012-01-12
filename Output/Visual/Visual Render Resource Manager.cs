using System;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Image;

namespace Bardez.Projects.InfinityPlus1.Output.Visual
{
    /// <summary>
    ///     Abstract class that allows PixelData resource posting to a resource Manager
    ///     that presumably sends the data to the video hardware / software manager
    /// </summary>
    public abstract class VisualRenderResourceManager
    {
        /// <summary>Posts a Frame resource to the resource manager and returns a unique key to access it.</summary>
        /// <param name="resource">Frame to be posted.</param>
        /// <returns>A unique Int32 key</returns>
        public abstract Int32 AddFrameResource(Frame resource);
    }
}