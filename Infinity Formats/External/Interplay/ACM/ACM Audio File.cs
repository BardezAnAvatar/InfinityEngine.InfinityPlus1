using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.ReusableCode;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM
{
    /// <summary>Represents an ACM file; is header and bitstream.</summary>
    /// <remarks>The bitstream appears to be overrunnable.</remarks>
    public class AcmAudioFile : IWaveFormatEx
    {
        /// <summary>Leading 14 bytes of ACM header data.</summary>
        public AcmHeader AcmHeader { get; set; }

        /// <summary>BitStream to populate </summary>
        public BitStream BitDataStream { get; set; }

        /// <summary>List of packed block data</summary>
        protected List<PackedBlock> blocks;

        #region Construction
        /// <summary>Default constructor</summary>
        public AcmAudioFile()
        {
            this.AcmHeader = null;
            this.BitDataStream = null;
            this.blocks = null;
        }

        /// <summary>Initializes field data.</summary>
        protected virtual void Initialize()
        {
            this.AcmHeader = new AcmHeader();
            this.blocks = new List<PackedBlock>();
        }
        #endregion

        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="initialize">Boolean indicating whether or not to initialize the field data</param>
        public virtual void Read(Stream input, Boolean initialize = true)
        {
            //(re-)Instatiate fields
            if (initialize)
                this.Initialize();

            this.AcmHeader.Read(input);
            
            //Assumption: most Acm files are less than 1 MB, and I have yet to see one > 10 MB. Read the whole thing into memory.
            Byte[] data = new Byte[input.Length - input.Position];
            input.Read(data, 0, data.Length);
            this.BitDataStream = new AcmBitStream(data);

            BitBlock bitBlock = BitBlock.Instance;
            bitBlock.SetFields(this.AcmHeader, this.BitDataStream);
            
            while (!this.BitDataStream.EndOfStream)
                blocks.Add(bitBlock.Decode());
        }

        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadHeader(Stream input)
        {
            //(re-)Instatiate fields
            this.Initialize();
            this.AcmHeader.Read(input);
        }

        /// <summary>Gets the sample data from the PackedBlocks.</summary>
        /// <returns>Sample data in individual samples (Int16)</returns>
        /// <remarks>
        ///     Without restricting sample count by the header, the same data returned will probably have extra 0'd data,
        ///     due to the trailing PackedBlock's encoding, junk after the last sample encoded.
        /// </remarks>
        public Int16[] GetSamples()
        {
            List<Int32> sampleData = new List<Int32>();
            foreach (PackedBlock block in this.blocks)
                sampleData.AddRange(block.Data);

            //I noticed that raw samples output was larger than PCM raw, due to the byte alignment of PackedBlocks' assumed size. Need to keep sample count real, based on header.
            Int64 sampleCount = Math.Min(sampleData.Count, this.AcmHeader.SamplesCount);

            Int16[] samples = new Int16[sampleCount];
            for (Int32 i = 0; i < sampleCount; ++i)
                samples[i] = Convert.ToInt16(sampleData[i] >> this.AcmHeader.PackingLevels); //de-shift sample data

            return samples;
        }

        /// <summary>Returns the float data in byte form for output</summary>
        /// <returns></returns>
        /// <remarks>No easy non-hackish way to really reinterpret Int16 as Bytes. Lame, but acceptable.</remarks>
        public Byte[] GetSampleData()
        {
            Int16[] data = this.GetSamples();
            Byte[] samples = new Byte[data.Length * 2];
            for (Int32 i = 0; i < data.Length; ++i)
            {
                Byte[] temp = BitConverter.GetBytes(data[i]);
                samples[2*i] = temp[0];
                samples[(2*i)+1] = temp[1];
            }

            return samples;
        }

        /// <summary>Returns a WaveFormatEx instance from this header data</summary>
        /// <returns>A WaveFormatEx instance to submit to API calls</returns>
        public virtual WaveFormatEx GetWaveFormat()
        {
            WaveFormatEx waveEx = new WaveFormatEx();

            waveEx.AverageBytesPerSec = this.AcmHeader.SampleRate * 2U /*sizeof(short)*/ * this.AcmHeader.ChannelsCount /* sizeof(usort) */;
            waveEx.BitsPerSample = 16; /* sizeof(short) */
            waveEx.BlockAlignment = Convert.ToUInt16(2U * this.AcmHeader.ChannelsCount);
            waveEx.FormatTag = 1;   //1 for PCM
            waveEx.NumberChannels = this.AcmHeader.ChannelsCount; //designating 1 causes errors
            waveEx.SamplesPerSec = this.AcmHeader.SampleRate;
            waveEx.Size = 0;    //no extra data; this is strictly a WaveFormatEx instance 

            return waveEx;
        }
    }
}