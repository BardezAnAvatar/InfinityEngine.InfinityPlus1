using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Dialog.Component;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Dialog.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Dialog.Version
{
    public class Dialog1 : IInfinityFormat
    {
        #region Members
        /// <summary>Dialog file header</summary>
        protected DialogHeader header;

        /// <summary>List of dialog states</summary>
        protected List<DialogState> states;

        /// <summary>List of dialog transitions</summary>
        protected List<DialogTransition> transitions;

        /// <summary>List of dialog state triggers</summary>
        protected List<DialogScript> stateTriggers;

        /// <summary>List of dialog transition triggers</summary>
        protected List<DialogScript> transitionTriggers;

        /// <summary>List of dialog actions</summary>
        protected List<DialogScript> actions;
        #endregion

        #region Properties
        /// <summary>Dialog file header</summary>
        public DialogHeader Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        /// <summary>List of dialog states</summary>
        public List<DialogState> States
        {
            get { return this.states; }
            set { this.states = value; }
        }

        /// <summary>List of dialog transitions</summary>
        public List<DialogTransition> Transitions
        {
            get { return this.transitions; }
            set { this.transitions = value; }
        }

        /// <summary>List of dialog state triggers</summary>
        public List<DialogScript> StateTriggers
        {
            get { return this.stateTriggers; }
            set { this.stateTriggers = value; }
        }

        /// <summary>List of dialog transition triggers</summary>
        public List<DialogScript> TransitionTriggers
        {
            get { return this.transitionTriggers; }
            set { this.transitionTriggers = value; }
        }

        /// <summary>List of dialog actions</summary>
        public List<DialogScript> Actions
        {
            get { return this.actions; }
            set { this.actions = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Dialog1()
        {
            this.header = null;
            this.states = null;
            this.transitions = null;
            this.stateTriggers = null;
            this.transitionTriggers = null;
            this.actions = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.header = new DialogHeader();
            this.states = new List<DialogState>();
            this.transitions = new List<DialogTransition>();
            this.stateTriggers = new List<DialogScript>();
            this.transitionTriggers = new List<DialogScript>();
            this.actions = new List<DialogScript>();
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.header = new DialogHeader();
                this.header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            this.header.Read(input);
            this.ReadStates(input);
            this.ReadTransitions(input);
            this.ReadStateTriggers(input);
            this.ReadTransitionTriggers(input);
            this.ReadActions(input);

            this.ReadStrings(input);
        }

        /// <summary>Reads the list of dialog states from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadStates(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetStates);

            //read items
            for (Int32 i = 0; i < this.header.CountStates; ++i)
            {
                DialogState state = new DialogState();
                state.Read(input);
                this.states.Add(state);
            }
        }

        /// <summary>Reads the list of dialog transitions from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadTransitions(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetTransitions, SeekOrigin.Begin);

            //read drinks
            for (Int32 i = 0; i < this.header.CountTransitions; ++i)
            {
                DialogTransition transition = new DialogTransition();
                transition.Read(input);
                this.transitions.Add(transition);
            }
        }

        /// <summary>Reads the list of state triggers from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadStateTriggers(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetTriggersState, SeekOrigin.Begin);

            //read spells
            for (Int32 i = 0; i < this.header.CountTriggersState; ++i)
            {
                DialogScript trigger = new DialogScript();
                trigger.Read(input);
                this.stateTriggers.Add(trigger);
            }
        }

        /// <summary>Reads the list of transition triggers from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadTransitionTriggers(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetTriggersTransition, SeekOrigin.Begin);

            //read spells
            for (Int32 i = 0; i < this.header.CountTriggersTransition; ++i)
            {
                DialogScript trigger = new DialogScript();
                trigger.Read(input);
                this.transitionTriggers.Add(trigger);
            }
        }

        /// <summary>Reads the list of actions from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadActions(Stream input)
        {
            ReusableIO.SeekIfAble(input, this.header.OffsetActions, SeekOrigin.Begin);

            //read spells
            for (Int32 i = 0; i < this.header.CountActions; ++i)
            {
                DialogScript action = new DialogScript();
                action.Read(input);
                this.actions.Add(action);
            }
        }

        /// <summary>Reads all the scripting strings from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected void ReadStrings(Stream input)
        {
            //state triggers
            foreach (DialogScript trigger in this.stateTriggers)
                trigger.ReadScript(input);

            //transition triggers
            foreach (DialogScript trigger in this.transitionTriggers)
                trigger.ReadScript(input);

            //actions
            foreach (DialogScript action in this.actions)
                action.ReadScript(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();

            this.header.Write(output);
            this.WriteStates(output);
            this.WriteTransitions(output);
            this.WriteStateTriggers(output);
            this.WriteTransitionTriggers(output);
            this.WriteActions(output);

            this.WriteStrings(output);
        }

        /// <summary>Writes the list of dialog states to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteStates(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetStates, SeekOrigin.Begin);

            //write states
            foreach (DialogState state in this.states)
                state.Write(output);
        }

        /// <summary>Writes the list of dialog transitions to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTransitions(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetTransitions, SeekOrigin.Begin);

            //write transitions
            foreach (DialogTransition transition in this.transitions)
                transition.Write(output);
        }

        /// <summary>Writes the list of state triggers to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteStateTriggers(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetTriggersState, SeekOrigin.Begin);

            //write triggers
            foreach (DialogScript trigger in this.stateTriggers)
                trigger.Write(output);
        }

        /// <summary>Writes the list of transition triggers to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTransitionTriggers(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetTriggersTransition, SeekOrigin.Begin);

            //write triggers
            foreach (DialogScript trigger in this.transitionTriggers)
                trigger.Write(output);
        }

        /// <summary>Writes the list of actions to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteActions(Stream output)
        {
            ReusableIO.SeekIfAble(output, this.header.OffsetActions, SeekOrigin.Begin);

            //write actions
            foreach (DialogScript trigger in this.actions)
                trigger.Write(output);
        }

        /// <summary>Writes all the scripting strings to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteStrings(Stream output)
        {
            //state triggers
            foreach (DialogScript trigger in this.stateTriggers)
                trigger.WriteScript(output);

            //transition triggers
            foreach (DialogScript trigger in this.transitionTriggers)
                trigger.WriteScript(output);

            //actions
            foreach (DialogScript action in this.actions)
                action.WriteScript(output);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Dialog version 1.0");
            builder.AppendLine(this.header.ToString());
            builder.AppendLine(this.GenerateStatesString());
            builder.AppendLine(this.GenerateTransitionsString());
            builder.AppendLine(this.GenerateStateTriggersString());
            builder.AppendLine(this.GenerateTransitionTriggersString());
            builder.Append(this.GenerateActionsString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable console output describing the dialog states</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateStatesString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.states.Count; ++i)
                sb.Append(this.states[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the dialog transitions</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateTransitionsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.transitions.Count; ++i)
                sb.Append(this.transitions[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the dialog state triggers</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateStateTriggersString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.stateTriggers.Count; ++i)
                sb.Append(this.stateTriggers[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the dialog transition triggers</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateTransitionTriggersString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.transitionTriggers.Count; ++i)
                sb.Append(this.transitionTriggers[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the dialog actions</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateActionsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.actions.Count; ++i)
                sb.Append(this.actions[i].ToString(i + 1));

            return sb.ToString();
        }
        #endregion


        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        /// <remarks>Dialog files have so very many data pointers to balance. Copying from the IWD2 creature model.</remarks>
        protected void MaintainMinimalDataIntegrity()
        {
            //iterate all strings andmake sure data reflects their length in bytes
            this.UpdateStringLengths();


            if (this.Overlaps())
            {
                Int32 statesSize = this.states.Count * DialogState.StructSize;
                Int32 transitionsSize = this.transitions.Count * DialogTransition.StructSize;
                Int32 stateTriggersSize = this.stateTriggers.Count * DialogScript.StructSize;
                Int32 transitionTriggersSize = this.stateTriggers.Count * DialogScript.StructSize;
                Int32 actionsSize = this.stateTriggers.Count * DialogScript.StructSize;


                //set first
                this.header.OffsetStates = Convert.ToUInt32(this.header.StrutureSize);
                this.header.OffsetTransitions = this.header.OffsetStates + Convert.ToUInt32(statesSize);
                this.header.OffsetTriggersState = this.header.OffsetTransitions + Convert.ToUInt32(transitionsSize);
                this.header.OffsetTriggersTransition = this.header.OffsetTriggersState + Convert.ToUInt32(stateTriggersSize);
                this.header.OffsetActions = this.header.OffsetTriggersTransition + Convert.ToUInt32(transitionTriggersSize);

                UInt32 offset = this.header.OffsetActions + Convert.ToUInt32(actionsSize);
                for (Int32 i = 0; i < this.stateTriggers.Count; ++i)
                {
                    this.stateTriggers[i].OffsetScript = offset;
                    offset += this.stateTriggers[i].LengthScript;
                }

                for (Int32 i = 0; i < this.transitionTriggers.Count; ++i)
                {
                    this.transitionTriggers[i].OffsetScript = offset;
                    offset += this.transitionTriggers[i].LengthScript;
                }

                for (Int32 i = 0; i < this.actions.Count; ++i)
                {
                    this.actions[i].OffsetScript = offset;
                    offset += this.actions[i].LengthScript;
                }
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        protected Boolean Overlaps()
        {
            return this.OverlapBaseAnyBase()    //base on base
                || this.OverlapBaseAnyScript()   //base on spell
                || this.OverlapScriptAny();      //spell on both
        }

        /// <summary>Determines if any offsets overlap with other offsets</summary>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapBaseAnyBase()
        {
            Int32 statesSize = this.states.Count * DialogState.StructSize;
            Int32 transitionsSize = this.transitions.Count * DialogTransition.StructSize;
            Int32 stateTriggersSize = this.stateTriggers.Count * DialogScript.StructSize;
            Int32 transitionTriggersSize = this.transitionTriggers.Count * DialogScript.StructSize;
            Int32 actionsSize = this.actions.Count * DialogScript.StructSize;

            Boolean overlaps =
                (
                    false
                  //|| IntExtension.Between(this.header.OffsetStates, statesSize, this.header.OffsetStates, this.header.OffsetStates + statesSize)
                    || IntExtension.Between(this.header.OffsetStates, statesSize, this.header.OffsetTransitions, this.header.OffsetTransitions +  transitionsSize)
                    || IntExtension.Between(this.header.OffsetStates, statesSize, this.header.OffsetTriggersState, this.header.OffsetTriggersState + stateTriggersSize)
                    || IntExtension.Between(this.header.OffsetStates, statesSize, this.header.OffsetTriggersTransition, this.header.OffsetTriggersTransition + transitionTriggersSize)
                    || IntExtension.Between(this.header.OffsetStates, statesSize, this.header.OffsetActions, this.header.OffsetActions + actionsSize)

                    || IntExtension.Between(this.header.OffsetTransitions, transitionsSize, this.header.OffsetStates, this.header.OffsetStates + statesSize)
                  //|| IntExtension.Between(this.header.OffsetTransitions, transitionsSize, this.header.OffsetTransitions, this.header.OffsetTransitions + transitionsSize)
                    || IntExtension.Between(this.header.OffsetTransitions, transitionsSize, this.header.OffsetTriggersState, this.header.OffsetTriggersState + stateTriggersSize)
                    || IntExtension.Between(this.header.OffsetTransitions, transitionsSize, this.header.OffsetTriggersTransition, this.header.OffsetTriggersTransition + transitionTriggersSize)
                    || IntExtension.Between(this.header.OffsetTransitions, transitionsSize, this.header.OffsetActions, this.header.OffsetActions + actionsSize)

                    || IntExtension.Between(this.header.OffsetTriggersState, stateTriggersSize, this.header.OffsetStates, this.header.OffsetStates + statesSize)
                    || IntExtension.Between(this.header.OffsetTriggersState, stateTriggersSize, this.header.OffsetTransitions, this.header.OffsetTransitions + transitionsSize)
                  //|| IntExtension.Between(this.header.OffsetTriggersState, stateTriggersSize, this.header.OffsetTriggersState, this.header.OffsetTriggersState + stateTriggersSize)
                    || IntExtension.Between(this.header.OffsetTriggersState, stateTriggersSize, this.header.OffsetTriggersTransition, this.header.OffsetTriggersTransition + transitionTriggersSize)
                    || IntExtension.Between(this.header.OffsetTriggersState, stateTriggersSize, this.header.OffsetActions, this.header.OffsetActions + actionsSize)

                    || IntExtension.Between(this.header.OffsetTriggersTransition, transitionTriggersSize, this.header.OffsetStates, this.header.OffsetStates + statesSize)
                    || IntExtension.Between(this.header.OffsetTriggersTransition, transitionTriggersSize, this.header.OffsetTransitions, this.header.OffsetTransitions + transitionsSize)
                    || IntExtension.Between(this.header.OffsetTriggersTransition, transitionTriggersSize, this.header.OffsetTriggersState, this.header.OffsetTriggersState + stateTriggersSize)
                  //|| IntExtension.Between(this.header.OffsetTriggersTransition, transitionTriggersSize, this.header.OffsetTriggersTransition, this.header.OffsetTriggersTransition + transitionTriggersSize)
                    || IntExtension.Between(this.header.OffsetTriggersTransition, transitionTriggersSize, this.header.OffsetActions, this.header.OffsetActions + actionsSize)

                    || IntExtension.Between(this.header.OffsetActions, actionsSize, this.header.OffsetStates, this.header.OffsetStates + statesSize)
                    || IntExtension.Between(this.header.OffsetActions, actionsSize, this.header.OffsetTransitions, this.header.OffsetTransitions + transitionsSize)
                    || IntExtension.Between(this.header.OffsetActions, actionsSize, this.header.OffsetTriggersState, this.header.OffsetTriggersState + stateTriggersSize)
                    || IntExtension.Between(this.header.OffsetActions, actionsSize, this.header.OffsetTriggersTransition, this.header.OffsetTriggersTransition + transitionTriggersSize)
                  //|| IntExtension.Between(this.header.OffsetActions, actionsSize, this.header.OffsetActions, this.header.OffsetActions + actionsSize)
                );

            return overlaps;
        }

        /// <summary>Determines if any offsets overlap with the header, or main secton offsets</summary>
        /// <param name="offset">Offset in question</param>
        /// <param name="size">data size at offset in question</param>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapsScriptAnyBaseOffsets(UInt32 offset, UInt32 size)
        {
            Int32 statesSize = this.states.Count * DialogState.StructSize;
            Int32 transitionsSize = this.transitions.Count * DialogTransition.StructSize;
            Int32 stateTriggersSize = this.stateTriggers.Count * DialogScript.StructSize;
            Int32 transitionTriggersSize = this.transitionTriggers.Count * DialogScript.StructSize;
            Int32 actionsSize = this.actions.Count * DialogScript.StructSize;

            //technically, any of these can follow the header in any order. Check for any overlaps with the header.
            Boolean overlaps =
                     IntExtension.Between(offset, size, 0, this.header.StrutureSize)
                  || IntExtension.Between(offset, size, this.header.OffsetStates, this.header.OffsetStates + statesSize)
                  || IntExtension.Between(offset, size, this.header.OffsetTransitions, this.header.OffsetTransitions + transitionsSize)
                  || IntExtension.Between(offset, size, this.header.OffsetTriggersState, this.header.OffsetTriggersState + stateTriggersSize)
                  || IntExtension.Between(offset, size, this.header.OffsetTriggersTransition, this.header.OffsetTriggersTransition + transitionTriggersSize)
                  || IntExtension.Between(offset, size, this.header.OffsetActions, this.header.OffsetActions + actionsSize);

            return overlaps;
        }

        /// <summary>Determines if any offsets overlap with other offsets</summary>
        /// <param name="offset">Offset in question</param>
        /// <param name="size">data size at offset in question</param>
        /// <param name="key">key of offset in dictionary, used to exclude a match</param>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapsScriptOffsets(UInt32 offset, Int64 size)
        {
            Boolean overlaps = false;

            //state triggers
            foreach (DialogScript trigger in this.stateTriggers)
            {
                if (trigger.OffsetScript != offset && trigger.LengthScript != size) //do not check yourself, as you will overlap yourself and wreck this whole thing. Yes, I went there.
                    overlaps |= IntExtension.Between(offset, size, trigger.OffsetScript, (trigger.OffsetScript + trigger.LengthScript));

                if (overlaps)   //no point continuing through it all
                    break;
            }

            //transition triggers
            if (!overlaps)
            {
                foreach (DialogScript trigger in this.transitionTriggers)
                {
                    if (trigger.OffsetScript != offset && trigger.LengthScript != size) //do not check yourself, as you will overlap yourself and wreck this whole thing. Yes, I went there.
                        overlaps |= IntExtension.Between(offset, size, trigger.OffsetScript, (trigger.OffsetScript + trigger.LengthScript));

                    if (overlaps)   //no point continuing through it all
                        break;
                }
            }

            //actions
            if (!overlaps)
            {
                foreach (DialogScript action in this.actions)
                {
                    if (action.OffsetScript != offset && action.LengthScript != size) //do not check yourself, as you will overlap yourself and wreck this whole thing. Yes, I went there.
                        overlaps |= IntExtension.Between(offset, size, action.OffsetScript, (action.OffsetScript + action.LengthScript));

                    if (overlaps)   //no point continuing through it all
                        break;
                }
            }

            return overlaps;
        }

        /// <summary>Determines if any offsets overlap for base offsets to spell offsets</summary>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapBaseAnyScript()
        {
            Int32 statesSize = this.states.Count * DialogState.StructSize;
            Int32 transitionsSize = this.transitions.Count * DialogTransition.StructSize;
            Int32 stateTriggersSize = this.stateTriggers.Count * DialogScript.StructSize;
            Int32 transitionTriggersSize = this.transitionTriggers.Count * DialogScript.StructSize;
            Int32 actionsSize = this.actions.Count * DialogScript.StructSize;

            return
                this.OverlapsScriptOffsets(this.header.OffsetStates, statesSize)
                || this.OverlapsScriptOffsets(this.header.OffsetTransitions, transitionsSize)
                || this.OverlapsScriptOffsets(this.header.OffsetTriggersState, stateTriggersSize)
                || this.OverlapsScriptOffsets(this.header.OffsetTriggersTransition, transitionTriggersSize)
                || this.OverlapsScriptOffsets(this.header.OffsetActions, actionsSize);
        }

        /// <summary>Determines if any offsets overlap with other offsets</summary>
        /// <returns>True if there is any overlap</returns>
        protected Boolean OverlapScriptAny()
        {
            Boolean overlaps = false;


            //state triggers
            foreach (DialogScript trigger in this.stateTriggers)
            {
                //base offsets
                overlaps |= this.OverlapsScriptAnyBaseOffsets(trigger.OffsetScript, trigger.LengthScript);

                //spell offsets
                overlaps |= this.OverlapsScriptOffsets(trigger.OffsetScript, trigger.LengthScript);

                if (overlaps)   //no point continuing through it all
                    break;
            }

            //transition triggers
            if (!overlaps)
            {
                foreach (DialogScript trigger in this.transitionTriggers)
                {
                    //base offsets
                    overlaps |= this.OverlapsScriptAnyBaseOffsets(trigger.OffsetScript, trigger.LengthScript);

                    //spell offsets
                    overlaps |= this.OverlapsScriptOffsets(trigger.OffsetScript, trigger.LengthScript);

                    if (overlaps)   //no point continuing through it all
                        break;
                }
            }

            //actions
            if (!overlaps)
            {
                foreach (DialogScript action in this.actions)
                {
                    //base offsets
                    overlaps |= this.OverlapsScriptAnyBaseOffsets(action.OffsetScript, action.LengthScript);

                    //spell offsets
                    overlaps |= this.OverlapsScriptOffsets(action.OffsetScript, action.LengthScript);

                    if (overlaps)   //no point continuing through it all
                        break;
                }
            }

            return overlaps;
        }
        #endregion

        /// <summary>Iterates through all Script block andensures that the lenths are updated</summary>
        protected void UpdateStringLengths()
        {
            //state triggers
            foreach (DialogScript trigger in this.stateTriggers)
                trigger.MaintainMinimalDataIntegrity();

            //transition triggers
            foreach (DialogScript trigger in this.transitionTriggers)
                trigger.MaintainMinimalDataIntegrity();

            //actions
            foreach (DialogScript action in this.actions)
                action.MaintainMinimalDataIntegrity();
        }
    }
}