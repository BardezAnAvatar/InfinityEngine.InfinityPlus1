using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Dialog.Component
{
    public class DialogState : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 16;

        #region Members
        /// <summary>String reference for text to display in this state.</summary>
        protected StringReference text;

        /// <summary>Index into the transitions table in which the first transition is found</summary>
        protected UInt32 indexTransition;

        /// <summary>Count within the transitions table of available transitions for this state</summary>
        protected UInt32 countTransition;

        /// <summary>Index into the triggers table of the assiciated trigger for this state</summary>
        /// <value>Index or -1 if none</value>
        protected Int32 indexTrigger;
        #endregion

        #region Properties
        /// <summary>String reference for text to display in this state.</summary>
        public StringReference Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        /// <summary>Index into the transitions table in which the first transition is found</summary>
        public UInt32 IndexTransition
        {
            get { return this.indexTransition; }
            set { this.indexTransition = value; }
        }

        /// <summary>Count within the transitions table of available transitions for this state</summary>
        public UInt32 CountTransition
        {
            get { return this.countTransition; }
            set { this.countTransition = value; }
        }

        /// <summary>Index into the triggers table of the assiciated trigger for this state</summary>
        /// <value>Index or -1 if none</value>
        public Int32 IndexTrigger
        {
            get { return this.indexTrigger; }
            set { this.indexTrigger = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public DialogState()
        {
            this.text = null;
        }
 
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.text = new StringReference();
        }
        #endregion

        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the input stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] state = ReusableIO.BinaryRead(input, StructSize);

            this.text.StringReferenceIndex = ReusableIO.ReadInt32FromArray(state, 0);
            this.indexTransition = ReusableIO.ReadUInt32FromArray(state, 4);
            this.countTransition = ReusableIO.ReadUInt32FromArray(state, 8);
            this.indexTrigger = ReusableIO.ReadInt32FromArray(state, 12);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.text.StringReferenceIndex, output);
            ReusableIO.WriteUInt32ToStream(this.indexTransition, output);
            ReusableIO.WriteUInt32ToStream(this.countTransition, output);
            ReusableIO.WriteInt32ToStream(this.indexTrigger, output);
        }
        #endregion

        #region ToString() Helpers
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
            String header = "Dialog state:";

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
            return StringFormat.ReturnAndIndent(String.Format("Dialog state # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Text StrRef"));
            builder.Append(this.text.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Transition index"));
            builder.Append(this.indexTransition);
            builder.Append(StringFormat.ToStringAlignment("Transition count"));
            builder.Append(this.countTransition);
            builder.Append(StringFormat.ToStringAlignment("Trigger index"));
            builder.Append(this.indexTrigger);

            return builder.ToString();
        }
        #endregion
    }
}