using System;
using System.IO;

using Bardez.Projects.Multimedia.MediaBase.Render;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>Creates and returns an instance of an IMovie based on an input stream</summary>
    public static class MultimediaFactory
    {
        /// <summary>Builds an A/V rendering manager from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>A multimedia rendering manager</returns>
        public static IAVStreamRenderManager BuildMultimedia(Stream input)
        {
            IAVStreamRenderManager renderer = null;

            return renderer;
        }
    }
}