using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Audio
{
    /// <summary>Decoder for audio frame, </summary>
    /// <remarks>
    ///     Based off of description from:
    ///     http://wiki.multimedia.cx/index.php?title=Bink_Audio"
    /// </remarks>
    public class AudioFrame
    {
        /// <summary>Critical frequencies</summary>
        protected static readonly Int16[] CriticalFrequencies = new Int16[]
        {
              100,   200,   300,   400,
              510,   630,   770,   920,
             1080,  1270,  1480,  1720,
             2000,  2320,  2700,  3150,
             3700,  4400,  5300,  6400,
             7700,  9500, 12000, 15500,
            24500
        };

        /// <summary>Table of run-length encoding lengths</summary>
        protected static readonly Int16[] RunLengthEncodingTable = new Int16[]
        { 
             2,  3,  4,  5,
             6,  8,  9, 10,
            11, 12, 13, 14,
            15, 16, 32, 64
        };

        public Byte[] DecodeFrameAudioSamples(Stream input, Int32 frameLength, Int32 channelCount, Int32 sampleRate, Boolean discreteCosineTransform)
        {
            throw new NotImplementedException("I got only so far before abandoning Bink audio of my own coding.");

            //compresses audio in chunks of varying sizes depending on sample rate: 
            Int32 frameSize;
            if (sampleRate < 22050)
                frameSize = 2048;
            else if (sampleRate < 44100)
                frameSize = 4096;
            else
                frameSize = 8192;

            //a frame is windowed with the previous frame; the size of the window is frame size / 16 
            Int32 windowOverlap = frameSize / 16;

            //compute half the sample rate as (sample rate + 1) / 2;
            Int32 halfSample = (sampleRate + 1) / 2;

            //initialize an array of band frequencies corresponding to an array of 25 critical frequencies (same as WMA, apparently), any for which the critical frequencies are less than half the sample rate 
            Int32[] bands = new Int32[25];
            Int32 bandCount = 1;
            for (; bandCount < 25; ++bandCount)
            {
                if (halfSample <= AudioFrame.CriticalFrequencies[bandCount - 1])
                    break;
            }

            //bands calculation
            bands[0] = 1;
            for (Int32 index = 1; index < bandCount; ++index)
                bands[index] = AudioFrame.CriticalFrequencies[index - 1] * (frameLength / 2) / (sampleRate / 2);
            bands[bandCount] = (frameLength / 2);


            /************************
            *   Decode Audio block  *
            ************************/
            Byte[] compressed = ReusableIO.BinaryRead(input, frameLength);
            BitStream bitStream = new BitStream(compressed);

            Double[] quantizers = new Double[25];
            Double quantizer = 0.0;

            //for each channel:
            for (Int32 channel = 0; channel < channelCount; ++channel)
            {
                //each sample is a coefficient, apparently. makes sense, I guess.
                Double[] coefficients = new Double[frameSize];

                //fetch 2 floats from the bitstream as the first 2 coefficients
                coefficients[0] = this.GetFloat(bitStream);
                coefficients[1] = this.GetFloat(bitStream);

                //unpack quantizers; for each band: 
                for (Int32 index = 0; index < bandCount; ++index)
                {
                    //value = next 8 bits in the bitstream 
                    Int32 value = bitStream.ReadInt32(8);
                    Double exponent = Math.Min(value, 95) * 0.0664;
                    quantizers[index] = Math.Pow(10.0, exponent);
                }

                //locate the initial band
                    //loop through and find the largest quantizer less than .5?
                Int32 bandNumber = 0;
                for (; bandNumber < 25; ++bandNumber)
                {
                    if (quantizers[bandNumber] >= 0.5)
                        break;

                    quantizer = quantizers[bandNumber];
                }



                //unpack and dequantize the transform coefficients while updating the current band
                    //read first two coefficents, read the next ones up to the frame size.
                    //There's an RLE table, so I'll bet it's Run-Length Encoded.
                        //RLE table has 16 values, size 4 bits?
                for (Int32 sample = 2; sample < frameSize; ++sample)
                {
                    //get a single bit to indicate if it is run_length encoded.
                    Int32 rle = bitStream.ReadInt32(1);

                    Int32 runLength = 1;
                    if (rle == 1)
                        runLength = AudioFrame.RunLengthEncodingTable[bitStream.ReadInt32(4)];

                    runLength *= 8; //? is this a (2 * root(two)) squared sort of deal?

                    
                    Int32 target = sample + runLength;  //scoping them because second case increments sample
                    Int32 width = bitStream.ReadInt32(4);

                    if (width == 0)
                    {
                        //set all to 0;
                        for (; sample < target; sample++)
                            coefficients[sample] = 0.0F;

                        //get new quantizer
                        for (; bandNumber < 25; ++bandNumber)
                        {
                            if (quantizers[bandNumber] >= 0.5)
                                break;

                            quantizer = quantizers[bandNumber];
                        }
                    }
                    else
                    {
                        //loop through all runlength
                        for (; sample < target; sample++)
                        {
                            if (bands[bandNumber] == sample)
                                quantizer = quantizers[bandNumber++];

                            Int32 coefficient = bitStream.ReadInt32(width);
                            if (coefficient == 0)
                                coefficients[sample] = 0.0F;
                            else
                            {
                                //quantize and sign
                                coefficients[sample] = (coefficient * quantizer);

                                //sign the coefficient
                                Int32 sign = bitStream.ReadInt32(1);
                                if (sign == 1)
                                    coefficients[sample] = -coefficients[sample];
                            }
                        }
                    }
                }

                
                //DCT or FFT
                if (!discreteCosineTransform)
                    throw new NotImplementedException("Fast Fourier transform not implemented due to non-use in IWD2 Bink video.");
                

                //convert samples to appropriate output format and interleave as necessary 


                //window the output with the previous frame

            }
        }

        /// <summary>Reads a 29-bit floating point number from the BitStream</summary>
        /// <param name="stream">BitStream to read from</param>
        /// <returns>A Single-precision floating point number</returns>
        public Single GetFloat(BitStream stream)
        {
            UInt32 exponent = stream.ReadUInt32(5);
            UInt32 mantissa = stream.ReadUInt32(23);
            UInt32 sign = stream.ReadUInt32(1);

            return this.GetFloat(mantissa, exponent, sign);
        }

        /// <summary>Gets the Single-precision floating point representation of a number using bit manipulation</summary>
        /// <param name="mantissa">Coefficient</param>
        /// <param name="exponent">Expontent of 10</param>
        /// <param name="sign">Sign of the value</param>
        /// <returns></returns>
        /// <remarks>Source of bit layout: http://en.wikipedia.org/wiki/Single_precision_floating-point_format</remarks>
        public Single GetFloat(UInt32 mantissa, UInt32 exponent, UInt32 sign)
        {
            /* verbose */
            //UInt32 signBits = (sign & 1U) << 31;
            //UInt32 exponentBits = (exponent & 255U) << 23;
            //UInt32 mantissaBits = mantissa & 0x7FFFFFU;
            //UInt32 floating = signBits | exponentBits | mantissaBits;
            //return BitConverter.ToSingle(BitConverter.GetBytes(floating), 0);

            /* unsafe */
            //unsafe
            //{
            //    UInt32 floating = (((sign & 1U) << 31) | ((exponent & 255U) << 23) | (mantissa & 0x7FFFFFU));
            //    Single* pointer = (Single*)(&floating);
            //    return *pointer;
            //}

            /* safe */
            return BitConverter.ToSingle(BitConverter.GetBytes(((sign & 1U) << 31) | ((exponent & 255U) << 23) | (mantissa & 0x7FFFFFU)), 0);
        }
    }
}