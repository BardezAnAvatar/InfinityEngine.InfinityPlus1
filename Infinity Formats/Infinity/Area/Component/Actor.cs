using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents an 'actor' within the area, a creature on screen</summary>
    public class Actor : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of the structure on disk</summary>
        public const Int32 StructSize = 272;
        #endregion


        #region Fields
        /// <summary>Actor's name</summary>
        public ZString Name { get; set; }

        /// <summary>Current position of the actor</summary>
        public Point Position { get; set; }

        /// <summary>Destination position of the actor</summary>
        public Point Destination { get; set; }

        /// <summary>Various flags fo the actor</summary>
        public ActorFlags Flags { get; set; }

        /// <summary>Flag indicating whether the actor has been spawned.</summary>
        public Boolean HasBeenSpawned { get; set; }

        /// <summary>Byte of the character with which the first character of the CRE file ResRef was replaced with</summary>
        /// <value>0 (null) if not performed or unnecessary.</value>
        public Byte ReplacedLetterByte { get; set; }

        /// <summary>Difficulty spawing flags</summary>
        public SpawnDifficultyFlags Difficulty { get; set; }
        
        /// <summary>Animation of the character</summary>
        /// <remarks>Matches to ANNIMATE.IDS</remarks>
        public UInt32 AnimationId { get; set; }

        /// <summary>Represents the orientation of the character</summary>
        public Direction Orientation { get; set; }

        /// <summary>Unknown at offset 0x36</summary>
        /// <remarks>This data appears to be two shorts rather than 1 DWORD</remarks>
        public Int16 Unknown0x0036 { get; set; }

        /// <summary>Removal timer, in seconds</summary>
        /// <value>usually -1 to avoid removal</value>
        public Int32 RemovalTimer { get; set; }

        /// <summary>Uncertain</summary>
        public Int16 MovementRestrictionDistance { get; set; }

        /// <summary>Uncertain</summary>
        public Int16 MovementRestrictionDistanceToObject { get; set; }

        /// <summary>Schedule flags during which this actor is valid</summary>
        public Schedule AppearanceSchedule { get; set; }

        /// <summary>Number of times actor has been talked to.</summary>
        public UInt32 NumTimesTalkedTo { get; set; }

        /// <summary>Actor's dialog (if override flag set?)</summary>
        public ResourceReference Dialog { get; set; }

        /// <summary>Script reference for script (Override) (if override flag set?)</summary>
        public ResourceReference ScriptOverride { get; set; }

        /// <summary>Script reference for script (General) (if override flag set?)</summary>
        public ResourceReference ScriptGeneral { get; set; }

        /// <summary>Script reference for script (Class) (if override flag set?)</summary>
        public ResourceReference ScriptClass { get; set; }

        /// <summary>Script reference for script (Race) (if override flag set?)</summary>
        public ResourceReference ScriptRace { get; set; }

        /// <summary>Script reference for script (Default) (if override flag set?)</summary>
        public ResourceReference ScriptDefault { get; set; }

        /// <summary>Script reference for script (Specific) (if override flag set?)</summary>
        public ResourceReference ScriptSpecific { get; set; }

        /// <summary>Reference to creature file</summary>
        public ResourceReference CreFile { get; set; }

        /// <summary>Offset to within the Area where the CRE struct resides</summary>
        public Int32 OffsetCreStruct { get; set; }

        /// <summary>Size of the stored CRE struct</summary>
        public Int32 SizeCreStruct { get; set; }

        /// <summary>Padding 128 bytes at the trail end of the actor</summary>
        public Byte[] Padding_0x0090 { get; set; }
        #endregion


        #region Properties
        /// <summary>Character with which the first character of the CRE file ResRef was replaced with</summary>
        /// <value>0 (null) if not performed or unnecessary.</value>
        public Char FirstLetterOfCreFileResRef
        {
            get
            {
                String character = ASCIIEncoding.ASCII.GetString(new Byte[] { this.ReplacedLetterByte });
                return character[0];
            }
            set
            {
                Byte[] characters = ASCIIEncoding.ASCII.GetBytes(new Char[] { value });
                this.ReplacedLetterByte = characters[0];
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.Position = new Point();
            this.Destination = new Point();
            this.Dialog = new ResourceReference(ResourceType.Dialog);
            this.ScriptOverride = new ResourceReference();
            this.ScriptGeneral = new ResourceReference();
            this.ScriptClass = new ResourceReference();
            this.ScriptRace = new ResourceReference();
            this.ScriptDefault = new ResourceReference();
            this.ScriptSpecific = new ResourceReference();
            this.CreFile = new ResourceReference(ResourceType.Creature);
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

            Byte[] buffer = ReusableIO.BinaryRead(input, 144);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Position.X = ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.Position.Y = ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.Destination.X = ReusableIO.ReadUInt16FromArray(buffer, 36);
            this.Destination.Y = ReusableIO.ReadUInt16FromArray(buffer, 38);
            this.Flags = (ActorFlags)ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.HasBeenSpawned = Convert.ToBoolean(ReusableIO.ReadUInt16FromArray(buffer, 44));
            this.ReplacedLetterByte = buffer[46];
            this.Difficulty = (SpawnDifficultyFlags)buffer[47];
            this.AnimationId = ReusableIO.ReadUInt32FromArray(buffer, 48);
            this.Orientation = (Direction)ReusableIO.ReadUInt16FromArray(buffer, 52);
            this.Unknown0x0036 = ReusableIO.ReadInt16FromArray(buffer, 54);
            this.RemovalTimer = ReusableIO.ReadInt32FromArray(buffer, 56);
            this.MovementRestrictionDistance = ReusableIO.ReadInt16FromArray(buffer, 60);
            this.MovementRestrictionDistanceToObject = ReusableIO.ReadInt16FromArray(buffer, 62);
            this.AppearanceSchedule = (Schedule)ReusableIO.ReadUInt32FromArray(buffer, 64);
            this.NumTimesTalkedTo = ReusableIO.ReadUInt32FromArray(buffer, 68);
            this.Dialog.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 72, CultureConstants.CultureCodeEnglish);
            this.ScriptOverride.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 80, CultureConstants.CultureCodeEnglish);
            this.ScriptGeneral.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 88, CultureConstants.CultureCodeEnglish);
            this.ScriptClass.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 96, CultureConstants.CultureCodeEnglish);
            this.ScriptRace.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 104, CultureConstants.CultureCodeEnglish);
            this.ScriptDefault.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 112, CultureConstants.CultureCodeEnglish);
            this.ScriptSpecific.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 120, CultureConstants.CultureCodeEnglish);
            this.CreFile.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 128, CultureConstants.CultureCodeEnglish);
            this.OffsetCreStruct = ReusableIO.ReadInt32FromArray(buffer, 136);
            this.SizeCreStruct = ReusableIO.ReadInt32FromArray(buffer, 140);

            this.Padding_0x0090 = ReusableIO.BinaryRead(input, 128);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream(this.Position.X, output);
            ReusableIO.WriteUInt16ToStream(this.Position.Y, output);
            ReusableIO.WriteUInt16ToStream(this.Destination.X, output);
            ReusableIO.WriteUInt16ToStream(this.Destination.Y, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Flags, output);
            ReusableIO.WriteUInt16ToStream(Convert.ToUInt16(this.HasBeenSpawned), output);
            output.WriteByte(this.ReplacedLetterByte);
            output.WriteByte((Byte)this.Difficulty);
            ReusableIO.WriteUInt32ToStream(this.AnimationId, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Orientation, output);
            ReusableIO.WriteInt16ToStream(this.Unknown0x0036, output);
            ReusableIO.WriteInt32ToStream(this.RemovalTimer, output);
            ReusableIO.WriteInt16ToStream(this.MovementRestrictionDistance, output);
            ReusableIO.WriteInt16ToStream(this.MovementRestrictionDistanceToObject, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.AppearanceSchedule, output);
            ReusableIO.WriteUInt32ToStream(this.NumTimesTalkedTo, output);
            ReusableIO.WriteStringToStream(this.Dialog.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptOverride.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptGeneral.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptClass.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptRace.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptDefault.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.ScriptSpecific.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteStringToStream(this.CreFile.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.OffsetCreStruct, output);
            ReusableIO.WriteInt32ToStream(this.SizeCreStruct, output);

            output.Write(this.Padding_0x0090, 0, 128);
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
            return StringFormat.ReturnAndIndent(String.Format("Actor # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Actor:";
        }
            
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Current position"));
            builder.Append(this.Position.ToString());
            builder.Append(StringFormat.ToStringAlignment("Destination position"));
            builder.Append(this.Destination.ToString());
            builder.Append(StringFormat.ToStringAlignment("Flags (value)"));
            builder.Append((UInt32)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Flags (enumeration)"));
            builder.Append(this.GetFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Has been spawned"));
            builder.Append(this.HasBeenSpawned);
            builder.Append(StringFormat.ToStringAlignment("First Letter Of Cre File ResRef"));
            if (this.FirstLetterOfCreFileResRef != Char.MinValue) //null
                builder.Append(this.FirstLetterOfCreFileResRef);
            builder.Append(StringFormat.ToStringAlignment("Difficulty flags (value)"));
            builder.Append((UInt32)this.Difficulty);
            builder.Append(StringFormat.ToStringAlignment("Difficulty flags (enumeration)"));
            builder.Append(this.GetSpawnDifficultyFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Animation ID"));
            builder.Append(this.AnimationId);
            builder.Append(StringFormat.ToStringAlignment("Orientation (value)"));
            builder.Append((UInt32)this.Orientation);
            builder.Append(StringFormat.ToStringAlignment("Orientation (description)"));
            builder.Append(this.Orientation.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Unknown @ offset 0x36"));
            builder.Append(this.Unknown0x0036);
            builder.Append(StringFormat.ToStringAlignment("Removal Timer"));
            builder.Append(this.RemovalTimer);
            builder.Append(StringFormat.ToStringAlignment("Movement restriction distance"));
            builder.Append(this.MovementRestrictionDistance);
            builder.Append(StringFormat.ToStringAlignment("Movement restriction distance to object"));
            builder.Append(this.MovementRestrictionDistanceToObject);
            builder.Append(StringFormat.ToStringAlignment("Appearance Schedule (value)"));
            builder.Append((UInt32)this.AppearanceSchedule);
            builder.Append(StringFormat.ToStringAlignment("Appearance Schedule (enumeration)"));
            builder.Append(this.GetScheduleFlagsEnumerationString());
            builder.Append(StringFormat.ToStringAlignment("Numer of times talked to"));
            builder.Append(this.NumTimesTalkedTo);
            builder.Append(StringFormat.ToStringAlignment("Dialog"));
            builder.Append(String.Format("'{0}'", this.Dialog.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Override)"));
            builder.Append(String.Format("'{0}'", this.ScriptOverride.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (General)"));
            builder.Append(String.Format("'{0}'", this.ScriptGeneral.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Class)"));
            builder.Append(String.Format("'{0}'", this.ScriptClass.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Race)"));
            builder.Append(String.Format("'{0}'", this.ScriptRace.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Default)"));
            builder.Append(String.Format("'{0}'", this.ScriptDefault.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Script (Specific)"));
            builder.Append(String.Format("'{0}'", this.ScriptSpecific.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("CRE File"));
            builder.Append(String.Format("'{0}'", this.CreFile.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Offset within file to CRE struct"));
            builder.Append(this.OffsetCreStruct);
            builder.Append(StringFormat.ToStringAlignment("Size of CRE struct"));
            builder.Append(this.SizeCreStruct);
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding_0x0090));

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Flags enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Flags enumeration values</returns>
        protected String GetFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & ActorFlags.EmbeddedCre) == ActorFlags.EmbeddedCre, ActorFlags.EmbeddedCre.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & ActorFlags.OverrideScriptNames) == ActorFlags.OverrideScriptNames, ActorFlags.OverrideScriptNames.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Gets a human-readable enumeration String of set Difficulty enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Difficulty enumeration values</returns>
        protected String GetSpawnDifficultyFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Difficulty & SpawnDifficultyFlags.DifficultyLevel1) == SpawnDifficultyFlags.DifficultyLevel1, SpawnDifficultyFlags.DifficultyLevel1.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Difficulty & SpawnDifficultyFlags.DifficultyLevel2) == SpawnDifficultyFlags.DifficultyLevel2, SpawnDifficultyFlags.DifficultyLevel2.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Difficulty & SpawnDifficultyFlags.DifficultyLevel3) == SpawnDifficultyFlags.DifficultyLevel3, SpawnDifficultyFlags.DifficultyLevel3.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Gets a human-readable enumeration String of set Schedule enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Schedule enumeration values</returns>
        protected String GetScheduleFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_00_30_to_01_30) == Schedule.Time_00_30_to_01_30, Schedule.Time_00_30_to_01_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_01_30_to_02_30) == Schedule.Time_01_30_to_02_30, Schedule.Time_01_30_to_02_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_02_30_to_03_30) == Schedule.Time_02_30_to_03_30, Schedule.Time_02_30_to_03_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_03_30_to_04_30) == Schedule.Time_03_30_to_04_30, Schedule.Time_03_30_to_04_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_04_30_to_05_30) == Schedule.Time_04_30_to_05_30, Schedule.Time_04_30_to_05_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_05_30_to_06_30) == Schedule.Time_05_30_to_06_30, Schedule.Time_05_30_to_06_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_06_30_to_07_30) == Schedule.Time_06_30_to_07_30, Schedule.Time_06_30_to_07_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_07_30_to_08_30) == Schedule.Time_07_30_to_08_30, Schedule.Time_07_30_to_08_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_08_30_to_09_30) == Schedule.Time_08_30_to_09_30, Schedule.Time_08_30_to_09_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_09_30_to_10_30) == Schedule.Time_09_30_to_10_30, Schedule.Time_09_30_to_10_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_10_30_to_11_30) == Schedule.Time_10_30_to_11_30, Schedule.Time_10_30_to_11_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_11_30_to_12_30) == Schedule.Time_11_30_to_12_30, Schedule.Time_11_30_to_12_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_12_30_to_13_30) == Schedule.Time_12_30_to_13_30, Schedule.Time_12_30_to_13_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_13_30_to_14_30) == Schedule.Time_13_30_to_14_30, Schedule.Time_13_30_to_14_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_14_30_to_15_30) == Schedule.Time_14_30_to_15_30, Schedule.Time_14_30_to_15_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_15_30_to_16_30) == Schedule.Time_15_30_to_16_30, Schedule.Time_15_30_to_16_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_16_30_to_17_30) == Schedule.Time_16_30_to_17_30, Schedule.Time_16_30_to_17_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_17_30_to_18_30) == Schedule.Time_17_30_to_18_30, Schedule.Time_17_30_to_18_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_18_30_to_19_30) == Schedule.Time_18_30_to_19_30, Schedule.Time_18_30_to_19_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_19_30_to_20_30) == Schedule.Time_19_30_to_20_30, Schedule.Time_19_30_to_20_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_20_30_to_21_30) == Schedule.Time_20_30_to_21_30, Schedule.Time_20_30_to_21_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_21_30_to_22_30) == Schedule.Time_21_30_to_22_30, Schedule.Time_21_30_to_22_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_22_30_to_23_30) == Schedule.Time_22_30_to_23_30, Schedule.Time_22_30_to_23_30.GetDescription());
            StringFormat.AppendSubItem(builder, (this.AppearanceSchedule & Schedule.Time_23_30_to_00_30) == Schedule.Time_23_30_to_00_30, Schedule.Time_23_30_to_00_30.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}