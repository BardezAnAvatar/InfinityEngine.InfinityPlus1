﻿using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature9;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents the character structure within the GAM stream, IWD</summary>
    public class TrackedIcewindCharacter : TrackedPlayableCharacterBase
    {
        #region Constants
        /// <summary>Size of the structure on disk</summary>
        public const Int32 StructSize = 384;
        #endregion

        
        #region Fields
        /// <summary>Quick slots for the character</summary>
        public CharacterSlots_v1 QuickSlots { get; set; }

        /// <summary>Creature data for the tracked character</summary>
        public Creature9 Creature { get; set; }
        
        /// <summary>Represents the voiceset's subfolder within the Sounds directory</summary>
        public ZString SoundSubfolder { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.QuickSlots = new CharacterSlots_v1();
            this.Creature = new Creature9();
            this.SoundSubfolder = new ZString();
        }
        #endregion


        #region I/O Methods
        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            base.ReadBody(input);

            Byte[] buffer = ReusableIO.BinaryRead(input, 32);
            this.SoundSubfolder.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
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

            return builder.ToString();
        }
        #endregion
    }
}