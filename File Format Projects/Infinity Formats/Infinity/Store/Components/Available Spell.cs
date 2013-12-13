using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Store.Components
{
    /// <summary>Structure representing a spell available by a store to be cast</summary>
    public class AvailableSpell : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 12;

        #region Members
        /// <summary>Resource reference to the spell</summary>
        protected ResourceReference spell;

        /// <summary>Cost of the spell being cast for the party</summary>
        protected UInt32 cost;
        #endregion

        #region Properties
        /// <summary>Resource reference to the spell</summary>
        public ResourceReference Spell
        {
            get { return this.spell; }
            set { this.spell = value; }
        }

        /// <summary>Cost of the spell being cast for the party</summary>
        public UInt32 Cost
        {
            get { return this.cost; }
            set { this.cost = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public AvailableSpell()
        {
            this.spell = null;
        }
 
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.spell = new ResourceReference();
        }
        #endregion

        #region Abstract IO methods
        /// <summary>This public method reads file format data structure from the output stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] spellForSale = ReusableIO.BinaryRead(input, StructSize);

            this.spell.ResRef = ReusableIO.ReadStringFromByteArray(spellForSale, 0, CultureConstants.CultureCodeEnglish);
            this.cost = ReusableIO.ReadUInt32FromArray(spellForSale, 8);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.spell.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.cost, output);
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
            String header = "Store Spell available:";

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
            return StringFormat.ReturnAndIndent(String.Format("Store Spell avaiable # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Spell available"));
            builder.Append(this.spell.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Cost"));
            builder.Append(this.cost);

            return builder.ToString();
        }
        #endregion
    }
}
