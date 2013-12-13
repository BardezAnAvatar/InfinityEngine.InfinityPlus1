using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Character.Header
{
    /// <summary>Character header base class</summary>
    public abstract class CharacterHeaderBase : InfinityFormat
    {
        #region Fields
        /// <summary>Character name, up to 32 bytes</summary>
        public ZString Name { get; set; }

        /// <summary>Offset to the creature structure</summary>
        public UInt32 OffsetCreature { get; set; }

        /// <summary>Length of the creature structure</summary>
        public UInt32 LengthCreature { get; set; }
        #endregion

        
        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.Name = new ZString();
        }
        #endregion


        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected abstract String GetStringRepresentation();
        #endregion
    }
}