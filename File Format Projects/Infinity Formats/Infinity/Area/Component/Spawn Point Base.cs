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
    /// <summary>Base class for spawn points</summary>
    public abstract class SpawnPointBase : IInfinityFormat, ISpawn
    {
        #region Constants
        /// <summary>Size of this structure on disk</summary>
        public const Int32 StructSize = 200;
        #endregion


        #region Fields
        /// <summary>Spawn point's name</summary>
        /// <remarks>32 bytes in length</remarks>
        public ZString Name { get; set; }

        /// <summary>Location of the spawn point</summary>
        public Point Location { get; set; }

        /// <summary>Array of creatures to spawn</summary>
        public ResourceReference[] Creatures { get; set; }

        /// <summary>Count of spawn creatures</summary>
        public UInt16 CountCreatures { get; set; }

        /// <summary>Base count of creatures to spawn</summary>
        public UInt16 BaseSpawnCount { get; set; }

        /// <summary>Frequency (in seconds) between spawning</summary>
        public UInt16 Frequency { get; set; }

        /* spawn method/flags */

        /// <summary>How long the spawn will exist (match to <see cref="Actor.RemovalTimer"/>)</summary>
        public Int32 Lifespan { get; set; }

        /// <summary>Uncertain</summary>
        public Int16 MovementRestrictionDistance { get; set; }

        /// <summary>Uncertain</summary>
        public Int16 MovementRestrictionDistanceToObject { get; set; }

        /// <summary>Maximum count of creatures to spawn</summary>
        public UInt16 MaximumSpawnCount { get; set; }

        /// <summary>Underlying source value for the Active Boolean</summary>
        public Int16 ActiveSourceValue { get; set; }

        /// <summary>Schedule flags during which this spawn point is valid</summary>
        public Schedule SpawnSchedule { get; set; }

        /// <summary>Probability of the spawn during the day</summary>
        public UInt16 ProbabilityDay { get; set; }

        /// <summary>Probability of the spawn during the night</summary>
        public UInt16 ProbabilityNight { get; set; }

        /// <summary>56 padding bytes at offset 0x90</summary>
        public Byte[] Padding_0x0090 { get; set; }
        #endregion


        #region Properties
        /// <summary>Is the spawn point active?</summary>
        public Boolean Active
        {
            get { return Convert.ToBoolean(this.ActiveSourceValue); }
            set { this.ActiveSourceValue = Convert.ToInt16(value); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.Location = new Point();
            this.Creatures = new ResourceReference[10];
            for (Int32 index = 0; index < 10; ++index)
                this.Creatures[index] = new ResourceReference(ResourceType.Creature);
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 122);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Location.X = ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.Location.Y = ReusableIO.ReadUInt16FromArray(buffer, 34);

            for (Int32 index = 0; index < 10; ++index)
                this.Creatures[index].ResRef = ReusableIO.ReadStringFromByteArray(buffer, 36 + (8 * index), CultureConstants.CultureCodeEnglish);

            this.CountCreatures = ReusableIO.ReadUInt16FromArray(buffer, 116);
            this.BaseSpawnCount = ReusableIO.ReadUInt16FromArray(buffer, 118);
            this.Frequency = ReusableIO.ReadUInt16FromArray(buffer, 120);

            this.ReadSpawnMethod(input);

            buffer = ReusableIO.BinaryRead(input, 20);

            this.Lifespan = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.MovementRestrictionDistance = ReusableIO.ReadInt16FromArray(buffer, 4);
            this.MovementRestrictionDistanceToObject = ReusableIO.ReadInt16FromArray(buffer, 6);
            this.MaximumSpawnCount = ReusableIO.ReadUInt16FromArray(buffer, 8);
            this.ActiveSourceValue = ReusableIO.ReadInt16FromArray(buffer, 10);
            this.SpawnSchedule = (Schedule)ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.ProbabilityDay = ReusableIO.ReadUInt16FromArray(buffer, 16);
            this.ProbabilityNight = ReusableIO.ReadUInt16FromArray(buffer, 18);

            this.Padding_0x0090 = ReusableIO.BinaryRead(input, 56);
        }

        /// <summary>Reads the spawn method from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        public abstract void ReadSpawnMethod(Stream input);

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.Location.X, output);
            ReusableIO.WriteUInt16ToStream(this.Location.Y, output);

            for (Int32 index = 0; index < 10; ++index)
                ReusableIO.WriteStringToStream(this.Creatures[index].ResRef, output, CultureConstants.CultureCodeEnglish);

            ReusableIO.WriteUInt16ToStream(this.CountCreatures, output);
            ReusableIO.WriteUInt16ToStream(this.BaseSpawnCount, output);
            ReusableIO.WriteUInt16ToStream(this.Frequency, output);

            this.WriteSpawnMethod(output);

            ReusableIO.WriteInt32ToStream(this.Lifespan, output);
            ReusableIO.WriteInt16ToStream(this.MovementRestrictionDistance, output);
            ReusableIO.WriteInt16ToStream(this.MovementRestrictionDistanceToObject, output);
            ReusableIO.WriteUInt16ToStream(this.MaximumSpawnCount, output);
            ReusableIO.WriteInt16ToStream(this.ActiveSourceValue, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.SpawnSchedule, output);
            ReusableIO.WriteUInt16ToStream(this.ProbabilityDay, output);
            ReusableIO.WriteUInt16ToStream(this.ProbabilityNight, output);
            output.Write(this.Padding_0x0090, 0, 56);
        }

        /// <summary>Writes the spawn method to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        public abstract void WriteSpawnMethod(Stream output);
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
            return StringFormat.ReturnAndIndent(String.Format("Spawn point # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Spawn point:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Location"));
            builder.Append(this.Location.ToString());

            for (Int32 index = 0; index < 10; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Creature # {0}", index)));
                builder.Append(String.Format("'{0}'", this.Creatures[index].ZResRef));
            }

            builder.Append(StringFormat.ToStringAlignment("Creature count"));
            builder.Append(this.CountCreatures);
            builder.Append(StringFormat.ToStringAlignment("Base spawn count"));
            builder.Append(this.BaseSpawnCount);
            builder.Append(StringFormat.ToStringAlignment("Frequency"));
            builder.Append(this.Frequency);

            builder.Append(this.GenerateSpawnMethodString());

            builder.Append(StringFormat.ToStringAlignment("Spawns' lifetime"));
            builder.Append(this.Lifespan);
            builder.Append(StringFormat.ToStringAlignment("Movement restriction distance"));
            builder.Append(this.MovementRestrictionDistance);
            builder.Append(StringFormat.ToStringAlignment("Movement restriction distance to object"));
            builder.Append(this.MovementRestrictionDistanceToObject);
            builder.Append(StringFormat.ToStringAlignment("Maximum spawn count"));
            builder.Append(this.MaximumSpawnCount);
            builder.Append(StringFormat.ToStringAlignment("Spawn point active"));
            builder.Append(this.Active);
            builder.Append(StringFormat.ToStringAlignment("Spawn Schedule (value)"));
            builder.Append((UInt32)this.SpawnSchedule);
            builder.Append(StringFormat.ToStringAlignment("Spawn Schedule (enumeration)"));
            builder.Append(this.GetScheduleFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Spawn probability during day"));
            builder.Append(this.ProbabilityDay);
            builder.Append(StringFormat.ToStringAlignment("Spawn probability during night"));
            builder.Append(this.ProbabilityNight);
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0090));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representing the spawn method</summary>
        /// <returns>A human-readable String representing the spawn method</returns>
        protected abstract String GenerateSpawnMethodString();

        /// <summary>Gets a human-readable enumeration String of set SpawnSchedule enumeration values</summary>
        /// <returns>A human-readable enumeration String of set SpawnSchedule enumeration values</returns>
        protected String GetScheduleFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_00_30_to_01_30) == Schedule.Time_00_30_to_01_30, Schedule.Time_00_30_to_01_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_01_30_to_02_30) == Schedule.Time_01_30_to_02_30, Schedule.Time_01_30_to_02_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_02_30_to_03_30) == Schedule.Time_02_30_to_03_30, Schedule.Time_02_30_to_03_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_03_30_to_04_30) == Schedule.Time_03_30_to_04_30, Schedule.Time_03_30_to_04_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_04_30_to_05_30) == Schedule.Time_04_30_to_05_30, Schedule.Time_04_30_to_05_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_05_30_to_06_30) == Schedule.Time_05_30_to_06_30, Schedule.Time_05_30_to_06_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_06_30_to_07_30) == Schedule.Time_06_30_to_07_30, Schedule.Time_06_30_to_07_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_07_30_to_08_30) == Schedule.Time_07_30_to_08_30, Schedule.Time_07_30_to_08_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_08_30_to_09_30) == Schedule.Time_08_30_to_09_30, Schedule.Time_08_30_to_09_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_09_30_to_10_30) == Schedule.Time_09_30_to_10_30, Schedule.Time_09_30_to_10_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_10_30_to_11_30) == Schedule.Time_10_30_to_11_30, Schedule.Time_10_30_to_11_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_11_30_to_12_30) == Schedule.Time_11_30_to_12_30, Schedule.Time_11_30_to_12_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_12_30_to_13_30) == Schedule.Time_12_30_to_13_30, Schedule.Time_12_30_to_13_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_13_30_to_14_30) == Schedule.Time_13_30_to_14_30, Schedule.Time_13_30_to_14_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_14_30_to_15_30) == Schedule.Time_14_30_to_15_30, Schedule.Time_14_30_to_15_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_15_30_to_16_30) == Schedule.Time_15_30_to_16_30, Schedule.Time_15_30_to_16_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_16_30_to_17_30) == Schedule.Time_16_30_to_17_30, Schedule.Time_16_30_to_17_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_17_30_to_18_30) == Schedule.Time_17_30_to_18_30, Schedule.Time_17_30_to_18_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_18_30_to_19_30) == Schedule.Time_18_30_to_19_30, Schedule.Time_18_30_to_19_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_19_30_to_20_30) == Schedule.Time_19_30_to_20_30, Schedule.Time_19_30_to_20_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_20_30_to_21_30) == Schedule.Time_20_30_to_21_30, Schedule.Time_20_30_to_21_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_21_30_to_22_30) == Schedule.Time_21_30_to_22_30, Schedule.Time_21_30_to_22_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_22_30_to_23_30) == Schedule.Time_22_30_to_23_30, Schedule.Time_22_30_to_23_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.SpawnSchedule & Schedule.Time_23_30_to_00_30) == Schedule.Time_23_30_to_00_30, Schedule.Time_23_30_to_00_30.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}