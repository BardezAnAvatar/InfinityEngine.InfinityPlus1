using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl
{
    public class ItemInstance : IInfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 20;

        #region Members
        /// <summary>Item resource referenced</summary>
        protected ResourceReference item;

        /// <summary>The total lifetime of the item</summary>
        /// <value>Per IESDP: Item expiration time - item creation hour (replace with drained item)</value>
        protected Byte itemLifeTime;

        /// <summary>Countdown of expiring item</summary>
        /// <value>
        ///     Per IESDP:
        ///     	Item expiration time - (elapsed hour count divided by 256, rounded down) + 1
        ///     	(replace with drained item)
        ///         When the game hour and elapsed hour count for the current game time exceed
        ///         these values, the item is removed. 
        /// </value>
        protected Byte itemCountDown;

        /// <summary>First set of quantity or charges</summary>
        protected UInt16 quantityCharges1;

        /// <summary>Second set of quantity or charges</summary>
        protected UInt16 quantityCharges2;

        /// <summary>Third set of quantity or charges</summary>
        protected UInt16 quantityCharges3;

        /// <summary>Instance flags of item (is this particular one stolen, etc.)</summary>
        protected ItemInstanceFlags flags;
        #endregion

        #region Properties
        /// <summary>Item resource referenced</summary>
        protected ResourceReference Item
        {
            get { return this.item; }
            set { this.item = value; }
        }
        /// <summary>The total lifetime of the item</summary>
        /// <value>Per IESDP: Item expiration time - item creation hour (replace with drained item)</value>
        public Byte ItemLifeTime
        {
            get { return this.itemLifeTime; }
            set { this.itemLifeTime = value; }
        }

        /// <summary>Countdown of expiring item</summary>
        /// <value>
        ///     Per IESDP:
        ///     	Item expiration time - (elapsed hour count divided by 256, rounded down) + 1
        ///     	(replace with drained item)
        ///         When the game hour and elapsed hour count for the current game time exceed
        ///         these values, the item is removed. 
        /// </value>
        public Byte ItemCountDown
        {
            get { return this.itemCountDown; }
            set { this.itemCountDown = value; }
        }

        /// <summary>First set of quantity or charges</summary>
        protected UInt16 QuantityCharges1
        {
            get { return this.quantityCharges1; }
            set { this.quantityCharges1 = value; }
        }

        /// <summary>Second set of quantity or charges</summary>
        protected UInt16 QuantityCharges2
        {
            get { return this.quantityCharges2; }
            set { this.quantityCharges2 = value; }
        }

        /// <summary>Third set of quantity or charges</summary>
        protected UInt16 QuantityCharges3
        {
            get { return this.quantityCharges3; }
            set { this.quantityCharges3 = value; }
        }

        /// <summary>Instance flags of item (is this particular one stolen, etc.)</summary>
        protected ItemInstanceFlags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>Default constructor</summary>
        public ItemInstance()
        {
            this.item = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.item = new ResourceReference();
        } 
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        /// <param name="input">Input stream to read from</param>
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

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read effect
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 20);

            this.item.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 0, Constants.CultureCodeEnglish);
            this.itemLifeTime = remainingBody[8];
            this.itemCountDown = remainingBody[9];
            this.quantityCharges1 = ReusableIO.ReadUInt16FromArray(remainingBody, 10);
            this.quantityCharges2 = ReusableIO.ReadUInt16FromArray(remainingBody, 12);
            this.quantityCharges3 = ReusableIO.ReadUInt16FromArray(remainingBody, 14);
            this.flags = (ItemInstanceFlags)ReusableIO.ReadUInt32FromArray(remainingBody, 16);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.item.ResRef, output, Constants.CultureCodeEnglish);
            output.WriteByte(this.itemLifeTime);
            output.WriteByte(this.itemCountDown);
            ReusableIO.WriteUInt16ToStream(this.quantityCharges1, output);
            ReusableIO.WriteUInt16ToStream(this.quantityCharges2, output);
            ReusableIO.WriteUInt16ToStream(this.quantityCharges3, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.flags, output);
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
            String header = "Creature 2E item:";

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
            return StringFormat.ReturnAndIndent(String.Format("Item Entry # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Referenced item resource"));
            builder.Append(String.Format("'{0}'", this.item.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Item lifetime"));
            builder.Append(this.itemLifeTime);
            builder.Append(StringFormat.ToStringAlignment("Item count down"));
            builder.Append(this.itemCountDown);
            builder.Append(StringFormat.ToStringAlignment("Quantity/Charges #1"));
            builder.Append(this.quantityCharges1);
            builder.Append(StringFormat.ToStringAlignment("Quantity/Charges #2"));
            builder.Append(this.quantityCharges2);
            builder.Append(StringFormat.ToStringAlignment("Quantity/Charges #3"));
            builder.Append(this.quantityCharges3);
            builder.Append(StringFormat.ToStringAlignment("Item flags"));
            builder.Append((UInt32)this.flags);
            builder.Append(StringFormat.ToStringAlignment("Item flags (enumerated)"));
            builder.Append(this.GetItemFlagsString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which item flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetItemFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.flags & ItemInstanceFlags.Identified) == ItemInstanceFlags.Identified, ItemInstanceFlags.Identified.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & ItemInstanceFlags.Unstealable) == ItemInstanceFlags.Unstealable, ItemInstanceFlags.Unstealable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & ItemInstanceFlags.Stolen) == ItemInstanceFlags.Stolen, ItemInstanceFlags.Stolen.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & ItemInstanceFlags.UndroppableOrLoreRequirement) == ItemInstanceFlags.UndroppableOrLoreRequirement, ItemInstanceFlags.UndroppableOrLoreRequirement.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}