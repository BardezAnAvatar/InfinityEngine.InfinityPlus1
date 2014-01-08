using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.IWD
{
    /// <summary>A BioWare script's object block container</summary>
    /// <remarks>
    ///     The object block varies from engine instance to engine instance.
    ///
    ///     In the various engine instances, the format is:
    ///     IWD:    [Int] x 12, [Point-tangle] x 1, [String] x 1
    /// </remarks>
    public class IcewindObjectBlock : ObjectBlock
    {
        //#region Properties
        ///// <summary>Exposes the first Unknown that appears to be a coordinate</summary>
        //public Int64 UnknownCoordinate1
        //{
        //    get { return this.coordinates[0];}
        //    set { this.coordinates[0] = value; }
        //}
        
        ///// <summary>Exposes the second Unknown that appears to be a coordinate</summary>
        //public Int64 UnknownCoordinate2
        //{
        //    get { return this.coordinates[1];}
        //    set { this.coordinates[1] = value; }
        //}

        ///// <summary>Exposes the third Unknown that appears to be a coordinate</summary>
        //public Int64 UnknownCoordinate3
        //{
        //    get { return this.coordinates[2];}
        //    set { this.coordinates[2] = value; }
        //}

        ///// <summary>Exposes the fourth Unknown that appears to be a coordinate</summary>
        //public Int64 UnknownCoordinate4
        //{
        //    get { return this.coordinates[3]; }
        //    set { this.coordinates[3] = value; }
        //}
        //#endregion
    }
}