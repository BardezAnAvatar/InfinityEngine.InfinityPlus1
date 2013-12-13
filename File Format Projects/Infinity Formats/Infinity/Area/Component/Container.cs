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
    /// <summary>Represents a container found in an area</summary>
    public class Container : IInfinityFormat, ITrappable
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 192;
        #endregion


        #region Fields
        /// <summary>Container's name</summary>
        public ZString Name { get; set; }

        /// <summary>Position of the container</summary>
        public Point Position { get; set; }

        /// <summary>Type of the container</summary>
        public ContainerType Type { get; set; }

        /// <summary>Difficulty of lock on container</summary>
        public Int16 DifficultyLock { get; set; }

        /// <summary>Various flags set on the container</summary>
        public ContainerFlags Flags { get; set; }

        /// <summary>Difficulty to detect a trap on this container</summary>
        public Int16 DifficultyTrapDetection { get; set; }

        /// <summary>Difficulty to remove a trap on this container</summary>
        public Int16 DifficultyTrapRemoval { get; set; }

        /// <summary>Container is trapped</summary>
        public Boolean Trapped { get; set; }

        /// <summary>Indicated whether a trap has been detected</summary>
        public Boolean TrapDetected { get; set; }

        /// <summary>Point from which this container's trap (if any) launches</summary>
        public Point TrapLaunchLocation { get; set; }

        /// <summary>Minimumn bounding box of the container</summary>
        public Rectangle BoundingBox { get; set; }

        /// <summary>Index of the first item in this container</summary>
        public Int32 IndexFirstItem { get; set; }

        /// <summary>Count of items in this container</summary>
        public Int32 CountItems { get; set; }

        /// <summary>Trap's script</summary>
        public ResourceReference ScriptTrap { get; set; }

        /// <summary>First index of vertices for this container's perimeter</summary>
        public Int32 IndexVertices { get; set; }

        /// <summary>Vertex count of the container's perimeter</summary>
        public UInt16 CountVertices { get; set; }

        /// <summary>Unknown 34 bytes at offset 0x56</summary>
        /// <remarks>
        ///     I almost think that two of these are part of the vertex count, and
        ///     that the other 32 are a scripting name or similar.
        /// </remarks>
        public Byte[] Unknown_0x0056 { get; set; }

        /// <summary>Item resref which is the key to this container</summary>
        public ResourceReference KeyItem { get; set; }

        /// <summary>Unknown 34 bytes at offset 0x80</summary>
        public Int32 Unknown_0x0080 { get; set; }

        /// <summary>Strref to lockpick string</summary>
        public StringReference LockpickText { get; set; }

        /// <summary>56 bytes of padding at offset 0x88</summary>
        public Byte[] Padding_0x0088 { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes a flag indicating whether or not the container is locked</summary>
        public Boolean Locked
        {
            get { return ((this.Flags & ContainerFlags.Locked) == ContainerFlags.Locked); }
            set
            {
                if (value)
                    this.Flags |= ContainerFlags.Locked;     //set the flag
                else
                    this.Flags &= (~ContainerFlags.Locked);  //unset the flag
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.Position = new Point();
            this.TrapLaunchLocation = new Point();
            this.BoundingBox = new Rectangle();
            this.ScriptTrap = new ResourceReference();
            this.KeyItem = new ResourceReference(ResourceType.Item);
            this.LockpickText = new StringReference();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 86);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Position.X = ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.Position.Y = ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.Type = (ContainerType)ReusableIO.ReadUInt16FromArray(buffer, 36);
            this.DifficultyLock = ReusableIO.ReadInt16FromArray(buffer, 38);
            this.Flags = (ContainerFlags)ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.DifficultyTrapDetection = ReusableIO.ReadInt16FromArray(buffer, 44);
            this.DifficultyTrapRemoval = ReusableIO.ReadInt16FromArray(buffer, 46);
            this.Trapped = Convert.ToBoolean(ReusableIO.ReadUInt16FromArray(buffer, 48));
            this.TrapDetected = Convert.ToBoolean(ReusableIO.ReadUInt16FromArray(buffer, 50));
            this.TrapLaunchLocation.X = ReusableIO.ReadUInt16FromArray(buffer, 52);
            this.TrapLaunchLocation.Y = ReusableIO.ReadUInt16FromArray(buffer, 54);
            this.BoundingBox.TopLeft.X = ReusableIO.ReadUInt16FromArray(buffer, 56);
            this.BoundingBox.TopLeft.Y = ReusableIO.ReadUInt16FromArray(buffer, 58);
            this.BoundingBox.BottomRight.X = ReusableIO.ReadUInt16FromArray(buffer, 60);
            this.BoundingBox.BottomRight.Y = ReusableIO.ReadUInt16FromArray(buffer, 62);
            this.IndexFirstItem = ReusableIO.ReadInt32FromArray(buffer, 64);
            this.CountItems = ReusableIO.ReadInt32FromArray(buffer, 68);
            this.ScriptTrap.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 72, CultureConstants.CultureCodeEnglish);
            this.IndexVertices = ReusableIO.ReadInt32FromArray(buffer, 80);
            this.CountVertices = ReusableIO.ReadUInt16FromArray(buffer, 84);

            this.Unknown_0x0056 = ReusableIO.BinaryRead(input, 34);

            buffer = ReusableIO.BinaryRead(input, 16);
            this.KeyItem.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.Unknown_0x0080 = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.LockpickText.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 12);

            this.Padding_0x0088 = ReusableIO.BinaryRead(input, 56);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.Position.X, output);
            ReusableIO.WriteUInt16ToStream(this.Position.Y, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Type, output);
            ReusableIO.WriteInt16ToStream(this.DifficultyLock, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteInt16ToStream(this.DifficultyTrapDetection, output);
            ReusableIO.WriteInt16ToStream(this.DifficultyTrapRemoval, output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.Trapped), output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.TrapDetected), output);
            ReusableIO.WriteUInt16ToStream(this.TrapLaunchLocation.X, output);
            ReusableIO.WriteUInt16ToStream(this.TrapLaunchLocation.Y, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.TopLeft.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.TopLeft.Y, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.BottomRight.X, output);
            ReusableIO.WriteUInt16ToStream(this.BoundingBox.BottomRight.Y, output);
            ReusableIO.WriteInt32ToStream(this.IndexFirstItem, output);
            ReusableIO.WriteInt32ToStream(this.CountItems, output);
            ReusableIO.WriteStringToStream(this.ScriptTrap.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.IndexVertices, output);
            ReusableIO.WriteUInt16ToStream(this.CountVertices, output);
            output.Write(this.Unknown_0x0056, 0, 34);
            ReusableIO.WriteStringToStream(this.KeyItem.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.Unknown_0x0080, output);
            ReusableIO.WriteInt32ToStream(this.LockpickText.StringReferenceIndex, output);
            output.Write(this.Padding_0x0088, 0, 56);
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
            return StringFormat.ReturnAndIndent(String.Format("Container # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Container:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Container location"));
            builder.Append(this.Position.ToString());
            builder.Append(StringFormat.ToStringAlignment("Type (value)"));
            builder.Append((UInt16)this.Type);
            builder.Append(StringFormat.ToStringAlignment("Type (description)"));
            builder.Append(this.Type.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Lock difficulty"));
            builder.Append(this.DifficultyLock);
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetContainerFlagsEnumerationString());
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
            builder.Append(StringFormat.ToStringAlignment("Bounding Box:"));
            builder.Append(this.BoundingBox.ToString());
            builder.Append(StringFormat.ToStringAlignment("First Item index"));
            builder.Append(this.IndexFirstItem);
            builder.Append(StringFormat.ToStringAlignment("Item count"));
            builder.Append(this.CountItems);
            builder.Append(StringFormat.ToStringAlignment("Script (Trap)"));
            builder.Append(String.Format("'{0}'", this.ScriptTrap.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("First Vertex index"));
            builder.Append(this.IndexVertices);
            builder.Append(StringFormat.ToStringAlignment("Vertex count"));
            builder.Append(this.CountVertices);
            builder.Append(StringFormat.ToStringAlignment("Unknown data @ offset 0x56"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Unknown_0x0056));
            builder.Append(StringFormat.ToStringAlignment("Key (item resref)"));
            builder.Append(String.Format("'{0}'", this.KeyItem.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x80"));
            builder.Append(this.Unknown_0x0080);
            builder.Append(StringFormat.ToStringAlignment("Lock pick text (strref)"));
            builder.Append(this.LockpickText.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Padding @ offset 0x88"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0088));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetContainerFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & ContainerFlags.Locked) == ContainerFlags.Locked, ContainerFlags.Locked.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & ContainerFlags.TrapResets) == ContainerFlags.TrapResets, ContainerFlags.TrapResets.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & ContainerFlags.Disabled) == ContainerFlags.Disabled, ContainerFlags.Disabled.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & ContainerFlags.DisabledPst) == ContainerFlags.DisabledPst, ContainerFlags.DisabledPst.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}