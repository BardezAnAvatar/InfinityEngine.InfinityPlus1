using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents the bitmask of 16x16 pixel cellsindiating whether or not the area has been explored</summary>
    public class ExploredBitmask
    {
        #region Fields
        /// <summary>Represents the size of the data, in bytes of the bitmask</summary>
        /// <remarks>Since an area is made of 64x64 tiles, there will always be a number of bytes as a multiple of 2.</remarks>
        public Int32 Size { get; set; }

        /// <summary>The raw binary array of the mask</summary>
        public Byte[] exploredData;
        #endregion


        #region Properties
        /// <summary>Exposes this class as a collection of Boolean flags</summary>
        /// <param name="index">Index of the flags to set</param>
        /// <returns>Flag indicating whether or not the area is explored</returns>
        public Boolean this[Int64 index]
        {
            get
            {
                if ((index >= this.Size / 8) || index < 0)
                    throw new ArgumentOutOfRangeException(String.Format("Parameter 'index' must be between values 0 and {0}.", this.Size / 8));

                Int64 byteIndex = index / 8;
                Int32 bitIndex = Convert.ToInt32(index % 8);

                Byte value = this.exploredData[byteIndex];
                Byte mask = 1;
                mask <<= bitIndex;

                value &= mask;

                return Convert.ToBoolean(value);
            }
            set
            {
                if ((index >= this.Size / 8) || index < 0)
                    throw new ArgumentOutOfRangeException(String.Format("Parameter 'index' must be between values 0 and {0}.", this.Size / 8));

                Int64 byteIndex = index / 8;
                Int32 bitIndex = Convert.ToInt32(index % 8);

                Byte mask = Convert.ToByte(value);
                mask <<= bitIndex;

                this.exploredData[byteIndex] |= mask;
            }
        }

        /// <summary>Exposes the raw binary array of the mask</summary>
        public Byte[] ExploredData
        {
            get { return this.exploredData; }
            set
            {
                if (value.Length % 2 != 0)
                    throw new ArgumentException("value must have a length as a multiple of 2");

                this.exploredData = value;
                this.Size = exploredData.Length;
            }
        }

        /// <summary>Exposes the count of 16x16 explorable cells</summary>
        public Int32 Count
        {
            get { return this.exploredData.Length * 8; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.exploredData = ReusableIO.BinaryRead(input, this.Size);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            output.Write(this.exploredData, 0, this.Size);
        }
        #endregion
    }
}