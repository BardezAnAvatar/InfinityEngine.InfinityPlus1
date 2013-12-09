using System;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;

namespace Bardez.Projects.Factories
{
    /// <summary>Factory that will return an IPixelConverter</summary>
    public static class IPixelConverterFactory
    {
        /// <summary>Gets an instance of an IPixelConverter</summary>
        /// <returns>An instance of an IPixelConverter</returns>
        public static IPixelConverter GetIPixelConverter()
        {
            //TODO: use some sort of configuration to figure out if I want to use this or LibAV or what
            return new BasicPixelConverter();
        }
    }
}