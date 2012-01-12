using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Components
{
    /// <summary>Structure representing a item available by a store for purchase</summary>
    public class AvailableItem : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public static Int32 StructSize
        {
            get { return 28; }
        }

        #region Members
        /// <summary>Item instance template for the item being sold</summary>
        protected ItemInstance itemTemplate;

        /// <summary>Amount of this item in stock</summary>
        protected UInt32 amountInStock;

        /// <summary>Interpreted as Boolean, 0 or 2</summary>
        protected UInt32 infiniteSupply;
        #endregion

        #region Properties
        /// <summary>Item instance template for the item being sold</summary>
        public ItemInstance ItemTemplate
        {
            get { return this.itemTemplate; }
            set { this.itemTemplate = value; }
        }

        /// <summary>Amount of this item in stock</summary>
        public UInt32 AmountInStock
        {
            get { return this.amountInStock; }
            set { this.amountInStock = value; }
        }

        /// <summary>Interpreted as Boolean, 0 or 2</summary>
        public UInt32 InfiniteSupplyValue
        {
            get { return this.infiniteSupply; }
            set { this.infiniteSupply = value; }
        }

        /// <summary>Is the supply infinite?</summary>
        public Boolean InfiniteSupply
        {
            get { return Convert.ToBoolean(this.infiniteSupply); }
            set { this.infiniteSupply = Convert.ToUInt32(value); }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public AvailableItem()
        {
            this.itemTemplate = null;
        }
 
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.itemTemplate = new ItemInstance();
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
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.itemTemplate.Read(input);
            Byte[] itemForSale = ReusableIO.BinaryRead(input, 8);

            this.amountInStock = ReusableIO.ReadUInt32FromArray(itemForSale, 0);
            this.infiniteSupply = ReusableIO.ReadUInt32FromArray(itemForSale, 4);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.itemTemplate.Write(output);
            ReusableIO.WriteUInt32ToStream(this.amountInStock, output);
            ReusableIO.WriteUInt32ToStream(this.infiniteSupply, output);
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
            String header = "Store item available:";

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
            return StringFormat.ReturnAndIndent(String.Format("Store item avaiable # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.itemTemplate.ToString(false));
            builder.Append(StringFormat.ToStringAlignment("Amount in stock"));
            builder.Append(this.amountInStock);
            builder.Append(StringFormat.ToStringAlignment("Infinite supply"));
            builder.Append(this.infiniteSupply);
            builder.Append(StringFormat.ToStringAlignment("Infinite supply (described)"));
            builder.Append(this.InfiniteSupply);

            return builder.ToString();
        }
        #endregion
    }
}