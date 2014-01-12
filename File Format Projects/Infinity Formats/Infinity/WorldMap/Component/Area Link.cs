using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap.Component
{
    /// <summary>Links areas on the world map</summary>
    public class AreaLink : IInfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 216;
        #endregion


        #region Fields
        /// <summary>The target area on the map</summary>
        public UInt32 AreaIndex { get; set; }

        /// <summary>Name of the entry point on the destination area to appear at</summary>
        public ZString Entrance { get; set; }

        /// <summary>The weight of the travel time (in multiples of 4 hours) that it takes to get to the destination</summary>
        public UInt32 TravelWeight { get; set; }

        /// <summary>The side of the destination area to spawn on if no entrance point specified</summary>
        public EntryFlags DestinationEntry { get; set; }

        /// <summary>First possible random encounter area</summary>
        public ResourceReference RandomEncounterArea1 { get; set; }

        /// <summary>Second possible random encounter area</summary>
        public ResourceReference RandomEncounterArea2 { get; set; }

        /// <summary>Third possible random encounter area</summary>
        public ResourceReference RandomEncounterArea3 { get; set; }

        /// <summary>Fourth possible random encounter area</summary>
        public ResourceReference RandomEncounterArea4 { get; set; }

        /// <summary>Fifth possible random encounter area</summary>
        public ResourceReference RandomEncounterArea5 { get; set; }

        /// <summary>The probability of a random encounter using this area link</summary>
        /// <remarks>Some of these have a very large probability, esp. in BG2. To area links get disabled somehow?</remarks>
        public UInt32 RandomEncounterProbability { get; set; }

        /// <summary>128 bytes of reserved data that are currently unused</summary>
        public Byte[] Reserved { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Entrance = new ZString();
            this.RandomEncounterArea1 = new ResourceReference(ResourceType.Area);
            this.RandomEncounterArea2 = new ResourceReference(ResourceType.Area);
            this.RandomEncounterArea3 = new ResourceReference(ResourceType.Area);
            this.RandomEncounterArea4 = new ResourceReference(ResourceType.Area);
            this.RandomEncounterArea5 = new ResourceReference(ResourceType.Area);
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
            Byte[] buffer = ReusableIO.BinaryRead(input, (AreaLink.StructSize - 128));

            this.AreaIndex = ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.Entrance.Source = ReusableIO.ReadStringFromByteArray(buffer, 4, CultureConstants.CultureCodeEnglish, 32);
            this.TravelWeight = ReusableIO.ReadUInt32FromArray(buffer, 36);
            this.DestinationEntry = (EntryFlags)(ReusableIO.ReadUInt32FromArray(buffer, 40));
            this.RandomEncounterArea1.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 44, CultureConstants.CultureCodeEnglish);
            this.RandomEncounterArea2.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 52, CultureConstants.CultureCodeEnglish);
            this.RandomEncounterArea3.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 60, CultureConstants.CultureCodeEnglish);
            this.RandomEncounterArea4.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 68, CultureConstants.CultureCodeEnglish);
            this.RandomEncounterArea5.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 76, CultureConstants.CultureCodeEnglish);
            this.RandomEncounterProbability = ReusableIO.ReadUInt32FromArray(buffer, 84);
            this.Reserved = ReusableIO.BinaryRead(input, 128);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.AreaIndex, output);
            ReusableIO.WriteStringToStream(this.Entrance.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.TravelWeight, output);
            ReusableIO.WriteUInt32ToStream((UInt32)(this.DestinationEntry), output);
            ReusableIO.WriteStringToStream(this.RandomEncounterArea1.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.RandomEncounterArea2.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.RandomEncounterArea3.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.RandomEncounterArea4.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.RandomEncounterArea5.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.RandomEncounterProbability, output);
            output.Write(this.Reserved, 0, 128);
        }
        #endregion


        #region Hash code and comparison
        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.AreaIndex.GetHashCode();
            hash ^= this.Entrance.GetHashCode();
            hash ^= this.TravelWeight.GetHashCode();
            hash ^= this.DestinationEntry.GetHashCode();
            hash ^= this.RandomEncounterArea1.GetHashCode();
            hash ^= this.RandomEncounterArea2.GetHashCode();
            hash ^= this.RandomEncounterArea3.GetHashCode();
            hash ^= this.RandomEncounterArea4.GetHashCode();
            hash ^= this.RandomEncounterArea5.GetHashCode();
            hash ^= this.RandomEncounterProbability.GetHashCode();
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
            builder.Append(String.Format("Area Link # {0}", index));
            builder.Append(StringFormat.ToStringAlignment("Area Index"));
            builder.Append(this.AreaIndex);
            builder.Append(StringFormat.ToStringAlignment("Entrance"));
            builder.Append(String.Format("'{0}'", this.Entrance.Value));
            builder.Append(StringFormat.ToStringAlignment("Travel time (multiples of 4 hours)"));
            builder.Append(this.TravelWeight);
            builder.Append(StringFormat.ToStringAlignment("Destination entry flags (value)"));
            builder.Append((UInt32)this.DestinationEntry);
            builder.Append(StringFormat.ToStringAlignment("Destination entry flags (enumerated)"));
            builder.Append(this.GenerateAreaEntryFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Random Encounter Area 1"));
            builder.Append(String.Format("'{0}'", this.RandomEncounterArea1.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Random Encounter Area 2"));
            builder.Append(String.Format("'{0}'", this.RandomEncounterArea2.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Random Encounter Area 3"));
            builder.Append(String.Format("'{0}'", this.RandomEncounterArea3.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Random Encounter Area 4"));
            builder.Append(String.Format("'{0}'", this.RandomEncounterArea4.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Random Encounter Area 5"));
            builder.Append(String.Format("'{0}'", this.RandomEncounterArea5.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Random Encounter Probability"));
            builder.Append(this.RandomEncounterProbability);
            builder.Append(StringFormat.ToStringAlignment("Reserved"));
            builder.AppendLine(StringFormat.ByteArrayToHexString(this.Reserved));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable representation of the MapAreaFlags enumerator</summary>
        /// <returns>A human-readable representation of the MapAreaFlags enumerator</returns>
        protected String GenerateAreaEntryFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.DestinationEntry & EntryFlags.North) == EntryFlags.North, EntryFlags.North.GetDescription());
            StringFormat.AppendSubItem(sb, (this.DestinationEntry & EntryFlags.South) == EntryFlags.South, EntryFlags.South.GetDescription());
            StringFormat.AppendSubItem(sb, (this.DestinationEntry & EntryFlags.East) == EntryFlags.East, EntryFlags.East.GetDescription());
            StringFormat.AppendSubItem(sb, (this.DestinationEntry & EntryFlags.West) == EntryFlags.West, EntryFlags.West.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}