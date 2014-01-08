using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's condition-response block container</summary>
    public class ConditionResponseBlock
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

        /// <summary>Collection of script condition-response blocks</summary>
        protected ConditionBlock condition;

        /// <summary>Block containing the possible responses</summary>
        protected ResponseSetBlock responses;
        #endregion


        #region IO methods
        /// <summary>This public method reads Condition-Response script block from the input reader.</summary>
        /// <param name="input">PeekSeekTextReader to read from</param>
        public virtual void Read(PeekSeekTextReader input)
        {
            try
            {
                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ConditionResponseDelimiter, false));
                this.openToken = ScriptReader.ReadToken(input);
                this.succeedingOpenToken = new Char[] { ScriptReader.ReadNewline(input) };

                //read the contained blocks
                this.condition = new ConditionBlock();
                this.condition.Read(input);
                this.responses = new ResponseSetBlock();
                this.responses.Read(input);

                //get the closing token
                this.preceedingCloseToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ConditionResponseDelimiter, false));
                this.closeToken = ScriptReader.ReadToken(input);
                this.succeedingCloseToken =  new Char[] { ScriptReader.ReadNewline(input) };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the condition-response block.", ex);
            }
        }

        /// <summary>This public method writes the Condition-Response block to the output Stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            StreamWriter writer = new StreamWriter(output);  //do not dispose, since I have no desire to close the underlying stream!

            //write the open script block
            writer.Write(this.preceedingOpenToken.ToArray());
            writer.Write(this.openToken.ToArray());
            writer.Write(this.succeedingOpenToken.ToArray());
            writer.Flush();
            
            //write the condition
            this.condition.Write(output);
            this.responses.Write(output);

            //write the close script block
            writer.Write(this.preceedingCloseToken.ToArray());
            writer.Write(this.closeToken.ToArray());
            writer.Write(this.succeedingCloseToken.ToArray());
            writer.Flush();
        }
        #endregion
    }
}