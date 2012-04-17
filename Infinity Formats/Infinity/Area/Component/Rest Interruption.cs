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
    /// <summary>Represents a single interruption from rest</summary>
    public class RestInterruption : IInfinityFormat, ISpawn
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 228;
        #endregion


        #region Fields
        /// <summary>Name of this rest interruption; 32 characters long</summary>
        public ZString Name { get; set; }

        /// <summary>Array of text to display when awoken</summary>
        public StringReference[] AwakenText { get; set; }

        /// <summary>Array of creatures spawned when awoken</summary>
        public ResourceReference[] Creatures { get; set; }

        /// <summary>Count of creatures spawned when awoken</summary>
        public UInt16 CountCreatures { get; set; }

        /// <summary>Difficulty of the spawn</summary>
        /// <remarks>if (Average Party Level * Difficulty) > (Total Creature Power), then spawn</remarks>
        public Int16 Difficulty { get; set; }

        /// <summary>Time, in seconds, that the spawned creatures survive</summary>
        public Int32 Lifespan { get; set; }

        /// <summary>Uncertain</summary>
        public Int16 MovementRestrictionDistance { get; set; }

        /// <summary>Uncertain</summary>
        public Int16 MovementRestrictionDistanceToObject { get; set; }

        /// <summary>Maximum count of creatures to spawn</summary>
        public UInt16 MaximumSpawnCount { get; set; }

        /// <summary>Is the spawn point active?</summary>
        public Boolean Active { get; set; }

        /// <summary>Probability of the spawn during the day</summary>
        public UInt16 ProbabilityDay { get; set; }

        /// <summary>Probability of the spawn during the night</summary>
        public UInt16 ProbabilityNight { get; set; }

        /// <summary>56 padding bytes at offset 0xAC</summary>
        public Byte[] Padding_0x00AC { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();

            this.AwakenText = new StringReference[10];
            for (Int32 index = 0; index < 10; ++index)
                this.AwakenText[index] = new StringReference();

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

            Byte[] buffer = ReusableIO.BinaryRead(input, 172);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);

            for (Int32 index = 0; index < 10; ++index)
                this.AwakenText[index].StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 32 + (4 * index));

            for (Int32 index = 0; index < 10; ++index)
                this.Creatures[index].ResRef = ReusableIO.ReadStringFromByteArray(buffer, 72 + (8 * index), CultureConstants.CultureCodeEnglish);

            this.CountCreatures = ReusableIO.ReadUInt16FromArray(buffer, 152);
            this.Difficulty = ReusableIO.ReadInt16FromArray(buffer, 154);
            this.Lifespan = ReusableIO.ReadInt32FromArray(buffer, 156);
            this.MovementRestrictionDistance = ReusableIO.ReadInt16FromArray(buffer, 160);
            this.MovementRestrictionDistanceToObject = ReusableIO.ReadInt16FromArray(buffer, 162);
            this.MaximumSpawnCount = ReusableIO.ReadUInt16FromArray(buffer, 164);
            this.Active = Convert.ToBoolean(ReusableIO.ReadUInt16FromArray(buffer, 166));
            this.ProbabilityDay = ReusableIO.ReadUInt16FromArray(buffer, 168);
            this.ProbabilityNight = ReusableIO.ReadUInt16FromArray(buffer, 170);

            this.Padding_0x00AC = ReusableIO.BinaryRead(input, 56);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);

            for (Int32 index = 0; index < 10; ++index)
                ReusableIO.WriteInt32ToStream(this.AwakenText[index].StringReferenceIndex, output);

            for (Int32 index = 0; index < 10; ++index)
                ReusableIO.WriteStringToStream(this.Creatures[index].ResRef, output, CultureConstants.CultureCodeEnglish);

            ReusableIO.WriteUInt16ToStream(this.CountCreatures, output);
            ReusableIO.WriteInt16ToStream(this.Difficulty, output);
            ReusableIO.WriteInt32ToStream(this.Lifespan, output);
            ReusableIO.WriteInt16ToStream(this.MovementRestrictionDistance, output);
            ReusableIO.WriteInt16ToStream(this.MovementRestrictionDistanceToObject, output);
            ReusableIO.WriteUInt16ToStream(this.MaximumSpawnCount, output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.Active), output);
            ReusableIO.WriteUInt16ToStream(this.ProbabilityDay, output);
            ReusableIO.WriteUInt16ToStream(this.ProbabilityNight, output);

            output.Write(this.Padding_0x00AC, 0, 56);
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
            return StringFormat.ReturnAndIndent(String.Format("Rest interruption # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Rest interruption:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));

            for (Int32 index = 0; index < 10; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Awaken text # {0}", index)));
                builder.Append(this.AwakenText[index].StringReferenceIndex);
            }

            for (Int32 index = 0; index < 10; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Creature # {0}", index)));
                builder.Append(String.Format("'{0}'", this.Creatures[index].ZResRef));
            }

            builder.Append(StringFormat.ToStringAlignment("Creature count"));
            builder.Append(this.CountCreatures);
            builder.Append(StringFormat.ToStringAlignment("Difficulty"));
            builder.Append(this.Difficulty);
            builder.Append(StringFormat.ToStringAlignment("Spawns' lifespan"));
            builder.Append(this.Lifespan);
            builder.Append(StringFormat.ToStringAlignment("Movement restriction distance"));
            builder.Append(this.MovementRestrictionDistance);
            builder.Append(StringFormat.ToStringAlignment("Movement restriction distance to object"));
            builder.Append(this.MovementRestrictionDistanceToObject);
            builder.Append(StringFormat.ToStringAlignment("Maximum spawn count"));
            builder.Append(this.MaximumSpawnCount);
            builder.Append(StringFormat.ToStringAlignment("Rest interruption spawn active"));
            builder.Append(this.Active);
            builder.Append(StringFormat.ToStringAlignment("Occurrance probability during day"));
            builder.Append(this.ProbabilityDay);
            builder.Append(StringFormat.ToStringAlignment("Occurrance probability during night"));
            builder.Append(this.ProbabilityNight);

            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x00AC));

            return builder.ToString();
        }
        #endregion
    }
}