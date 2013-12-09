using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.PackedLayeredTexture.Manager;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    /// <summary>Wrapper of a Plt Manager and a String indicating its path</summary>
    public class PltDisplayable
    {
        #region Fields
        /// <summary>Path to the file</summary>
        public String Path { get; set; }

        /// <summary>PLT Manager</summary>
        public PixelTableManager Manager { get; set; }

        /// <summary>Represents an order to the collection when sorted</summary>
        public Int32 Order { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="manager">PLT Manager</param>
        /// <param name="path">Path to the file</param>
        /// <param name="order">Represents an order to the collection when sorted</param>
        public PltDisplayable(PixelTableManager manager, String path, Int32 order)
        {
            this.Path = path;
            this.Manager = manager;
            this.Order = order;
        }
        #endregion


        #region ToString()
        /// <summary>Overridden ToString() implementation</summary>
        /// <returns>The Path member</returns>
        public override String ToString()
        {
            return this.Path;
        }
        #endregion


        #region Compare methods
        /// <summary>Compares this instance to any given Object</summary>
        /// <param name="comparable">Object reference to compare to</param>
        /// <returns>An Int32 comparison</returns>
        public Int32 CompareTo(Object comparable)
        {
            Int32 comparison;

            if (comparable is PltDisplayable)
                comparison = PltDisplayable.Compare(this, comparable as PltDisplayable);
            else
                comparison = -1;

            return comparison;
        }

        /// <summary>Comparison method for two ImageReferences</summary>
        /// <param name="left">Left ImageReference to compare</param>
        /// <param name="right">Right ImageReference to compare</param>
        /// <returns>An Int32 comparison</returns>
        public static Int32 Compare(PltDisplayable left, PltDisplayable right)
        {
            Int32 comparison;

            if (left == right)      //same reference
                comparison = 0;
            else if (left == null)  //not both null
                comparison = -1;
            else if (right == null) //not both null
                comparison = 1;
            else if (left.Order == right.Order)
                comparison = String.Compare(left.Path, right.Path);
            else
                comparison = left.Order < right.Order ? -1 : 1;

            return comparison;
        }
        #endregion
    }
}