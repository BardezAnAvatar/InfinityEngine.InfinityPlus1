using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Dialog.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Dialog.Component
{
    public class DialogTransition : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 32;

        #region Members
        /// <summary>Flags associated with the dialog transition</summary>
        protected TransitionFlags flags;

        /// <summary>String reference for text to display in this transition.</summary>
        protected StringReference text;

        /// <summary>String reference for journal entry to add to the player's journal state.</summary>
        protected StringReference journalEntry;

        /// <summary>Index into the triggers table of the assiciated trigger for this transition</summary>
        /// <value>Index or -1 if none</value>
        protected Int32 indexTrigger;

        /// <summary>Index into the actions table of the assiciated action for this transition</summary>
        /// <value>Index or -1 if none</value>
        protected Int32 indexAction;

        /// <summary>Resource reference to next dialog file transitioned in to</summary>
        protected ResourceReference nextDialog;

        /// <summary>Index into the states table in which the next state will be found</summary>
        protected Int32 indexState;
        #endregion

        #region Properties
        /// <summary>Flags associated with the dialog transition</summary>
        public TransitionFlags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }

        /// <summary>String reference for text to display in this transition.</summary>
        public StringReference Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        /// <summary>String reference for journal entry to add to the player's journal state.</summary>
        public StringReference JournalEntry
        {
            get { return this.journalEntry; }
            set { this.journalEntry = value; }
        }

        /// <summary>Index into the triggers table of the assiciated trigger for this transition</summary>
        /// <value>Index or -1 if none</value>
        public Int32 IndexTrigger
        {
            get { return this.indexTrigger; }
            set { this.indexTrigger = value; }
        }

        /// <summary>Index into the actions table of the assiciated action for this transition</summary>
        /// <value>Index or -1 if none</value>
        public Int32 IndexAction
        {
            get { return this.indexAction; }
            set { this.indexAction = value; }
        }

        /// <summary>Resource reference to next dialog file transitioned in to</summary>
        public ResourceReference NextDialog
        {
            get { return this.nextDialog; }
            set { this.nextDialog = value; }
        }

        /// <summary>Index into the states table in which the next state will be found</summary>
        public Int32 IndexState
        {
            get { return this.indexState; }
            set { this.indexState = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public DialogTransition()
        {
            this.text = null;
            this.journalEntry = null;
            this.nextDialog = null;
        }
 
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.text = new StringReference();
            this.journalEntry = new StringReference();
            this.nextDialog = new ResourceReference();
        }
        #endregion

        #region Abstract IO methods
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

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] trans = ReusableIO.BinaryRead(input, StructSize);

            this.flags = (TransitionFlags)ReusableIO.ReadUInt32FromArray(trans, 0);
            this.text.StringReferenceIndex = ReusableIO.ReadInt32FromArray(trans, 4);
            this.journalEntry.StringReferenceIndex = ReusableIO.ReadInt32FromArray(trans, 8);
            this.indexTrigger = ReusableIO.ReadInt32FromArray(trans, 12);
            this.indexAction = ReusableIO.ReadInt32FromArray(trans, 16);
            this.nextDialog.ResRef = ReusableIO.ReadStringFromByteArray(trans, 20, Constants.CultureCodeEnglish);
            this.indexState = ReusableIO.ReadInt32FromArray(trans, 28);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream((UInt32)this.flags, output);
            ReusableIO.WriteInt32ToStream(this.text.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.journalEntry.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.indexTrigger, output);
            ReusableIO.WriteInt32ToStream(this.indexAction, output);
            ReusableIO.WriteStringToStream(this.nextDialog.ResRef, output, Constants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.indexState, output);
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
            String header = "Dialog transition:";

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
            return String.Format("Dialog transition # {0}:", entryIndex) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Flags"));
            builder.Append((UInt32)this.flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumerated)"));
            builder.Append(this.GetFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Text StrRef"));
            builder.Append(this.text.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Journal StrRef"));
            builder.Append(this.journalEntry.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Trigger index"));
            builder.Append(this.indexTrigger);
            builder.Append(StringFormat.ToStringAlignment("Action index"));
            builder.Append(this.indexAction);
            builder.Append(StringFormat.ToStringAlignment("Next dialog file"));
            builder.Append(this.nextDialog.ResRef);
            builder.Append(StringFormat.ToStringAlignment("Next dialog state"));
            builder.Append(this.indexState);
            builder.Append("\n\n");

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which D20 arcetype flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetFlagsString()
        {
            StringBuilder sb = new StringBuilder();
            
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.HasText) == TransitionFlags.HasText, TransitionFlags.HasText.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.HasTrigger) == TransitionFlags.HasTrigger, TransitionFlags.HasTrigger.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.HasAction) == TransitionFlags.HasAction, TransitionFlags.HasAction.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.TerminatesDialog) == TransitionFlags.TerminatesDialog, TransitionFlags.TerminatesDialog.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.AddsJournalEntry) == TransitionFlags.AddsJournalEntry, TransitionFlags.AddsJournalEntry.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.AddsJournalEntryQuest) == TransitionFlags.AddsJournalEntryQuest, TransitionFlags.AddsJournalEntryQuest.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.RemovesJournalEntry) == TransitionFlags.RemovesJournalEntry, TransitionFlags.RemovesJournalEntry.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & TransitionFlags.RemovesJournalEntryQuest) == TransitionFlags.RemovesJournalEntryQuest, TransitionFlags.RemovesJournalEntryQuest.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? "\n\t\tNone" : result;
        }
        #endregion
    }
}