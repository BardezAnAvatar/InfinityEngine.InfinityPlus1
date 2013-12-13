using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Interfaces
{
    /// <summary>Interface that defines common spawning properties</summary>
    interface ISpawn : ILiving
    {
        #region Properties
        /// <summary>Array of creatures spawned</summary>
        ResourceReference[] Creatures { get; set; }

        /// <summary>Count of creatures in the Creatures array</summary>
        UInt16 CountCreatures { get; set; }

        /// <summary>Maximum count of creatures to spawn</summary>
        UInt16 MaximumSpawnCount { get; set; }

        /// <summary>Is the spawn point active?</summary>
        Boolean Active { get; set; }

        /// <summary>Probability of the spawn during the day</summary>
        UInt16 ProbabilityDay { get; set; }

        /// <summary>Probability of the spawn during the night</summary>
        UInt16 ProbabilityNight { get; set; }
        #endregion
    }
}