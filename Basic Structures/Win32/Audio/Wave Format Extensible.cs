using System;
using System.Text;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Managed representation of Win32 WAVEFORMATEXTENSIBLE structure, inheriting the WaveFormatEx structure</summary>
    public class WaveFormatExtensible : WaveFormatEx
    {
        #region Fields
		/// <summary>Union of three fields here</summary>
		protected UInt16 samples;

		/// <summary>Positions of the audio channels</summary>
		protected UInt32 channelMask;

		/// <summary>Format identifier GUID</summary>
		protected Guid subFormat;
        #endregion


        #region Properties
		/// <summary>Base WAVEFORMATEX data</summary>
		/// <remarks>Keeping with the base type paradigm in C/++</remarks>
		public WaveFormatEx Format
		{
			get { return this as WaveFormatEx; }
		}

		/// <summary>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</summary>
		public UInt16 Samples
		{
            get { return this.samples; }
			set { this.samples = value; }
		}
					
		/// <summary>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</summary>
		public UInt16 ValidBitsPerSample
		{
            get { return this.samples; }
			set { this.samples = value; }
		}
    
		/// <summary>Union of three fields here: wValidBitsPerSample, wSamplesPerBlock, and wReserved</summary>
		public UInt16 Reserved
		{
            get { return this.samples; }
			set { this.samples = value; }
		}

		/// <summary>Positions of the audio channels</summary>
		public UInt32 ChannelMask
		{
            get { return this.channelMask; }
			set { this.channelMask = value; }
		}

		/// <summary>Format identifier GUID</summary>
		public Guid SubFormat
		{
			get { return this.subFormat; }
			set { this.subFormat = value; }
		}
        #endregion


		#region Construction
		/// <summary>Default constructor</summary>
		public WaveFormatExtensible() { }
        #endregion


		#region Methods
		/// <summary>Generates a descriptive string (to be displayed to the end user)</summary>
		/// <returns>A String representing the WaveFormatEx Object contents</returns>
		public override String ToDescriptionString()
        {
	        String desc = null;

	        StringBuilder builder = new StringBuilder();

	        //base type
	        builder.Append(base.ToDescriptionString());

	        //remainder data
	        builder.Append("\tNumber of samples:                 ");
	        builder.Append(this.samples);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tAudio channel positions(s):        ");
	        builder.Append(this.channelMask);
	        builder.AppendLine(String.Empty);
	        builder.Append("\tFormat GUID identifier:            ");
	        builder.Append(this.subFormat.ToString());
	        builder.AppendLine(String.Empty);

	        desc = builder.ToString();
	
	        return desc;
        }
		#endregion

    }
}