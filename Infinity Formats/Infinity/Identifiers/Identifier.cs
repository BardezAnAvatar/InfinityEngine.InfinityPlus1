using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Identifiers
{
    /// <summary>Represents an IDS (IDentifierS) file of string and numerical pairs.</summary>
    /// <remarks>
    ///     The file format is occasionally XOR obscured. It usually lacks the IDS V1.0 or IDS header.
    ///     The 'second' line usually is a number (often inaccurate) indicating the number of entries to follow.
    ///     Also, usually, lines are a number followed by a space and a text string withour spaces;
    ///         I am assuming that it is a pair with the first element being the numeric value and any subsequent spaces part of the matching string.
    ///     Next, this format is very similar to 2DA; each ile has a separate purpose, with no two defining the same functionality. Few are similar.
    ///     The key difference is that only a handful of IDS files are actually used by the engine, instead representing hard-coded values for a script compiler.
    ///     Finally, the pairs need to be distinct, but there is no real key; mosthave the number as the key, but a lot of scripting IDS' have
    ///         the opposite where two spell names represent the same numerical spell.
    ///     
    ///     Fails on Prefab.ids
    /// </remarks>
    public class Identifier : IInfinityFormat
    {
        #region Fields
        /// <summary>The collection of integer/string pairs</summary>
        public List<IdentifierEntity> Entries { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Entries = new List<IdentifierEntity>();
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            //decrypt or pass through the 2DA file
            MemoryStream binData = InfinityXorEncryption.Decrypt(input);
            using (TextReader reader = new StreamReader(binData))
            {
                String line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("IDS"))
                    {
                        String[] entries;
                        
                        //IWD prefab has this weird condition:
                        if (line.Contains("="))
                            entries = line.Split(new Char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        else
                            entries = line.Split(new Char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);

                        //two items?
                        if (entries.Length > 1)
                            this.Entries.Add(new IdentifierEntity(entries[0], entries[1]));
                    }
                }
            } //should dispose binData
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            using (StreamWriter writer = new StreamWriter(output))
                this.Write(writer);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="writer">Text writer to write to</param>
        public virtual void Write(TextWriter writer)
        {
            //write signature & version
            writer.WriteLine("IDS V1.0");

            //write number of lines
            writer.WriteLine(this.Entries.Count.ToString());

            //write each entry
            foreach (IdentifierEntity pair in this.Entries)
                writer.WriteLine(pair.ToString());
        }
        #endregion


        #region ToString override
        /// <summary>Gets a human-readable String representation of the class</summary>
        /// <returns>A human-readable String representation of the class</returns>
        public override String ToString()
        {
            String ids = null;

            using (StringWriter writer = new StringWriter())
            {
                this.Write(writer);
                ids = writer.ToString();
            }
            return ids;
        }
        #endregion
    }
}