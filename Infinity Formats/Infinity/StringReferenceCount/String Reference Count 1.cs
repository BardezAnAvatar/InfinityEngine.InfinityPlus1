using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.StringReferenceCount
{
    /// <summary>Planescape: Torment's SRC file format. Guessing as to the extension's meaning</summary>
    public class StringReferenceCount1 : IInfinityFormat
    {
        #region Members
        /// <summary>The number of entries in this file</summary>
        protected UInt32 countEntries;

        /// <summary>List of strings emotable</summary>
        protected List<EmotionStringReference> emotions;
        #endregion

        #region Properties
        /// <summary>The number of entries in this file</summary>
        public UInt32 CountEntries
        {
            get { return this.countEntries; }
            set { this.countEntries = value; }
        }

        /// <summary>List of strings emotable</summary>
        public List<EmotionStringReference> Emotions
        {
            get { return this.emotions; }
            set { this.emotions = value; }
        }
        #endregion
        
        #region Construction
        /// <summary>Default constructor</summary>
        public StringReferenceCount1()
        {
            this.emotions = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.emotions = new List<EmotionStringReference>();
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] emote = ReusableIO.BinaryRead(input, 4);

            this.countEntries = ReusableIO.ReadUInt32FromArray(emote, 0);

            for (Int32 index = 0; index < this.countEntries; ++index)
            {
                EmotionStringReference esr = new EmotionStringReference();
                esr.Read(input);
                this.emotions.Add(esr);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public void Write(Stream output)
        {
            //minimal data integrity check
            this.MaintainMinimalDataIntegrity();

            ReusableIO.WriteUInt32ToStream(this.countEntries, output);
            foreach (EmotionStringReference emotion in this.emotions)
                emotion.Write(output);
        }
        #endregion

        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = "Emotion string count:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Emotion count"));
            builder.Append(this.countEntries);

            for (Int32 i = 0; i < this.emotions.Count; ++i)
                builder.Append(this.emotions[i].ToString(i + 1));

            return builder.ToString();
        }
        #endregion

        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected void MaintainMinimalDataIntegrity()
        {
            this.countEntries = Convert.ToUInt32(this.emotions.Count);
        }
        #endregion
    }
}