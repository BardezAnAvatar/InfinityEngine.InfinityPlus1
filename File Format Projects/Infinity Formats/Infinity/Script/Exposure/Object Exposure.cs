using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Exposure
{
    /// <summary>A BioWare script's object block exposure</summary>
    /// <remarks>
    ///     The object block data exposure varies from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     BG Series:
    ///     [Int] x 12, [String] x 1
    ///     IWD:
    ///     [Int] x 12, [Point-tangle] x 1, [String] x 1
    ///     IWD2:
    ///     [Int] x 13, [Point-tangle] x 1, [String] x 1, [Int] x 2
    ///     PST:
    ///     [Int] x 14, [Point-tangle] x 1, [String] x 1
    /// </remarks>
    public class Object_Exposure
    {
        #region Constants
        /// <summary>Index for enemy/ally ID</summary>
        protected virtual Int32 indexEA { get { return 0; } }

        /// <summary>Index for General ID</summary>
        protected virtual Int32 indexGeneral { get { return 1; } }

        /// <summary>Index for Race ID</summary>
        protected virtual Int32 indexRace { get { return 2; } }

        /// <summary>Index for Class ID</summary>
        protected virtual Int32 indexClass { get { return 3; } }

        /// <summary>Index for Specific ID</summary>
        protected virtual Int32 indexSpecific { get { return 4; } }

        /// <summary>Index for Gender ID</summary>
        protected virtual Int32 indexGender { get { return 5; } }

        /// <summary>Index for Alignment ID</summary>
        protected virtual Int32 indexAlignment { get { return 6; } }

        /// <summary>Index for first Object ID</summary>
        protected virtual Int32 indexObject1 { get { return 7; } }

        /// <summary>Index for second Object ID</summary>
        protected virtual Int32 indexObject2 { get { return 8; } }

        /// <summary>Index for third Object ID</summary>
        protected virtual Int32 indexObject3 { get { return 9; } }

        /// <summary>Index for fourth Object ID</summary>
        protected virtual Int32 indexObject4 { get { return 10; } }

        /// <summary>Index for fifth Object ID</summary>
        protected virtual Int32 indexObject5 { get { return 11; } }
        #endregion


        #region Fields
        /// <summary>Collection of X number of specifics in the object</summary>
        protected Int64[] specifics;

        /// <summary>Name of the object to target</summary>
        protected String name;

        /// <summary>The rectangle-box of 4 coordinate points</summary>
        protected Int64[] coordinates;

        /// <summary>Additional IWD integer parameters</summary>
        protected Int64[] parameters;
        #endregion


        #region Properties
        /// <summary>Exposes the flags that are transformed for EA (Enemy/Ally) type specifics</summary>
        public Int64 EnemyAlly
        {
            get { return this.specifics[this.indexEA]; }
            set { this.specifics[this.indexEA] = value; }
        }

        /// <summary>Exposes the flags that are transformed for GENERAL type specifics</summary>
        public Int64 General
        {
            get { return this.specifics[this.indexGeneral]; }
            set { this.specifics[this.indexGeneral] = value; }
        }

        /// <summary>Exposes the flags that are transformed for RACE type specifics</summary>
        public Int64 Race
        {
            get { return this.specifics[this.indexRace]; }
            set { this.specifics[this.indexRace] = value; }
        }

        /// <summary>Exposes the flags that are transformed for CLASS type specifics</summary>
        public Int64 Class
        {
            get { return this.specifics[this.indexClass]; }
            set { this.specifics[this.indexClass] = value; }
        }

        /// <summary>Exposes the flags that are transformed for SPECIFIC type specifics</summary>
        public Int64 Specific
        {
            get { return this.specifics[this.indexSpecific]; }
            set { this.specifics[this.indexSpecific] = value; }
        }

        /// <summary>Exposes the flags that are transformed for GENDER type specifics</summary>
        public Int64 Gender
        {
            get { return this.specifics[this.indexGender]; }
            set { this.specifics[this.indexGender] = value; }
        }

        /// <summary>Exposes the flags that are transformed for ALIGNMENT type specifics</summary>
        public Int64 Alignment
        {
            get { return this.specifics[this.indexAlignment]; }
            set { this.specifics[this.indexAlignment] = value; }
        }

        /// <summary>Exposes the flags that are transformed for the first OBJECT type specifics</summary>
        public Int64 ObjectIdentifiers1
        {
            get { return this.specifics[this.indexObject1]; }
            set { this.specifics[this.indexObject1] = value; }
        }

        /// <summary>Exposes the flags that are transformed for the second OBJECT type specifics</summary>
        public Int64 ObjectIdentifiers2
        {
            get { return this.specifics[this.indexObject2]; }
            set { this.specifics[this.indexObject2] = value; }
        }

        /// <summary>Exposes the flags that are transformed for the third OBJECT type specifics</summary>
        public Int64 ObjectIdentifiers3
        {
            get { return this.specifics[this.indexObject3]; }
            set { this.specifics[this.indexObject3] = value; }
        }

        /// <summary>Exposes the flags that are transformed for the fourth OBJECT type specifics</summary>
        public Int64 ObjectIdentifiers4
        {
            get { return this.specifics[this.indexObject4]; }
            set { this.specifics[this.indexObject4] = value; }
        }

        /// <summary>Exposes the flags that are transformed for the fifth OBJECT type specifics</summary>
        public Int64 ObjectIdentifiers5
        {
            get { return this.specifics[this.indexObject5]; }
            set { this.specifics[this.indexObject5] = value; }
        }

        /// <summary>Name of the object to target</summary>
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        #endregion
    }
}
