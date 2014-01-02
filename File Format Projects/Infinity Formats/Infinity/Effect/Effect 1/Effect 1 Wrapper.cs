using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect1
{
    /// <summary>Wrapper for a version 1 effect</summary>
    public class Effect1Wrapper : EffectWrapper
    {
        #region Fields
        /// <summary>The effect being wraped</summary>
        protected Effect1 effect;
        #endregion


        #region Properties
        /// <summary>Should interface with an Effect1</summary>
        public override IInfinityFormat Effect
        {
            get { return this.effect; }
            set { this.effect = value as Effect1; }
        }
        #endregion


        #region Construction
        /// <summary>efault constructor</summary>
        public Effect1Wrapper()
        {
            this.effect = null;
        }

        /// <summary>Instantiates the members</summary>
        public override void Initialize()
        {
            this.effect = new Effect1();
        }
        #endregion
    }
}