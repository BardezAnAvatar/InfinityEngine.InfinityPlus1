using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Base chunk class that has sub-chunks</summary>
    public class RiffSuperChunk : RiffChunkExtensionBase
    {
        #region Fields
        /// <summary>List of sub-chunks</summary>
        public List<RiffSubChunk> SubChunks { get; set; }

        /// <summary>Four character code indicating the type of RIFF content within this super-chunk</summary>
        public ChunkType ContainerType { get; set; }
        #endregion


        #region Properties
        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        public override UInt32 Size
        {
            get { return this.rootChunk.Size - 4; }
        }

        /// <summary>Unsigned offset to the data</summary>
        public override Int64 DataOffset
        {
            get { return this.ChunkOffset + 12; }
        }
        #endregion


        #region Construction
        /// <summary>Chunk constructor</summary>
        /// <param name="baseChunk">Chunk already read that contains basic details for this chunk</param>
        public RiffSuperChunk(IRiffChunk baseChunk) : base(baseChunk)
        {
            this.Initialize();
            this.ContainerType = RiffChunk.ReadFourCC(this.DataStream);
        }

        /// <summary>Instantiates reference types</summary>
        protected void Initialize()
        {
            this.SubChunks = new List<RiffSubChunk>();
        }
        #endregion


        #region Methods
        /// <summary>This public method reads the RIFF sub-chunks from the data stream.</summary>
        public void ReadSubChunks()
        {
            if (this.Size > 4U) //4 could indicate just a single format ("WAVE" with no data, for example)
            {
                //read extended data
                ReusableIO.SeekIfAble(this.DataStream, this.DataOffset);

                //should be at first sub-chunk. Read chunks.
                try
                {
                    while ((this.DataStream.Position - this.DataOffset) <  this.Size)
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
            RiffSubChunk subChunk = new RiffSubChunk(this.DataStream.Position);
            subChunk.Chunk = RiffChunkFactory.CreateChunk(this.DataStream);

            //Keep track of the chunk
            this.SubChunks.Add(subChunk);

            //get the next chunk's location
            Int64 nextPosition = subChunk.Chunk.DataOffset + subChunk.Chunk.Size;
            if ((subChunk.Chunk.Size & 1U) == 1U)
                ++nextPosition;

            //recursively read subchunks as necessary
            if (subChunk.Chunk is RiffSuperChunk)
                (subChunk.Chunk as RiffSuperChunk).ReadSubChunks();

            //seek to next chunk location
            ReusableIO.SeekIfAble(this.DataStream, nextPosition, SeekOrigin.Begin);
        }

        /// <summary>Finds the first subchunk of the type specified</summary>
        /// <param name="fourCC">Chunk type to search for</param>
        /// <returns>The first found sub-chunk or null</returns>
        public RiffSubChunk FindFirstSubChunk(ChunkType fourCC)
        {
            RiffSubChunk chunk = null;

            //search
            foreach (RiffSubChunk subChunk in this.SubChunks)
            {
                if (subChunk.Chunk.ChunkId == fourCC)
                {
                    chunk = subChunk;
                    break;
                }
            }

            return chunk;
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing a human-readable representation</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            this.WriteString(builder);

            return builder.ToString();
        }

        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public override void WriteString(StringBuilder builder)
        {
            base.WriteString(builder);
            StringFormat.ToStringAlignment("Sub-Chunks", builder);

            foreach (RiffSubChunk subChunk in this.SubChunks)
                StringFormat.IndentAllLines(subChunk.ToString(), 1, builder);
        }
        #endregion
    }
}