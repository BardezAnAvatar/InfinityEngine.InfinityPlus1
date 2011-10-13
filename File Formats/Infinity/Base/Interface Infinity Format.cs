using System;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Base
{
    public interface IInfinityFormat
    {
        /// <summary>Instantiates reference types</summary>
        void Initialize();

        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the output stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        void Read(Stream input);

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        void Read(Stream input, Boolean fullRead);

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        void ReadBody(Stream input);

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        void Write(Stream output);
        #endregion
    }
}