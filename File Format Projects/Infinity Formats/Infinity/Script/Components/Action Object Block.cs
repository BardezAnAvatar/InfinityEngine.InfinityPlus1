using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's object block container inside of a trigger</summary>
    public class ActionObjectBlock : ObjectBlock
    {
        #region Fields
        /// <summary>Collection of characters that succeed the opening token</summary>
        protected IList<Char> succeedingOpenToken;

        /// <summary>Collection of characters that succeed the closing token</summary>
        protected IList<Char> succeedingCloseToken;
        #endregion


        #region IO methods
        /// <summary>This public method reads Object script block from the input reader.</summary>
        /// <param name="input">PeekSeekTextReader to read from</param>
        public virtual void Read(PeekSeekTextReader input)
        {
            try
            {
                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ObjectDelimiter, false));
                this.openToken = ScriptReader.ReadToken(input);

                //read the parameters
                this.parameters = ScriptReader.ReadParametersUntilToken(input, new ReadableToken(Delimiters.ObjectDelimiter, true));

                //get the closing token
                this.preceedingCloseToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ObjectDelimiter, false));
                this.closeToken = ScriptReader.ReadToken(input);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the object block.", ex);
            }
        }

        /// <summary>This public method writes the Object block to the output Stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            StreamWriter writer = new StreamWriter(output);  //do not dispose, since I have no desire to close the underlying stream!

            //write the open script block
            writer.Write(this.preceedingOpenToken.ToArray());
            writer.Write(this.openToken.ToArray());

            //write parameters
            writer.Write(this.parameters.ToArray());

            //write the close script block
            writer.Write(this.preceedingCloseToken.ToArray());
            writer.Write(this.closeToken.ToArray());
            writer.Flush();
        }
        #endregion
    }
}