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
        /// <summary>Represents playback info for wave sample data</summary>
        public WaveFormat WaveFormatData { get; set; }
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
        protected void Read()
        {
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

                //WAVEFORMAT fields
                //DataFormat formatTag = (DataFormat)ReusableIO.ReadUInt16FromStream(this.DataStream);
                UInt16 formatTag = ReusableIO.ReadUInt16FromStream(this.DataStream);
                UInt16 numberChannels = ReusableIO.ReadUInt16FromStream(this.DataStream);
                UInt32 sampleRate = ReusableIO.ReadUInt32FromStream(this.DataStream);
                UInt32 averageBytesPerSec = ReusableIO.ReadUInt32FromStream(this.DataStream);
                UInt16 blockAlignment = ReusableIO.ReadUInt16FromStream(this.DataStream);

                this.WaveFormatData = new WaveFormat(formatTag, numberChannels, sampleRate, averageBytesPerSec, blockAlignment);

                if (this.Size > 14) //Size(14) = WAVEFORMAT
                {
                    UInt16 bitsPerSample = ReusableIO.ReadUInt16FromStream(this.DataStream);
                    this.WaveFormatData = new PcmWaveFormat(this.WaveFormatData, bitsPerSample);

                    if (this.Size > 16) //Size(16) = pcmwaveformat_tag
                    {
                        UInt16 size = ReusableIO.ReadUInt16FromStream(this.DataStream);
                        this.WaveFormatData = new WaveFormatEx(this.WaveFormatData as PcmWaveFormat, size);

                        if (this.Size > 18) //Size(18) = WAVEFORMATEX
                        {
                            UInt16 union = ReusableIO.ReadUInt16FromStream(this.DataStream);
                            SpeakerPositions channelMask = (SpeakerPositions)(ReusableIO.ReadUInt32FromStream(this.DataStream));
                            Byte[] guidData = ReusableIO.BinaryRead(this.DataStream, 16);
                            Guid specificFormat = new Guid(guidData);

                            this.WaveFormatData = new WaveFormatExtensible(this.WaveFormatData as WaveFormatEx, union, channelMask, specificFormat);

                            if (this.Size > 40) //Size(24+GUID = 24+16=40) = WAVEFORMATEXTENSIBLE
                            {
                                //custom format...
                            }
                        }
                    }
                }
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
            return new WaveFormatEx(this.WaveFormatData);   //ensure that it is transformed into a WaveFormatEx from, say, waveformat_tag or pcmwaveformat_tag
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
            this.WaveFormatData.WriteString(builder);
        }
        #endregion
    }
}