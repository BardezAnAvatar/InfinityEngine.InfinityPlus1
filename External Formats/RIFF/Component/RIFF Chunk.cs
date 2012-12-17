using System;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Base class for RIFF chunks. Contains the chunk type/id/name, size and offset to data, as well as the data stream to read from</summary>
    public class RiffChunk : IRiffChunk
    {
        #region Fields
        /// <summary>Leading four byte character code of the chunk</summary>
        private ChunkType chunkId;

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        private UInt32 size;

        /// <summary>Position within the stream where this chunk starts</summary>
        private Int64 chunkOffset;

        /// <summary>Stream object containing the RIFF data to read from.</summary>
        private Stream dataStream;
        #endregion


        #region Properties
        /// <summary>Leading four byte character code of the chunk</summary>
        public ChunkType ChunkId
        {
            get { return this.chunkId; }
        }

        /// <summary>Exposes the name of the chunk type. Preserves spaces in the name.</summary>
        public String ChunkName
        {
            get { return RiffChunk.GetFourCCName((UInt32)this.ChunkId); }
        }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        public UInt32 Size
        {
            get { return this.size; }
        }        

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        /// <remarks>
        ///     The spec. defines this as a byte array. However, the problem with this is that, in the example of raw audio-video interleave,
        ///     the data chunk may be so incredibly large as to surpass RAM, in which case reading the data is foolish. Instead, open the file
        ///     and read the stream, seeking as appropriate, storing offsets and reading on-demand.
        /// </remarks>
        public Byte[] Data
        {
            get
            {
                Byte[] data = null;

                if (this.DataStream != null && this.DataStream.CanSeek)
                {
                    ReusableIO.SeekIfAble(this.DataStream, this.DataOffset);
                    data = ReusableIO.BinaryRead(this.DataStream, this.Size);
                }

                return data;
            }
        }

        /// <summary>Unsigned offset to the data</summary>
        public Int64 DataOffset
        {
            get { return this.ChunkOffset + 8L; }
        }

        /// <summary>Position within the stream where this chunk starts</summary>
        public Int64 ChunkOffset
        {
            get { return this.chunkOffset; }
        }
        
        /// <summary>Stream object containing the RIFF data to read from.</summary>
        public Stream DataStream
        {
            get { return this.dataStream; }
        }
        #endregion


        #region Construction
        /// <summary>Read constructor</summary>
        /// <param name="input">Stream to read from.</param>
        public RiffChunk(Stream input)
        {
            this.Initialize(input);
            this.ReadChunkHeader();
        }

        /// <summary>Write constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        /// <param name="size">Size of the chunk data</param>
        /// <param name="data">Stream to read from.</param>
        public RiffChunk(ChunkType type, UInt32 size, Stream data)
        {
            this.Initialize(data);
            this.chunkId = type;
            this.size = size;
        }

        /// <summary>Instantiates reference types</summary>
        private void Initialize(Stream input)
        {
            if (input == null)
                throw new ApplicationException("RIFF Chunk's source stream cannot be null.");

            this.chunkOffset = input.Position;
            this.dataStream = input;
        }
        #endregion


        #region Read methods
        /// <summary>This public method reads the RIFF chunk from the data stream. Reads the chunk, reading header info and/or indexing sub-chunks.</summary>
        public virtual void Read() { }

        /// <summary>This public method reads the RIFF chunk from the data stream. Reads sub-chunks.</summary>
        public virtual void ReadChunkHeader()
        {
            this.chunkId = RiffChunk.ReadFourCC(this.DataStream);
            this.size = ReusableIO.ReadUInt32FromStream(this.DataStream);
        }

        /// <summary>Reads the fourcc at the current index of the stream</summary>
        /// <returns>ChunkType describing the FourCC</returns>
        public static ChunkType ReadFourCC(Stream dataStream)
        {
            UInt32 marker = ReusableIO.ReadUInt32FromStream(dataStream);

            ////check validity
            //if (!RiffChunk.IsKnownFourCC(marker))
            //    System.Diagnostics.Debug.Print("Could not convert value \"{0:X8}\" (\"{1}\") into a RIFF ChunkType.", marker, RiffChunk.GetFourCCName(marker));

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
                case ChunkType.ISFT:    //generating software
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
        public static String GetFourCCName(UInt32 fourCC)
        {
            Byte[] binary = BitConverter.GetBytes(fourCC);
            return ReusableIO.ReadStringFromByteArray(binary, 0, "en-US", 4);
        }
        #endregion


        #region Write Methods
        //TODO: allow a write(stream datasource) method that will wrte out to the datastream. So the chunk will read any data from the provided out to the DataSTream?
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
        public virtual void WriteString(StringBuilder builder)
        {
            StringFormat.ToStringAlignment("Chunk Type", builder);
            builder.Append(this.ChunkName);
            StringFormat.ToStringAlignment("Size", builder);
            builder.Append(this.Size);
            StringFormat.ToStringAlignment("Position of Data", builder);
            builder.Append(this.DataOffset);
        }
        #endregion
    }
}