using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component
{
    /// <summary>Base clas for RIFF chunks. Contains the chunk type/id/name, size and offset to data, as well as the data stream to read from</summary>
    public class RiffChunk
    {
        #region Members
        /// <summary>Leading four byte character code of the chunk</summary>
        protected ChunkType chunkId;

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        protected UInt32 size;

        /// <summary>Unsigned offset to the data</summary>
        /// <remarks>
        ///     The spec. defines this as a byte array. However, the problem with this is that, in the example of raw audio-video interleave,
        ///     the data chunk may be so incredibly large as to surpass RAM, in which case reading the data is foolish. Instead, open the file
        ///     and read the stream, seeking as appropriate, storing offsets and reading on-demand.
        /// </remarks>
        protected Int64 dataOffset;

        /// <summary>Stream object containing the RIFF data to read from.</summary>
        protected Stream dataStream;
        #endregion

        #region Properties
        /// <summary>Leading four byte character code of the chunk</summary>
        public ChunkType ChunkId
        {
            get { return this.chunkId; }
            set { this.chunkId = value; }
        }

        public String ChunkName
        {
            get
            {
                Byte[] binary = BitConverter.GetBytes((UInt32)this.chunkId);
                return ReusableIO.ReadStringFromByteArray(binary, 0, "en-US", 4);
            }
        }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        public UInt32 Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        public Byte[] Data
        {
            get
            {
                ReusableIO.SeekIfAble(this.dataStream, this.dataOffset);
                return ReusableIO.BinaryRead(this.dataStream, this.size);
            }
        }

        /// <summary>Stream object containing the RIFF data to read from.</summary>
        public Stream DataStream
        {
            get { return this.dataStream; }
            set { this.dataStream = value; }
        }

        /// <summary>Unsigned offset to the data</summary>
        /// <remarks>
        ///     The spec. defines this as a byte array. However, the problem with this is that, in the example of raw audio-video interleave,
        ///     the data chunk may be so incredibly large as to surpass RAM, in which case reading the data is foolish. Instead, open the file
        ///     and read the stream, seeking as appropriate, storing offsets and reading on-demand.
        /// </remarks>
        public Int64 DataOffset
        {
            get { return this.dataOffset; }
            set { this.dataOffset = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public RiffChunk()
        {
            this.Initialize();
        }

        /// <summary>Stream constructor</summary>
        /// <param name="input">Stream to read from.</param>
        public RiffChunk(Stream input) : this()
        {
            this.dataStream = input;
        }

        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        public RiffChunk(ChunkType type) : this()
        {
            this.chunkId = type;
        }

        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        /// <param name="input">Stream to read from.</param>
        public RiffChunk(ChunkType type, Stream input) : this()
        {
            this.chunkId = type;
            this.dataStream = input;
        }

        /// <summary>Instantiates reference types</summary>
        protected virtual void Initialize() { }
        #endregion

        /// <summary>This public method reads the RIFF chunk from the data stream. Reads the chunk, reading header info and/or indexing sub-chunks.</summary>
        public virtual void Read() { }

        /// <summary>This public method reads the RIFF chunk from the data stream. Reads sub-chunks.</summary>
        public virtual void ReadChunkHeader()
        {
            this.chunkId = ReadFourCC();
            this.size = ReadUInt32();
            this.dataOffset = this.dataStream.Position; //Convert.ToUInt32(this.dataStream.Position);
        }

        /// <summary>Reads the fourcc at the current index of the stream</summary>
        /// <returns>ChunkType descrbing the FourCC</returns>
        protected ChunkType ReadFourCC()
        {
            return (ChunkType)this.ReadUInt32();
        }

        /// <summary>Reads the chunk size at the current index of the stream</summary>
        /// <returns>UInt32 at the current position in the datastream</returns>
        protected UInt32 ReadUInt32()
        {
            Byte[] bin = ReusableIO.BinaryRead(this.dataStream, 4);
            return ReusableIO.ReadUInt32FromArray(bin, 0);
        }

        /// <summary>Reads the chunk size at the current index of the stream</summary>
        /// <returns>UInt16 at the current position in the datastream</returns>
        protected UInt16 ReadUInt16()
        {
            Byte[] bin = ReusableIO.BinaryRead(this.dataStream, 2);
            return ReusableIO.ReadUInt16FromArray(bin, 0);
        }
    }
}