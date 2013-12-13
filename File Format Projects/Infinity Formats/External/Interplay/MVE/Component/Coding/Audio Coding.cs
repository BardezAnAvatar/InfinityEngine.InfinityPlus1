using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding
{
    /// <summary>Class that handles audio coding from an audio opcode</summary>
    /// <remarks>Only addresses a 16 bit encoding, since the quantization table far exceeds vakues of 255</remarks>
    public class AudioCoding
    {
        #region Constants
        /// <summary>Semi-constant array of audio quantization values</summary>
        protected static readonly Int16[] AudioQuantizationTable =
        {
                 0,      1,      2,      3,      4,      5,      6,      7,      8,      9,     10,     11,     12,     13,     14,     15,
                16,     17,     18,     19,     20,     21,     22,     23,     24,     25,     26,     27,     28,     29,     30,     31,
                32,     33,     34,     35,     36,     37,     38,     39,     40,     41,     42,     43,     47,     51,     56,     61,
                66,     72,     79,     86,     94,    102,    112,    122,    133,    145,    158,    173,    189,    206,    225,    245,
               267,    292,    318,    348,    379,    414,    452,    493,    538,    587,    640,    699,    763,    832,    908,    991,
              1081,   1180,   1288,   1405,   1534,   1673,   1826,   1993,   2175,   2373,   2590,   2826,   3084,   3365,   3672,   4008,
              4373,   4772,   5208,   5683,   6202,   6767,   7385,   8059,   8794,   9597,  10472,  11428,  12471,  13609,  14851,  16206,
             17685,  19298,  21060,  22981,  25078,  27367,  29864,  32589, -29973, -26728, -23186, -19322, -15105, -10503,  -5481,     -1,
                 1,      1,   5481,  10503,  15105,  19322,  23186,  26728,  29973, -32589, -29864, -27367, -25078, -22981, -21060, -19298,
            -17685, -16206, -14851, -13609, -12471, -11428, -10472,  -9597,  -8794,  -8059,  -7385,  -6767,  -6202,  -5683,  -5208,  -4772,
             -4373,  -4008,  -3672,  -3365,  -3084,  -2826,  -2590,  -2373,  -2175,  -1993,  -1826,  -1673,  -1534,  -1405,  -1288,  -1180,
             -1081,   -991,   -908,   -832,   -763,   -699,   -640,   -587,   -538,   -493,   -452,   -414,   -379,   -348,   -318,   -292,
              -267,   -245,   -225,   -206,   -189,   -173,   -158,   -145,   -133,   -122,   -112,   -102,    -94,    -86,    -79,    -72,
               -66,    -61,    -56,    -51,    -47,    -43,    -42,    -41,    -40,    -39,    -38,    -37,    -36,    -35,    -34,    -33,
               -32,    -31,    -30,    -29,    -28,    -27,    -26,    -25,    -24,    -23,    -22,    -21,    -20,    -19,    -18,    -17,
               -16,    -15,    -14,    -13,    -12,    -11,    -10,     -9,     -8,     -7,     -6,     -5,     -4,     -3,     -2,     -1
        };
        #endregion


        #region Fields
        /// <summary>Count of channels in the data stream</summary>
        public Int32 Channels { get; set; }

        /// <summary>Collection of current samples, one from each stream</summary>
        protected IList<Int16> CurrentSamples { get; set; }
        #endregion


        #region Construction
        /// <summary>Partial definition constructor</summary>
        /// <param name="channels">Number of channels in each stream</param>
        public AudioCoding(Int32 channels)
        {
            this.Channels = channels;
        }
        #endregion


        /*
        //rough process:
        *
        * 1) read one Int16 for each channel in the stream.
        * 2) read samples l/r/l/r/l/r/l/r
        *   3) dequantize each subsequent sample to add to the current sample
        * 4) two and 3) are performed on a per-channel basis; left does not affect right, etc.
        */

        /// <summary>Decodes a single block of audio and returns he binary values of it for output</summary>
        /// <param name="data">Data to decompress</param>
        /// <param name="uncompressedLength">Expected output length</param>
        /// <returns>Uncompressed audio</returns>
        /// <remarks>
        ///     I *think* that the current samples are to be reset at the start of each opcode group, not just the initial one.
        ///     There is an intermitted 'knocking' that I believe coincides with the start of a block of audio data.
        ///     
        ///     Confirmed: First level of unknocking is to ensure that each opcode reads a start delta 'prediction';
        ///     the second level of unknocking is by outputting the delta 'prediction' as a sample.
        /// </remarks>
        public Byte[] DecodeAudioBlock(Byte[] data, Int32 uncompressedLength)
        {
            Int64 position = 0L;
            MemoryStream ms = new MemoryStream(uncompressedLength);

            //removed condition due to 'clicking' which seems per-block
            //if (this.CurrentSamples == null)  //read leading delta values if stream is uninitialized
            {
                this.CurrentSamples = new Int16[this.Channels];

                for (Int32 channel = 0; channel < this.Channels; ++channel)
                {
                    this.CurrentSamples[channel] = ReusableIO.ReadInt16FromArray(data, position);
                    ReusableIO.WriteInt16ToStream(this.CurrentSamples[channel], ms);
                    position += 2L;
                }
            }

            while (position < data.Length)
                for (Int32 channel = 0; channel < this.Channels; ++channel)
                {
                    if (this.CurrentSamples[channel] + AudioCoding.AudioQuantizationTable[data[position]] < Int16.MinValue)
                        this.CurrentSamples[channel] = Int16.MinValue;
                    else if (this.CurrentSamples[channel] + AudioCoding.AudioQuantizationTable[data[position]] > Int16.MaxValue)
                        this.CurrentSamples[channel] = Int16.MaxValue;
                    else
                        this.CurrentSamples[channel] += AudioCoding.AudioQuantizationTable[data[position]];

                    position += 1L;     //read 1 byte
                    
                    ReusableIO.WriteInt16ToStream(this.CurrentSamples[channel], ms);
                }

            return ms.GetBuffer();
        }
    }
}