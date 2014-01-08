using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    /// <summary>A BioWare script's action block container</summary>
    /// <remarks>
    ///     The action block varies very little from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     BG, BG2, IWD, IWD2, PST:
    ///     [Int: Action Identifier] x 1, [Object] x3, [Int] x 5, [String] x 2
    ///     
    ///     Solely in the case of IWD (not IWD2) have I seen variables that are split have a colon
    ///     to delimit the namespace. This *may* indicate that it is an option, but not a requirement
    ///     That is to say:
    ///     IWD2:   "LOCALSML_1", "MYAREABattleSquareState"
    ///     IWD:    "MYAREA:AREA_VAR", "LOCALS:CREATURE_STATE_VAR", "LOCALSSPELL_PHASE"
    ///     BG2:    "GLOBALYagaShuraHeart", "AR5203PlayTextScreen"
    ///     PST:    "GLOBALKill_Vorten", 
    /// </remarks>
    public class ActionBlock
    {
        #region Fields
        /// <summary>Collection of characters that preceed the opening token</summary>
        protected IList<Char> preceedingOpenToken;

        /// <summary>Collection of characters that represent the opening token</summary>
        protected IList<Char> openToken;

        /// <summary>Collection of characters that succeed the opening token</summary>
        protected IList<Char> succeedingOpenToken;

        /// <summary>Collection of characters that represent the closing token</summary>
        protected IList<Char> closeToken;

        /// <summary>Collection of characters that succeed the closing token</summary>
        protected IList<Char> succeedingCloseToken;

        /// <summary>Collection of characters that make up the action ID for the action</summary>
        protected IList<Char> actionId;

        /// <summary>Collection of characters that make up the parameters for the action</summary>
        protected IList<Char> parameters;

        /// <summary>Collection of action's object blocks found in the action</summary>
        protected List<ActionObjectBlock> objects;
        #endregion


        #region IO methods
        /// <summary>This public method reads Response Set script block from the input reader.</summary>
        /// <param name="input">PeekSeekTextReader to read from</param>
        public virtual void Read(PeekSeekTextReader input)
        {
            //Initialize the actions
            this.objects = new List<ActionObjectBlock>();

            try
            {
                //get the opening token
                this.preceedingOpenToken = ScriptReader.ReadUntilToken(input, new ReadableToken(Delimiters.ActionDelimiter, false));
                this.openToken = ScriptReader.ReadToken(input);
                this.succeedingOpenToken = new Char[] { ScriptReader.ReadNewline(input) };

                //consume the characters for the action ID
                this.actionId = ScriptReader.ReadInteger(input);

                //I need to know what the next token is. Is it a close of action or is it a Object open?)
                String foundToken = null;
                do
                {
                    foundToken = ScriptReader.PeekNextOfTokens(input, new ReadableToken[] { new ReadableToken(Delimiters.ObjectDelimiter, false), new ReadableToken(Delimiters.ActionDelimiter, false) });

                    //found a condition-response
                    if (foundToken == Delimiters.ObjectDelimiter)
                    {
                        ActionObjectBlock objectBlock = new ActionObjectBlock();
                        objectBlock.Read(input);
                        this.objects.Add(objectBlock);
                    }
                }
                while (foundToken != null && foundToken != Delimiters.ActionDelimiter);

                if (foundToken == null)
                    throw new InvalidDataException("Expected to find an end of action block, but reader could not find one.");

                //read the parameters
                this.parameters = ScriptReader.ReadParametersUntilToken(input, new ReadableToken(Delimiters.ActionDelimiter, false));

                //get the closing token
                this.closeToken = ScriptReader.ReadToken(input);
                this.succeedingCloseToken = new Char[] { ScriptReader.ReadNewline(input) };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the action block.", ex);
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

            //write the action's ID
            writer.Write(this.actionId.ToArray());
            writer.Flush();

            //write each Condition-response block
            foreach (ActionObjectBlock block in this.objects)
                block.Write(output);

            //write parameters
            writer.Write(this.parameters.ToArray());

            //write the close script block
            writer.Write(this.closeToken.ToArray());
            writer.Write(this.succeedingCloseToken.ToArray());
            writer.Flush();
        }
        #endregion
    }
}