using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components
{
    class ActionExposure
    {
        #region Fields
        /// <summary>Represents the ID of the trigger, which references the trigger identifiers file (TRIGGER.IDS)</summary>
        protected Int32 triggerId;

        /// <summary>First integer parameter to various trigger conditions</summary>
        protected Int64 integer1;

        /// <summary>
        ///     Flags parameter which appear at first view to only be negation; if bit 0 (value 1, 3, 5, etc.) is set,
        ///     the code becomes !trigger() rather than trigger()
        /// </summary>
        protected UInt32 flags;

        /// <summary>Second integer parameter to various trigger conditions</summary>
        protected Int64 integer2;

        /// <summary>Third integer parameter to various trigger conditions</summary>
        protected Int64 integer3;

        /// <summary>First string parameter</summary>
        protected String string1;

        /// <summary>Second string parameter</summary>
        protected String string2;

        /// <summary>Related object block</summary>
        protected ObjectBlock objBlock;
        #endregion


        //#region Construction
        ///// <summary>Instantiates reference types</summary>
        //public abstract void Initialize();
        //#endregion


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
            //this.Initialize();

            try
            {
                StreamReader reader = new StreamReader(input);  //do not dispose, since I have no desire to close the underlying stream!

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encounted an error while reading the trigger block.", ex);
            }
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
        }
        #endregion


        #region Read helpers
        /// <summary>Reads a trigger delimiter from the stream reader</summary>
        /// <param name="reader">Stream reader to read from</param>
        protected void ReadTriggerDelimiter(StreamReader reader)
        {
            //read the delimiter, two characters
            Char[] buffer = new Char[2];
            Int32 charactersRead = reader.Read(buffer, 0, 2);

            if (charactersRead < 2)
                throw new InvalidDataException(String.Format("Did not read enough characters from the input reader. Found {0} characters, expected 2.", charactersRead));
            else if (Delimiters.TriggerDelimiter != new String(buffer))
                throw new InvalidDataException(String.Format("Encountered invalid data when attempting to read a trigger block. Looking for {\"0\"}, found \"{1}\".", Delimiters.TriggerDelimiter, buffer));


            //read the newline
            charactersRead = reader.Read(buffer, 0, 1);
            if (charactersRead < 1)
                throw new InvalidDataException(String.Format("Did not read enough characters from the input reader. Found {0} characters, expected 2.", charactersRead));
            else if (false)
                throw new InvalidDataException(String.Format("Encountered invalid data when attempting to read a trigger block. Looking for {\"0\"}, found \"{1}\".", Delimiters.TriggerDelimiter, buffer));
        }
        #endregion
    }
}
