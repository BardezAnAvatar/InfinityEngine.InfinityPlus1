using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell
{
    /// <summary>Base class for a spell</summary>
    public abstract class Spell : ItemSpell
    {
        #region Fields
        /// <summary>Collection of spell abilities associated with this spell</summary>
        public List<SpellAbility> SpellAbilities { get; set; }
        #endregion


        #region Properties
        /// <summary>Size of one of this spell's abilities</summary>
        protected override UInt32 AbilitySize
        {
            get { return SpellAbility.StructSize; }
        }

        /// <summary>Collection of abilities associated with this spell</summary>
        protected override IList<ItemSpellAbility> Abilities
        {
            get { return this.SpellAbilities.ToArray(); }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.InstantiateHeader();
            this.SpellAbilities = new List<SpellAbility>();
            this.Effects = new List<Effect1>();
        }
        #endregion


        #region IO method implemetations
        /// <summary>Reads the abilities from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected override void ReadAbilities(Stream input)
        {
            for (Int32 index = 0; index < this.header.CountAbilities; ++index)
            {
                SpellAbility ability = new SpellAbility();
                ability.Read(input);
                this.SpellAbilities.Add(ability);
            }
        }
        #endregion
    }
}