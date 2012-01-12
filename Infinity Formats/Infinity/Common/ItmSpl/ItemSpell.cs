using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect1;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl
{
    public abstract class ItemSpell : IInfinityFormat
    {
        #region Members
        protected ItemSpellHeader1 header;
        protected List<Effect1> effects;
        #endregion

        #region Properties
        public List<Effect1> Effects
        {
            get { return this.effects; }
            set { this.effects = value; }
        }

        protected abstract String Headline
        {
            get;
        }

        protected abstract UInt32 HeaderSize
        {
            get;
        }

        protected abstract UInt32 AbilitySize
        {
            get;
        }

        protected abstract ItemSpellAbility[] abilitiesArray
        {
            get;
        }
        #endregion

        /// <summary>Instantiates reference types</summary>
        public abstract void Initialize();

        /// <summary>Instantiates a new header</summary>
        protected abstract void InstantiateHeader();
        
        #region IO method implemetations
        /// <summary>This public method reads file format from the output stream. Reads the whole structure.</summary>
        /// <param name="input">Stream to read from</param>
        public void Read(Stream input)
        {
            this.Initialize();
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.InstantiateHeader();
                this.header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.header.Read(input);

            if (input.Position != this.header.OffsetAbilities)
                input.Seek(this.header.OffsetAbilities, SeekOrigin.Begin);
            this.ReadAbilities(input);

            if (input.Position != this.header.OffsetAbilityEffects)
                input.Seek(this.header.OffsetAbilityEffects, SeekOrigin.Begin);
            for (Int32 index = 0; index < GetEffectsCount(); ++index)
            {
                Effect1 effect = new Effect1();
                effect.Read(input);
                this.effects.Add(effect);
            }
        }

        /// <summary>Reads the abilities from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        protected abstract void ReadAbilities(Stream input);
        
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();

            this.header.Write(output);

            output.Seek(this.header.OffsetAbilities, SeekOrigin.Begin);
            foreach (ItemSpellAbility ability in this.abilitiesArray)
                ability.Write(output);

            output.Seek(this.header.OffsetAbilityEffects, SeekOrigin.Begin);
            foreach (Effect1 effect in this.effects)
                effect.Write(output);
        }
        #endregion

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Headline);
            builder.Append(this.header.ToString());

            for (Int32 i = 0; i < this.abilitiesArray.Length; ++i)
                builder.Append(abilitiesArray[i].ToString(i + 1));

            for (Int32 i = 0; i < this.effects.Count; ++i)
                builder.Append(effects[i].ToString(i + 1));

            return builder.ToString();
        }

        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected void MaintainMinimalDataIntegrity()
        {
            UInt32 headerSize = this.HeaderSize;
            Int32 effectSize = Effect1.StructSize;
            UInt32 abilitySize = this.AbilitySize;

            this.header.CountAbilities = Convert.ToUInt16(this.abilitiesArray.Length);

            if (this.header.OffsetAbilities < headerSize)
                this.header.OffsetAbilities = headerSize;

            //make sure there is space for the effects. Yes, they *can* be in between the header and the abilities, so poo.
            Int64 size = this.effects.Count * effectSize;
            if (    //(
                        //!((this.header.OffsetAbilityEffects + size) < this.header.OffsetAbilities)
                        //&& !(this.header.OffsetAbilityEffects > this.header.OffsetAbilities && this.header.OffsetAbilityEffects < (this.header.OffsetAbilities + (this.abilitiesArray.Length * abilitySize)))
                    //)
                    //...wow that was bad code
                    (this.header.OffsetAbilityEffects < this.header.OffsetAbilities && (this.header.OffsetAbilityEffects + size) >= this.header.OffsetAbilities) //it starts before and spills into
                    || (this.header.OffsetAbilityEffects >= this.header.OffsetAbilities && (this.header.OffsetAbilityEffects < (this.header.OffsetAbilities + (this.abilitiesArray.Length * abilitySize))))            //it starts after abilities, but before the end of abilities
                )
                this.header.OffsetAbilityEffects = headerSize + (abilitySize * this.header.CountAbilities);
        }

        /// <summary>Gets the number of effects in the file.</summary>
        /// <returns>An integer representing the count of effects in this item or overlay</returns>
        protected Int32 GetEffectsCount()
        {
            Int32 count = this.header.CountEquippedEffects;

            foreach (ItemSpellAbility ability in this.abilitiesArray)
                count += ability.FeatureCount;

            return count;
        }
    }
}