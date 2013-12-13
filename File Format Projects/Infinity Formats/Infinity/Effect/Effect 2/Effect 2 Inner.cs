using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect2
{
    /// <summary>
    ///     Inner wrapper of the Effect version 2 core.
    ///     Secondary signatures make it necessary for the wrapping given reusable code base, and the first version's lack of stand-alone format with headers.
    /// </summary>
    public class Effect2Inner : InfinityFormat
    {
        #region Members
        /// <summary>Body of the effect</summary>
        protected Effect2Core core; 
        #endregion

        #region Properties
        /// <summary>Body of the effect</summary>
        public Effect2Core Core
        {
            get { return this.core; }
            set { this.core = value; }
        } 
        #endregion

        /// <summary>Default constructor</summary>
        public Effect2Inner()
        {
            this.core = null;
        }

        /// <summary>Instantiates the members</summary>
        public override void Initialize()
        {
            this.core = new Effect2Core();
        }

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            this.core.ReadBody(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            this.core.Write(output);
        }
        #endregion

        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Inner signature"));
            builder.Append(ZString.GetZString(this.signature)); //noticed at least one file that did not duplicate the header, was 0'd out
            builder.Append(StringFormat.ToStringAlignment("Inner version"));
            builder.Append(ZString.GetZString(this.version));   //noticed at least one file that did not duplicate the header, was 0'd out
            builder.Append(this.core.ToString(false));

            return builder.ToString();
        }
        #endregion
    }
}
