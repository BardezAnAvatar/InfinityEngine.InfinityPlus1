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
    /// <summary>Represents the header of the dialog file, be it a set of actions, state triggers or transition triggers</summary>
    public class DialogHeader : InfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        /// <remarks>Possibly 4 too big</remarks>
        public const Int32 StructSize = 52;

        #region Members
        /// <summary>Number of dialog states</summary>
        protected UInt32 countStates;

        /// <summary>Offset to states from start of file</summary>
        protected UInt32 offsetStates;

        /// <summary>Number of dialog transitions/responses</summary>
        protected UInt32 countTransitions;

        /// <summary>Offset to transitions from start of file</summary>
        protected UInt32 offsetTransitions;

        /// <summary>Offset to state triggers from start of file</summary>
        protected UInt32 offsetTriggersState;

        /// <summary>Number of state triggers</summary>
        protected UInt32 countTriggersState;

        /// <summary>Offset to transition triggers from start of file</summary>
        protected UInt32 offsetTriggersTransition;

        /// <summary>Number of transition triggers</summary>
        protected UInt32 countTriggersTransition;

        /// <summary>Offset to actions from start of file</summary>
        protected UInt32 offsetActions;

        /// <summary>Number of actions</summary>
        protected UInt32 countActions;

        /// <summary>Optional interruption action flags</summary>
        /// <remarks>I think the existance is determined by the first offset, being 0x34 instead of the old 0x30</remarks>
        protected InterruptionFlagAction? interruptionFlags;
        #endregion

        #region Properties
        /// <summary>Number of dialog states</summary>
        public UInt32 CountStates
        {
            get { return this.countStates; }
            set { this.countStates = value; }
        }

        /// <summary>Offset to states from start of file</summary>
        public UInt32 OffsetStates
        {
            get { return this.offsetStates; }
            set { this.offsetStates = value; }
        }

        /// <summary>Number of dialog transitions/responses</summary>
        public UInt32 CountTransitions
        {
            get { return this.countTransitions; }
            set { this.countTransitions = value; }
        }

        /// <summary>Offset to transitions from start of file</summary>
        public UInt32 OffsetTransitions
        {
            get { return this.offsetTransitions; }
            set { this.offsetTransitions = value; }
        }

        /// <summary>Offset to state triggers from start of file</summary>
        public UInt32 OffsetTriggersState
        {
            get { return this.offsetTriggersState; }
            set { this.offsetTriggersState = value; }
        }

        /// <summary>Number of state triggers</summary>
        public UInt32 CountTriggersState
        {
            get { return this.countTriggersState; }
            set { this.countTriggersState = value; }
        }

        /// <summary>Offset to transition triggers from start of file</summary>
        public UInt32 OffsetTriggersTransition
        {
            get { return this.offsetTriggersTransition; }
            set { this.offsetTriggersTransition = value; }
        }

        /// <summary>Number of transition triggers</summary>
        public UInt32 CountTriggersTransition
        {
            get { return this.countTriggersTransition; }
            set { this.countTriggersTransition = value; }
        }

        /// <summary>Offset to actions from start of file</summary>
        public UInt32 OffsetActions
        {
            get { return this.offsetActions; }
            set { this.offsetActions = value; }
        }

        /// <summary>Number of actions</summary>
        public UInt32 CountActions
        {
            get { return this.countActions; }
            set { this.countActions = value; }
        }

        /// <summary>Optional interruption action flags</summary>
        /// <remarks>I think the existance is determined by the first offset, being 0x34 instead of the old 0x30</remarks>
        public InterruptionFlagAction? InterruptionFlags
        {
            get { return this.interruptionFlags; }
            set { this.interruptionFlags = value; }
        }

        /// <summary>Returns a Boolean indicating whether or not the dialog is pausing in nature</summary>
        public Boolean PauseEngine
        {
            get
            {
                Boolean pause = false;

                if (this.interruptionFlags == null || this.interruptionFlags == ((InterruptionFlagAction)0))
                    pause = true;

                return pause;
            }
        }

        /// <summary>Integer exposing the effective size of the header.</summary>
        public Int32 StrutureSize
        {
            get
            {
                Int32 size = StructSize;

                if (this.interruptionFlags == null)
                    size -= 4;

                return size;
            }
        }

        #endregion
        
        #region Construction
        /// <summary>Default constructor</summary>
        public DialogHeader()
        {
            this.Initialize();
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.interruptionFlags = null;
        }
        #endregion

        #region Abstract IO methods
        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] header = ReusableIO.BinaryRead(input, StructSize - 8);

            this.countStates = ReusableIO.ReadUInt32FromArray(header, 0);
            this.offsetStates = ReusableIO.ReadUInt32FromArray(header, 4);
            this.countTransitions = ReusableIO.ReadUInt32FromArray(header, 8);
            this.offsetTransitions = ReusableIO.ReadUInt32FromArray(header, 12);
            this.offsetTriggersState = ReusableIO.ReadUInt32FromArray(header, 16);
            this.countTriggersState = ReusableIO.ReadUInt32FromArray(header, 20);
            this.offsetTriggersTransition = ReusableIO.ReadUInt32FromArray(header, 24);
            this.countTriggersTransition = ReusableIO.ReadUInt32FromArray(header, 28);
            this.offsetActions = ReusableIO.ReadUInt32FromArray(header, 32);
            this.countActions = ReusableIO.ReadUInt32FromArray(header, 36);

            if (this.offsetStates > 0x33U && this.offsetTransitions > 0x33U && this.offsetTriggersTransition > 0x33U && this.offsetActions > 0x33U)
                this.interruptionFlags = (InterruptionFlagAction)ReusableIO.ReadUInt32FromArray(header, 40);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, Constants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, Constants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt32ToStream(this.countStates, output);
            ReusableIO.WriteUInt32ToStream(this.offsetStates, output);
            ReusableIO.WriteUInt32ToStream(this.countTransitions, output);
            ReusableIO.WriteUInt32ToStream(this.offsetTransitions, output);
            ReusableIO.WriteUInt32ToStream(this.offsetTriggersState, output);
            ReusableIO.WriteUInt32ToStream(this.countTriggersState, output);
            ReusableIO.WriteUInt32ToStream(this.offsetTriggersTransition, output);
            ReusableIO.WriteUInt32ToStream(this.countTriggersTransition, output);
            ReusableIO.WriteUInt32ToStream(this.offsetActions, output);
            ReusableIO.WriteUInt32ToStream(this.countActions, output);

            if (this.interruptionFlags != null)
                ReusableIO.WriteUInt32ToStream((UInt32)this.interruptionFlags, output);
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
        public String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Dialog header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.version);
            builder.Append(StringFormat.ToStringAlignment("Count of States"));
            builder.Append(this.countStates);
            builder.Append(StringFormat.ToStringAlignment("Offset to States"));
            builder.Append(this.offsetStates);
            builder.Append(StringFormat.ToStringAlignment("Count of Transitions"));
            builder.Append(countTransitions);
            builder.Append(StringFormat.ToStringAlignment("Offset to Transitions"));
            builder.Append(this.offsetTransitions);
            builder.Append(StringFormat.ToStringAlignment("Offset to State Triggers"));
            builder.Append(this.offsetTriggersState);
            builder.Append(StringFormat.ToStringAlignment("Count of State Triggers"));
            builder.Append(this.countTriggersState);
            builder.Append(StringFormat.ToStringAlignment("Offset to Transition Triggers"));
            builder.Append(this.offsetTriggersTransition);
            builder.Append(StringFormat.ToStringAlignment("Count of Transition Triggers"));
            builder.Append(this.countTriggersTransition);
            builder.Append(StringFormat.ToStringAlignment("Offset to Actions"));
            builder.Append(this.offsetActions);
            builder.Append(StringFormat.ToStringAlignment("Count of Actions"));
            builder.Append(this.countActions);
            builder.Append(StringFormat.ToStringAlignment("Dialog Interruption Flag Action"));
            builder.Append(this.interruptionFlags == null ? "NULL" : ((UInt32)this.interruptionFlags).ToString());
            builder.Append(StringFormat.ToStringAlignment("Dialog Interruption Flag Action (enumerated)"));
            builder.Append(this.GetInterruptionFlagString());
            builder.Append("\n\n");

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which Storelags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetInterruptionFlagString()
        {
            StringBuilder sb = new StringBuilder();

            if (this.interruptionFlags != null)
            {
                StringFormat.AppendSubItem(sb, (this.interruptionFlags & InterruptionFlagAction.Enemy) == InterruptionFlagAction.Enemy, InterruptionFlagAction.Enemy.GetDescription());
                StringFormat.AppendSubItem(sb, (this.interruptionFlags & InterruptionFlagAction.EscapeArea) == InterruptionFlagAction.EscapeArea, InterruptionFlagAction.EscapeArea.GetDescription());
                StringFormat.AppendSubItem(sb, (this.interruptionFlags & InterruptionFlagAction.Nothing) == InterruptionFlagAction.Nothing, InterruptionFlagAction.Nothing.GetDescription());
            }

            String result = sb.ToString();

            if (this.interruptionFlags == null)
                result = "NULL";
            else if (result == String.Empty)
                result = "\n\t\tNone";

            return result;
        }
        #endregion
    }
}