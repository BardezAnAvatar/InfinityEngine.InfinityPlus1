using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents a door in the ARE file</summary>
    public class Door : IInfinityFormat, ILockable, ITrappable, ICursor, ITalkable
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 200;
        #endregion


        #region Fields
        /// <summary>Door's name; 32 characters long</summary>
        public ZString Name { get; set; }

        /// <summary>Door's ID (matches to WED); 8 characters long</summary>
        public ZString DoorId { get; set; }

        /// <summary>Miscellaneous flags for this door</summary>
        public DoorFlags Flags { get; set; }

        /// <summary>First index of vertices for this door's perimeter while open</summary>
        public Int32 IndexVerticesOpen { get; set; }

        /// <summary>Vertex count of the door's perimeter while open</summary>
        public UInt16 CountVerticesOpen { get; set; }

        /// <summary>Vertex count of the door's perimeter while closed</summary>
        public UInt16 CountVerticesClosed { get; set; }

        /// <summary>First index of vertices for this door's perimeter while closed</summary>
        public Int32 IndexVerticesClosed { get; set; }

        /// <summary>Minimumn bounding box of the door while open</summary>
        public Rectangle BoundingBoxOpen { get; set; }

        /// <summary>Minimumn bounding box of the door while closed</summary>
        public Rectangle BoundingBoxClosed { get; set; }

        /// <summary>Represents the first vertex co-ordinate within the search map that is impeded while door is open</summary>
        public Point FirstImpededSearchMapVertexOpen { get; set; }

        /// <summary>Count of vertex co-ordinate within the search map that is impeded while door is open</summary>
        public Int16 CountImpededSearchMapVertexOpen { get; set; }

        /// <summary>Count of vertex co-ordinate within the search map that is impeded while door is closed</summary>
        public Int16 CountImpededSearchMapVertexClosed { get; set; }

        /// <summary>Represents the first vertex co-ordinate within the search map that is impeded while door is closed</summary>
        public Point FirstImpededSearchMapVertexClosed { get; set; }

        /// <summary>Unknown 4 bytes at offset 0x54</summary>
        public Int32 Unknown_0x0054 { get; set; }

        /// <summary>Sound to play when the door is opened</summary>
        public ResourceReference SoundOpen { get; set; }

        /// <summary>Sound to play when the door is closed</summary>
        public ResourceReference SoundClosed { get; set; }

        /// <summary>Index of the cursor to display for this door</summary>
        /// <remarks>Matches to Cursors.BAM</remarks>
        public Int32 CursorIndex { get; set; }

        /// <summary>Difficulty to detect a trap on this door</summary>
        public Int16 DifficultyTrapDetection { get; set; }

        /// <summary>Difficulty to remove a trap on this door</summary>
        public Int16 DifficultyTrapRemoval { get; set; }

        /// <summary>Door is trapped</summary>
        public Boolean Trapped { get; set; }

        /// <summary>Indicated whether a trap has been detected</summary>
        public Boolean TrapDetected { get; set; }

        /// <summary>Point from which this door's trap (if any) launches</summary>
        public Point TrapLaunchLocation { get; set; }

        /// <summary>Item resref which is the key to this door</summary>
        public ResourceReference KeyItem { get; set; }

        /// <summary>Door's script</summary>
        public ResourceReference Script { get; set; }

        /// <summary>Difficulty to detect this door</summary>
        public Int32 DifficultyDetection { get; set; }

        /// <summary>Difficulty of lock on door</summary>
        public Int32 DifficultyLock { get; set; }

        /// <summary>Represents the first co-ordinate from where the door state can be toggled</summary>
        public Point DoorToggleA { get; set; }

        /// <summary>Represents the second co-ordinate from where the door state can be toggled</summary>
        public Point DoorToggleB { get; set; }

        /// <summary>Strref to lockpick string</summary>
        public StringReference LockpickText { get; set; }

        /// <summary>Travel region name</summary>
        public ZString TravelRegionName { get; set; }

        /// <summary>Strref to name (displayed) of the door if chatting</summary>
        public StringReference DisplayName { get; set; }

        /// <summary>Associated dialog file</summary>
        public ResourceReference Dialog { get; set; }

        /// <summary>8 bytes of trailin padding at offset 0xC0</summary>
        /// <remarks>Probably another resref</remarks>
        public Byte[] Padding_0x00C0 { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes a flag indicating whether or not the door is locked</summary>
        public Boolean Locked
        {
            get { return ((this.Flags & DoorFlags.Locked) == DoorFlags.Locked); }
            set
            {
                if (value)
                    this.Flags |= DoorFlags.Locked;     //set the flag
                else
                    this.Flags &= (~DoorFlags.Locked);  //unset the flag
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.DoorId = new ZString();
            this.BoundingBoxOpen = new Rectangle();
            this.BoundingBoxClosed = new Rectangle();
            this.FirstImpededSearchMapVertexOpen = new Point();
            this.FirstImpededSearchMapVertexClosed = new Point();
            this.SoundOpen = new ResourceReference();
            this.SoundClosed = new ResourceReference();
            this.TrapLaunchLocation = new Point();
            this.KeyItem = new ResourceReference(ResourceType.Item);
            this.Script = new ResourceReference();
            this.DoorToggleA = new Point();
            this.DoorToggleB = new Point();
            this.LockpickText = new StringReference();
            this.TravelRegionName = new ZString();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 192);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.DoorId.Source = ReusableIO.ReadStringFromByteArray(buffer, 32, CultureConstants.CultureCodeEnglish);
            this.Flags = (DoorFlags)ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.IndexVerticesOpen = ReusableIO.ReadInt32FromArray(buffer, 44);
            this.CountVerticesOpen = ReusableIO.ReadUInt16FromArray(buffer, 48);
            this.CountVerticesClosed = ReusableIO.ReadUInt16FromArray(buffer, 50);
            this.IndexVerticesClosed = ReusableIO.ReadInt32FromArray(buffer, 52);
            this.BoundingBoxOpen.TopLeft.X = ReusableIO.ReadUInt16FromArray(buffer, 56);
            this.BoundingBoxOpen.TopLeft.Y = ReusableIO.ReadUInt16FromArray(buffer, 58);
            this.BoundingBoxOpen.BottomRight.X = ReusableIO.ReadUInt16FromArray(buffer, 60);
            this.BoundingBoxOpen.BottomRight.Y = ReusableIO.ReadUInt16FromArray(buffer, 62);
            this.BoundingBoxClosed.TopLeft.X = ReusableIO.ReadUInt16FromArray(buffer, 64);
            this.BoundingBoxClosed.TopLeft.Y = ReusableIO.ReadUInt16FromArray(buffer, 66);
            this.BoundingBoxClosed.BottomRight.X = ReusableIO.ReadUInt16FromArray(buffer, 68);
            this.BoundingBoxClosed.BottomRight.Y = ReusableIO.ReadUInt16FromArray(buffer, 70);
            this.FirstImpededSearchMapVertexOpen.X = ReusableIO.ReadUInt16FromArray(buffer, 72);
            this.FirstImpededSearchMapVertexOpen.Y = ReusableIO.ReadUInt16FromArray(buffer, 74);
            this.CountImpededSearchMapVertexOpen = ReusableIO.ReadInt16FromArray(buffer, 76);
            this.CountImpededSearchMapVertexClosed = ReusableIO.ReadInt16FromArray(buffer, 78);
            this.FirstImpededSearchMapVertexClosed.X = ReusableIO.ReadUInt16FromArray(buffer, 80);
            this.FirstImpededSearchMapVertexClosed.Y = ReusableIO.ReadUInt16FromArray(buffer, 82);
            this.Unknown_0x0054 = ReusableIO.ReadInt32FromArray(buffer, 84);
            this.SoundOpen.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 88, CultureConstants.CultureCodeEnglish);
            this.SoundClosed.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 96, CultureConstants.CultureCodeEnglish);
            this.CursorIndex = ReusableIO.ReadInt32FromArray(buffer, 104);
            this.DifficultyTrapDetection = ReusableIO.ReadInt16FromArray(buffer, 108);
            this.DifficultyTrapRemoval = ReusableIO.ReadInt16FromArray(buffer, 110);
            this.Trapped = Convert.ToBoolean(ReusableIO.ReadUInt16FromArray(buffer, 112));
            this.TrapDetected = Convert.ToBoolean(ReusableIO.ReadUInt16FromArray(buffer, 114));
            this.TrapLaunchLocation.X = ReusableIO.ReadUInt16FromArray(buffer, 116);
            this.TrapLaunchLocation.Y = ReusableIO.ReadUInt16FromArray(buffer, 118);
            this.KeyItem.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 120, CultureConstants.CultureCodeEnglish);
            this.Script.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 128, CultureConstants.CultureCodeEnglish);
            this.DifficultyDetection = ReusableIO.ReadInt32FromArray(buffer, 136);
            this.DifficultyLock = ReusableIO.ReadInt32FromArray(buffer, 140);
            this.DoorToggleA.X = ReusableIO.ReadUInt16FromArray(buffer, 144);
            this.DoorToggleA.Y = ReusableIO.ReadUInt16FromArray(buffer, 146);
            this.DoorToggleB.X = ReusableIO.ReadUInt16FromArray(buffer, 148);
            this.DoorToggleB.Y = ReusableIO.ReadUInt16FromArray(buffer, 150);
            this.LockpickText.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 152);
            this.TravelRegionName.Source = ReusableIO.ReadStringFromByteArray(buffer, 156, CultureConstants.CultureCodeEnglish, 24);
            this.DisplayName.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 180);
            this.Dialog.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 184, CultureConstants.CultureCodeEnglish);

            this.Padding_0x00C0 = ReusableIO.BinaryRead(input, 8);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteStringToStream(this.DoorId.Source, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteInt32ToStream(this.IndexVerticesOpen, output);
            ReusableIO.WriteUInt16ToStream(this.CountVerticesOpen, output);
            ReusableIO.WriteUInt16ToStream(this.CountVerticesClosed, output);
            ReusableIO.WriteInt32ToStream(this.IndexVerticesClosed, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxOpen.TopLeft.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxOpen.TopLeft.Y, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxOpen.BottomRight.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxOpen.BottomRight.Y, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxClosed.TopLeft.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxClosed.TopLeft.Y, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxClosed.BottomRight.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBoxClosed.BottomRight.Y, output);
            ReusableIO.WriteUInt16ToStream(this.FirstImpededSearchMapVertexOpen.X, output);
            ReusableIO.WriteUInt16ToStream(this.FirstImpededSearchMapVertexOpen.Y, output);
            ReusableIO.WriteInt16ToStream(this.CountImpededSearchMapVertexOpen, output);
            ReusableIO.WriteInt16ToStream(this.CountImpededSearchMapVertexClosed, output);
            ReusableIO.WriteUInt16ToStream(this.FirstImpededSearchMapVertexClosed.X, output);
            ReusableIO.WriteUInt16ToStream(this.FirstImpededSearchMapVertexClosed.Y, output);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0054, output);
            ReusableIO.WriteStringToStream(this.SoundOpen.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.SoundClosed.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.CursorIndex, output);
            ReusableIO.WriteInt16ToStream(this.DifficultyTrapDetection, output);
            ReusableIO.WriteInt16ToStream(this.DifficultyTrapRemoval, output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.Trapped), output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.TrapDetected), output);
            ReusableIO.WriteUInt16ToStream(this.TrapLaunchLocation.X, output);
            ReusableIO.WriteUInt16ToStream(this.TrapLaunchLocation.Y, output);
            ReusableIO.WriteStringToStream(this.KeyItem.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.Script.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.DifficultyDetection, output);
            ReusableIO.WriteInt32ToStream(this.DifficultyLock, output);
            ReusableIO.WriteUInt16ToStream(this.DoorToggleA.X, output);
            ReusableIO.WriteUInt16ToStream(this.DoorToggleA.Y, output);
            ReusableIO.WriteUInt16ToStream(this.DoorToggleB.X, output);
            ReusableIO.WriteUInt16ToStream(this.DoorToggleB.Y, output);
            ReusableIO.WriteInt32ToStream(this.LockpickText.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.TravelRegionName.Source, output, CultureConstants.CultureCodeEnglish, false, 24);
            ReusableIO.WriteInt32ToStream(this.DisplayName.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.Dialog.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.Write(this.Padding_0x00C0, 0, 8);
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
            return StringFormat.ReturnAndIndent(String.Format("Door # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Door:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Door ID (match to WED)"));
            builder.Append(String.Format("'{0}'", this.DoorId.Value));
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetContainerFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("First open Vertex index"));
            builder.Append(this.IndexVerticesOpen);
            builder.Append(StringFormat.ToStringAlignment("Open Vertex count"));
            builder.Append(this.CountVerticesOpen);
            builder.Append(StringFormat.ToStringAlignment("Closed Vertex count"));
            builder.Append(this.CountVerticesClosed);
            builder.Append(StringFormat.ToStringAlignment("First closed Vertex index"));
            builder.Append(this.IndexVerticesClosed);
            builder.Append(StringFormat.ToStringAlignment("Open Bounding Box:"));
            builder.Append(this.BoundingBoxOpen.ToString());
            builder.Append(StringFormat.ToStringAlignment("Closed Bounding Box:"));
            builder.Append(this.BoundingBoxClosed.ToString());
            builder.Append(StringFormat.ToStringAlignment("Open state First Impeded Search Map Vertex"));
            builder.Append(this.FirstImpededSearchMapVertexOpen.ToString());
            builder.Append(StringFormat.ToStringAlignment("Open state Impeded Search Map Vertex count"));
            builder.Append(this.CountImpededSearchMapVertexOpen);
            builder.Append(StringFormat.ToStringAlignment("Closed state Impeded Search Map Vertex count"));
            builder.Append(this.CountImpededSearchMapVertexClosed);
            builder.Append(StringFormat.ToStringAlignment("Closed state First Impeded Search Map Vertex"));
            builder.Append(this.FirstImpededSearchMapVertexClosed.ToString());
            builder.Append(StringFormat.ToStringAlignment("Unknown data @ offset 0x54"));
            builder.Append(this.Unknown_0x0054);
            builder.Append(StringFormat.ToStringAlignment("Sound played when opened"));
            builder.Append(String.Format("'{0}'", this.SoundOpen.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Sound played when closed"));
            builder.Append(String.Format("'{0}'", this.SoundClosed.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Cursor BAM index"));
            builder.Append(this.CursorIndex);
            builder.Append(StringFormat.ToStringAlignment("Detect trap difficulty"));
            builder.Append(this.DifficultyTrapDetection);
            builder.Append(StringFormat.ToStringAlignment("Remove trap difficulty"));
            builder.Append(this.DifficultyTrapRemoval);
            builder.Append(StringFormat.ToStringAlignment("Is trapped"));
            builder.Append(this.Trapped);
            builder.Append(StringFormat.ToStringAlignment("Trap detected"));
            builder.Append(this.TrapDetected);
            builder.Append(StringFormat.ToStringAlignment("Key (item resref)"));
            builder.Append(String.Format("'{0}'", this.KeyItem.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script"));
            builder.Append(String.Format("'{0}'", this.Script.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Detect door difficulty"));
            builder.Append(this.DifficultyDetection);
            builder.Append(StringFormat.ToStringAlignment("Open lock difficulty"));
            builder.Append(this.DifficultyLock);
            builder.Append(StringFormat.ToStringAlignment("Toggle door state position # 1"));
            builder.Append(this.DoorToggleA.ToString());
            builder.Append(StringFormat.ToStringAlignment("Toggle door state position # 2"));
            builder.Append(this.DoorToggleB.ToString());
            builder.Append(StringFormat.ToStringAlignment("Lock pick text (strref)"));
            builder.Append(this.LockpickText.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Destination entrance name"));
            builder.Append(String.Format("'{0}'", this.TravelRegionName.Value));
            builder.Append(StringFormat.ToStringAlignment("Door name (strref)"));
            builder.Append(this.DisplayName.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Dialog"));
            builder.Append(String.Format("'{0}'", this.Dialog.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0xC0"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x00C0));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetContainerFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Open) == DoorFlags.Open, DoorFlags.Open.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Locked) == DoorFlags.Locked, DoorFlags.Locked.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.TrapResets) == DoorFlags.TrapResets, DoorFlags.TrapResets.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.TrapDetectable) == DoorFlags.TrapDetectable, DoorFlags.TrapDetectable.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Bashed) == DoorFlags.Bashed, DoorFlags.Bashed.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.CannotClose) == DoorFlags.CannotClose, DoorFlags.CannotClose.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Linked) == DoorFlags.Linked, DoorFlags.Linked.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Hidden) == DoorFlags.Hidden, DoorFlags.Hidden.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Discovered) == DoorFlags.Discovered, DoorFlags.Discovered.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Transparent) == DoorFlags.Transparent, DoorFlags.Transparent.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.DestroysKey) == DoorFlags.DestroysKey, DoorFlags.DestroysKey.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & DoorFlags.Slide) == DoorFlags.Slide, DoorFlags.Slide.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}