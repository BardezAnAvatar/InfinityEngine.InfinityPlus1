using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents a common header base class for GAM file headers</summary>
    /// <remarks>
    ///     There will be *an* issue with Torment.gam files; its signature/version is the same as BG/IWD,
    ///     yet its data structure is not.
    /// </remarks>
    public abstract class HeaderBase : InfinityFormat
    {
        #region Constants
        /// <summary>Length of the base class length</summary>
        private const Int32 BaseLength = 0x4C;
        #endregion


        #region Fields
        /// <summary>Represents game time. one hour = 300 units; 5 units per minute, ergo 1 unit = 12 seconds</summary>
        protected UInt32 gameTime;

        /// <summary>Represents the formations for movement</summary>
        public Formations Formations { get; set; }

        /// <summary>Represents the amount of total party gold</summary>
        public UInt32 Gold { get; set; }

        /// <summary>Represents the count of party members past the first</summary>
        /// <remarks>In BG/PS:T, this means additional non-CHARNAME party members. In IWD this is party count -1.</remarks>
        public UInt16 AdditionalPartyMemberCount { get; set; }

        /// <summary>Represents the flags currently set for weather effects</summary>
        public Weather WeatherFlags { get; set; }

        /// <summary>Offset within the stream to party member structures</summary>
        public Int32 PartyMemberOffset { get; set; }

        /// <summary>Count of party member structures</summary>
        public UInt32 PartyMemberCount { get; set; }

        /// <summary>First unknown, after the party member offset & count</summary>
        public UInt32 Unknown1 { get; set; }

        /// <summary>second unknown, after the party member offset & count</summary>
        public UInt32 Unknown2 { get; set; }

        /// <summary>Offset within the stream to recruitable creatures</summary>
        public Int32 RecruitablePartyMemberOffset { get; set; }

        /// <summary>Count of recruitable creatures</summary>
        public UInt32 RecruitablePartyMemberCount { get; set; }

        /// <summary>Offset within the stream to GLOBAL variables</summary>
        public Int32 GlobalVariableOffset { get; set; }

        /// <summary>Count of GLOBAL variables</summary>
        public UInt32 GlobalVariableCount { get; set; }

        /// <summary>Represents the current master area (i.e.: current area could be a house; master would be district)</summary>
        public ResourceReference MasterAreaCode { get; set; }

        /// <summary>Offset within the stream to familiar resource references</summary>
        public Int32 Unknown3 { get; set; }

        /// <summary>Count of journal entries</summary>
        public UInt32 JournalEntriesCount { get; set; }

        /// <summary>Offset within the stream to journal entries</summary>
        public Int32 JournalEntriesOffset { get; set; }

        /// <summary>Represents the value of the party's reputation</summary>
        /// <remarks>This value is ten times the in-game rep.</remarks>
        public UInt32 Reputation { get; set; }

        /// <summary>Represents the current area (i.e.: current area could be a house; master would be district)</summary>
        public ResourceReference CurrentAreaCode { get; set; }

        /// <summary>Flags currently set for the GUI</summary>
        public GuiFlags UserInterfaceFlags { get; set; }

        /// <summary>Trailing unused data.</summary>
        /// <remarks>Should be 64 Bytes in length</remarks>
        public Byte[] Padding { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes game time. one hour = 300 units; 5 units per minute, ergo 1 unit = 12 seconds</summary>
        public virtual UInt32 GameTime
        {
            get { return this.gameTime; }
            set { this.gameTime = value; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.Formations = new Formations();
            this.MasterAreaCode = new ResourceReference();
            this.CurrentAreaCode = new ResourceReference();
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads the basic fields for the file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        protected void ReadBaseHeader(Stream input)
        {
            //read the game time
            Byte[] buffer = ReusableIO.BinaryRead(input, HeaderBase.BaseLength);
            this.gameTime = ReusableIO.ReadUInt32FromArray(buffer, 0);

            //read formations
            this.Formations.Read(buffer, 4);

            this.Gold = ReusableIO.ReadUInt32FromArray(buffer, 16);
            this.AdditionalPartyMemberCount = ReusableIO.ReadUInt16FromArray(buffer, 20);
            this.WeatherFlags = (Weather)ReusableIO.ReadUInt16FromArray(buffer, 22);
            this.PartyMemberOffset = ReusableIO.ReadInt32FromArray(buffer, 24);
            this.PartyMemberCount = ReusableIO.ReadUInt32FromArray(buffer, 28);
            this.Unknown1 = ReusableIO.ReadUInt32FromArray(buffer, 32);
            this.Unknown2 = ReusableIO.ReadUInt32FromArray(buffer, 36);
            this.RecruitablePartyMemberOffset = ReusableIO.ReadInt32FromArray(buffer, 40);
            this.RecruitablePartyMemberCount = ReusableIO.ReadUInt32FromArray(buffer, 44);
            this.GlobalVariableOffset = ReusableIO.ReadInt32FromArray(buffer, 48);
            this.GlobalVariableCount = ReusableIO.ReadUInt32FromArray(buffer, 52);
            this.MasterAreaCode.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 56, CultureConstants.CultureCodeEnglish);
            this.Unknown3 = ReusableIO.ReadInt32FromArray(buffer, 64);
            this.JournalEntriesCount = ReusableIO.ReadUInt32FromArray(buffer, 68);
            this.JournalEntriesOffset = ReusableIO.ReadInt32FromArray(buffer, 72);

            //PS:T diverges here, the rest down the line.
        }

        /// <summary>Writes the basic fields for the headr base to the output stream</summary>
        /// <param name="output">Output Stream to write to</param>
        protected void WriteBaseHeader(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.gameTime, output);
            this.Formations.Write(output);
            ReusableIO.WriteUInt32ToStream(this.Gold, output);
            ReusableIO.WriteUInt16ToStream(this.AdditionalPartyMemberCount, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.WeatherFlags, output);
            ReusableIO.WriteInt32ToStream(this.PartyMemberOffset, output);
            ReusableIO.WriteUInt32ToStream(this.PartyMemberCount, output);
            ReusableIO.WriteUInt32ToStream(this.Unknown1, output);
            ReusableIO.WriteUInt32ToStream(this.Unknown2, output);
            ReusableIO.WriteInt32ToStream(this.RecruitablePartyMemberOffset, output);
            ReusableIO.WriteUInt32ToStream(this.RecruitablePartyMemberCount, output);
            ReusableIO.WriteInt32ToStream(this.GlobalVariableOffset, output);
            ReusableIO.WriteUInt32ToStream(this.GlobalVariableCount, output);
            ReusableIO.WriteStringToStream(this.MasterAreaCode.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.Unknown3, output);
            ReusableIO.WriteUInt32ToStream(this.JournalEntriesCount, output);
            ReusableIO.WriteInt32ToStream(this.JournalEntriesOffset, output);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the base header through the journal entry data</summary>
        /// <returns>A human-readable String representation of the base header through the journal entry data</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Game time (every other round)"));
            builder.Append(this.gameTime);
            builder.Append(this.Formations.ToString());
            builder.Append(StringFormat.ToStringAlignment("Gold"));
            builder.Append(this.Gold);
            builder.Append(StringFormat.ToStringAlignment("Count of additional party members"));
            builder.Append(this.AdditionalPartyMemberCount);
            builder.Append(StringFormat.ToStringAlignment("Weather flags (value)"));
            builder.Append((UInt16)this.WeatherFlags);
            builder.Append(StringFormat.ToStringAlignment("Weather flags (enumerated)"));
            builder.Append(this.GenerateWeatherFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Offset to party members"));
            builder.Append(this.PartyMemberOffset);
            builder.Append(StringFormat.ToStringAlignment("Count of party members"));
            builder.Append(this.PartyMemberCount);
            builder.Append(StringFormat.ToStringAlignment("Unknown #1"));
            builder.Append(this.Unknown1);
            builder.Append(StringFormat.ToStringAlignment("Unknown #2"));
            builder.Append(this.Unknown2);
            builder.Append(StringFormat.ToStringAlignment("Offset to recruitable party members"));
            builder.Append(this.RecruitablePartyMemberOffset);
            builder.Append(StringFormat.ToStringAlignment("Count of recruitable party members"));
            builder.Append(this.RecruitablePartyMemberCount);
            builder.Append(StringFormat.ToStringAlignment("Offset to global variables"));
            builder.Append(this.GlobalVariableOffset);
            builder.Append(StringFormat.ToStringAlignment("Count of global variables"));
            builder.Append(this.GlobalVariableCount);
            builder.Append(StringFormat.ToStringAlignment("Master Area"));
            builder.Append(this.MasterAreaCode.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Offset to familiar resources"));
            builder.Append(this.Unknown3);
            builder.Append(StringFormat.ToStringAlignment("Count of journal entries"));
            builder.Append(this.GlobalVariableCount);
            builder.Append(StringFormat.ToStringAlignment("Offset to journal entries"));
            builder.Append(this.GlobalVariableOffset);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which Weather flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GenerateWeatherFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.WeatherFlags & Weather.Rain) == Weather.Rain, Weather.Rain.GetDescription());
            StringFormat.AppendSubItem(sb, (this.WeatherFlags & Weather.Snow) == Weather.Snow, Weather.Snow.GetDescription());
            StringFormat.AppendSubItem(sb, (this.WeatherFlags & Weather.Fog) == Weather.Fog, Weather.Fog.GetDescription());
            StringFormat.AppendSubItem(sb, (this.WeatherFlags & Weather.Lightning) == Weather.Lightning, Weather.Lightning.GetDescription());
            StringFormat.AppendSubItem(sb, (this.WeatherFlags & Weather.HasWeather) == Weather.HasWeather, Weather.HasWeather.GetDescription());
            StringFormat.AppendSubItem(sb, (this.WeatherFlags & Weather.Active) == Weather.Active, Weather.Active.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which GuiFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GenerateGuiFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.PartyScriptsEnabled) == GuiFlags.PartyScriptsEnabled, GuiFlags.PartyScriptsEnabled.GetDescription());
            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.TextWindowSize1) == GuiFlags.TextWindowSize1, GuiFlags.TextWindowSize1.GetDescription());
            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.TextWindowSize2) == GuiFlags.TextWindowSize2, GuiFlags.TextWindowSize2.GetDescription());
            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.DialogRunning) == GuiFlags.DialogRunning, GuiFlags.DialogRunning.GetDescription());
            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.HideGui) == GuiFlags.HideGui, GuiFlags.HideGui.GetDescription());
            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.HideUserActionsPanel) == GuiFlags.HideUserActionsPanel, GuiFlags.HideUserActionsPanel.GetDescription());
            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.HidePortraitsPanel) == GuiFlags.HidePortraitsPanel, GuiFlags.HidePortraitsPanel.GetDescription());
            StringFormat.AppendSubItem(sb, (this.UserInterfaceFlags & GuiFlags.HideMapNotes) == GuiFlags.HideMapNotes, GuiFlags.HideMapNotes.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}