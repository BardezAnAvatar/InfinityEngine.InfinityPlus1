using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature2_2;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents the character structure within the GAM stream, IWD2</summary>
    public class TrackedIcewind2Character : TrackedPlayableCharacterBase
    {
        #region Constants
        /// <summary>Size of structures on disk trailing the standard read model</summary>
        private const Int32 TrailStructSize = 74;
        #endregion


        #region Fields
        /// <summary>Quick slots for the character</summary>
        public CharacterSlots_v2_2 QuickSlots { get; set; }

        /// <summary>Creature data for the tracked character</summary>
        public Creature2_2 Creature { get; set; }

        /// <summary>Represents the voiceset's subfolder within the Sounds directory</summary>
        public ZString SoundSubfolder { get; set; }

        /// <summary>First unknown after the Voiceset ZString</summary>
        public Byte Unknown1 { get; set; }

        /// <summary>Second unknown after the Voiceset ZString</summary>
        /// <remarks>I've seen 4, 5 and 6 in this space</remarks>
        public Byte Unknown2 { get; set; }

        /// <summary>Third unknown after the Voiceset ZString</summary>
        public UInt16 Unknown3 { get; set; }

        /// <summary>Array of UInt32 that seem to indicate any abilities currently in use</summary>
        /// <remarks>
        ///     Array length is 8.
        ///     Third is expertise, fourth is power attack, fifth is arterial strike, sixth is hamstring,
        ///     seventh is rapid shot.
        /// </remarks>
        public UInt32[] AbilitiesCurrentlyInUse { get; set; }

        /* IESDP suggests 3 more bytes, then 2 for the selected weapon index, then 153 unknown bytes
         * but that does not make sense. */

        /// <summary>Fourth unknown, after abilities array</summary>
        public UInt32 Unknown4 { get; set; }

        /// <summary>Weapon slot selected</summary>
        public UInt16 SelectedWeaponSlot { get; set; }

        /// <summary>Trailing unknown 152 bytes of padding (planning ahead? Math error? Actually used?)</summary>
        public Byte[] Padding { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.QuickSlots = new CharacterSlots_v2_2();
            this.Creature = new Creature2_2();
            this.SoundSubfolder = new ZString();
            this.AbilitiesCurrentlyInUse = new UInt32[8];
            this.Padding = new Byte[152];
        }
        #endregion


        #region I/O Methods
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            base.ReadBody(input);

            Byte[] buffer = ReusableIO.BinaryRead(input, TrackedIcewind2Character.TrailStructSize);
            this.SoundSubfolder.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Unknown1 = buffer[32];
            this.Unknown2 = buffer[33];
            this.Unknown3 = ReusableIO.ReadUInt16FromArray(buffer, 34);
            
            //read abilities in use
            for (Int32 index = 0; index < 8; ++index)
                this.AbilitiesCurrentlyInUse[index] = ReusableIO.ReadUInt32FromArray(buffer, 36 + (4 * index));

            this.Unknown4 = ReusableIO.ReadUInt32FromArray(buffer, 68);
            this.SelectedWeaponSlot = ReusableIO.ReadUInt16FromArray(buffer, 72);
            this.Padding = ReusableIO.BinaryRead(input, 152);
        }

        /// <summary>Reads the quick slot data for the character</summary>
        /// <param name="input">Input stream to read from</param>
        protected override void ReadQuickSlots(Stream input)
        {
            this.QuickSlots.Read(input);
        }

        /// <summary>Reads the creature structure data for the character</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadCreature(Stream input)
        {
            //in the packaged BALDUR.GAM for BG1, for example, the creatures are not loaded, so the offsets are 0, size 0.
            if (this.CreatureSize > 0)
            {
                ReusableIO.SeekIfAble(input, this.CreatureOffset);
                Byte[] buffer = ReusableIO.BinaryRead(input, this.CreatureSize);
                using (MemoryStream subStream = new MemoryStream(buffer))
                    this.Creature.Read(subStream);
                buffer = null;
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);
            ReusableIO.WriteStringToStream(this.SoundSubfolder.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            output.WriteByte(this.Unknown1);
            output.WriteByte(this.Unknown2);
            ReusableIO.WriteUInt16ToStream(this.Unknown3, output);

            //Write abilities in use
            for (Int32 index = 0; index < 8; ++index)
                ReusableIO.WriteUInt32ToStream(this.AbilitiesCurrentlyInUse[index], output);

            ReusableIO.WriteUInt32ToStream(this.Unknown4, output);
            ReusableIO.WriteUInt16ToStream(this.SelectedWeaponSlot, output);
            output.Write(this.Padding, 0, this.Padding.Length);
        }

        /// <summary>Writes the quick slot data for the character</summary>
        /// <param name="output">Output stream to write to</param>
        protected override void WriteQuickSlots(Stream output)
        {
            this.QuickSlots.Write(output);
        }

        /// <summary>Writes the creature structure data for the character to the output stream</summary>
        /// <param name="output">Output stream to write to</param>
        public override void WriteCreature(Stream output)
        {
            if (this.CreatureSize > 0)
            {
                using (MemoryStream subStream = new MemoryStream())
                {
                    this.Creature.Write(subStream);
                    Byte[] buffer = subStream.GetBuffer();

                    ReusableIO.SeekIfAble(output, this.CreatureOffset);
                    output.Write(buffer, 0, Convert.ToInt32(subStream.Length));
                    buffer = null;
                }
            }
        }
        #endregion


        #region ToString methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(this.GenerateBaseString());
            builder.Append(this.QuickSlots.ToString());
            builder.Append(this.GenerateNameAndHistoryString());
            builder.Append(StringFormat.ToStringAlignment("Soundset directory"));
            builder.Append(String.Format("'{0}'", this.SoundSubfolder.Value));
            builder.Append(this.GenerateIcewind2TrailingString());

            return builder.ToString();
        }

        /// <summary>Generated a human-readable String representation of the Icewind Dale 2 addon structures</summary>
        /// <returns>A human-readable String representation of the Icewind Dale 2 addon structures</returns>
        protected String GenerateIcewind2TrailingString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("IWD2 Unknown #1"));
            builder.Append(this.Unknown1);
            builder.Append(StringFormat.ToStringAlignment("IWD2 Unknown #2"));
            builder.Append(this.Unknown2);
            builder.Append(StringFormat.ToStringAlignment("IWD2 Unknown #3"));
            builder.Append(this.Unknown3);

            for (Int32 index = 0; index < 8; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Abilities currenly in use {0}", index)));
                builder.Append(this.AbilitiesCurrentlyInUse[index]);
            }

            builder.Append(StringFormat.ToStringAlignment("IWD2 Unknown #4"));
            builder.Append(this.Unknown4);
            builder.Append(StringFormat.ToStringAlignment("Selected weapon slot"));
            builder.Append(this.SelectedWeaponSlot);
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding));

            return builder.ToString();
        }
        #endregion
    }
}