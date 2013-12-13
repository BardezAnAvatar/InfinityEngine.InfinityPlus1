using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Base class for (playable/recruitable) characters being tracked in the game</summary>
    public abstract class TrackedPlayableCharacterBase : IInfinityFormat
    {
        #region Constants
        /// <summary>Count of interactio entries</summary>
        protected const Int32 InteractionCount = 24;

        /// <summary>Size of the base structure's leading variables</summary>
        protected const Int32 BaseStructSize = 140;

        /// <summary>Size of the base structure's name and talked to struct size</summary>
        protected const Int32 NameTalkStructSize = 36;
        #endregion


        #region Fields
        /// <summary>REpresents various flags for the character, such as selection in UI or death.</summary>
        public CharacterFlags Flags { get; set; }

        /// <summary>Represents the order of the character in the party</summary>
        /// <value>-1 indicates not in party.</value>
        public Int16 PartyOrder { get; set; }

        /// <summary>Represents the offset within this stream to the CRE data for this character</summary>
        public Int32 CreatureOffset { get; set; }

        /// <summary>Represents the size within this stream of the CRE data</summary>
        public Int32 CreatureSize { get; set; }

        /// <summary>Resource to the CRE file associated with this character.</summary>
        /// <remarks>
        ///     IWD keeps this blank. BG2 replace the first character with a '*' for savegames, but not for baldur.gam
        ///     BG1 has the references correct in baludr.gam, but blanks once a game has been started.
        ///     PS:T does not have members in Tormant.gam until after a game has been started;
        ///         then it blanks out the CRE resource.
        /// </remarks>
        public ResourceReference CreResource { get; set; }

        /// <summary>Represents the orientation of the character</summary>
        public Direction Orientation { get; set; }

        /// <summary>Resource of the area that the character is currently in</summary>
        public ResourceReference CurrentArea { get; set; }

        /// <summary>Represents the X coordinate of the character</summary>
        public UInt16 CoordinateX { get; set; }

        /// <summary>Represents the Y coordinate of the character</summary>
        public UInt16 CoordinateY { get; set; }

        /// <summary>Represents the X coordinate of the character's viewing rectangle</summary>
        public UInt16 ViewingRectangleCoordinateX { get; set; }

        /// <summary>Represents the Y coordinate of the character's viewing rectangle</summary>
        public UInt16 ViewingRectangleCoordinateY { get; set; }

        /// <summary>Represents the character's current action; any ongoing ailities being used</summary>
        /// <remarks>Matches Modal.IDS</remarks>
        public Int16 ModalAction { get; set; }

        /// <summary>Represents the character's current happiness</summary>
        public Int16 Happiness { get; set; }

        /// <summary>Appears to be an array of 24 intreaction counts between this and other PCs</summary>
        /// <remarks>Seems to be related to INTERACT.2DA, but unused or not persisted?</remarks>
        public UInt32[] InteractionTable { get; set; }

        /* Register a divergence from IWD2 from BG & IWD; IWD2 has 8 slots, not 4 */

        /// <summary>Represents the name of the character</summary>
        /// <remarks>32 bytes in length</remarks>
        public ZString Name { get; set; }

        /// <summary>Represents the number of times the party has talked to this character.</summary>
        public UInt32 NumberOfTimeTalkedTo { get; set; }

        /// <summary>Represents the history of the character within the party; kills, favorite spells, etc.</summary>
        public CharacterInfo History { get; set; }

        /// <summary>Represents the soundset base name for this character</summary>
        public ZString SoundSetPrefix { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.CreResource = new ResourceReference();
            this.CurrentArea = new ResourceReference();
            this.InteractionTable = new UInt32[TrackedPlayableCharacterBase.InteractionCount];
            this.Name = new ZString();
            this.History = new CharacterInfo();
            this.SoundSetPrefix = new ZString();
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
            Byte[] buffer = ReusableIO.BinaryRead(input, TrackedPlayableCharacterBase.BaseStructSize);

            this.Flags = (CharacterFlags)ReusableIO.ReadUInt16FromArray(buffer, 0);
            this.PartyOrder = ReusableIO.ReadInt16FromArray(buffer, 2);
            this.CreatureOffset = ReusableIO.ReadInt32FromArray(buffer, 4);
            this.CreatureSize = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.CreResource.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 12, CultureConstants.CultureCodeEnglish);
            this.Orientation = (Direction)ReusableIO.ReadInt32FromArray(buffer, 20);
            this.CurrentArea.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 24, CultureConstants.CultureCodeEnglish);
            this.CoordinateX = ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.CoordinateY = ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.ViewingRectangleCoordinateX = ReusableIO.ReadUInt16FromArray(buffer, 36);
            this.ViewingRectangleCoordinateY = ReusableIO.ReadUInt16FromArray(buffer, 38);
            this.ModalAction = ReusableIO.ReadInt16FromArray(buffer, 40);
            this.Happiness = ReusableIO.ReadInt16FromArray(buffer, 42);

            for (Int32 i = 0; i < TrackedPlayableCharacterBase.InteractionCount; ++i)
                InteractionTable[i] = ReusableIO.ReadUInt32FromArray(buffer, 44 + (4 * i));

            //Quick Slots
            this.ReadQuickSlots(input);

            //Name and times talked to
            buffer = ReusableIO.BinaryRead(input, TrackedPlayableCharacterBase.NameTalkStructSize);
            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.NumberOfTimeTalkedTo = ReusableIO.ReadUInt32FromArray(buffer, 32);

            //history
            this.History.Read(input);

            //Soundset
            buffer = ReusableIO.BinaryRead(input, 8);
            this.SoundSetPrefix.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Reads the quick slot data for the character</summary>
        /// <param name="input">Input stream to read from</param>
        protected abstract void ReadQuickSlots(Stream input);

        /// <summary>Reads the creature structure data for the character</summary>
        /// <param name="input">Input stream to read from</param>
        public abstract void ReadCreature(Stream input);

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream((UInt16)this.Flags, output);
            ReusableIO.WriteInt16ToStream(this.PartyOrder, output);
            ReusableIO.WriteInt32ToStream(this.CreatureOffset, output);
            ReusableIO.WriteInt32ToStream(this.CreatureSize, output);
            ReusableIO.WriteStringToStream(this.CreResource.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream((UInt32)this.Orientation, output);
            ReusableIO.WriteStringToStream(this.CurrentArea.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.CoordinateX, output);
            ReusableIO.WriteUInt16ToStream(this.CoordinateY, output);
            ReusableIO.WriteUInt16ToStream(this.ViewingRectangleCoordinateX, output);
            ReusableIO.WriteUInt16ToStream(this.ViewingRectangleCoordinateY, output);
            ReusableIO.WriteInt16ToStream(this.ModalAction, output);
            ReusableIO.WriteInt16ToStream(this.Happiness, output);

            for (Int32 i = 0; i < TrackedPlayableCharacterBase.InteractionCount; ++i)
                ReusableIO.WriteUInt32ToStream(InteractionTable[i], output);

            this.WriteQuickSlots(output);

            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt32ToStream(this.NumberOfTimeTalkedTo, output);
            this.History.Write(output);
            ReusableIO.WriteStringToStream(this.SoundSetPrefix.Value, output, CultureConstants.CultureCodeEnglish);
        }

        /// <summary>Writes the quick slot data for the character</summary>
        /// <param name="output">Output stream to write to</param>
        protected abstract void WriteQuickSlots(Stream output);

        /// <summary>Writes the creature structure data for the character to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        public abstract void WriteCreature(Stream output);
        #endregion


        #region ToString methods
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
            return StringFormat.ReturnAndIndent(String.Format("Tracked Character # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Tracked Character:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected abstract String GetStringRepresentation();

        /// <summary>Generates a human-readable String representation of the base fields</summary>
        /// <returns>A human-readable String representation of the base fields</returns>
        protected virtual String GenerateBaseString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Character flags (value)"));
            builder.Append((UInt16)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Character flags (enumerated)"));
            builder.Append(this.GenerateCharacterFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Party order"));
            builder.Append(this.PartyOrder);
            builder.Append(StringFormat.ToStringAlignment("Offset to creature structure"));
            builder.Append(this.CreatureOffset);
            builder.Append(StringFormat.ToStringAlignment("Size of creature structure"));
            builder.Append(this.CreatureSize);
            builder.Append(StringFormat.ToStringAlignment("Resource of creature structure"));
            builder.Append(String.Format("'{0}'", this.CreResource.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Orientation (value)"));
            builder.Append((UInt32)this.Orientation);
            builder.Append(StringFormat.ToStringAlignment("Orientation (description)"));
            builder.Append(this.Orientation.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Current Area"));
            builder.Append(String.Format("'{0}'", this.CurrentArea.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("X coordinate"));
            builder.Append(this.CoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Y coordinate"));
            builder.Append(this.CoordinateY);
            builder.Append(StringFormat.ToStringAlignment("Viewing rectangle X coordinate"));
            builder.Append(this.ViewingRectangleCoordinateX);
            builder.Append(StringFormat.ToStringAlignment("Viewing rectangle Y coordinate"));
            builder.Append(this.ViewingRectangleCoordinateY);
            builder.Append(StringFormat.ToStringAlignment("Modal Action"));
            builder.Append(this.ModalAction);
            builder.Append(StringFormat.ToStringAlignment("Happiness"));
            builder.Append(this.Happiness);

            for (Int32 index = 0; index < 24; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Interaction count #{0}", index)));
                builder.Append(this.InteractionTable[index]);
            }

            return builder.ToString();
        }

        /// <summary>Generates a human-readable String representation of the fields after the quickslots</summary>
        /// <returns>A human-readable String representation of the fields after the quickslots</returns>
        protected virtual String GenerateNameAndHistoryString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Number of time party has talked to"));
            builder.Append(this.NumberOfTimeTalkedTo);
            builder.Append(this.History.ToString());
            builder.Append(StringFormat.ToStringAlignment("Sound set"));
            builder.Append(String.Format("'{0}'", this.SoundSetPrefix.Value));

            return builder.ToString();
        }
        
        /// <summary>Generates a human-readable representation of the Character Flags enumerator</summary>
        /// <returns>A human-readable representation of the Character Flags enumerator</returns>
        protected String GenerateCharacterFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Flags & CharacterFlags.Dead) == CharacterFlags.Dead, CharacterFlags.Dead.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Flags & CharacterFlags.Selected) == CharacterFlags.Selected, CharacterFlags.Selected.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}