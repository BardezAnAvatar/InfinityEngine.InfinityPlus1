using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Wave.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.RIFF.Wave
{
    /// <summary>Describes the basic PCM WAVE fmt chunk.</summary>
    /// <remarks>
    ///     In the event that I will need to read non-PCM 'fmt ' chunks,
    ///     I will need to re-think how the factory works. Or, maybe have an object for
    ///     extended data inside this class?
    /// </remarks>
    public class WaveFormatChunk : RiffChunk
    {
        #region Members
        /// <summary>Wave data type</summary>
        protected DataFormat dataType;

        /// <summary>Number of channels in this wave file</summary>
        protected UInt16 numChannels;

        /// <summary>Number of wave samples per second</summary>
        /// <remarks>
        ///     As a display of personal stupidity, I just made the connection of digitized
        ///     waveform sound to calculus, and then to digitization in general (i.e. scanning). Wow.
        ///     I should have gotten that a lot sooner.
        /// </remarks>
        protected UInt32 sampleRate;

        /// <summary>Number of bytes to push through per second</summary>
        /// <value>= SampleRate * NumChannels * BitsPerSample/8</value>
        /// <remarks>Seems redundant and superfluous, honestly. Maybe relevant for compressed data?</remarks>
        protected UInt32 byteRate;

        /// <summary>Number of bytes for a single sample of all channels</summary>
        /// <value>= NumChannels * BitsPerSample/8</value>
        protected UInt16 blockAlignment;

        /// <summary>Indicates the number of bits per sample</summary>
        /// <value>For PCM, multiple of 8</value>
        protected UInt16 bitsPerSample;
        #endregion

        #region Properties
        /// <summary>Wave data type</summary>
        public DataFormat DataType
        {
            get { return this.dataType; }
            set { this.dataType = value; }
        }

        /// <summary>Number of channels in this wave file</summary>
        public UInt16 NumChannels
        {
            get { return this.numChannels; }
            set { this.numChannels = value; }
        }

        /// <summary>Number of wave samples per second</summary>
        /// <remarks>
        ///     As a display of personal stupidity, I just made the connection of digitized
        ///     waveform sound to calculus, and then to digitization in general (i.e. scanning). Wow.
        ///     I should have gotten that a lot sooner.
        /// </remarks>
        public UInt32 SampleRate
        {
            get { return this.sampleRate; }
            set { this.sampleRate = value; }
        }

        /// <summary>Number of bytes to push through per second</summary>
        /// <value>= SampleRate * NumChannels * BitsPerSample/8</value>
        /// <remarks>Seems redundant and superfluous, honestly. Maybe relevant for compressed data?</remarks>
        public UInt32 ByteRate
        {
            get { return this.byteRate; }
            set { this.byteRate = value; }
        }

        /// <summary>Number of bytes for a single sample of all channels</summary>
        /// <value>= NumChannels * BitsPerSample/8</value>
        public UInt16 BlockAlignment
        {
            get { return this.blockAlignment; }
            set { this.blockAlignment = value; }
        }

        /// <summary>Indicates the number of bits per sample</summary>
        /// <value>For PCM, multiple of 8</value>
        public UInt16 BitsPerSample
        {
            get { return this.bitsPerSample; }
            set { this.bitsPerSample = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public WaveFormatChunk() : base() { }

        /// <summary>Stream constructor</summary>
        /// <param name="input">Stream to read from.</param>
        public WaveFormatChunk(Stream input) : base(input) { }

        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        public WaveFormatChunk(ChunkType type) : base(type) { }

        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        /// <param name="input">Stream to read from.</param>
        public WaveFormatChunk(ChunkType type, Stream input) : base(type, input) { }
        #endregion

        /// <summary>This public method reads the RIFF chunk from the data stream. Reads sub-chunks.</summary>
        public override void Read()
        {
            //should be at first sub-chunk. Read chunks.
            try
            {
                /*
                *   NOTICE
                see audiodefs.h in Windows SDK.
                Size(16) = pcmwaveformat_tag
                Size(14) = WAVEFORMAT
                Size(18) = WAVEFORMATEX
                Size(24+GUID = 24+16=40) = WAVEFORMATEXTENSIBLE
                */
                ReusableIO.SeekIfAble(this.dataStream, this.dataOffset);
                this.dataType = (DataFormat)this.ReadUInt16();
                this.numChannels = this.ReadUInt16();
                this.sampleRate = this.ReadUInt32();
                this.byteRate = this.ReadUInt32();
                this.blockAlignment = this.ReadUInt16();
                this.bitsPerSample = this.ReadUInt16();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while reading WAVE format chunk data.", ex);
            }
        }
    }
}