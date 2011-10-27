using System;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component
{
    public class RiffFile
    {
        /// <summary>Main Riff chunk</summary>
        protected RiffThroneChunk header;

        /// <summary>Main Riff chunk</summary>
        public RiffThroneChunk Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.header = new RiffThroneChunk(input);
            this.header.ReadChunkHeader();
            this.header.Read();
        }
    }
}