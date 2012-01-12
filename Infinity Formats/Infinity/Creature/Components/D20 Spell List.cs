using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Components
{
    public class D20SpellList : IInfinityFormat
    {
        #region Members
        /// <summary>List of known spells</summary>
        /// <remarks>This will need to use Insert at if adding known spells.</remarks>
        protected List<CreatureD20KnownSpell> knownSpells;

        /// <summary>Available spell memorization slots</summary>
        protected CreatureD20SpellMemorization memorization;
        #endregion

        #region Properties
        /// <summary>List of known spells</summary>
        public List<CreatureD20KnownSpell> KnownSpells
        {
            get { return this.knownSpells; }
            set { this.knownSpells = value; }
        }

        /// <summary>Available spell memorization slots</summary>
        public CreatureD20SpellMemorization Memorization
        {
            get { return this.memorization; }
            set { this.memorization = value; }
        }
        #endregion
        
        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.knownSpells = new List<CreatureD20KnownSpell>();
            this.memorization = new CreatureD20SpellMemorization();
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from.</param>
        public void ReadBody(Stream input)
        {
            this.ReadBody(input, 0);   //no number specified, so read none.
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="spellCount">Number of known spells to read.</param>
        public void ReadBody(Stream input, Int64 spellCount)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read effect
            this.ReadKnownSpells(input, spellCount);
            this.memorization.Read(input);
        }

        /// <summary>This public method reads known spells from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="spellCount">Number of known spells to read.</param>
        public void ReadKnownSpells(Stream input, Int64 spellCount)
        {
            //read known spells
            for (Int64 i = 0L; i < spellCount; ++i)
            {
                CreatureD20KnownSpell spell = new CreatureD20KnownSpell();
                spell.Read(input);
                this.knownSpells.Add(spell);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public void Write(Stream output)
        {
            this.WriteKnownSpells(output);
            this.memorization.Write(output);
        }

        /// <summary>Writes the list of known spells to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteKnownSpells(Stream output)
        {
            //write known spells
            foreach (CreatureD20KnownSpell spell in this.knownSpells)
                spell.Write(output);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = "Creature D20 Spell List:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndex">Known spells entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Spell level memorization Entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            for (Int32 i = 0; i < this.knownSpells.Count; ++i)
                this.knownSpells[i].ToString(i);
            builder.Append(this.memorization.ToString(false));

            return builder.ToString();
        }
        #endregion
    }
}