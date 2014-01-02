using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Effect2
{
    /// <summary>Wrapper for a version 2 effect</summary>
    public class Effect2Wrapper : EffectWrapper
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 264;
        #endregion


        #region Fields
        /// <summary>Wrapped effect (version 2) to be exposed</summary>
        /// <remarks>The dual header is not embedded in other files, just in its own</remarks>
        protected Effect2Inner effect;
        #endregion


        #region Properties
        /// <summary>Should interface with an Effect2</summary>
        public override IInfinityFormat Effect
        {
            get { return this.effect; }
            set { this.effect = value as Effect2Inner; }
        }
        #endregion


        #region Construction
        /// <summary>efault constructor</summary>
        public Effect2Wrapper()
        {
            this.effect = null;
        }

        /// <summary>Instantiates the members</summary>
        public override void Initialize()
        {
            this.effect = new Effect2Inner();
        }
        #endregion


        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.effect.ToString();
        }
        #endregion
    }
}