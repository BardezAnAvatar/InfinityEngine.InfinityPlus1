using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap.Component
{
    /// <summary>A map's area entry</summary>
    public class AreaEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 240;
        #endregion


        #region Fields
        /// <summary>Resource (ARE) that the map area currently targets</summary>
        public ResourceReference CurrentArea { get; set; }

        /// <summary>Resource (ARE) that the map area originally targeted</summary>
        /// <remarks>Look at ToB's Imnsvale forest?</remarks>
        public ResourceReference OriginalArea { get; set; }

        /// <summary>32-character name of the area (for scripting?)</summary>
        /// <remarks>This appears to be a clone of the area</remarks>
        public ZString ScriptName { get; set; }

        /// <summary>Miscellaneous flags for the map area</summary>
        public MapAreaFlags Flags { get; set; }

        /// <summary>The animation number for this area in the map's BAM</summary>
        public Int32 AnimationNumber { get; set; }

        /// <summary>X-Coordinate on the map</summary>
        public Int32 CoordinateX { get; set; }

        /// <summary>Y-Coordinate on the map</summary>
        public Int32 CoordinateY { get; set; }

        /// <summary>Name of the area on the map</summary>
        public StringReference Name { get; set; }

        /// <summary>Tooltip for the area on the map</summary>
        public StringReference ToolTip { get; set; }

        /// <summary>Image (MOS) reference to use for this area's loading screen</summary>
        public ResourceReference LoadingScreen { get; set; }

        /// <summary>The index of the first area link to the north</summary>
        public UInt32 AreaLinkIndexNorth { get; set; }

        /// <summary>The count of area links to the north</summary>
        public UInt32 AreaLinkCountNorth { get; set; }

        /// <summary>The index of the first area link to the west</summary>
        public UInt32 AreaLinkIndexWest { get; set; }

        /// <summary>The count of area links to the west</summary>
        public UInt32 AreaLinkCountWest { get; set; }

        /// <summary>The index of the first area link to the south</summary>
        public UInt32 AreaLinkIndexSouth { get; set; }

        /// <summary>The count of area links to the south</summary>
        public UInt32 AreaLinkCountSouth { get; set; }

        /// <summary>The index of the first area link to the east</summary>
        public UInt32 AreaLinkIndexEast { get; set; }

        /// <summary>The count of area links to the east</summary>
        public UInt32 AreaLinkCountEast { get; set; }

        /// <summary>128 bytes of reserved data that are currently unused</summary>
        public Byte[] Reserved { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.CurrentArea = new ResourceReference(ResourceType.Area);
            this.OriginalArea = new ResourceReference(ResourceType.Area);
            this.ScriptName = new ZString();
            this.Name = new StringReference();
            this.ToolTip = new StringReference();
            this.LoadingScreen = new ResourceReference(ResourceType.Mosaic);
        }
        #endregion


        #region I/O Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] buffer = ReusableIO.BinaryRead(input, (MapEntry.StructSize - 128));

            this.CurrentArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.OriginalArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 8, CultureConstants.CultureCodeEnglish);
            this.ScriptName.Source = ReusableIO.ReadStringFromByteArray(buffer, 16, CultureConstants.CultureCodeEnglish, 32);
            this.Flags = (MapAreaFlags)(ReusableIO.ReadUInt32FromArray(buffer, 48));
            this.AnimationNumber = ReusableIO.ReadInt32FromArray(buffer, 52);
            this.CoordinateX = ReusableIO.ReadInt32FromArray(buffer, 56);
            this.CoordinateY = ReusableIO.ReadInt32FromArray(buffer, 60);
            this.Name.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 64);
            this.ToolTip.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 68);
            this.LoadingScreen.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 72, CultureConstants.CultureCodeEnglish);
            this.AreaLinkIndexNorth = ReusableIO.ReadUInt32FromArray(buffer, 80);
            this.AreaLinkCountNorth = ReusableIO.ReadUInt32FromArray(buffer, 84);
            this.AreaLinkIndexWest = ReusableIO.ReadUInt32FromArray(buffer, 88);
            this.AreaLinkCountWest = ReusableIO.ReadUInt32FromArray(buffer, 92);
            this.AreaLinkIndexSouth = ReusableIO.ReadUInt32FromArray(buffer, 96);
            this.AreaLinkCountSouth = ReusableIO.ReadUInt32FromArray(buffer, 100);
            this.AreaLinkIndexEast = ReusableIO.ReadUInt32FromArray(buffer, 104);
            this.AreaLinkCountEast = ReusableIO.ReadUInt32FromArray(buffer, 108);
            this.Reserved = ReusableIO.BinaryRead(input, 128);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.CurrentArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.OriginalArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptName.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteInt32ToStream(this.AnimationNumber, output);
            ReusableIO.WriteInt32ToStream(this.CoordinateX, output);
            ReusableIO.WriteInt32ToStream(this.CoordinateY, output);
            ReusableIO.WriteInt32ToStream(this.Name.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.ToolTip.StringReferenceIndex, output);
            ReusableIO.WriteStringToStream(this.LoadingScreen.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkIndexNorth, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkCountNorth, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkIndexWest, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkCountWest, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkIndexSouth, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkCountSouth, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkIndexEast, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkCountEast, output);
            output.Write(this.Reserved, 0, 128);
        }
        #endregion


        #region Hash code and comparison
        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.CurrentArea.GetHashCode();
            hash ^= this.OriginalArea.GetHashCode();
            hash ^= this.ScriptName.GetHashCode();
            hash ^= this.Flags.GetHashCode();
            hash ^= this.AnimationNumber.GetHashCode();
            hash ^= this.CoordinateX.GetHashCode();
            hash ^= this.CoordinateY.GetHashCode();
            hash ^= this.Name.GetHashCode();
            hash ^= this.ToolTip.GetHashCode();
            hash ^= this.LoadingScreen.GetHashCode();
            hash ^= this.AreaLinkIndexNorth.GetHashCode();
            hash ^= this.AreaLinkCountNorth.GetHashCode();
            hash ^= this.AreaLinkIndexWest.GetHashCode();
            hash ^= this.AreaLinkCountWest.GetHashCode();
            hash ^= this.AreaLinkIndexSouth.GetHashCode();
            hash ^= this.AreaLinkCountSouth.GetHashCode();
            hash ^= this.AreaLinkIndexEast.GetHashCode();
            hash ^= this.AreaLinkCountEast.GetHashCode();
            hash ^= this.Reserved.GetHashCode();

            return hash;
        }
        #endregion


        #region ToString override(s)
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <param name="index">Index of this area entry to describe</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 index)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("Area Entry # {0}", index));
            builder.Append(StringFormat.ToStringAlignment("Current Area"));
            builder.Append(String.Format("'{0}'", this.CurrentArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Original Area"));
            builder.Append(String.Format("'{0}'", this.OriginalArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script Name"));
            builder.Append(this.ScriptName.Value);
            builder.Append(StringFormat.ToStringAlignment("Area flags (value)"));
            builder.Append((UInt16)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Area flags (enumerated)"));
            builder.Append(this.GenerateMapAreaFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Animation number"));
            builder.Append(this.AnimationNumber);
            builder.Append(StringFormat.ToStringAlignment("Coordinate X"));
            builder.Append(this.CoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Coordinate Y"));
            builder.Append(this.CoordinateY);
            builder.Append(StringFormat.ToStringAlignment("Name StrRef"));
            builder.Append(this.Name.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Tool Tip StrRef"));
            builder.Append(this.ToolTip.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Loading Screen"));
            builder.Append(String.Format("'{0}'", this.LoadingScreen.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("North Area Links first Index"));
            builder.Append(this.AreaLinkIndexNorth);
            builder.Append(StringFormat.ToStringAlignment("North Area Links Count"));
            builder.Append(this.AreaLinkCountNorth);
            builder.Append(StringFormat.ToStringAlignment("West Area Links first Index"));
            builder.Append(this.AreaLinkIndexWest);
            builder.Append(StringFormat.ToStringAlignment("West Area Links Count"));
            builder.Append(this.AreaLinkCountWest);
            builder.Append(StringFormat.ToStringAlignment("South Area Links first Index"));
            builder.Append(this.AreaLinkIndexSouth);
            builder.Append(StringFormat.ToStringAlignment("South Area Links Count"));
            builder.Append(this.AreaLinkCountSouth);
            builder.Append(StringFormat.ToStringAlignment("East Area Links first Index"));
            builder.Append(this.AreaLinkIndexEast);
            builder.Append(StringFormat.ToStringAlignment("East Area Links Count"));
            builder.Append(this.AreaLinkCountEast);
            builder.Append(StringFormat.ToStringAlignment("Reserved"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Reserved));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable representation of the MapAreaFlags enumerator</summary>
        /// <returns>A human-readable representation of the MapAreaFlags enumerator</returns>
        protected String GenerateMapAreaFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Flags & MapAreaFlags.Reachable) == MapAreaFlags.Reachable, MapAreaFlags.Reachable.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & MapAreaFlags.Visible) == MapAreaFlags.Visible, MapAreaFlags.Visible.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & MapAreaFlags.VisibleByAdjacent) == MapAreaFlags.VisibleByAdjacent, MapAreaFlags.VisibleByAdjacent.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & MapAreaFlags.Visited) == MapAreaFlags.Visited, MapAreaFlags.Visited.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}