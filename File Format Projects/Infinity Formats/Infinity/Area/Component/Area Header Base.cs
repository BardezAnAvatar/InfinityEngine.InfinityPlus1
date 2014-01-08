using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Base class for ARE file header</summary>
    public abstract class AreaHeaderBase : InfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 284;
        #endregion


        #region Fields
        /// <summary>Related WED resource</summary>
        public ResourceReference Wed { get; set; }

        /// <summary>Time, in real seconds</summary>
        public UInt32 LastSaved { get; set; }

        /* AREA FLAGS */

        /// <summary>Resref to area north of this area</summary>
        public ResourceReference NorthArea { get; set; }

        /// <summary>Area's north edge flags</summary>
        public AreaEdgeFlags NorthEdgeFlags { get; set; }

        /// <summary>Resref to area north of this area</summary>
        public ResourceReference EastArea { get; set; }

        /// <summary>Area's east edge flags</summary>
        public AreaEdgeFlags EastEdgeFlags { get; set; }

        /// <summary>Resref to area north of this area</summary>
        public ResourceReference SouthArea { get; set; }

        /// <summary>Area's south edge flags</summary>
        public AreaEdgeFlags SouthEdgeFlags { get; set; }

        /// <summary>Resref to area north of this area</summary>
        public ResourceReference WestArea { get; set; }

        /// <summary>Area's west edge flags</summary>
        public AreaEdgeFlags WestEdgeFlags { get; set; }

        /* Area Type */

        /// <summary>Probability that it will rain</summary>
        public Int16 ProbabilityRain { get; set; }

        /// <summary>Probability that it will snow</summary>
        public Int16 ProbabilitySnow { get; set; }

        /// <summary>Probability that it will fog</summary>
        public Int16 ProbabilityFog { get; set; }

        /// <summary>Probability that lightning will strike</summary>
        public Int16 ProbabilityLightning { get; set; }

        /// <summary>Base wind speed (not used)</summary>
        /// <remarks>BGEE devs say this is base wind speed (no effect?)</remarks>
        public Int16 BaseWindSpeed { get; set; }

        /// <summary>Offset within this file to actors</summary>
        public Int32 OffsetActors { get; set; }

        /// <summary>Count of actors within this file</summary>
        public Int16 CountActors { get; set; }

        /// <summary>Count of regions within this file</summary>
        public Int16 CountRegions { get; set; }

        /// <summary>Offset within this file to regions</summary>
        public Int32 OffsetRegions { get; set; }

        /// <summary>Offset within this file to spawn points</summary>
        public Int32 OffsetSpawnPoints { get; set; }

        /// <summary>Count of spawn points within this file</summary>
        public Int32 CountSpawnPoints { get; set; }

        /// <summary>Offset within this file to entrances</summary>
        public Int32 OffsetEntrances { get; set; }

        /// <summary>Count of entrances within this file</summary>
        public Int32 CountEntrances { get; set; }

        /// <summary>Offset within this file to containers</summary>
        public Int32 OffsetContainers { get; set; }

        /// <summary>Count of containers within this file</summary>
        public Int16 CountContainers { get; set; }

        /// <summary>Count of items within this file</summary>
        public Int16 CountItems { get; set; }

        /// <summary>Offset within this file to items</summary>
        public Int32 OffsetItems { get; set; }

        /// <summary>Offset within this file to vertices</summary>
        public Int32 OffsetVertices { get; set; }

        /// <summary>Count of vertices within this file</summary>
        public Int16 CountVertices { get; set; }

        /// <summary>Count of ambients within this file</summary>
        public Int16 CountAmbients { get; set; }

        /// <summary>Offset within this file to ambients</summary>
        public Int32 OffsetAmbients { get; set; }

        /// <summary>Offset within this file to variables</summary>
        public Int32 OffsetVariables { get; set; }

        /// <summary>Count of variables within this file</summary>
        public Int32 CountVariables { get; set; }

        /// <summary>Offset to tiled object flags</summary>
        public UInt16 OffsetTiledObjectFlags { get; set; }

        /// <summary>Count of tiled object flags within this file</summary>
        public UInt16 CountTiledObjectFlags { get; set; }

        /// <summary>Unknown at offset 0x90</summary>
        public Int32 Unknown_0x0090 { get; set; }

        /// <summary>Area script to run</summary>
        public ResourceReference AreaScript { get; set; }

        /// <summary>Size of area explored bitmask within this file</summary>
        public Int32 SizeExploredBitmask { get; set; }

        /// <summary>Offset within this file to explored bitmask</summary>
        public Int32 OffsetExploredBitmask { get; set; }

        /// <summary>Count of doors within this file</summary>
        public Int32 CountDoors { get; set; }

        /// <summary>Offset within this file to doors</summary>
        public Int32 OffsetDoors { get; set; }

        /// <summary>Count of animations within this file</summary>
        public Int32 CountAnimations { get; set; }

        /// <summary>Offset within this file to animations</summary>
        public Int32 OffsetAnimations { get; set; }

        /// <summary>Count of tiled objects within this file</summary>
        public Int32 CountTiledObjects { get; set; }

        /// <summary>Offset within this file to tiled objects</summary>
        public Int32 OffsetTiledObjects { get; set; }

        /// <summary>Offset within this file to song entries</summary>
        public Int32 OffsetSongEntries { get; set; }

        /// <summary>Offset within this file to rest interruptions</summary>
        public Int32 OffsetRestInterruptions { get; set; }

        /// <summary>Offset within this file to map notes</summary>
        public Int32 OffsetMapNote { get; set; }

        /// <summary>Count of map notes within this file</summary>
        public Int32 CountMapNote { get; set; }

        /// <summary>Offset within this file to projectile traps</summary>
        public Int32 OffsetProjectileTrap { get; set; }

        /// <summary>Count of projectile traps notes within this file</summary>
        public Int32 CountProjectileTrap { get; set; }

        /// <summary>Movie to play when resting during the day</summary>
        public ResourceReference RestMovieDay { get; set; }

        /// <summary>Movie to play when resting during the night</summary>
        public ResourceReference RestMovieNight { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.Wed = new ResourceReference(ResourceType.Wed);
            this.NorthArea = new ResourceReference(ResourceType.Area);
            this.EastArea = new ResourceReference(ResourceType.Area);
            this.SouthArea = new ResourceReference(ResourceType.Area);
            this.WestArea = new ResourceReference(ResourceType.Area);
            this.AreaScript = new ResourceReference();
            this.RestMovieDay = new ResourceReference();
            this.RestMovieNight = new ResourceReference();
        }
        #endregion


        #region IO method implemetations
        #region Read
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            //read first data groups
            this.ReadLeadingData(input);

            //offset group
            this.ReadOffsetGroup1(input);

            //offset group #3
            this.ReadTrailingData(input);

            //read trailing padding bytes
            this.ReadTrailingPadding(input);
        }

        /// <summary>Reads the area flags value from the Byte buffer</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected abstract void ReadAreaFlags(Byte[] buffer);

        /// <summary>Reads the area type flags value from the Byte buffer array</summary>
        /// <param name="buffer">Byte array to read value from</param>
        protected abstract void ReadAreaTypeFlags(Byte[] buffer);

        /// <summary>Reads the leading data from the input stream</summary>
        /// <param name="input">Input Stream to read from</param>
        protected void ReadLeadingData(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 76);

            this.Wed.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.LastSaved = ReusableIO.ReadUInt32FromArray(buffer, 8);

            this.ReadAreaFlags(buffer);

            this.NorthArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 16, CultureConstants.CultureCodeEnglish);
            this.NorthEdgeFlags = (AreaEdgeFlags)(ReusableIO.ReadUInt32FromArray(buffer, 24));
            this.EastArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 28, CultureConstants.CultureCodeEnglish);
            this.EastEdgeFlags = (AreaEdgeFlags)(ReusableIO.ReadUInt32FromArray(buffer, 36));
            this.SouthArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 40, CultureConstants.CultureCodeEnglish);
            this.SouthEdgeFlags = (AreaEdgeFlags)(ReusableIO.ReadUInt32FromArray(buffer, 48));
            this.WestArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 52, CultureConstants.CultureCodeEnglish);
            this.WestEdgeFlags = (AreaEdgeFlags)(ReusableIO.ReadUInt32FromArray(buffer, 60));

            this.ReadAreaTypeFlags(buffer);

            this.ProbabilityRain = ReusableIO.ReadInt16FromArray(buffer, 66);
            this.ProbabilitySnow = ReusableIO.ReadInt16FromArray(buffer, 68);
            this.ProbabilityFog = ReusableIO.ReadInt16FromArray(buffer, 70);
            this.ProbabilityLightning = ReusableIO.ReadInt16FromArray(buffer, 72);
            this.BaseWindSpeed = ReusableIO.ReadInt16FromArray(buffer, 74);
        }

        /// <summary>Reads the first and second groups of offsets into the header</summary>
        /// <param name="input">Input Stream to read from</param>
        protected void ReadOffsetGroup1(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 112);

            //offset group #1
            this.OffsetActors = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.CountActors = ReusableIO.ReadInt16FromArray(buffer, 4);
            this.CountRegions = ReusableIO.ReadInt16FromArray(buffer, 6);
            this.OffsetRegions = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.OffsetSpawnPoints = ReusableIO.ReadInt32FromArray(buffer, 12);
            this.CountSpawnPoints = ReusableIO.ReadInt32FromArray(buffer, 16);
            this.OffsetEntrances = ReusableIO.ReadInt32FromArray(buffer, 20);
            this.CountEntrances = ReusableIO.ReadInt32FromArray(buffer, 24);
            this.OffsetContainers = ReusableIO.ReadInt32FromArray(buffer, 28);
            this.CountContainers = ReusableIO.ReadInt16FromArray(buffer, 32);
            this.CountItems = ReusableIO.ReadInt16FromArray(buffer, 34);
            this.OffsetItems = ReusableIO.ReadInt32FromArray(buffer, 36);
            this.OffsetVertices = ReusableIO.ReadInt32FromArray(buffer, 40);
            this.CountVertices = ReusableIO.ReadInt16FromArray(buffer, 44);
            this.CountAmbients = ReusableIO.ReadInt16FromArray(buffer, 46);
            this.OffsetAmbients = ReusableIO.ReadInt32FromArray(buffer, 48);
            this.OffsetVariables = ReusableIO.ReadInt32FromArray(buffer, 52);
            this.CountVariables = ReusableIO.ReadInt32FromArray(buffer, 56);
            this.OffsetTiledObjectFlags = ReusableIO.ReadUInt16FromArray(buffer, 60);
            this.CountTiledObjectFlags = ReusableIO.ReadUInt16FromArray(buffer, 62);

            //area script
            this.AreaScript.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 64, CultureConstants.CultureCodeEnglish);

            //offset group #2
            this.SizeExploredBitmask = ReusableIO.ReadInt32FromArray(buffer, 72);
            this.OffsetExploredBitmask = ReusableIO.ReadInt32FromArray(buffer, 76);
            this.CountDoors = ReusableIO.ReadInt32FromArray(buffer, 80);
            this.OffsetDoors = ReusableIO.ReadInt32FromArray(buffer, 84);
            this.CountAnimations = ReusableIO.ReadInt32FromArray(buffer, 88);
            this.OffsetAnimations = ReusableIO.ReadInt32FromArray(buffer, 92);
            this.CountTiledObjects = ReusableIO.ReadInt32FromArray(buffer, 96);
            this.OffsetTiledObjects = ReusableIO.ReadInt32FromArray(buffer, 100);
            this.OffsetSongEntries = ReusableIO.ReadInt32FromArray(buffer, 104);
            this.OffsetRestInterruptions = ReusableIO.ReadInt32FromArray(buffer, 108);
        }

        /// <summary>Reads the trailing offsets and movie data from the input stream</summary>
        /// <param name="input">Input Stream to read from</param>
        protected void ReadTrailingData(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 32);

            this.OffsetMapNote = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.CountMapNote = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.OffsetProjectileTrap = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.CountProjectileTrap = ReusableIO.ReadInt32FromArray(buffer, 12);
            this.RestMovieDay.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 16, CultureConstants.CultureCodeEnglish);
            this.RestMovieNight.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 24, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Reads the trailing padding bytes for the header from the input stream</summary>
        /// <param name="input">Input Stream to read from</param>
        protected abstract void ReadTrailingPadding(Stream input);
        #endregion


        #region Write
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            this.WriteLeadingData(output);
            this.WriteOffsetGroup1(output);
            this.WriteTrailingData(output);
            this.WriteTrailingPadding(output);
        }

        /// <summary>Writes the area flags to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected abstract void WriteAreaFlags(Stream output);

        /// <summary>Writes the area type flags to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected abstract void WriteAreaTypeFlags(Stream output);

        /// <summary>Writes the leading common data of the header to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteLeadingData(Stream output)
        {
            //signature and version
            base.Write(output);

            ReusableIO.WriteStringToStream(this.Wed.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.LastSaved, output);
            this.WriteAreaFlags(output);
            ReusableIO.WriteStringToStream(this.NorthArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)(this.NorthEdgeFlags), output);
            ReusableIO.WriteStringToStream(this.EastArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)(this.EastEdgeFlags), output);
            ReusableIO.WriteStringToStream(this.SouthArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)(this.SouthEdgeFlags), output);
            ReusableIO.WriteStringToStream(this.WestArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)(this.WestEdgeFlags), output);
            this.WriteAreaTypeFlags(output);
            ReusableIO.WriteInt16ToStream(this.ProbabilityRain, output);
            ReusableIO.WriteInt16ToStream(this.ProbabilitySnow, output);
            ReusableIO.WriteInt16ToStream(this.ProbabilityFog, output);
            ReusableIO.WriteInt16ToStream(this.ProbabilityLightning, output);
            ReusableIO.WriteInt16ToStream(this.BaseWindSpeed, output);
        }

        /// <summary>Writes the first and second groups of offsets from the header</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteOffsetGroup1(Stream output)
        {
            //offset group #1
            ReusableIO.WriteInt32ToStream(this.OffsetActors, output);
            ReusableIO.WriteInt16ToStream(this.CountActors, output);
            ReusableIO.WriteInt16ToStream(this.CountRegions, output);
            ReusableIO.WriteInt32ToStream(this.OffsetRegions, output);
            ReusableIO.WriteInt32ToStream(this.OffsetSpawnPoints, output);
            ReusableIO.WriteInt32ToStream(this.CountSpawnPoints, output);
            ReusableIO.WriteInt32ToStream(this.OffsetEntrances, output);
            ReusableIO.WriteInt32ToStream(this.CountEntrances, output);
            ReusableIO.WriteInt32ToStream(this.OffsetContainers, output);
            ReusableIO.WriteInt16ToStream(this.CountContainers, output);
            ReusableIO.WriteInt16ToStream(this.CountItems, output);
            ReusableIO.WriteInt32ToStream(this.OffsetItems, output);
            ReusableIO.WriteInt32ToStream(this.OffsetVertices, output);
            ReusableIO.WriteInt16ToStream(this.CountVertices, output);
            ReusableIO.WriteInt16ToStream(this.CountAmbients, output);
            ReusableIO.WriteInt32ToStream(this.OffsetAmbients, output);
            ReusableIO.WriteInt32ToStream(this.OffsetVariables, output);
            ReusableIO.WriteInt32ToStream(this.CountVariables, output);
            ReusableIO.WriteUInt16ToStream(this.OffsetTiledObjectFlags, output);
            ReusableIO.WriteUInt16ToStream(this.CountTiledObjectFlags, output);

            //area script
            ReusableIO.WriteStringToStream(this.AreaScript.ResRef, output, CultureConstants.CultureCodeEnglish);

            //offset group #2
            ReusableIO.WriteInt32ToStream(this.SizeExploredBitmask, output);
            ReusableIO.WriteInt32ToStream(this.OffsetExploredBitmask, output);
            ReusableIO.WriteInt32ToStream(this.CountDoors, output);
            ReusableIO.WriteInt32ToStream(this.OffsetDoors, output);
            ReusableIO.WriteInt32ToStream(this.CountAnimations, output);
            ReusableIO.WriteInt32ToStream(this.OffsetAnimations, output);
            ReusableIO.WriteInt32ToStream(this.CountTiledObjects, output);
            ReusableIO.WriteInt32ToStream(this.OffsetTiledObjects, output);
            ReusableIO.WriteInt32ToStream(this.OffsetSongEntries, output);
            ReusableIO.WriteInt32ToStream(this.OffsetRestInterruptions, output);
        }

        /// <summary>Writes the trailing offsets and movie data to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTrailingData(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.OffsetMapNote, output);
            ReusableIO.WriteInt32ToStream(this.CountMapNote, output);
            ReusableIO.WriteInt32ToStream(this.OffsetProjectileTrap, output);
            ReusableIO.WriteInt32ToStream(this.CountProjectileTrap, output);
            ReusableIO.WriteStringToStream(this.RestMovieDay.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.RestMovieNight.ResRef, output, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Writes the trailing padding bytes for the header to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected abstract void WriteTrailingPadding(Stream output);
        #endregion
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(this.GenerateAreaDescriptionString());
            builder.AppendLine(this.GenerateLeadingHeaderString());
            builder.AppendLine(this.GenerateOffsetGroup1String());
            builder.AppendLine(this.GenerateOffsetGroup2String());
            builder.AppendLine(this.GenerateTrailingPaddingString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the type of the area</summary>
        /// <returns>A human-readable String representing the type of the area</returns>
        protected abstract String GenerateAreaDescriptionString();

        /// <summary>Generates a human-readable String representing the area flags and values</summary>
        /// <returns>A human-readable String representing the area flags and values</returns>
        protected abstract String GenerateAreaFlagsString();

        /// <summary>Generates a human-readable String representing the area type flags and values</summary>
        /// <returns>A human-readable String representing the area type flags and values</returns>
        protected abstract String GenerateAreaTypeFlagsString();

        /// <summary>Generates a human-readable String representing the leading header data types</summary>
        /// <returns>A human-readable String representing the leading header data types</returns>
        protected String GenerateLeadingHeaderString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Signature)));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", ZString.GetZString(this.Version)));
            builder.Append(StringFormat.ToStringAlignment("WED reference"));
            builder.Append(String.Format("'{0}'", this.Wed.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Last saved"));
            builder.Append(this.LastSaved);

            builder.Append(this.GenerateAreaFlagsString());

            builder.Append(StringFormat.ToStringAlignment("North Area"));
            builder.Append(String.Format("'{0}'", this.NorthArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("North-facing edge flags (value)"));
            builder.Append((UInt32)(this.NorthEdgeFlags));
            builder.Append(StringFormat.ToStringAlignment("North-facing edge flags (enumeration)"));
            builder.Append(this.GetEdgeFlagsEnumerationString(this.NorthEdgeFlags));
            builder.Append(StringFormat.ToStringAlignment("East Area"));
            builder.Append(String.Format("'{0}'", this.EastArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("East-facing edge flags (value)"));
            builder.Append((UInt32)(this.EastEdgeFlags));
            builder.Append(StringFormat.ToStringAlignment("East-facing edge flags (enumeration)"));
            builder.Append(this.GetEdgeFlagsEnumerationString(this.EastEdgeFlags));
            builder.Append(StringFormat.ToStringAlignment("South Area"));
            builder.Append(String.Format("'{0}'", this.SouthArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("South-facing edge flags (value)"));
            builder.Append((UInt32)(this.SouthEdgeFlags));
            builder.Append(StringFormat.ToStringAlignment("South-facing edge flags (enumeration)"));
            builder.Append(this.GetEdgeFlagsEnumerationString(this.SouthEdgeFlags));
            builder.Append(StringFormat.ToStringAlignment("West Area"));
            builder.Append(String.Format("'{0}'", this.WestArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("West-facing edge flags (value)"));
            builder.Append((UInt32)(this.WestEdgeFlags));
            builder.Append(StringFormat.ToStringAlignment("West-facing edge flags (enumeration)"));
            builder.Append(this.GetEdgeFlagsEnumerationString(this.WestEdgeFlags));

            builder.Append(this.GenerateAreaTypeFlagsString());

            builder.Append(StringFormat.ToStringAlignment("Probability of rain"));
            builder.Append(this.ProbabilityRain);
            builder.Append(StringFormat.ToStringAlignment("Probability of snow"));
            builder.Append(this.ProbabilitySnow);
            builder.Append(StringFormat.ToStringAlignment("Probability of fog"));
            builder.Append(this.ProbabilityFog);
            builder.Append(StringFormat.ToStringAlignment("Probability of lightning"));
            builder.Append(this.ProbabilityLightning);
            builder.Append(StringFormat.ToStringAlignment("Base Wind Speed (value not used)"));
            builder.Append(this.BaseWindSpeed);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the first groups of offsets</summary>
        /// <returns>A human-readable String representing the first groups of offsets</returns>
        protected String GenerateOffsetGroup1String()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Offset to actors"));
            builder.Append(this.OffsetActors);
            builder.Append(StringFormat.ToStringAlignment("Count of actors"));
            builder.Append(this.CountActors);
            builder.Append(StringFormat.ToStringAlignment("Count of regions"));
            builder.Append(this.CountRegions);
            builder.Append(StringFormat.ToStringAlignment("Offset to regions"));
            builder.Append(this.OffsetRegions);
            builder.Append(StringFormat.ToStringAlignment("Offset to spawn points"));
            builder.Append(this.OffsetSpawnPoints);
            builder.Append(StringFormat.ToStringAlignment("Count of spawn points"));
            builder.Append(this.CountSpawnPoints);
            builder.Append(StringFormat.ToStringAlignment("Offset to actors"));
            builder.Append(this.OffsetActors);
            builder.Append(StringFormat.ToStringAlignment("Count of actors"));
            builder.Append(this.CountActors);
            builder.Append(StringFormat.ToStringAlignment("Offset to entrances"));
            builder.Append(this.OffsetEntrances);
            builder.Append(StringFormat.ToStringAlignment("Count of entrances"));
            builder.Append(this.CountEntrances);
            builder.Append(StringFormat.ToStringAlignment("Offset to containers"));
            builder.Append(this.OffsetContainers);
            builder.Append(StringFormat.ToStringAlignment("Count of containers"));
            builder.Append(this.CountContainers);
            builder.Append(StringFormat.ToStringAlignment("Count of items"));
            builder.Append(this.CountItems);
            builder.Append(StringFormat.ToStringAlignment("Offset to items"));
            builder.Append(this.OffsetItems);
            builder.Append(StringFormat.ToStringAlignment("Offset to vertices"));
            builder.Append(this.OffsetVertices);
            builder.Append(StringFormat.ToStringAlignment("Count of vertices"));
            builder.Append(this.CountVertices);
            builder.Append(StringFormat.ToStringAlignment("Count of ambient sounds"));
            builder.Append(this.CountAmbients);
            builder.Append(StringFormat.ToStringAlignment("Offset to ambient sounds"));
            builder.Append(this.OffsetAmbients);
            builder.Append(StringFormat.ToStringAlignment("Offset to variables"));
            builder.Append(this.OffsetVariables);
            builder.Append(StringFormat.ToStringAlignment("Count of variables"));
            builder.Append(this.CountVariables);
            builder.Append(StringFormat.ToStringAlignment("Offset to tiled object flags"));
            builder.Append(this.OffsetTiledObjectFlags);
            builder.Append(StringFormat.ToStringAlignment("Count of tiled object flags"));
            builder.Append(this.CountTiledObjectFlags);


            builder.Append(StringFormat.ToStringAlignment("Area script"));
            builder.Append(String.Format("'{0}'", this.AreaScript.ZResRef));


            builder.Append(StringFormat.ToStringAlignment("Size of explored bitmask"));
            builder.Append(this.SizeExploredBitmask);
            builder.Append(StringFormat.ToStringAlignment("Offset to explored bitmask"));
            builder.Append(this.OffsetExploredBitmask);
            builder.Append(StringFormat.ToStringAlignment("Count of doors"));
            builder.Append(this.CountDoors);
            builder.Append(StringFormat.ToStringAlignment("Offset to doors"));
            builder.Append(this.OffsetDoors);
            builder.Append(StringFormat.ToStringAlignment("Count of animations"));
            builder.Append(this.CountAnimations);
            builder.Append(StringFormat.ToStringAlignment("Offset to animations"));
            builder.Append(this.OffsetAnimations);
            builder.Append(StringFormat.ToStringAlignment("Count of tiled objects"));
            builder.Append(this.CountTiledObjects);
            builder.Append(StringFormat.ToStringAlignment("Offset to tiled objects"));
            builder.Append(this.OffsetTiledObjects);
            builder.Append(StringFormat.ToStringAlignment("Offset to song entries"));
            builder.Append(this.OffsetSongEntries);
            builder.Append(StringFormat.ToStringAlignment("Offset to rest interruptions"));
            builder.Append(this.OffsetRestInterruptions);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the trailing groups of offsets</summary>
        /// <returns>A human-readable String representing the trailing groups of offsets</returns>
        protected String GenerateOffsetGroup2String()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Offset to map notes"));
            builder.Append(this.OffsetMapNote);
            builder.Append(StringFormat.ToStringAlignment("Count of map notes"));
            builder.Append(this.CountMapNote);
            builder.Append(StringFormat.ToStringAlignment("Offset to traps"));
            builder.Append(this.OffsetProjectileTrap);
            builder.Append(StringFormat.ToStringAlignment("Count of traps"));
            builder.Append(this.CountProjectileTrap);
            
            builder.Append(StringFormat.ToStringAlignment("Daytime rest movie"));
            builder.Append(String.Format("'{0}'", this.RestMovieDay.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Nighttime rest movie"));
            builder.Append(String.Format("'{0}'", this.RestMovieNight.ZResRef));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the trailing padding of the header</summary>
        /// <returns>A human-readable String representing the trailing padding of the header</returns>
        protected abstract String GenerateTrailingPaddingString();

        /// <summary>Gets a human-readable enumeration String of AreaEdgeFlag enumeration values</summary>
        /// <returns>A human-readable enumeration String of AreaEdgeFlag enumeration values</returns>
        protected String GetEdgeFlagsEnumerationString(AreaEdgeFlags flags)
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (flags & AreaEdgeFlags.PartyRequired) == AreaEdgeFlags.PartyRequired, AreaEdgeFlags.PartyRequired.GetDescription());
            StringFormat.AppendSubItem(builder, (flags & AreaEdgeFlags.PartyEnabled) == AreaEdgeFlags.PartyEnabled, AreaEdgeFlags.PartyEnabled.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}