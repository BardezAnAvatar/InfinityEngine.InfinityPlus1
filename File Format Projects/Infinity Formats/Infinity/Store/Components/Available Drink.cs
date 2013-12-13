using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Store.Components
{
    /// <summary>Structure representing a drink available by a store to be consumed</summary>
    public class AvailableDrink : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 20;

        #region Members
        /// <summary>Resource reference to the spell</summary>
        protected ResourceReference specialRumor;

        /// <summary>String reference to the drink name</summary>
        protected StringReference drinkName;

        /// <summary>Cost of the spell being cast for the party</summary>
        protected UInt32 cost;

        /// <summary>Strength of the beverage</summary>
        /// <remarks>Are there any negatives? It would be cool to use, say, coffee as an in-joke.</remarks>
        protected Int32 alcoholicStrength;
        #endregion

        #region Properties
        /// <summary>Resource reference to the special rumor resource</summary>
        public ResourceReference SpecialRumor
        {
            get { return this.specialRumor; }
            set { this.specialRumor = value; }
        }

        /// <summary>String reference to the drink name</summary>
        public StringReference DrinkName
        {
            get { return this.drinkName; }
            set { this.drinkName = value; }
        }

        /// <summary>Cost of the spell being cast for the party</summary>
        public UInt32 Cost
        {
            get { return this.cost; }
            set { this.cost = value; }
        }

        /// <summary>Strength of the beverage</summary>
        /// <remarks>Are there any negatives? It would be cool to use, say, coffee as an in-joke.</remarks>
        public Int32 AlcoholicStrength
        {
            get { return this.alcoholicStrength; }
            set { this.alcoholicStrength = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public AvailableDrink()
        {
            this.specialRumor = null;
            this.drinkName = null;
        }
 
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.specialRumor = new ResourceReference();
            this.drinkName = new StringReference();
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

            Byte[] drinkForSale = ReusableIO.BinaryRead(input, StructSize);

            this.specialRumor.ResRef = ReusableIO.ReadStringFromByteArray(drinkForSale, 0, CultureConstants.CultureCodeEnglish);
            this.drinkName.StringReferenceIndex = ReusableIO.ReadInt32FromArray(drinkForSale, 8);
            this.cost = ReusableIO.ReadUInt32FromArray(drinkForSale, 12);
            this.alcoholicStrength = ReusableIO.ReadInt32FromArray(drinkForSale, 16);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.specialRumor.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.drinkName.StringReferenceIndex, output);
            ReusableIO.WriteUInt32ToStream(this.cost, output);
            ReusableIO.WriteInt32ToStream(this.alcoholicStrength, output);
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
            String header = "Store drink available:";

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
            return StringFormat.ReturnAndIndent(String.Format("Store drink avaiable # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Overriding Rumor Dialog"));
            builder.Append(this.specialRumor.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Drink on tap"));
            builder.Append(this.drinkName.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Cost"));
            builder.Append(this.cost);
            builder.Append(StringFormat.ToStringAlignment("Drink strength"));
            builder.Append(this.alcoholicStrength);

            return builder.ToString();
        }
        #endregion
    }
}