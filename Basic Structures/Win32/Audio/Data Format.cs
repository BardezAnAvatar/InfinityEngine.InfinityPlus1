using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave.Enums
{
    /// <summary>Enmerator indicating the type of audio data (typically PCM)</summary>
    /// <remarks>Non-PCM values taken from http://www.signalogic.com/index.pl?page=ms_waveform</remarks>
    public enum DataFormat : ushort /* UInt16 */
    {
        Unknown = 0,

        /// <summary>Linear PCM/uncompressed</summary>
        PCM = 1,

        /// <summary>Microsoft ADPCM</summary>
        ADPCM = 0x0002,

        /// <summary>IEEE floating-point</summary>
        IEEE_FP = 0x0003,

        /// <summary>IBM CVSD</summary>
        CVSD = 0x0005,

        /// <summary>Microsoft ALAW (8-bit ITU-T G.711BA ALAW)</summary>
        Microsoft_ALAW = 0x0006,

        /// <summary>Microsoft M-LAW (8-bit ITU-T G.711 M-LAW</summary>
        Microsoft_MLAW = 0x0007,

        /// <summary>Intel IMA/DVI ADPCM</summary>
        DVI_ADPCM = 0x0011,

        /// <summary>ITU G.723 ADPCM</summary>
        ITU_G723_ADPCM = 0x0016,

        /// <summary>Dialogic OKI ADPCM</summary>
        OKI_ADPCM = 0x0017,

        /// <summary>Dolby AAC</summary>
        Dolby_AAC = 0x0030,

        /// <summary>Microsoft GSM 6.10</summary>
        GSM = 0x0031,

        /// <summary>Rockwell ADPCM</summary>
        Rockwell_ADPCM = 0x0036,

        /// <summary>ITU G.721 ADPCM</summary>
        ITU_G_721_ADPCM = 0x0040,

        /// <summary>Microsoft MSG723</summary>
        MSG723 = 0x0042,

        /// <summary>ITU-T G.726</summary>
        ITU_G726_ADPCM = 0x0045,

        /// <summary>APICOM G.726 ADPCM</summary>
        APICOM_ADPCM = 0x0064,

        /// <summary>IBM M-LAW</summary>
        IBM_MLAW = 0x00101,

        /// <summary>IBM A-LAW</summary>
        IBM_ALAW = 0x00102,

        /// <summary>IBM ADPCM</summary>
        IBM_ADPCM = 0x00103,
    }
}