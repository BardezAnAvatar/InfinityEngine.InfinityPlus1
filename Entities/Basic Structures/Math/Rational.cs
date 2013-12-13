using System;

namespace Bardez.Projects.BasicStructures.Math
{
    /// <summary>Representation of numerator/denominator rational number</summary>
    public class Rational
    {
        #region Fields
        /// <summary>Numerator of the rational number</summary>
        public Int32 Numerator { get; set; }

        /// <summary>Denominator of the rational number</summary>
        public Int32 Denominator { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the double-precision floating-point representation of the rational</summary>
		public Double ValueDouble
		{
			get { return (Convert.ToDouble(this.Numerator) / Convert.ToDouble(this.Denominator)); }
		}

        /// <summary>Exposes the single-precision floating-point representation of the rational</summary>
        public Single ValueSingle
        {
            get { return (Convert.ToSingle(this.Numerator) / Convert.ToSingle(this.Denominator)); }
        }

        /// <summary>Exposes the integer representation of the rational</summary>
        public Int32 ValueInt32
        {
            get { return this.Numerator / this.Denominator; }
        }

        /// <summary>A rational indicating a value of 1</summary>
        public static Rational One
        {
            get { return new Rational(1, 1); }
        }
        #endregion


		#region Construction
		/// <summary>Definition constructor</summary>
		/// <param name="numerator">Numerator of this rational</param>
		/// <param name="denominator">Denominator of this rational</param>
		public Rational(Int32 numerator, Int32 denominator)
		{
			this.Numerator = numerator;
			this.Denominator = denominator;
        }
        #endregion
    }
}