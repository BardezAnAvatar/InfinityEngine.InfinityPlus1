using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common
{
    /// <summary>Represents a single color entry for an Infinity Engine palette</summary>
    /// <remarks>This is a 32-bit palette, stored in B G R 0 order</remarks>
    public class TransparencyColorEntry : ColorEntry
    {
        #region Fields
        public Byte Alpha { get; set; }
        #endregion

        #region Construction
        public TransparencyColorEntry(ColorEntry baseColor, Byte alpha)
        {
            this.Red = baseColor.Red;
            this.Green = baseColor.Green;
            this.Blue = baseColor.Blue;
            this.Alpha = alpha;
        }
        #endregion


        #region ToString() methods
        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected override String GetVersionString()
        {
            return "Transparency Color:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Red"));
            builder.Append(this.Red);
            builder.Append(StringFormat.ToStringAlignment("Green"));
            builder.Append(this.Green);
            builder.Append(StringFormat.ToStringAlignment("Blue"));
            builder.Append(this.Blue);
            builder.Append(StringFormat.ToStringAlignment("Alpha"));
            builder.Append(this.Alpha);
            builder.Append(StringFormat.ToStringAlignment("Hex"));
            builder.Append(this.GetHexStringRepresentation());

            return builder.ToString();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetHexStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Hex"));
            //intentional space for alpha transparency
            builder.Append(String.Format("#{0:X2}{1:X2}{2:X2} {3:X2}", this.Red, this.Green, this.Blue, this.Alpha));

            return builder.ToString();
        }
        #endregion
    }
}