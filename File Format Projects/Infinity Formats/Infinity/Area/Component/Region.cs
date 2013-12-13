using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents a polygon region that triggers doors, regions, info, etc.</summary>
    public class Region : IInfinityFormat, ITrappable, ICursor, ITalkable
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        /// <remarks>IESDP says 0xB8 for IWD2, but it is still 0xC4</remarks>
        public const Int32 StructSize = 196;
        #endregion


        #region Fields
        /// <summary>Region's name</summary>
        /// <remarks>Often has data after the name, possibly junk, possibly not. 32 bytes in length</remarks>
        public ZString Name { get; set; }

        /// <summary>Type of the region</summary>
        public RegionType Type { get; set; }

        /// <summary>Minimumn bounding box of the region</summary>
        public Rectangle BoundingBox { get; set; }

        /// <summary>Vertex count of the region's perimeter</summary>
        public UInt16 CountVertices { get; set; }

        /// <summary>First index of vertices for this region's perimeter</summary>
        public Int32 IndexVertices { get; set; }

        /// <summary>Unknown at offset 0x30</summary>
        public Int32 Unknown_0x0030 { get; set; }

        /// <summary>Index for cursor for this region</summary>
        /// <remarks>Index matches to CURSORS.BAM</remarks>
        public Int32 CursorIndex { get; set; }

        /// <summary>Destination area for travel regions</summary>
        public ResourceReference DestinationArea { get; set; }

        /// <summary>Entrance name for destination area</summary>
        /// <remarks>Used for travel regions</remarks>
        public ZString EntranceName { get; set; }

        /// <summary>Trigger region's flags</summary>
        public TriggerFlags Flags { get; set; }

        /// <summary>Strref to region's text</summary>
        public StringReference Text { get; set; }

        /// <summary>Difficulty % to find the trap</summary>
        public Int16 DifficultyTrapDetection { get; set; }

        /// <summary>Difficulty % to remove the trap</summary>
        public Int16 DifficultyTrapRemoval { get; set; }

        /// <summary>Indicates whether the region is trapped</summary>
        /// <remarks>Stored as an Int16</remarks>
        public Boolean Trapped { get; set; }

        /// <summary>Indicates whether the region's trap is detected</summary>
        /// <remarks>Stored as an Int16</remarks>
        public Boolean TrapDetected { get; set; }

        /// <summary>Point from which this region's trap (if any) launches</summary>
        public Point TrapLaunchLocation { get; set; }

        /// <summary>Item resref which is the key to this region</summary>
        public ResourceReference KeyItem { get; set; }

        /// <summary>Script for this region</summary>
        public ResourceReference Script { get; set; }

        /// <summary>Alternative use point</summary>
        public Point AlternativeUse { get; set; }

        /// <summary>Unknown value at offset 0x88</summary>
        public Int32 Unknown_0x0088 { get; set; }

        /// <summary>Unknown 32 bytes at offset 0x8C</summary>
        /// <remarks>Given the size, is this another Char array?</remarks>
        public Byte[] Unknown_0x008C { get; set; }

        /// <summary>Sound played by the region</summary>
        public ResourceReference Sound { get; set; }

        /// <summary>Point used to talking?</summary>
        public Point TalkLocation { get; set; }

        /// <summary>Strref to name (displayed) of the region</summary>
        public StringReference DisplayName { get; set; }

        /// <summary>Associated dialog file</summary>
        public ResourceReference Dialog { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.BoundingBox = new Rectangle();
            this.DestinationArea = new ResourceReference(ResourceType.Area);
            this.EntranceName = new ZString();
            this.Text = new StringReference();
            this.TrapLaunchLocation = new Point();
            this.KeyItem = new ResourceReference(ResourceType.Item);
            this.Script = new ResourceReference();
            this.AlternativeUse = new Point();
            this.Sound = new ResourceReference();
            this.TalkLocation = new Point();
            this.DisplayName = new StringReference();
            this.Dialog = new ResourceReference(ResourceType.Dialog);
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 140);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Type = (RegionType)ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.BoundingBox.TopLeft.X = ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.BoundingBox.TopLeft.Y = ReusableIO.ReadUInt16FromArray(buffer, 36);
            this.BoundingBox.BottomRight.X = ReusableIO.ReadUInt16FromArray(buffer, 38);
            this.BoundingBox.BottomRight.Y = ReusableIO.ReadUInt16FromArray(buffer, 40);
            this.CountVertices = ReusableIO.ReadUInt16FromArray(buffer, 42);
            this.IndexVertices = ReusableIO.ReadInt32FromArray(buffer, 44);
            this.Unknown_0x0030 = ReusableIO.ReadInt32FromArray(buffer, 48);
            this.CursorIndex = ReusableIO.ReadInt32FromArray(buffer, 52);
            this.DestinationArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 56, CultureConstants.CultureCodeEnglish);
            this.EntranceName.Source = ReusableIO.ReadStringFromByteArray(buffer, 64, CultureConstants.CultureCodeEnglish, 32);
            this.Flags = (TriggerFlags)ReusableIO.ReadUInt32FromArray(buffer, 96);
            this.Text.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 100);
            this.DifficultyTrapDetection = ReusableIO.ReadInt16FromArray(buffer, 104);
            this.DifficultyTrapRemoval = ReusableIO.ReadInt16FromArray(buffer, 106);
            this.Trapped = Convert.ToBoolean(ReusableIO.ReadInt16FromArray(buffer, 108));
            this.TrapDetected = Convert.ToBoolean(ReusableIO.ReadInt16FromArray(buffer, 110));
            this.TrapLaunchLocation.X = ReusableIO.ReadUInt16FromArray(buffer, 112);
            this.TrapLaunchLocation.Y = ReusableIO.ReadUInt16FromArray(buffer, 114);
            this.KeyItem.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 116, CultureConstants.CultureCodeEnglish);
            this.Script.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 124, CultureConstants.CultureCodeEnglish);
            this.AlternativeUse.X = ReusableIO.ReadUInt16FromArray(buffer, 132);
            this.AlternativeUse.Y = ReusableIO.ReadUInt16FromArray(buffer, 134);
            this.Unknown_0x0088 = ReusableIO.ReadInt32FromArray(buffer, 136);
            
            this.Unknown_0x008C = ReusableIO.BinaryRead(input, 32);
            buffer = ReusableIO.BinaryRead(input, 24);

            this.Sound.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.TalkLocation.X = ReusableIO.ReadUInt16FromArray(buffer, 8);
            this.TalkLocation.Y = ReusableIO.ReadUInt16FromArray(buffer, 10);
            this.DisplayName.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 12);
            this.Dialog.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 16, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Type, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.TopLeft.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.TopLeft.Y, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.BottomRight.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.BottomRight.Y, output);
            ReusableIO.WriteUInt16ToStream(this.CountVertices, output);
            ReusableIO.WriteInt32ToStream(this.IndexVertices, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0030, output);
            ReusableIO.WriteInt32ToStream(this.CursorIndex, output);
            ReusableIO.WriteStringToStream(this.DestinationArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.EntranceName.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteInt32ToStream(this.Text.StringReferenceIndex, output);
            ReusableIO.WriteInt16ToStream(this.DifficultyTrapDetection, output);
            ReusableIO.WriteInt16ToStream(this.DifficultyTrapRemoval, output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.Trapped), output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.TrapDetected), output);
            ReusableIO.WriteUInt16ToStream(this.TrapLaunchLocation.X, output);
            ReusableIO.WriteUInt16ToStream(this.TrapLaunchLocation.Y, output);
            ReusableIO.WriteStringToStream(this.KeyItem.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.Script.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.AlternativeUse.X, output);
            ReusableIO.WriteUInt16ToStream(this.AlternativeUse.Y, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0088, output);
            output.Write(this.Unknown_0x008C, 0, 32);
            ReusableIO.WriteStringToStream(this.Sound.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.TalkLocation.X, output);
            ReusableIO.WriteUInt16ToStream(this.TalkLocation.Y, output);
            ReusableIO.WriteInt32ToStream(this.DisplayName.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.Dialog.ResRef, output, CultureConstants.CultureCodeEnglish);
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
            String header = this.GetVersionString();

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
            return StringFormat.ReturnAndIndent(String.Format("Region # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Region:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Type (value)"));
            builder.Append((UInt16)this.Type);
            builder.Append(StringFormat.ToStringAlignment("Type (description)"));
            builder.Append(this.Type.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Bounding Box:"));
            builder.Append(this.BoundingBox.ToString());
            builder.Append(StringFormat.ToStringAlignment("Vertex count"));
            builder.Append(this.CountVertices);
            builder.Append(StringFormat.ToStringAlignment("First Vertex index"));
            builder.Append(this.IndexVertices);
            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x30"));
            builder.Append(this.Unknown_0x0030);
            builder.Append(StringFormat.ToStringAlignment("Cursor BAM index"));
            builder.Append(this.CursorIndex);
            builder.Append(StringFormat.ToStringAlignment("Destination Area"));
            builder.Append(String.Format("'{0}'", this.DestinationArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Entrance Name"));
            builder.Append(String.Format("'{0}'", this.EntranceName.Value));
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetTriggerFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Text (strref)"));
            builder.Append(this.Text.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Detect trap difficulty"));
            builder.Append(this.DifficultyTrapDetection);
            builder.Append(StringFormat.ToStringAlignment("Remove trap difficulty"));
            builder.Append(this.DifficultyTrapRemoval);
            builder.Append(StringFormat.ToStringAlignment("Is trapped"));
            builder.Append(this.Trapped);
            builder.Append(StringFormat.ToStringAlignment("Trap detected"));
            builder.Append(this.TrapDetected);
            builder.Append(StringFormat.ToStringAlignment("Trap launch location"));
            builder.Append(this.TrapLaunchLocation.ToString());
            builder.Append(StringFormat.ToStringAlignment("Key (item resref)"));
            builder.Append(String.Format("'{0}'", this.KeyItem.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script"));
            builder.Append(String.Format("'{0}'", this.Script.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Alternative use point"));
            builder.Append(this.AlternativeUse.ToString());
            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x88"));
            builder.Append(this.Unknown_0x0088);
            builder.Append(StringFormat.ToStringAlignment("Unknown data @ offset 0x8C"));
            builder.Append(StringFormat.ByteArrayToHexString(Unknown_0x008C));
            builder.Append(StringFormat.ToStringAlignment("Sound"));
            builder.Append(String.Format("'{0}'", this.Sound.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Talk location"));
            builder.Append(this.TalkLocation.ToString());
            builder.Append(StringFormat.ToStringAlignment("Region name (strref) (in dialog?)"));
            builder.Append(this.DisplayName.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Dialog"));
            builder.Append(String.Format("'{0}'", this.Dialog.ZResRef));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetTriggerFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.Invisible) == TriggerFlags.Invisible, TriggerFlags.Invisible.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.ResetTrap) == TriggerFlags.ResetTrap, TriggerFlags.ResetTrap.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.PartyRequired) == TriggerFlags.PartyRequired, TriggerFlags.PartyRequired.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.Detectable) == TriggerFlags.Detectable, TriggerFlags.Detectable.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.LowMemory) == TriggerFlags.LowMemory, TriggerFlags.LowMemory.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.NpcTriggerable) == TriggerFlags.NpcTriggerable, TriggerFlags.NpcTriggerable.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.Deactivated) == TriggerFlags.Deactivated, TriggerFlags.Deactivated.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.TravelNonPC) == TriggerFlags.TravelNonPC, TriggerFlags.TravelNonPC.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.UsePoint) == TriggerFlags.UsePoint, TriggerFlags.UsePoint.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & TriggerFlags.InfoBlockedDoor) == TriggerFlags.InfoBlockedDoor, TriggerFlags.InfoBlockedDoor.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}