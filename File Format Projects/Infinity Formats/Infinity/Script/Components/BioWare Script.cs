using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>Represents a type of script that an infinity game runs off of</summary>
    public class BiowareScript : IInfinityFormat
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

        /// <summary>Collection of characters that succeed the closing token</summary>
        protected IList<Char> succeedingCloseToken;

        /// <summary>Collection of script condition-response blocks</summary>
        protected List<ConditionResponseBlock> conditionResponseBlocks;
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.conditionResponseBlocks = new List<ConditionResponseBlock>();
        }
        #endregion


        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the input stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the input stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            try
            {
                StreamReader textReader = new StreamReader(input);  //do not dispose, since I have no desire to close the underlying stream!
                PeekSeekTextReader peekReader = new PeekSeekTextReader(textReader);
                textReader = null;

                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(peekReader, new ReadableToken(Delimiters.ScriptDelimiter, false));
                this.openToken = ScriptReader.ReadToken(peekReader);

                //I need to know what the next token is. Is it a close of script or is it a ConditionResponse open?)
                String foundToken = null;
                do
                {
                    foundToken = ScriptReader.PeekNextOfTokens(peekReader, new ReadableToken[] { new ReadableToken(Delimiters.ScriptDelimiter, true), new ReadableToken(Delimiters.ConditionResponseDelimiter, false) });

                    //found a condition-response
                    if (foundToken == Delimiters.ConditionResponseDelimiter)
                    {
                        ConditionResponseBlock crBlock = new ConditionResponseBlock();
                        crBlock.Read(peekReader);
                        this.conditionResponseBlocks.Add(crBlock);
                    }
                }
                while (foundToken != null && foundToken != Delimiters.ScriptDelimiter);

                if (foundToken == null)
                    throw new InvalidDataException("Expected to find an end of script block, but reader could not find one.");

                //get the closing token
                this.preceedingCloseToken = ScriptReader.ReadUntilToken(peekReader, new ReadableToken(Delimiters.ScriptDelimiter, true));
                this.closeToken = ScriptReader.ReadToken(peekReader);
                this.succeedingCloseToken = ScriptReader.ReadToEnd(peekReader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the script.", ex);
            }
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            StreamWriter writer = new StreamWriter(output);  //do not dispose, since I have no desire to close the underlying stream!

            //write the open script block
            writer.Write(this.preceedingOpenToken.ToArray());
            writer.Write(this.openToken.ToArray());
            writer.Flush();
            
            //write each Condition-response block
            foreach (ConditionResponseBlock block in this.conditionResponseBlocks)
                block.Write(output);

            //write the close script block
            writer.Write(this.preceedingCloseToken.ToArray());
            writer.Write(this.closeToken.ToArray());
            writer.Write(this.succeedingCloseToken.ToArray());
            writer.Flush();
        }
        #endregion


        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            String output = null;

            MemoryStream memory = new MemoryStream();
            this.Write(memory);
            ReusableIO.SeekIfAble(memory, 0);
            
            using (TextReader reader = new StreamReader(memory))
                output = reader.ReadToEnd();

            return output;
        }
        #endregion
    }
}