using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Store.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Store.Version
{
    public class StoreHeader1 : InfinityFormat
    {
        /// <summary>Binary size of the struct on disk</summary>
        public virtual Int32 StructSize
        {
            get { return 156; }
        }

        #region Members
        /// <summary>Store's type</summary>
        protected StoreType type;

        /// <summary>String Reference to Store's name</summary>
        protected StringReference name;

        /// <summary>Store's flags</summary>
        protected StoreFlags flags;

        /// <summary>Percentage of base cost store charges</summary>
        protected UInt32 sellMarkup;

        /// <summary>Percentage of base cost store is willing to pay</summary>
        protected UInt32 buyMarkdown;

        /// <summary>Price depreciation</summary>
        /// <remarks>
        ///     The price goes down this muchafter the first sale, then again after the second, stopping there.
        ///     Does this 1,2,3 setting reflect upon the items in stack?
        /// </remarks>
        protected UInt32 depreciationRate;

        /// <summary>Opposing theft difficulty; must overcome to successfully steal</summary>
        protected UInt16 theftDifficulty;

        /// <summary>Number of items the store can hold.</summary>
        /// <value>0 is infinite, others are total item capacity (for containers)</value>
        /// <remarks>Not in the same place for IWD2, that data offset becomes unused</remarks>
        protected UInt16 capacity;

        /// <summary>Dialog ResRef.</summary>
        /// <remarks>Only example found is a dialog in TierNon.sto, referencing DTiersto.dlg, which is a theft dialog</remarks>
        protected ResourceReference theftDialog;

        /// <summary>Offset to the item types purchased in this store</summary>
        protected UInt32 offsetPurchaseTypes;

        /// <summary>Count of the item types purchased in this store</summary>
        protected UInt32 countPurchaseTypes;

        /// <summary>Offset to the items for sale in this store</summary>
        protected UInt32 offsetItems;

        /// <summary>Count of the items for sale in this store</summary>
        protected UInt32 countItems;

        /// <summary>Lore ability of the store (low values cannot identify large lore requirements?)</summary>
        protected UInt32 lore;

        /// <summary>Cost to identify items</summary>
        protected UInt32 costIdentify;

        /// <summary>Rumors ResRef</summary>
        /// <remarks>Dialog file containing rumors from the bar</remarks>
        protected ResourceReference rumorsBar;

        /// <summary>Offset to the drinks served in this store</summary>
        protected UInt32 offsetDrinks;

        /// <summary>Count of the drinks served in this store</summary>
        protected UInt32 countDrinks;

        /// <summary>Rumors ResRef</summary>
        /// <remarks>Dialog file containing rumors from the temple</remarks>
        protected ResourceReference rumorsTemple;

        /// <summary>Flags indicating which rooms are available</summary>
        protected AvailableRooms roomsAvailable;

        /// <summary>Cost of a peasant's room</summary>
        protected UInt32 costRoomPeasant;

        /// <summary>Cost of a merchant's room</summary>
        protected UInt32 costRoomMerchant;

        /// <summary>Cost of a noble's room</summary>
        protected UInt32 costRoomNoble;

        /// <summary>Cost of a king's room</summary>
        protected UInt32 costRoomRoyal;

        /// <summary>Offset to the healing spells provided at this store</summary>
        protected UInt32 offsetHealing;

        /// <summary>Count of the healing spells provided at this store</summary>
        protected UInt32 countHealing;

        /// <summary>Reserved 36 bytes</summary>
        protected Byte[] reserved1;
        #endregion

        #region Public Properties
        /// <summary>Store's type</summary>
        public StoreType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>String Reference to Store's name</summary>
        public StringReference Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>Store's flags</summary>
        public StoreFlags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }

        /// <summary>Percentage of base cost store charges</summary>
        public UInt32 SellMarkup
        {
            get { return this.sellMarkup; }
            set { this.sellMarkup = value; }
        }

        /// <summary>Percentage of base cost store is willing to pay</summary>
        public UInt32 BuyMarkdown
        {
            get { return this.buyMarkdown; }
            set { this.buyMarkdown = value; }
        }

        /// <summary>Price depreciation</summary>
        /// <remarks>
        ///     The price goes down this muchafter the first sale, then again after the second, stopping there.
        ///     Does this 1,2,3 setting reflect upon the items in stack?
        /// </remarks>
        public UInt32 DepreciationRate
        {
            get { return this.depreciationRate; }
            set { this.depreciationRate = value; }
        }

        /// <summary>Opposing theft difficulty; must overcome to successfully steal</summary>
        public UInt16 TheftDifficulty
        {
            get { return this.theftDifficulty; }
            set { this.theftDifficulty = value; }
        }

        /// <summary>Number of items the store can hold.</summary>
        /// <value>0 is infinite, others are total item capacity (for containers)</value>
        /// <remarks>Not in the same place for IWD2, that data offset becomes unused</remarks>
        public UInt16 Capacity
        {
            get { return this.capacity; }
            set { this.capacity = value; }
        }

        /// <summary>Dialog ResRef.</summary>
        /// <remarks>Only example found is a dialog in TierNon.sto, referencing DTiersto.dlg, which is a theft dialog</remarks>
        public ResourceReference TheftDialog
        {
            get { return this.theftDialog; }
            set { this.theftDialog = value; }
        }

        /// <summary>Offset to the item types purchased in this store</summary>
        public UInt32 OffsetPurchaseTypes
        {
            get { return this.offsetPurchaseTypes; }
            set { this.offsetPurchaseTypes = value; }
        }

        /// <summary>Count of the item types purchased in this store</summary>
        public UInt32 CountPurchaseTypes
        {
            get { return this.countPurchaseTypes; }
            set { this.countPurchaseTypes = value; }
        }

        /// <summary>Offset to the items for sale in this store</summary>
        public UInt32 OffsetItems
        {
            get { return this.offsetItems; }
            set { this.offsetItems = value; }
        }

        /// <summary>Count of the items for sale in this store</summary>
        public UInt32 CountItems
        {
            get { return this.countItems; }
            set { this.countItems = value; }
        }

        /// <summary>Lore ability of the store (low values cannot identify large lore requirements?)</summary>
        public UInt32 Lore
        {
            get { return this.lore; }
            set { this.lore = value; }
        }

        /// <summary>Cost to identify items</summary>
        public UInt32 CostIdentify
        {
            get { return this.costIdentify; }
            set { this.costIdentify = value; }
        }

        /// <summary>Rumors ResRef</summary>
        /// <remarks>Dialog file containing rumors from the bar</remarks>
        public ResourceReference RumorsBar
        {
            get { return this.rumorsBar; }
            set { this.rumorsBar = value; }
        }

        /// <summary>Offset to the drinks served in this store</summary>
        public UInt32 OffsetDrinks
        {
            get { return this.offsetDrinks; }
            set { this.offsetDrinks = value; }
        }

        /// <summary>Count of the drinks served in this store</summary>
        public UInt32 CountDrinks
        {
            get { return this.countDrinks; }
            set { this.countDrinks = value; }
        }

        /// <summary>Rumors ResRef</summary>
        /// <remarks>Dialog file containing rumors from the temple</remarks>
        public ResourceReference RumorsTemple
        {
            get { return this.rumorsTemple; }
            set { this.rumorsTemple = value; }
        }

        /// <summary>Flags indicating which rooms are available</summary>
        public AvailableRooms RoomsAvailable
        {
            get { return this.roomsAvailable; }
            set { this.roomsAvailable = value; }
        }

        /// <summary>Cost of a peasant's room</summary>
        public UInt32 CostRoomPeasant
        {
            get { return this.costRoomPeasant; }
            set { this.costRoomPeasant = value; }
        }

        /// <summary>Cost of a merchant's room</summary>
        public UInt32 CostRoomMerchant
        {
            get { return this.costRoomMerchant; }
            set { this.costRoomMerchant = value; }
        }

        /// <summary>Cost of a noble's room</summary>
        public UInt32 CostRoomNoble
        {
            get { return this.costRoomNoble; }
            set { this.costRoomNoble = value; }
        }

        /// <summary>Cost of a king's room</summary>
        public UInt32 CostRoomRoyal
        {
            get { return this.costRoomRoyal; }
            set { this.costRoomRoyal = value; }
        }

        /// <summary>Offset to the healing spells provided at this store</summary>
        public UInt32 OffsetHealing
        {
            get { return this.offsetHealing; }
            set { this.offsetHealing = value; }
        }

        /// <summary>Count of the healing spells provided at this store</summary>
        public UInt32 CountHealing
        {
            get { return this.countHealing; }
            set { this.countHealing = value; }
        }

        /// <summary>Reserved 36 bytes</summary>
        public Byte[] Reserved1
        {
            get { return this.reserved1; }
            set { this.reserved1 = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public StoreHeader1()
        {
            this.name = null;
            this.theftDialog = null;
            this.rumorsBar = null;
            this.rumorsTemple = null;
            this.reserved1 = null;
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.name = new StringReference();
            this.theftDialog = new ResourceReference();
            this.rumorsBar = new ResourceReference();
            this.rumorsTemple = new ResourceReference();
            this.reserved1 = new Byte[36];
        }
        #endregion

        #region Abstract IO methods
        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] header = ReusableIO.BinaryRead(input, this.StructSize - 8);

            this.type = (StoreType)ReusableIO.ReadUInt32FromArray(header, 0);
            this.name.StringReferenceIndex = ReusableIO.ReadInt32FromArray(header, 4);
            this.flags = (StoreFlags)ReusableIO.ReadUInt32FromArray(header, 8);
            this.sellMarkup = ReusableIO.ReadUInt32FromArray(header, 12);
            this.buyMarkdown = ReusableIO.ReadUInt32FromArray(header, 16);
            this.depreciationRate = ReusableIO.ReadUInt32FromArray(header, 20);
            this.theftDifficulty = ReusableIO.ReadUInt16FromArray(header, 24);
            this.capacity = ReusableIO.ReadUInt16FromArray(header, 26);
            this.theftDialog.ResRef = ReusableIO.ReadStringFromByteArray(header, 28, CultureConstants.CultureCodeEnglish);
            this.offsetPurchaseTypes = ReusableIO.ReadUInt32FromArray(header, 36);
            this.countPurchaseTypes = ReusableIO.ReadUInt32FromArray(header, 40);
            this.offsetItems = ReusableIO.ReadUInt32FromArray(header, 44);
            this.countItems = ReusableIO.ReadUInt32FromArray(header, 48);
            this.lore = ReusableIO.ReadUInt32FromArray(header, 52);
            this.costIdentify = ReusableIO.ReadUInt32FromArray(header, 56);
            this.rumorsBar.ResRef = ReusableIO.ReadStringFromByteArray(header, 60, CultureConstants.CultureCodeEnglish);
            this.offsetDrinks = ReusableIO.ReadUInt32FromArray(header, 68);
            this.countDrinks = ReusableIO.ReadUInt32FromArray(header, 72);
            this.rumorsTemple.ResRef = ReusableIO.ReadStringFromByteArray(header, 76, CultureConstants.CultureCodeEnglish);
            this.roomsAvailable = (AvailableRooms)ReusableIO.ReadUInt32FromArray(header, 84);
            this.costRoomPeasant = ReusableIO.ReadUInt32FromArray(header, 88);
            this.costRoomMerchant = ReusableIO.ReadUInt32FromArray(header, 92);
            this.costRoomNoble = ReusableIO.ReadUInt32FromArray(header, 96);
            this.costRoomRoyal = ReusableIO.ReadUInt32FromArray(header, 100);
            this.offsetHealing = ReusableIO.ReadUInt32FromArray(header, 104);
            this.countHealing = ReusableIO.ReadUInt32FromArray(header, 108);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt32ToStream((UInt32)this.type, output);
            ReusableIO.WriteInt32ToStream(this.name.StringReferenceIndex, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.flags, output);
            ReusableIO.WriteUInt32ToStream(this.sellMarkup, output);
            ReusableIO.WriteUInt32ToStream(this.buyMarkdown, output);
            ReusableIO.WriteUInt32ToStream(this.depreciationRate, output);
            ReusableIO.WriteUInt16ToStream(this.theftDifficulty, output);
            ReusableIO.WriteUInt16ToStream(this.capacity, output);
            ReusableIO.WriteStringToStream(this.theftDialog.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.offsetPurchaseTypes, output);
            ReusableIO.WriteUInt32ToStream(this.countPurchaseTypes, output);
            ReusableIO.WriteUInt32ToStream(this.offsetItems, output);
            ReusableIO.WriteUInt32ToStream(this.countItems, output);
            ReusableIO.WriteUInt32ToStream(this.lore, output);
            ReusableIO.WriteUInt32ToStream(this.costIdentify, output);
            ReusableIO.WriteStringToStream(this.rumorsBar.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.offsetDrinks, output);
            ReusableIO.WriteUInt32ToStream(this.countDrinks, output);
            ReusableIO.WriteStringToStream(this.rumorsTemple.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.roomsAvailable, output);
            ReusableIO.WriteUInt32ToStream(this.costRoomPeasant, output);
            ReusableIO.WriteUInt32ToStream(this.costRoomMerchant, output);
            ReusableIO.WriteUInt32ToStream(this.costRoomNoble, output);
            ReusableIO.WriteUInt32ToStream(this.costRoomRoyal, output);
            ReusableIO.WriteUInt32ToStream(this.offsetHealing, output);
            ReusableIO.WriteUInt32ToStream(this.countHealing, output);
            output.Write(this.reserved1, 0, 36);
        }
        #endregion

        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public virtual String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected virtual String GetVersionString()
        {
            return "Store header 1.0:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.version);
            builder.Append(StringFormat.ToStringAlignment("Type"));
            builder.Append((UInt32)this.type);
            builder.Append(StringFormat.ToStringAlignment("Type (description)"));
            builder.Append(this.type.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Name Strref"));
            builder.Append(this.name.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Flags"));
            builder.Append((UInt32)this.flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumerated)"));
            builder.Append(this.GetStoreFlagString());
            builder.Append(StringFormat.ToStringAlignment("Sale price %"));
            builder.Append(this.sellMarkup);
            builder.Append(StringFormat.ToStringAlignment("Purchase price %"));
            builder.Append(this.buyMarkdown);
            builder.Append(StringFormat.ToStringAlignment("Price depreciation %"));
            builder.Append(this.depreciationRate);
            builder.Append(StringFormat.ToStringAlignment("Theft difficulty %"));
            builder.Append(this.theftDifficulty);
            builder.Append(StringFormat.ToStringAlignment("Item capacity"));
            builder.Append(this.capacity);
            builder.Append(StringFormat.ToStringAlignment("Theft dialog"));
            builder.Append(this.theftDialog.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Purchase types offset"));
            builder.Append(this.offsetPurchaseTypes);
            builder.Append(StringFormat.ToStringAlignment("Purchase types count"));
            builder.Append(this.countPurchaseTypes);
            builder.Append(StringFormat.ToStringAlignment("Items offset"));
            builder.Append(this.offsetItems);
            builder.Append(StringFormat.ToStringAlignment("Items count"));
            builder.Append(this.countItems);
            builder.Append(StringFormat.ToStringAlignment("Store's lore"));
            builder.Append(this.lore);
            builder.Append(StringFormat.ToStringAlignment("Cost to perform identify"));
            builder.Append(this.costIdentify);
            builder.Append(StringFormat.ToStringAlignment("Bar rumors dialog"));
            builder.Append(this.rumorsBar.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Drinks offset"));
            builder.Append(this.offsetDrinks);
            builder.Append(StringFormat.ToStringAlignment("Drinks count"));
            builder.Append(this.countDrinks);
            builder.Append(StringFormat.ToStringAlignment("Temple rumors dialog"));
            builder.Append(this.rumorsTemple.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Rooms available"));
            builder.Append((UInt32)this.roomsAvailable);
            builder.Append(StringFormat.ToStringAlignment("Rooms available (enumerated)"));
            builder.Append(this.GetRoomsAvailableString());
            builder.Append(StringFormat.ToStringAlignment("Cost for peasant room"));
            builder.Append(this.costRoomPeasant);
            builder.Append(StringFormat.ToStringAlignment("Cost for merchant room"));
            builder.Append(this.costRoomMerchant);
            builder.Append(StringFormat.ToStringAlignment("Cost for noble room"));
            builder.Append(this.costRoomNoble);
            builder.Append(StringFormat.ToStringAlignment("Cost for royal room"));
            builder.Append(this.costRoomRoyal);
            builder.Append(StringFormat.ToStringAlignment("Healing offset"));
            builder.Append(this.offsetHealing);
            builder.Append(StringFormat.ToStringAlignment("Healing count"));
            builder.Append(this.countHealing);
            builder.Append(StringFormat.ToStringAlignment("Reserved"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved1));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which Storelags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetStoreFlagString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.UserCanBuy) == StoreFlags.UserCanBuy, StoreFlags.UserCanBuy.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.UserCanSell) == StoreFlags.UserCanSell, StoreFlags.UserCanSell.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.Identifies) == StoreFlags.Identifies, StoreFlags.Identifies.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.UserCanSteal) == StoreFlags.UserCanSteal, StoreFlags.UserCanSteal.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.DonateMoney) == StoreFlags.DonateMoney, StoreFlags.DonateMoney.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.Heals) == StoreFlags.Heals, StoreFlags.Heals.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.LiquorLicense) == StoreFlags.LiquorLicense, StoreFlags.LiquorLicense.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.Quality1) == StoreFlags.Quality1, StoreFlags.Quality1.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.Quality2) == StoreFlags.Quality2, StoreFlags.Quality2.GetDescription());
            StringFormat.AppendSubItem(sb, (this.flags & StoreFlags.Fence) == StoreFlags.Fence, StoreFlags.Fence.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which Storelags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetRoomsAvailableString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.roomsAvailable & AvailableRooms.Peasant) == AvailableRooms.Peasant, AvailableRooms.Peasant.GetDescription());
            StringFormat.AppendSubItem(sb, (this.roomsAvailable & AvailableRooms.Merchant) == AvailableRooms.Merchant, AvailableRooms.Merchant.GetDescription());
            StringFormat.AppendSubItem(sb, (this.roomsAvailable & AvailableRooms.Noble) == AvailableRooms.Noble, AvailableRooms.Noble.GetDescription());
            StringFormat.AppendSubItem(sb, (this.roomsAvailable & AvailableRooms.Royal) == AvailableRooms.Royal, AvailableRooms.Royal.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}