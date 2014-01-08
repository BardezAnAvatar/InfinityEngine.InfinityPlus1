using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Exposure;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Torment
{
    /// <summary>A BioWare script's object block container</summary>
    /// <remarks>
    ///     The object block varies from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     PST:    [Int] x 14, [Point-tangle] x 1, [String] x 1
    /// </remarks>
    public class TormentObjectBlockExposure : Object_Exposure
    {
        #region Constants
        /// <summary>Index for General ID</summary>
        protected override Int32 indexGeneral { get { return 3; } }

        /// <summary>Index for Race ID</summary>
        protected override Int32 indexRace { get { return 4; } }

        /// <summary>Index for Class ID</summary>
        protected override Int32 indexClass { get { return 5; } }

        /// <summary>Index for Specific ID</summary>
        protected override Int32 indexSpecific { get { return 6; } }

        /// <summary>Index for Gender ID</summary>
        protected override Int32 indexGender { get { return 7; } }

        /// <summary>Index for Alignment ID</summary>
        protected override Int32 indexAlignment { get { return 8; } }

        /// <summary>Index for first Object ID</summary>
        protected override Int32 indexObject1 { get { return 9; } }

        /// <summary>Index for second Object ID</summary>
        protected override Int32 indexObject2 { get { return 10; } }

        /// <summary>Index for third Object ID</summary>
        protected override Int32 indexObject3 { get { return 11; } }

        /// <summary>Index for fourth Object ID</summary>
        protected override Int32 indexObject4 { get { return 12; } }

        /// <summary>Index for fifth Object ID</summary>
        protected override Int32 indexObject5 { get { return 13; } }

        /// <summary>Index for Faction ID</summary>
        private const Int32 indexFaction = 1;

        /// <summary>Index for Team ID</summary>
        private const Int32 indexTeam = 2;
        #endregion


        #region Properties
        /// <summary>Exposes the flags that are transformed for FACTION type specifics</summary>
        public Int64 Faction
        {
            get { return this.specifics[TormentObjectBlockExposure.indexFaction]; }
            set { this.specifics[TormentObjectBlockExposure.indexFaction] = value; }
        }

        /// <summary>Exposes the flags that are transformed for TEAM type specifics</summary>
        public Int64 Team
        {
            get { return this.specifics[TormentObjectBlockExposure.indexTeam]; }
            set { this.specifics[TormentObjectBlockExposure.indexTeam] = value; }
        }
       
        /// <summary>Exposes the first Unknown that appears to be a coordinate</summary>
        public Int64 UnknownCoordinate1
        {
            get { return this.coordinates[0];}
            set { this.coordinates[0] = value; }
        }
        
        /// <summary>Exposes the second Unknown that appears to be a coordinate</summary>
        public Int64 UnknownCoordinate2
        {
            get { return this.coordinates[1];}
            set { this.coordinates[1] = value; }
        }

        /// <summary>Exposes the third Unknown that appears to be a coordinate</summary>
        public Int64 UnknownCoordinate3
        {
            get { return this.coordinates[2];}
            set { this.coordinates[2] = value; }
        }

        /// <summary>Exposes the fourth Unknown that appears to be a coordinate</summary>
        public Int64 UnknownCoordinate4
        {
            get { return this.coordinates[3]; }
            set { this.coordinates[3] = value; }
        }
        #endregion
    }
}