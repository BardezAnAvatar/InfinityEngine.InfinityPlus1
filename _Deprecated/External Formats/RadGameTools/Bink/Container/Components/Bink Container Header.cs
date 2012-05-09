using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Components
{
    /// <summary>Represents the Bink container's wrapper for the header and sub-header items</summary>
    public class BinkContainerHeader
    {
        #region Fields
        /// <summary>Basic header sotred</summary>
        public BinkHeader Header { get; set; }

        /// <summary>Collection of audio track channel counts</summary>
        public List<AudioTrackChannels> TrackChannels { get; set; }

        /// <summary>Collection of audio track sample rates and flags</summary>
        public List<AudioTrackSamples> TrackSampleRates { get; set; }

        /// <summary>Collection of unique track IDs</summary>
        public List<Int32> TrackIds { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new BinkHeader();
            this.TrackChannels = new List<AudioTrackChannels>();
            this.TrackSampleRates = new List<AudioTrackSamples>();
            this.TrackIds = new List<Int32>();
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
                this.Header = new BinkHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);

            //audio track channels
            for (Int32 track = 0; track < this.Header.AudioTracks; ++track)
            {
                AudioTrackChannels channels = new AudioTrackChannels();
                channels.Read(input);
                this.TrackChannels.Add(channels);
            }

            //audio track sample rate & flags
            for (Int32 track = 0; track < this.Header.AudioTracks; ++track)
            {
                AudioTrackSamples samplesFlags = new AudioTrackSamples();
                samplesFlags.Read(input);
                this.TrackSampleRates.Add(samplesFlags);
            }

            //audio track unique IDs
            for (Int32 track = 0; track < this.Header.AudioTracks; ++track)
            {
                Byte[] buffer = ReusableIO.BinaryRead(input, 4);
                this.TrackIds.Add(ReusableIO.ReadInt32FromArray(buffer, 0));
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.Header.Write(output);

            foreach (AudioTrackChannels channels in this.TrackChannels)
                channels.Write(output);

            foreach (AudioTrackSamples samplesFlags in this.TrackSampleRates)
                samplesFlags.Write(output);

            foreach (Int32 trackId in this.TrackIds)
                ReusableIO.WriteInt32ToStream(trackId, output);
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
            return "Bink Container Header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(this.Header.ToString());

            for (Int32 index = 0; index < this.TrackChannels.Count; ++ index)
                builder.Append(this.TrackChannels[index].ToString(index));

            for (Int32 index = 0; index < this.TrackSampleRates.Count; ++index)
                builder.Append(this.TrackSampleRates[index].ToString(index));

            for (Int32 index = 0; index < this.TrackIds.Count; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Track # {0} ID", index)));
                builder.Append(this.TrackIds[index]);
            }

            return builder.ToString();
        }
        #endregion
    }
}