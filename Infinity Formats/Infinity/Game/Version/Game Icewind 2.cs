using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Version
{
    /// <summary>Represents a Icewind Dale 2 save game</summary>
    /// <remarks>
    ///     This type does seem to use the Familiar Info offset
    ///     
    ///     It also appears to have a bizarre trailing DWORD of 0. Unlike a standard IE game,
    ///     where the latest (offset or datum written) sets the file size, there appears to be a final trailing 4 bytes of 0.
    /// </remarks>
    public class GameIcewind2 : IInfinityFormat
    {
        #region Fields
        /// <summary>Header to the save game format</summary>
        public HeaderBaldurIcewind Header { get; set; }

        /// <summary>Collection of party members</summary>
        public List<TrackedIcewind2Character> Party { get; set; }

        /// <summary>Collection of recruitable members not currently in the party</summary>
        public List<TrackedIcewind2Character> RecruitableCharacters { get; set; }

        /// <summary>Collection of GLOBAL variables</summary>
        public List<Variable> GlobalVariables { get; set; }

        /// <summary>Collection of Journal Entries</summary>
        public List<JournalEntry_v1> JournalEntries { get; set; }

        /// <summary>Wrapper around a collection of familiar info</summary>
        public FamiliarInfo FamiliarInformation { get; set; }

        /// <summary>Trailing 4 bytes</summary>
        /// <value>I have only ever observed 0</value>
        public UInt32 UnknownTrailing { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new HeaderBaldurIcewind();
            this.Party = new List<TrackedIcewind2Character>();
            this.RecruitableCharacters = new List<TrackedIcewind2Character>();
            this.GlobalVariables = new List<Variable>();
            this.JournalEntries = new List<JournalEntry_v1>();
            this.FamiliarInformation = new FamiliarInfo();
        }
        #endregion


        #region I/O Methods
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
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new HeaderBaldurIcewind();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //header
            this.Header.Read(input);

            //party members
            ReusableIO.SeekIfAble(input, this.Header.PartyMemberOffset);
            for (Int32 index = 0; index < this.Header.PartyMemberCount; ++index)
            {
                TrackedIcewind2Character member = new TrackedIcewind2Character();
                member.Read(input);
                this.Party.Add(member);
            }

            //recruitable party members
            ReusableIO.SeekIfAble(input, this.Header.RecruitablePartyMemberOffset);
            for (Int32 index = 0; index < this.Header.RecruitablePartyMemberCount; ++index)
            {
                TrackedIcewind2Character npc = new TrackedIcewind2Character();
                npc.Read(input);
                this.RecruitableCharacters.Add(npc);
            }

            //variables
            ReusableIO.SeekIfAble(input, this.Header.GlobalVariableOffset);
            for (Int32 index = 0; index < this.Header.GlobalVariableCount; ++index)
            {
                Variable v = new Variable();
                v.Read(input);
                this.GlobalVariables.Add(v);
            }

            //journal entries
            ReusableIO.SeekIfAble(input, this.Header.JournalEntriesOffset);
            for (Int32 index = 0; index < this.Header.JournalEntriesCount; ++index)
            {
                JournalEntry_v1 entry = new JournalEntry_v1();
                entry.Read(input);
                this.JournalEntries.Add(entry);
            }

            //familiar information
            ReusableIO.SeekIfAble(input, this.Header.FamiliarsOffset);
            this.FamiliarInformation.Read(input);

            //read party character CREs
            foreach (TrackedIcewind2Character character in this.Party)
                character.ReadCreature(input);

            //read NPC character CREs
            foreach (TrackedIcewind2Character character in this.RecruitableCharacters)
                character.ReadCreature(input);

            //final DWORD
            ReusableIO.SeekIfAble(input, -4L, SeekOrigin.End);
            Byte[] datum = ReusableIO.BinaryRead(input, 4);
            this.UnknownTrailing = ReusableIO.ReadUInt32FromArray(datum, 0);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            //data integrity
            this.MaintainMinimalDataIntegrity();

            //header
            this.Header.Write(output);

            //party members
            ReusableIO.SeekIfAble(output, this.Header.PartyMemberOffset);
            if (output.Length < this.Header.PartyMemberOffset)
                output.SetLength(this.Header.PartyMemberOffset);
            foreach (TrackedIcewind2Character character in this.Party)
                character.Write(output);

            //recruitable party members
            ReusableIO.SeekIfAble(output, this.Header.RecruitablePartyMemberOffset);
            if (output.Length < this.Header.RecruitablePartyMemberOffset)
                output.SetLength(this.Header.RecruitablePartyMemberOffset);
            foreach (TrackedIcewind2Character character in this.RecruitableCharacters)
                character.Write(output);

            //variables
            ReusableIO.SeekIfAble(output, this.Header.GlobalVariableOffset);
            if (output.Length < this.Header.GlobalVariableOffset)
                output.SetLength(this.Header.GlobalVariableOffset);
            foreach (Variable variable in this.GlobalVariables)
                variable.Write(output);

            //journal entries
            ReusableIO.SeekIfAble(output, this.Header.JournalEntriesOffset);
            if (output.Length < this.Header.JournalEntriesOffset)
                output.SetLength(this.Header.JournalEntriesOffset);
            foreach (JournalEntry_v1 entry in this.JournalEntries)
                entry.Write(output);

            //familiar information
            if (this.Header.FamiliarsOffset > 0)
            {
                ReusableIO.SeekIfAble(output, this.Header.FamiliarsOffset);
                if (output.Length < this.Header.FamiliarsOffset)
                    output.SetLength(this.Header.FamiliarsOffset);
                this.FamiliarInformation.Write(output);
            }

            //party character CREs
            foreach (TrackedIcewind2Character character in this.Party)
                character.WriteCreature(output);

            //NPC character CREs
            foreach (TrackedIcewind2Character character in this.RecruitableCharacters)
                character.WriteCreature(output);

            //Final DWORD
            ReusableIO.SeekIfAble(output, 0L, SeekOrigin.End);
            ReusableIO.WriteUInt32ToStream(this.UnknownTrailing, output);
        }
        #endregion


        #region Data integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        /// <remarks>GAM files many data pointers to balance. Copying from the IWD2 creature model.</remarks>
        protected void MaintainMinimalDataIntegrity()
        {
            List<Tuple<Int64, Int64>> offsets = this.CollectOffsets();
            if (this.OffsetsOverlap(offsets))
                this.ResetOffsets();
        }

        /// <summary>Collects a set of offsets and their lengths to be examined</summary>
        /// <returns>A List of Tuples (Offset and length)</returns>
        protected List<Tuple<Int64, Int64>> CollectOffsets()
        {
            List<Tuple<Int64, Int64>> offsetCollection = new List<Tuple<Int64, Int64>>();

            //header
            offsetCollection.Add(new Tuple<Int64, Int64>(0L, HeaderBaldurIcewind.StructSize));

            //party members
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.PartyMemberOffset, (this.Header.PartyMemberCount * TrackedIcewindCharacter.StructSize)));

            //NPCs
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.RecruitablePartyMemberOffset, (this.Header.RecruitablePartyMemberCount * TrackedIcewindCharacter.StructSize)));

            //Variables
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.GlobalVariableOffset, (this.Header.GlobalVariableCount * Variable.StructSize)));

            //Journal entries
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.JournalEntriesOffset, (this.Header.JournalEntriesCount * JournalEntryBase.StructSize)));

            //familiar info
            if (this.Header.FamiliarsOffset > 0)
                offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.FamiliarsOffset, FamiliarInfo.StructSize));

            //extra familiars
            if (this.FamiliarInformation.FamiliarResRefsOffset > 0)
                offsetCollection.Add(new Tuple<Int64, Int64>(this.FamiliarInformation.FamiliarResRefsOffset, (this.FamiliarInformation.TotalCount * 8)));

            //every party member
            foreach (TrackedIcewind2Character character in this.Party)
                offsetCollection.Add(new Tuple<Int64, Int64>(character.CreatureOffset, character.CreatureSize));

            //every recruitable NPC
            foreach (TrackedIcewind2Character character in this.RecruitableCharacters)
                offsetCollection.Add(new Tuple<Int64, Int64>(character.CreatureOffset, character.CreatureSize));

            return offsetCollection;
        }

        /// <summary>Traverses a collection of Tuples (offset and length) and determines if any overlap</summary>
        /// <param name="offsets">Collection of offset tuples</param>
        /// <returns>True if any overlap; false if not</returns>
        protected Boolean OffsetsOverlap(List<Tuple<Int64, Int64>> offsets)
        {
            Boolean overlaps = false;

            for (Int32 i = 0; i < offsets.Count; ++i)
                for (Int32 j = 0; j < offsets.Count; ++j)
                {
                    if (i == j)
                        continue;

                    if (IntExtension.Between(offsets[i].Item1, offsets[i].Item2, offsets[j].Item1, offsets[j].Item1 + offsets[j].Item2))
                    {
                        overlaps = true;
                        goto ExitPoint; //short-circuit
                    }
                }

        //short-circuit destination
        ExitPoint:

            return overlaps;
        }

        /// <summary>Resets the offsets within the file stream</summary>
        protected virtual void ResetOffsets()
        {
            Int32 offset = 0;

            //skip header
            offset += HeaderBaldurIcewind.StructSize;

            //party members
            this.Header.PartyMemberOffset = offset;
            offset += (Convert.ToInt32(this.Header.PartyMemberCount) * TrackedIcewindCharacter.StructSize);

            //NPCs
            this.Header.RecruitablePartyMemberOffset = offset;
            offset += (Convert.ToInt32(this.Header.RecruitablePartyMemberCount) * TrackedIcewindCharacter.StructSize);

            //perform on party, then NPC
            foreach (TrackedIcewind2Character character in this.Party)
            {
                character.CreatureOffset = offset;
                offset += character.CreatureSize;
            }

            foreach (TrackedIcewind2Character character in this.RecruitableCharacters)
            {
                character.CreatureOffset = offset;
                offset += character.CreatureSize;
            }

            //variables
            this.Header.GlobalVariableOffset = offset;
            offset += (Convert.ToInt32(this.Header.GlobalVariableCount) * Variable.StructSize);

            //journal entries
            this.Header.JournalEntriesOffset = offset;
            offset += (Convert.ToInt32(this.Header.JournalEntriesCount) * JournalEntryBase.StructSize);

            //familiar info
            this.Header.FamiliarsOffset = offset;
            offset += FamiliarInfo.StructSize;

            //failiar ResRefs
            this.FamiliarInformation.FamiliarResRefsOffset = offset;
            offset += (Convert.ToInt32(this.FamiliarInformation.TotalCount) * 8);
        }
        #endregion


        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Icewind Game version 1.1");
            builder.AppendLine(this.Header.ToString());
            builder.AppendLine(this.GeneratePartyString());
            builder.AppendLine(this.GenerateRecruitablePartyString());
            builder.AppendLine(this.GenerateVariableString());
            builder.Append(this.GenerateJournalString());
            builder.AppendLine(this.FamiliarInformation.ToString());

            return builder.ToString();
        }

        /// <summary>Generates a human-readable console output describing the party</summary>
        /// <returns>A multi-line String</returns>
        protected String GeneratePartyString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Party.Count; ++i)
                sb.Append(this.Party[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the tracked, recruitable party members</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateRecruitablePartyString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.RecruitableCharacters.Count; ++i)
                sb.Append(this.RecruitableCharacters[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the tracked, recruitable party members</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateVariableString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.GlobalVariables.Count; ++i)
                sb.Append(this.GlobalVariables[i].ToString());

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the tracked, recruitable party members</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateJournalString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.JournalEntries.Count; ++i)
                sb.Append(this.JournalEntries[i].ToString(i + 1));

            return sb.ToString();
        }
        #endregion
    }
}