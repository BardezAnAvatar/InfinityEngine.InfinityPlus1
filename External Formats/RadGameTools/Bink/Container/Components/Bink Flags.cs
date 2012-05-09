using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Components
{
    /// <summary>Wraps the Bink flags in a single reference</summary>
    public class BinkFlags
    {
        #region Constants
        /// <summary>Mask for the grayscale bit</summary>
        private const UInt32 MaskGrayscale  = 0x00020000U;

        /// <summary>Has an alpha plane</summary>
        private const UInt32 MaskAlphaPlane = 0x00100000U;

        /// <summary>Mask for the 4 most significant bits, used to describe scaling</summary>
        private const UInt32 MaskScaling    = 0xF0000000U;
        #endregion


        #region Fields
        /// <summary>Raw value of the flags</summary>
        public UInt32 FlagsValue { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the four most significant bits of the underlying value</summary>
        public UInt32 Scaling
        {
            get { return (this.FlagsValue & BinkFlags.MaskScaling) >> 28; }
            set
            {
                UInt32 scaling = (this.FlagsValue & ~BinkFlags.MaskScaling);
                this.FlagsValue = scaling | ((value << 28) & BinkFlags.MaskScaling);
            }
        }

        /// <summary>Exposes the flag for grayscale color</summary>
        public Boolean Grayscale
        {
            get { return Convert.ToBoolean((this.FlagsValue & BinkFlags.MaskGrayscale) >> 17); }
            set
            {
                UInt32 grayscale = Convert.ToUInt32(value);
                UInt32 flags = (this.FlagsValue & ~BinkFlags.MaskGrayscale);
                this.FlagsValue = flags | (grayscale << 17);
            }
        }

        /// <summary>Exposes the flag for alpha transparencycolor</summary>
        public Boolean HasAlpha
        {
            get { return Convert.ToBoolean((this.FlagsValue & BinkFlags.MaskGrayscale) >> 17); }
            set
            {
                UInt32 alpha = Convert.ToUInt32(value);
                UInt32 flags = (this.FlagsValue & ~BinkFlags.MaskAlphaPlane);
                this.FlagsValue = flags | (alpha << 20);
            }
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Bink Flags (value)", 2));
            builder.Append(this.FlagsValue);
            builder.Append(StringFormat.ToStringAlignment("Scaling", 2));
            builder.Append(this.Scaling);
            builder.Append(StringFormat.ToStringAlignment("Grayscale", 2));
            builder.Append(this.Grayscale);
            builder.Append(StringFormat.ToStringAlignment("Has alpha", 2));
            builder.Append(this.HasAlpha);

            return builder.ToString();
        }
        #endregion
    }
}