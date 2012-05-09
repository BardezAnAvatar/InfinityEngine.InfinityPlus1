using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Components
{
    /// <summary>Represents the common Bink video header (without audio tracks)</summary>
    /// <remarks>
    ///     Based off of documentation at:
    ///     http://wiki.multimedia.cx/index.php?title=Bink_File_Format
    /// </remarks>
    public class BinkHeader
    {
        #region Constants
        /// <summary>Represnts the size of this structure on disk</summary>
        public const Int32 StructSize = 44;
        #endregion


        #region Fields
        /// <summary>Three-byte file signature, should be 'BIK'</summary>
        public String Signature { get; set; }

        /// <summary>Version of the file container (and media?)</summary>
        /// <value>'b', 'd', 'f', 'g', 'h', 'i'</value>
        public Byte Version { get; set; }

        /// <summary>Size of the data in the file after the leading 8 bytes (starting after this field)</summary>
        public Int32 RemainingFileSize { get; set; }

        /// <summary>Count of frames in the video</summary>
        public Int32 CountFrames { get; set; }

        /// <summary>Size of the largest frame, in Bytes</summary>
        public Int32 SizeLargestFrame { get; set; }

        /// <summary>Redundant frame count? Possibly a buffer size</summary>
        public Int32 CountFrames2 { get; set; }

        /// <summary>Width, in pixels, of the video</summary>
        public Int32 Width { get; set; }

        /// <summary>Height, in pixels, of the video</summary>
        public Int32 Height { get; set; }

        /// <summary>Numerator of the frame rate</summary>
        public Int32 FrameRateNumerator { get; set; }

        /// <summary>Denominator of the frame rate</summary>
        public Int32 FrameRateDenominator { get; set; }

        /// <summary>Bink header video flags</summary>
        public BinkFlags Flags { get; set; }

        /// <summary>Number of channels in this video</summary>
        public Int32 AudioTracks { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the frame rate, as a Double</summary>
        public virtual Double FrameRateDouble
        {
            get { return Convert.ToDouble(this.FrameRateNumerator) / Convert.ToDouble(this.FrameRateDenominator); }
        }

        /// <summary>Exposes the frame rate, as a Decimal</summary>
        public virtual Decimal FrameRateDecimal
        {
            get { return Convert.ToDecimal(this.FrameRateNumerator) / Convert.ToDecimal(this.FrameRateDenominator); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Flags = new BinkFlags();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                Byte[] buffer = ReusableIO.BinaryRead(input, 4);
                this.Signature = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 3);
                this.Version = buffer[3];
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 4);

            this.Signature = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 3);
            this.Version = buffer[3];
            this.RemainingFileSize = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.CountFrames = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.SizeLargestFrame = ReusableIO.ReadInt32FromArray(buffer, 12);
            this.CountFrames2 = ReusableIO.ReadInt32FromArray(buffer, 16);
            this.Width = ReusableIO.ReadInt32FromArray(buffer, 20);
            this.Height = ReusableIO.ReadInt32FromArray(buffer, 24);
            this.FrameRateNumerator = ReusableIO.ReadInt32FromArray(buffer, 28);
            this.FrameRateDenominator = ReusableIO.ReadInt32FromArray(buffer, 32);
            this.Flags.FlagsValue = ReusableIO.ReadUInt32FromArray(buffer, 36);
            this.AudioTracks = ReusableIO.ReadInt32FromArray(buffer, 40);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Signature, output, CultureConstants.CultureCodeEnglish, false, 3);
            output.WriteByte(this.Version);
            ReusableIO.WriteInt32ToStream(this.RemainingFileSize, output);
            ReusableIO.WriteInt32ToStream(this.CountFrames, output);
            ReusableIO.WriteInt32ToStream(this.SizeLargestFrame, output);
            ReusableIO.WriteInt32ToStream(this.CountFrames2, output);
            ReusableIO.WriteInt32ToStream(this.Width, output);
            ReusableIO.WriteInt32ToStream(this.Height, output);
            ReusableIO.WriteInt32ToStream(this.FrameRateNumerator, output);
            ReusableIO.WriteInt32ToStream(this.FrameRateDenominator, output);
            ReusableIO.WriteUInt32ToStream(this.Flags.FlagsValue, output);
            ReusableIO.WriteInt32ToStream(this.AudioTracks, output);
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

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected virtual String GetVersionString()
        {
            return "Bink Header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Signature)));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.Version);
            builder.Append(StringFormat.ToStringAlignment("Remaining file size"));
            builder.Append(this.RemainingFileSize);
            builder.Append(StringFormat.ToStringAlignment("Frame count"));
            builder.Append(this.CountFrames);
            builder.Append(StringFormat.ToStringAlignment("Largest frame binary size"));
            builder.Append(this.SizeLargestFrame);
            builder.Append(StringFormat.ToStringAlignment("Frame count (redundant, buffer lenghh?)"));
            builder.Append(this.CountFrames2);
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Frame rate numerator"));
            builder.Append(this.FrameRateNumerator);
            builder.Append(StringFormat.ToStringAlignment("Frame rate denominator"));
            builder.Append(this.FrameRateDenominator);
            builder.Append(StringFormat.ToStringAlignment("Frame rate"));
            builder.Append(this.FrameRateDecimal);
            builder.Append(StringFormat.ToStringAlignment("Flags"));
            builder.Append(this.Flags.ToString());
            builder.Append(StringFormat.ToStringAlignment("Audio track count"));
            builder.Append(this.AudioTracks);

            return builder.ToString();
        }
        #endregion
    }
}