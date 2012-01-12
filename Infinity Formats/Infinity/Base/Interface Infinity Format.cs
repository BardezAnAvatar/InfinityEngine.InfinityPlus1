using System;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base
{
    /// <summary>Interface dictating I/O <see cref="System.IO.Stream"/> methods for reading and writing the associated data</summary>
    public interface IInfinityFormat
    {
        /// <summary>Instantiates reference types</summary>
        void Initialize();

        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the input stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        void Read(Stream input);

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        void Read(Stream input, Boolean fullRead);

        /// <summary>This public method reads file format from the input stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        void ReadBody(Stream input);

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        void Write(Stream output);
        #endregion
    }
}