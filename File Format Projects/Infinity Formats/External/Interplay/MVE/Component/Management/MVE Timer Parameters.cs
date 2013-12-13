using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    public class MveTimerParams
    {
        #region Fields
        /// <summary>Exposes the numertor of the frame rate</summary>
        public Int64 Numerator { get; set; }

        /// <summary>Exposes the denominator of the frame rate</summary>
        public Int64 Denominator { get; set; }
        #endregion


        #region Properties
        /// <summary>Frequency of frame refresh, in milliseconds</summary>
        public Int32 MillisecondDelay
        {
            get { return Convert.ToInt32(this.FrameIntervalDouble); }
        }

        /// <summary>Frames per second rate of the media file</summary>
        public Double FrameRateDouble
        {
            get { return Convert.ToDouble(this.Numerator) / Convert.ToDouble(this.Denominator); }
        }

        /// <summary>Milliseconds per Frame of the media file</summary>
        public Double FrameIntervalDouble
        {
            get { return Convert.ToDouble(this.Denominator) / Convert.ToDouble(this.Numerator) * 1000.0; }
        }

        /// <summary>Frames per second rate of the media file</summary>
        public Decimal FrameRateDecimal
        {
            get { return Convert.ToDecimal(this.Numerator) / Convert.ToDecimal(this.Denominator); }
        }

        /// <summary>Milliseconds per Frame of the media file</summary>
        public Decimal FrameIntervalDecimal
        {
            get { return Convert.ToDecimal(this.Denominator) / Convert.ToDecimal(this.Numerator) * 1000M; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="numerator">Frame rate numerator</param>
        /// <param name="denominator">Frame rate denominator</param>
        public MveTimerParams(Int64 numerator, Int64 denominator)
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
        }
        #endregion
    }
}
