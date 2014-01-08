using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's response block container</summary>
    public class ResponseBlock
    {
        #region Fields
        /// <summary>Collection of characters that preceed the opening token</summary>
        protected IList<Char> preceedingOpenToken;

        /// <summary>Collection of characters that represent the opening token</summary>
        protected IList<Char> openToken;

        /// <summary>Collection of characters that succeed the opening token</summary>
        protected IList<Char> succeedingOpenToken;

        /// <summary>Collection of characters that preceed the closing token</summary>
        protected IList<Char> preceedingCloseToken;

        /// <summary>Collection of characters that represent the closing token</summary>
        protected IList<Char> closeToken;

        /// <summary>Collection of characters that succeed the closing token</summary>
        protected IList<Char> succeedingCloseToken;

        /// <summary>Collection of characters that represent the weight of the response</summary>
        protected IList<Char> weight;

        /// <summary>Collection of actions found in the response</summary>
        protected List<ActionBlock> actions;
        #endregion


        #region IO methods
        /// <summary>This public method reads Response Set script block from the input reader.</summary>
        /// <param name="input">PeekSeekTextReader to read from</param>
        public virtual void Read(PeekSeekTextReader input)
        {
            //Initialize the actions
            this.actions = new List<ActionBlock>();

            try
            {
                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ResponseDelimiter, false));
                this.openToken = ScriptReader.ReadToken(input);
                this.succeedingOpenToken = new Char[] { ScriptReader.ReadNewline(input) };

                //I need to read the weight, which is immediately followed by either action or no action; I need to know the next token first
                IList<ReadableToken> validTokens = new ReadableToken[] { new ReadableToken(Delimiters.ResponseDelimiter, false), new ReadableToken(Delimiters.ActionDelimiter, false) };
                String foundToken = ScriptReader.PeekNextOfTokens(input, validTokens);

                //read the weight, given the next token delimiter found
                this.weight = ScriptReader.ReadParametersUntilToken(input, new ReadableToken(foundToken, false));

                //I need to know what the next token is. Is it a close of response or is it an action open?)
                do
                {
                    foundToken = ScriptReader.PeekNextOfTokens(input, validTokens);

                    //found a condition-response
                    if (foundToken == Delimiters.ActionDelimiter)
                    {
                        ActionBlock action = new ActionBlock();
                        action.Read(input);
                        this.actions.Add(action);
                    }
                }
                while (foundToken != null && foundToken != Delimiters.ResponseDelimiter);

                if (foundToken == null)
                    throw new InvalidDataException("Expected to find an end of response block, but reader could not find one.");

                //get the closing token
                this.preceedingCloseToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ResponseDelimiter, true));
                this.closeToken = ScriptReader.ReadToken(input);
                this.succeedingCloseToken = new Char[] { ScriptReader.ReadNewline(input) };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the response block.", ex);
            }
        }

        /// <summary>This public method writes the Response Set block to the output Stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            StreamWriter writer = new StreamWriter(output);  //do not dispose, since I have no desire to close the underlying stream!

            //write the open script block
            writer.Write(this.preceedingOpenToken.ToArray());
            writer.Write(this.openToken.ToArray());
            writer.Write(this.succeedingOpenToken.ToArray());
            writer.Write(this.weight.ToArray());
            writer.Flush();

            //write each Condition-response block
            foreach (ActionBlock block in this.actions)
                block.Write(output);

            //write the close script block
            writer.Write(this.preceedingCloseToken.ToArray());
            writer.Write(this.closeToken.ToArray());
            writer.Write(this.succeedingCloseToken.ToArray());
            writer.Flush();
        }
        #endregion
    }
}