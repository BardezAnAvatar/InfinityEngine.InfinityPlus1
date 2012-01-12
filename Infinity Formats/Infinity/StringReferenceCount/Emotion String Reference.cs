using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.StringReferenceCount
{
    public class EmotionStringReference : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 8;

        #region Members
        /// <summary>String reference to display</summary>
        protected StringReference emotion;
        
        /// <summary>Unknown, so guessing. Always(?) set to 1</summary>
        protected Int32 displayable;
        #endregion

        #region Properties
        /// <summary>String reference to display</summary>
        public StringReference Emotion
        {
            get { return this.emotion; }
            set { this.emotion = value; }
        }
        
        /// <summary>Unknown, so guessing. Always(?) set to 1</summary>
        public Int32 Displayable
        {
            get { return this.displayable; }
            set { this.displayable = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public EmotionStringReference()
        {
            this.emotion = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.emotion = new StringReference();
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
            Byte[] emote = ReusableIO.BinaryRead(input, StructSize);

            this.emotion.StringReferenceIndex = ReusableIO.ReadInt32FromArray(emote, 0);
            this.displayable = ReusableIO.ReadInt32FromArray(emote, 4);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.emotion.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.displayable, output);
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
            String header = "Emotion string:";

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
            return StringFormat.ToStringAlignment(String.Format("Emotion string # {0}", entryIndex)) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Emotion"));
            builder.Append(this.emotion.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Displayable"));
            builder.Append(this.displayable);

            return builder.ToString();
        }
        #endregion
    }
}