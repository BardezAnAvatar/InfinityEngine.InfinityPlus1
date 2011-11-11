using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM
{
    /// <summary>Wrapper around the DWORD buffer for amplitude decoding</summary>
    public class AmplitudeBuffer
    {
        #region Fields
        /// <summary>The Amplitude buffer</summary>
        /// <remarks>Should be 0x10000 bytes in size</remarks>
        protected Int16[] buffer;

        /// <summary>Index to the middle of the amplitude buffer</summary>
        protected const Int32 middle = 0x8000;

        /// <summary>Expected size of the amplitude buffer</summary>
        protected const Int32 bufferSize = 0x10000;
        #endregion

        #region Properties
        /// <summary>Represents the accessor to the middle of the amplitude buffer</summary>
        /// <param name="index">Index of the amplitude buffer</param>
        public Int16 this[Int32 index]
        {
            get { return this.buffer[index + middle]; }
            set { this.buffer[index + middle] = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public AmplitudeBuffer()
        {
            this.buffer = new Int16[AmplitudeBuffer.bufferSize];
        }

        /// <summary>Filler constructor</summary>
        /// <param name="shift">Bits to shift, which determine the population range</param>
        /// <param name="value">Value to be multiplied on index to assign to that point in memory</param>
        public AmplitudeBuffer(Int16 shift, Int16 value) : this()
        {
            this.Fill(shift, value);
        }
        #endregion

        /// <summary>Fills the Amplitude Buffer with values, ranging from -power to power</summary>
        /// <param name="shift">Start and end of the value, from negative range to positive range</param>
        /// <param name="value">Value to be multiplied on index to assign to that point in memory</param>
        public void Fill(Int16 shift, Int16 value)
        {
            Int32 range = 1 << shift;

            for (Int32 index = -range; index < range; ++index)
                this[index] = Convert.ToInt16(index * value);
        }
    }
}