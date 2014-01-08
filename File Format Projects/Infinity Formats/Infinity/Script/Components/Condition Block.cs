using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's condition block container</summary>
    public class ConditionBlock
    {
        #region Fields
        /// <summary>Collection of characters that preceed the opening token</summary>
        protected IList<Char> preceedingOpenToken;

        /// <summary>Collection of characters that represent the opening token</summary>
        protected IList<Char> openToken;

        /// <summary>Collection of characters that preceed the closing token</summary>
        protected IList<Char> preceedingCloseToken;

        /// <summary>Collection of characters that represent the closing token</summary>
        protected IList<Char> closeToken;

        /// <summary>Collection of triggers to the condition</summary>
        protected List<TriggerBlock> triggers;
        #endregion


        #region IO methods
        /// <summary>This public method reads Condition script block from the input reader.</summary>
        /// <param name="input">PeekSeekTextReader to read from</param>
        public virtual void Read(PeekSeekTextReader input)
        {
            //Initialize the triggers
            this.triggers = new List<TriggerBlock>();

            try
            {
                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ConditionDelimiter, true));
                this.openToken = ScriptReader.ReadToken(input);

                //I need to know what the next token is. Is it a close of condition or is it a trigger open?)
                String foundToken = null;
                do
                {
                    foundToken = ScriptReader.PeekNextOfTokens(input, new ReadableToken[] { new ReadableToken(Delimiters.TriggerDelimiter, false), new ReadableToken(Delimiters.ConditionDelimiter, true) });

                    //found a condition-response
                    if (foundToken == Delimiters.TriggerDelimiter)
                    {
                        TriggerBlock trigger = new TriggerBlock();
                        trigger.Read(input);
                        this.triggers.Add(trigger);
                    }
                }
                while (foundToken != null && foundToken != Delimiters.ConditionDelimiter);

                if (foundToken == null)
                    throw new InvalidDataException("Expected to find an end of condition block, but reader could not find one.");

                //get the closing token
                this.preceedingCloseToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ConditionDelimiter, true));
                this.closeToken = ScriptReader.ReadToken(input);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the condition block.", ex);
            }
        }

        /// <summary>This public method writes the Condition block to the output Stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            StreamWriter writer = new StreamWriter(output);  //do not dispose, since I have no desire to close the underlying stream!

            //write the open script block
            writer.Write(this.preceedingOpenToken.ToArray());
            writer.Write(this.openToken.ToArray());
            writer.Flush();

            //write each Condition-response block
            foreach (TriggerBlock block in this.triggers)
                block.Write(output);

            //write the close script block
            writer.Write(this.preceedingCloseToken.ToArray());
            writer.Write(this.closeToken.ToArray());
            writer.Flush();
        }
        #endregion
    }
}
