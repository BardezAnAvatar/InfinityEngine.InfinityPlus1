using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Store.Version
{
    public class StoreHeader2 : StoreHeader1
    {
        /// <summary>Binary size of the struct on disk</summary>
        public override Int32 StructSize
        {
            get { return 240; }
        }

        #region Members
        /// <summary>first two unknown bytes, where capacity used to reside</summary>
        protected UInt16 unknown1;

        /// <summary>Reserved 82 bytes</summary>
        protected Byte[] reserved2;
        #endregion

        #region Public Properties
        /// <summary>first two unknown bytes, where capacity used to reside</summary>
        public UInt16 Unknown1
        {
            get { return this.unknown1; }
            set { this.unknown1 = value; }
        }

        /// <summary>Reserved 82 bytes</summary>
        public Byte[] Reserved2
        {
            get { return this.reserved2; }
            set { this.reserved2 = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public StoreHeader2() : base()
        {
            this.reserved2 = null;
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.reserved2 = new Byte[82];
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
            this.unknown1 = ReusableIO.ReadUInt16FromArray(header, 26);
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
            Array.Copy(header, 112, this.reserved1, 0, 36);
            this.capacity = ReusableIO.ReadUInt16FromArray(header, 148);
            Array.Copy(header, 150, this.reserved2, 0, 82);
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
            ReusableIO.WriteUInt16ToStream(this.unknown1, output);
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
            ReusableIO.WriteUInt16ToStream(this.capacity, output);
            output.Write(this.reserved2, 0, 82);
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
        public override String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected override String GetVersionString()
        {
            return "Store header 2.0:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
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
            builder.Append(StringFormat.ToStringAlignment("Unknown #1"));
            builder.Append(this.unknown1);
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
            builder.Append(StringFormat.ToStringAlignment("Reserved #1"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved1));
            builder.Append(StringFormat.ToStringAlignment("Item capacity"));
            builder.Append(StringFormat.ToStringAlignment("Reserved #2"));
            builder.Append(StringFormat.ByteArrayToHexString(this.reserved2));
            builder.Append(this.capacity);

            return builder.ToString();
        }
        #endregion
    }
}