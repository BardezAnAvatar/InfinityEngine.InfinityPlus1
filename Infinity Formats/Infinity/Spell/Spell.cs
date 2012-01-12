using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell
{
    public abstract class Spell : ItemSpell
    {
        protected List<SpellAbility> abilities;

        public List<SpellAbility> Abilities
        {
            get { return this.abilities; }
            set { this.abilities = value; }
        }

        protected override UInt32 AbilitySize
        {
            get { return SpellAbility.StructSize; }
        }

        protected override ItemSpellAbility[] abilitiesArray
        {
            get { return this.abilities.ToArray(); }
        }

        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.InstantiateHeader();
            this.abilities = new List<SpellAbility>();
            this.effects = new List<Effect1>();
        }

        /// <summary>Reads the abilities from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected override void ReadAbilities(Stream input)
        {
            for (Int32 index = 0; index < this.header.CountAbilities; ++index)
            {
                SpellAbility ability = new SpellAbility();
                ability.Read(input);
                this.abilities.Add(ability);
            }
        }
    }
}