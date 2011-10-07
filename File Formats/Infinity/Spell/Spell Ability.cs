﻿using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Spell
{
    public class SpellAbility : ItemSpellAbility
    {
        /// <summary>Binary size of the struct on disk</summary>
        public new const Int32 StructSize = 40;    //yes, hard-coded. These data structures have a static size.

        #region Members
        /// <remarks>
        ///     This is the same byte as identifyRequired for items, unused by Spells.
        ///     Except for PS:T, which uses it to identify the level of hostility of the overlay.
        ///     i.e.: Strength of One (SPWI219.SPL) is Non-Hostile (4)
        ///     
        ///     See <see cref="Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Enums.SpellHeader1Flags" /> for comparison
        /// </remarks>
        /// <value>
        ///     Near Infinity is the only app to report on this.
        ///     Values known are:
        ///     Hostile (0)
        ///     Unknown (1)
        ///     Unknown (2)
        ///     Unknown (3)
        ///     Non-hostile (4)
        /// </value>
        //TODO: turn this into an Enum
        protected Byte spellHostility;

        protected UInt16 levelRequirement;

        /// <summary>
        ///     This is for 2E. It is also the 'speed' item slot, followed by the attack bonus field.
        ///     IESDP suggests this as a 32-bit field, but the internal struckture would, then, suggest otherwise.
        ///     However, since the data field is irrelevant for spells, I will follow IESDP for the hell of it.
        /// </summary>
        protected UInt32 castingSpeed;
        #endregion

        #region Properties
        public Byte SpellHostility
        {
            get { return this.spellHostility; }
            set { this.spellHostility = value; }
        }

        public UInt16 LevelRequirement
        {
            get { return this.levelRequirement; }
            set { this.levelRequirement = value; }
        }

        public UInt32 CastingSpeed
        {
            get { return this.castingSpeed; }
            set { this.castingSpeed = value; }
        }
        #endregion

        #region Constructor(s)
        public SpellAbility()
        {
            this.icon = null;
            this.featureCount = 0;
        }
        #endregion
        
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.icon = new ResourceReference();
        }

        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public override void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        /// <remarks>Calls readbody directly, as there exists no signature or version for this structure.</remarks>
        public override void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] remainingBody = ReusableIO.BinaryRead(input, 40);

            this.abilityType = (ItemSpellAbilityType)remainingBody[0];
            this.spellHostility = remainingBody[1];
            this.abilityLocation = (ItemSpellAbilityLocation)ReusableIO.ReadUInt16FromArray(remainingBody, 2);
            this.icon.ResRef = ReusableIO.ReadStringFromByteArray(remainingBody, 4, Constants.CultureCodeEnglish);
            this.target = (ItemSpellAbilityTarget)remainingBody[12];
            this.targetCount = remainingBody[13];
            this.range = ReusableIO.ReadUInt16FromArray(remainingBody, 14);
            this.levelRequirement = ReusableIO.ReadUInt16FromArray(remainingBody, 16);
            this.castingSpeed = ReusableIO.ReadUInt32FromArray(remainingBody, 18);
            this.diceSides = ReusableIO.ReadUInt16FromArray(remainingBody, 22);
            this.diceNumber = ReusableIO.ReadUInt16FromArray(remainingBody, 24);
            this.damageBonus = ReusableIO.ReadInt16FromArray(remainingBody, 26);
            this.damageType = (ItemSpellAbilityDamageType)ReusableIO.ReadUInt16FromArray(remainingBody, 28);
            this.featureCount = ReusableIO.ReadUInt16FromArray(remainingBody, 30);
            this.featureOffset = ReusableIO.ReadUInt16FromArray(remainingBody, 32);
            this.charges = ReusableIO.ReadUInt16FromArray(remainingBody, 34);
            this.depletionBehavior = (ItemSpellAbilityDepletionBehavior)ReusableIO.ReadUInt16FromArray(remainingBody, 36);
            this.projectileAnimation = ReusableIO.ReadUInt16FromArray(remainingBody, 38);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        public override void Write(Stream output)
        {
            output.WriteByte((Byte)this.abilityType);
            output.WriteByte(this.spellHostility);
            ReusableIO.WriteUInt16ToStream((UInt16)this.abilityLocation, output);
            ReusableIO.WriteStringToStream(this.icon.ResRef, output, Constants.CultureCodeEnglish);
            output.WriteByte((Byte)this.target);
            output.WriteByte(this.targetCount);
            ReusableIO.WriteUInt16ToStream(this.range, output);
            ReusableIO.WriteUInt16ToStream(this.levelRequirement, output);
            ReusableIO.WriteUInt32ToStream(this.castingSpeed, output);
            ReusableIO.WriteUInt16ToStream(this.diceSides, output);
            ReusableIO.WriteUInt16ToStream(this.diceNumber, output);
            ReusableIO.WriteInt16ToStream(this.damageBonus, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.damageType, output);
            ReusableIO.WriteUInt16ToStream(this.featureCount, output);
            ReusableIO.WriteUInt16ToStream(this.featureOffset, output);
            ReusableIO.WriteUInt16ToStream(this.charges, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.depletionBehavior, output);
            ReusableIO.WriteUInt16ToStream(this.projectileAnimation, output);
        }
        #endregion
        
        #region ToString() Helpers
        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString(Boolean showType)
        {
            String header = "Spell Ability & Extended Header:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString(Int32 abilityIndex)
        {
            return String.Format("Spell Ability # {0}:", abilityIndex) + this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n\tAbility type:                                   '");
            builder.Append((Byte)this.abilityType);
            builder.Append("'");
            builder.Append("\n\tAbility type (description):                     ");
            builder.Append(this.abilityType.GetDescription());
            builder.Append("\n\tUse of ability hostility level:                 ");
            builder.Append(this.spellHostility);
            builder.Append("\n\tAbility UI location:                            ");
            builder.Append((UInt16)this.abilityLocation);
            builder.Append("\n\tAbility UI location (description):              ");
            builder.Append(this.abilityLocation.GetDescription());
            builder.Append("\n\tAbility Icon:                                   '");
            builder.Append(this.icon.ResRef);
            builder.Append("'");
            builder.Append("\n\tAbility Target:                                 ");
            builder.Append((Byte)this.target);
            builder.Append("\n\tAbility Target (description):                   ");
            builder.Append(this.target.GetDescription());
            builder.Append("\n\tAbility Target Count:                           ");
            builder.Append(this.targetCount);
            builder.Append("\n\tAbility Range (in search rectangles?):          ");
            builder.Append(this.range);
            builder.Append("\n\tLevel requirement:                              ");
            builder.Append(this.levelRequirement);
            builder.Append("\n\tCasting speed:                                  ");
            builder.Append(this.castingSpeed);
            builder.Append("\n\tAbility Dice Sides:                             ");
            builder.Append(this.diceSides);
            builder.Append("\n\tAbility Dice Number:                            ");
            builder.Append(this.diceNumber);
            builder.Append("\n\tAbility Damage Bonus:                           ");
            builder.Append(this.damageBonus);
            builder.Append("\n\tAbility Damage Type:                            ");
            builder.Append((UInt16)this.damageType);
            builder.Append("\n\tAbility Damage Type (description):\n\t\t");
            builder.Append(this.damageType.GetDescription());
            builder.Append("\n\tAbility Effects Count:                          ");
            builder.Append(this.featureCount);
            builder.Append("\n\tAbility Effects Offset:                         ");
            builder.Append(this.featureOffset);
            builder.Append("\n\tAbility Charges Count:                          ");
            builder.Append(this.charges);
            builder.Append("\n\tCharge depletion behavior:                      ");
            builder.Append((UInt16)this.depletionBehavior);
            builder.Append("\n\tCharge depletion behavior (description):        ");
            builder.Append(this.depletionBehavior.GetDescription());
            builder.Append("\n\tAbility Projectile Animation (see *.IDS):       ");
            builder.Append(this.projectileAnimation);
            builder.Append("\n\n");

            return builder.ToString();
        }
        #endregion
    }
}