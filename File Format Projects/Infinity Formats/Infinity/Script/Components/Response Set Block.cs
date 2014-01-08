using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's response set block container</summary>
    public class ResponseSetBlock
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

        /// <summary>Collection of responses found in the response set</summary>
        protected List<ResponseBlock> responses;
        #endregion


        #region IO methods
        /// <summary>This public method reads Response Set script block from the input reader.</summary>
        /// <param name="input">PeekSeekTextReader to read from</param>
        public virtual void Read(PeekSeekTextReader input)
        {
            //Initialize the responses
            this.responses = new List<ResponseBlock>();

            try
            {
                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ResponseSetDelimiter, false));
                this.openToken = ScriptReader.ReadToken(input);
                this.succeedingOpenToken = new Char[] { ScriptReader.ReadNewline(input) };

                //I need to know what the next token is. Is it a close of response set or is it a response open?)
                String foundToken = null;
                do
                {
                    foundToken = ScriptReader.PeekNextOfTokens(input, new ReadableToken[] { new ReadableToken(Delimiters.ResponseDelimiter, false), new ReadableToken(Delimiters.ResponseSetDelimiter, true) });

                    //found a condition-response
                    if (foundToken == Delimiters.ResponseDelimiter)
                    {
                        ResponseBlock response = new ResponseBlock();
                        response.Read(input);
                        this.responses.Add(response);
                    }
                }
                while (foundToken != null && foundToken != Delimiters.ResponseSetDelimiter);

                if (foundToken == null)
                    throw new InvalidDataException("Expected to find an end of response set block, but reader could not find one.");

                //get the closing token
                this.preceedingCloseToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ResponseSetDelimiter, true));
                this.closeToken = ScriptReader.ReadToken(input);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the response set block.", ex);
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
            writer.Flush();

            //write each Condition-response block
            foreach (ResponseBlock block in this.responses)
                block.Write(output);

            //write the close script block
            writer.Write(this.preceedingCloseToken.ToArray());
            writer.Write(this.closeToken.ToArray());
            writer.Flush();
        }
        #endregion
    }
}