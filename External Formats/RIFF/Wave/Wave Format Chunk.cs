using System;
using System.IO;
using System.Text;

using Bardez.Projects.BasicStructures.Win32.Audio;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave
{
    /// <summary>Describes the basic PCM WAVE fmt chunk.</summary>
    /// <remarks>
    ///     In the event that I will need to read non-PCM 'fmt ' chunks,
    ///     I will need to re-think how the factory works. Or, maybe have an object for
    ///     extended data inside this class?
    /// </remarks>
    public class WaveFormatChunk : RiffChunkExtensionBase, IWaveFormatEx
    {
        #region Fields
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
        /// <summary>Chunk constructor</summary>
        /// <param name="baseChunk">Chunk already read that contains basic details for this chunk</param>
        public WaveFormatChunk(IRiffChunk baseChunk) : base(baseChunk)
        {
            this.Read();
        }
        #endregion


        #region Read method
        /// <summary>This public method reads the RIFF chunk from the data stream. Reads sub-chunks.</summary>
        public void Read()
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
                ReusableIO.SeekIfAble(this.DataStream, this.DataOffset);
                this.dataType = (DataFormat)ReusableIO.ReadUInt16FromStream(this.DataStream);
                this.numChannels = ReusableIO.ReadUInt16FromStream(this.DataStream);
                this.sampleRate = ReusableIO.ReadUInt32FromStream(this.DataStream);
                this.byteRate = ReusableIO.ReadUInt32FromStream(this.DataStream);
                this.blockAlignment = ReusableIO.ReadUInt16FromStream(this.DataStream);
                this.bitsPerSample = ReusableIO.ReadUInt16FromStream(this.DataStream);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while reading WAVE format chunk data.", ex);
            }
        }
        #endregion


        #region IWaveFormatEx
        /// <summary>Returns a WaveFormatEx instance from this header data</summary>
        /// <returns>A WaveFormatEx instance to submit to API calls</returns>
        public WaveFormatEx GetWaveFormat()
        {
            WaveFormatEx waveEx = new WaveFormatEx();

            waveEx.AverageBytesPerSec = this.SampleRate * 2U /*sizeof(short)*/ * this.numChannels /* sizeof(usort) */;
            waveEx.BitsPerSample = 16; /* sizeof(short) */
            waveEx.BlockAlignment = Convert.ToUInt16(2U * this.numChannels);
            waveEx.FormatTag = 1;   //1 for PCM
            waveEx.NumberChannels = this.numChannels; //designating 1 causes errors
            waveEx.SamplesPerSec = this.sampleRate;
            waveEx.Size = 0;    //no extra data; this is strictly a WaveFormatEx instance 

            return waveEx;
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
            builder.Append(this.GetWaveFormat().ToDescriptionString());
        }
        #endregion
    }
}