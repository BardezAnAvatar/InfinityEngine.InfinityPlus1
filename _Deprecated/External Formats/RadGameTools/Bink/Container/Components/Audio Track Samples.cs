using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Components
{
    /// <summary>Second set of audio track values in an array after the header</summary>
    /// <remarks>
    ///     Based off of documentation at:
    ///     http://wiki.multimedia.cx/index.php?title=Bink_File_Format
    /// </remarks>
    public class AudioTrackSamples
    {
        #region Constants
        /// <summary>Represents thte size of this structure on disk</summary>
        public const Int32 StructSize = 4;
        #endregion


        #region Fields
        /// <summary>Sample rate of the track</summary>
        public Int16 SampleRate { get; set; }

        /// <summary>Flags for the audio track</summary>
        public AudioFlags Flags { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes whether the audio uses DCT</summary>
        public Boolean AudioDCT
        {
            get { return (this.Flags & AudioFlags.DCT) == AudioFlags.DCT; }
        }

        /// <summary>Exposes whether the audio uses FFT</summary>
        public Boolean AudioFFT
        {
            get { return (this.Flags & AudioFlags.DCT) != AudioFlags.DCT; }
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
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 4);
            this.SampleRate = ReusableIO.ReadInt16FromArray(buffer, 0);
            this.Flags = (AudioFlags)ReusableIO.ReadUInt16FromArray(buffer, 2);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteInt16ToStream(this.SampleRate, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Flags, output);
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

            builder.Append(StringFormat.ToStringAlignment("Sample rate"));
            builder.Append(this.SampleRate);
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt16)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetFlagsEnumerationString());

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & AudioFlags.Stereo) == AudioFlags.Stereo, AudioFlags.Stereo.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & AudioFlags.DCT) == AudioFlags.DCT, AudioFlags.DCT.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}