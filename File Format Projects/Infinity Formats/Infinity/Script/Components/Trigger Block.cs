using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's trigger block container</summary>
    /// <remarks>
    ///     The trigger block varies from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     BG, BG2, IWD, IWD2:
    ///     [Int] x 5, [String] x 2, OBject (sic)
    ///     PST:
    ///     [Int] x 5, [Point], [String] x 2, OBject (sic)
    /// </remarks>
    public class TriggerBlock
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

        /// <summary>Collection of characters that make up the parameters for the trigger</summary>
        protected IList<Char> parameters;

        /// <summary>Related object block</summary>
        protected TriggerObjectBlock objBlock;
        #endregion


        #region IO methods
        /// <summary>This public method reads Trigger script block from the input reader.</summary>
        /// <param name="input">PeekSeekTextReader to read from</param>
        public virtual void Read(PeekSeekTextReader input)
        {
            try
            {
                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.TriggerDelimiter, true));
                this.openToken = ScriptReader.ReadToken(input);
                this.succeedingOpenToken = new Char[] { ScriptReader.ReadNewline(input) };
                
                //read the parameters
                this.parameters = ScriptReader.ReadParametersUntilToken(input, new ReadableToken(Delimiters.ObjectDelimiter, true));

                //read the targeting object block
                this.objBlock = new TriggerObjectBlock();
                this.objBlock.Read(input);

                //get the closing token
                this.preceedingCloseToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.TriggerDelimiter, false));
                this.closeToken = ScriptReader.ReadToken(input);
                this.succeedingCloseToken = new Char[] { ScriptReader.ReadNewline(input) };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the trigger block.", ex);
            }
        }

        /// <summary>This public method writes the Trigger block to the output Stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            StreamWriter writer = new StreamWriter(output);  //do not dispose, since I have no desire to close the underlying stream!

            //write the open script block
            writer.Write(this.preceedingOpenToken.ToArray());
            writer.Write(this.openToken.ToArray());
            writer.Write(this.succeedingOpenToken.ToArray());

            //write parameters
            writer.Write(this.parameters.ToArray());
            writer.Flush();

            //write the Object block block
            this.objBlock.Write(output);

            //write the close script block
            writer.Write(this.preceedingCloseToken.ToArray());
            writer.Write(this.closeToken.ToArray());
            writer.Write(this.succeedingCloseToken.ToArray());
            writer.Flush();
        }
        #endregion
    }
}