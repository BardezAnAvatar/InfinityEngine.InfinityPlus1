using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Components
{
    /// <summary>Represents an offset to a frame</summary>
    public class FrameOffset
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 4;

        /// <summary>Offset mask</summary>
        public const Int32 Mask = -2;   //FFFFFFFE
        #endregion


        #region Fields
        /// <summary>Offset raw value read</summary>
        public Int32 OffsetValue { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the offset to the frame</summary>
        public Int32 Offset
        {
            get { return this.OffsetValue & FrameOffset.Mask; }
            set
            {
                Int32 keyframe = this.OffsetValue & 1;
                this.OffsetValue = ((value & FrameOffset.Mask) | keyframe);
            }
        }

        /// <summary>Exposes whether or not this frame is a key frame</summary>
        public Boolean KeyFrame
        {
            get { return Convert.ToBoolean(this.OffsetValue & 1); }
            set
            {
                Int32 offset = this.OffsetValue & FrameOffset.Mask;
                this.OffsetValue = (Convert.ToInt32(value) | offset);
            }
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public virtual String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndex">Known spells entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Bink audio track # {0} samples:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected virtual String GetVersionString()
        {
            return "Bink audio track samples:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Offset (value)"));
            builder.Append(this.OffsetValue);
            builder.Append(StringFormat.ToStringAlignment("Offset (actual)"));
            builder.Append(this.Offset);
            builder.Append(StringFormat.ToStringAlignment("Keyframe"));
            builder.Append(this.KeyFrame);

            return builder.ToString();
        }
        #endregion
    }
}