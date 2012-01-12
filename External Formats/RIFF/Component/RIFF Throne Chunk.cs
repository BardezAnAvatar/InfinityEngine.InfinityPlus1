using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component
{
    /// <summary>Main RIFF chunk, a super-chunk, indicating the sub-type of the RIFF data file (WAVE, AVI, etc.), containing all the RIFF sub-chunks</summary>
    public class RiffThroneChunk : RiffSuperChunk
    {
        /// <summary>Four character code indicating the type of RIFF content</summary>
        protected ChunkType type;

        /// <summary>Four character code indicating the type of RIFF content</summary>
        public ChunkType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        #region Construction
        /// <summary>Default constructor</summary>
        public RiffThroneChunk() : base() { }

        /// <summary>Stream constructor</summary>
        /// <param name="input">Stream to read from.</param>
        public RiffThroneChunk(Stream input) : base(input) { }

        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        public RiffThroneChunk(ChunkType type) : base(type) { }

        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        /// <param name="input">Stream to read from.</param>
        public RiffThroneChunk(ChunkType type, Stream input) : base(type, input) { }
        #endregion

        /// <summary>This public method reads the RIFF chunk from the data stream. Reads sub-chunks.</summary>
        public override void Read()
        {
            if (this.size > 3U)
            {
                //read extended data
                ReusableIO.SeekIfAble(this.dataStream, this.dataOffset);
                this.type = this.ReadFourCC();

                //should be at first sub-chunk. Read chunks.
                try
                {
                    while ((this.dataStream.Position + 4U) < this.size)
                    {
                        //read the chunk
                        this.ReadSubChunk();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred while reading sub-chunks.", ex);
                }
            }
        }

        /// <summary>Reads the subchunk and attaches it to the sub-chunks list</summary>
        protected void ReadSubChunk()
        {
            //read four bytes for a FourCC. If unable to read that much, an error has occurred, so don't sweat it too much. Maybe throw a descriptive exception, but that's it
            RiffSubChunk subChunk = new RiffSubChunk(this.dataStream.Position);

            subChunk.Chunk = RiffChunkFactory.CreateChunk(this.ReadFourCC());
            subChunk.Chunk.Size = this.ReadUInt32();
            subChunk.Chunk.DataStream = dataStream;
            subChunk.Chunk.DataOffset = dataStream.Position;

            this.subChunks.Add(subChunk);

            //seek to the next chunk, 16-bit aligned, so seek an additional byte if data is of odd/uneven length
            if ((subChunk.Chunk.Size & 1U) == 1U)
                ReusableIO.SeekIfAble(this.dataStream, subChunk.Chunk.Size + 1, SeekOrigin.Current);
            else
                ReusableIO.SeekIfAble(this.dataStream, subChunk.Chunk.Size, SeekOrigin.Current);
        }
    }
}