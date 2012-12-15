using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Base class for RIFF chunks. Contains the chunk type/id/name, size and offset to data, as well as the data stream to read from</summary>
    public class RiffChunk
    {
        #region Members
        /// <summary>Stream object containing the RIFF data to read from.</summary>
        protected Stream dataStream;
        #endregion


        #region Properties
        /// <summary>Leading four byte character code of the chunk</summary>
        public ChunkType ChunkId { get; set; }

        /// <summary>Exposes the name of the chunk type. Preserves spaces in the name.</summary>
        public String ChunkName
        {
            get
            {
                return GetFourCCName((UInt32)this.ChunkId);
            }
        }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        public UInt32 Size { get; set; }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        public Byte[] Data
        {
            get
            {
                Byte[] data = null;

                if (this.dataStream != null && this.dataStream.CanSeek)
                {
                    ReusableIO.SeekIfAble(this.dataStream, this.DataOffset);
                    data = ReusableIO.BinaryRead(this.dataStream, this.Size);
                }

                return data;
            }
        }

        /// <summary>Unsigned offset to the data</summary>
        /// <remarks>
        ///     The spec. defines this as a byte array. However, the problem with this is that, in the example of raw audio-video interleave,
        ///     the data chunk may be so incredibly large as to surpass RAM, in which case reading the data is foolish. Instead, open the file
        ///     and read the stream, seeking as appropriate, storing offsets and reading on-demand.
        /// </remarks>
        public Int64 DataOffset { get; set; }
        #endregion


        #region Construction
        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        /// <param name="input">Stream to read from.</param>
        public RiffChunk(ChunkType type, Stream input)
        {
            this.ChunkId = type;
            this.dataStream = input;
        }

        /// <summary>Instantiates reference types</summary>
        protected virtual void Initialize() { }
        #endregion


        #region Read methods
        /// <summary>This public method reads the RIFF chunk from the data stream. Reads the chunk, reading header info and/or indexing sub-chunks.</summary>
        public virtual void Read() { }

        /// <summary>This public method reads the RIFF chunk from the data stream. Reads sub-chunks.</summary>
        public virtual void ReadChunkHeader()
        {
            this.ChunkId = ReadFourCC(this.dataStream);
            this.Size = ReadUInt32(this.dataStream);
            this.DataOffset = this.dataStream.Position;
        }

        /// <summary>Reads the fourcc at the current index of the stream</summary>
        /// <returns>ChunkType describing the FourCC</returns>
        public static ChunkType ReadFourCC(Stream dataStream)
        {
            UInt32 marker = RiffChunk.ReadUInt32(dataStream);

            //check validity
            if (!RiffChunk.IsKnownFourCC(marker))
                throw new ApplicationException(String.Format("Could not convert value \"{0:X8}\" (\"{1}\") into a RIFF ChunkType.", marker, RiffChunk.GetFourCCName(marker)));

            return (ChunkType)marker;
        }

        /// <summary>Determines whether the value provided is a recognized FourCC code</summary>
        /// <param name="value">UInt32 value to test</param>
        /// <returns>True if it is known to this framework, otherwise false</returns>
        protected static Boolean IsKnownFourCC(UInt32 value)
        {
            Boolean isKnown = false;

            ChunkType fourcc = (ChunkType)value;
            switch (fourcc)
            {
                case ChunkType.AVI:
                case ChunkType.avih:
                case ChunkType.data:
                case ChunkType.fmt:
                case ChunkType.hdrl:
                case ChunkType.idx1:
                case ChunkType.INFO:
                case ChunkType.JUNK:
                case ChunkType.LIST:
                case ChunkType.movi:
                case ChunkType.rec:
                case ChunkType.RIFF:
                case ChunkType.strd:
                case ChunkType.strf:
                case ChunkType.strh:
                case ChunkType.strl:
                case ChunkType.WAVE:
                    isKnown = true;
                    break;
            }

            return isKnown;
        }

        /// <summary>Gets the String representation of memory for a suspected UInt32 fourCC code</summary>
        /// <param name="fourCC">UInt32 provided to attempt to render as a fourCC string of characters</param>
        /// <returns>String representation for the suspected UInt32 fourCC code</returns>
        protected static String GetFourCCName(UInt32 fourCC)
        {
            Byte[] binary = BitConverter.GetBytes(fourCC);
            return ReusableIO.ReadStringFromByteArray(binary, 0, "en-US", 4);
        }
        #endregion


        #region Data primitive read methods
        /// <summary>Reads the chunk size at the current index of the stream</summary>
        /// <returns>UInt32 at the current position in the datastream</returns>
        protected static UInt32 ReadUInt32(Stream dataStream)
        {
            Byte[] bin = ReusableIO.BinaryRead(dataStream, 4);
            return ReusableIO.ReadUInt32FromArray(bin, 0);
        }

        /// <summary>Reads the chunk size at the current index of the stream</summary>
        /// <returns>UInt16 at the current position in the datastream</returns>
        protected static UInt16 ReadUInt16(Stream dataStream)
        {
            Byte[] bin = ReusableIO.BinaryRead(dataStream, 2);
            return ReusableIO.ReadUInt16FromArray(bin, 0);
        }
        #endregion
    }
}