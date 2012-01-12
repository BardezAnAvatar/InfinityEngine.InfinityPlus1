using System;
using System.IO;

using Bardez.Projects.ReusableCode;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM
{
    /// <summary>Represents an ACM file with a WAVC header</summary>
    /// <remarks>This is literally just a WAVC header prepended to an ACM file stream; its ACM header exists after the WAVC header</remarks>
    public class WavCAudioFile : AcmAudioFile
    {
        /// <summary>WAVC Header information</summary>
        public WavCHeader WavcHeader { get; set; }
        
        #region Construction
        /// <summary>Default constructor</summary>
        public WavCAudioFile() : base()
        {
            this.WavcHeader = null;
        }

        /// <summary>Initializes field data.</summary>
        protected override void Initialize()
        {
            this.WavcHeader = new WavCHeader();
            base.Initialize();
        }
        #endregion

        #region IO
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="initialize">Boolean indicating whether or not to initialize the field data</param>
        public override void Read(Stream input, Boolean initialize = true)
        {
            //(re-)Instatiate fields
            if (initialize)
                this.Initialize();

            this.WavcHeader.Read(input);
            ReusableIO.SeekIfAble(input, this.WavcHeader.OffsetAcm);
            base.Read(input, false);
        }

        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadHeader(Stream input)
        {
            this.Initialize();
            this.WavcHeader.Read(input);
            ReusableIO.SeekIfAble(input, this.WavcHeader.OffsetAcm);
            base.ReadHeader(input);
        }
        #endregion

        /// <summary>Returns a WaveFormatEx instance from this header data</summary>
        /// <returns>A WaveFormatEx instance to submit to API calls</returns>
        public override WaveFormatEx GetWaveFormat()
        {
            WaveFormatEx waveEx = new WaveFormatEx();

            waveEx.AverageBytesPerSec = this.AcmHeader.SampleRate * 2U /*sizeof(short)*/ * this.WavcHeader.Channels /* sizeof(usort) */;
            waveEx.BitsPerSample = 16; /* sizeof(short) */
            waveEx.BlockAlignment = Convert.ToUInt16(2U * this.WavcHeader.Channels);
            waveEx.FormatTag = 1;   //1 for PCM
            waveEx.NumberChannels = this.WavcHeader.Channels; //designating 1 causes errors
            waveEx.SamplesPerSec = this.AcmHeader.SampleRate;
            waveEx.Size = 0;    //no extra data; this is strictly a WaveFormatEx instance 

            return waveEx;
        }
    }
}