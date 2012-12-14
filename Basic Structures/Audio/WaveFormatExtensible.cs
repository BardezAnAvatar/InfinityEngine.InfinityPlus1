using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Bardez.Projects.Win32.Audio
{
    /// <summary>Managed representation of Win32 WAVEFORMATEXTENSIBLE structure, inheriting the WaveFormatEx structure</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WaveFormatExtensible
    {
        #region Fields
        /// <summary>Base WaveFormatEx data</summary>
        public WaveFormatEx Format;

        /// <summary>Union of three fields here</summary>
        private UInt16 samples;

        /// <summary>Positions of the audio channels</summary>
        public UInt32 ChannelMask;

        /// <summary>Format identifier GUID</summary>
        public Guid SubFormat;
        #endregion


        #region Properties
        /// <summary>Samples per block of audio data; valid if wBitsPerSample=0 (but rarely used)</summary>
        /// <remarks>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</remarks>
        public UInt16 Samples
        {
            get { return this.samples; }
            set { this.samples = value; }
        }

        /// <summary>Valid bits in each sample container</summary>
        /// <remarks>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</remarks>
        public UInt16 ValidBitsPerSample
        {
            get { return this.samples; }
            set { this.samples = value; }
        }

        /// <summary>Zero if neither case (Samples or ValidBitsPerSample) applies</summary>
        /// <remarks>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</remarks>
		public UInt16 Reserved
        {
            get { return this.samples; }
            set { this.samples = value; }
        }
        #endregion


        #region ToString override
        /// <summary>Generates a descriptive string (to be displayed to the end user)</summary>
		/// <returns>A String representing the WaveFormatEx Object contents</returns>
        public override String ToString()
        {
	        StringBuilder builder = new StringBuilder();
            builder.Append(this.Format.ToString());
	        builder.Append("\tNumber of samples:                 ");
	        builder.Append(this.samples);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tAudio channel positions(s):        ");
	        builder.Append(this.ChannelMask);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tFormat GUID identifier:            ");
	        builder.Append(this.SubFormat.ToString());
	        builder.AppendLine(String.Empty);
	
	        return builder.ToString();
        }
        #endregion
    }
}