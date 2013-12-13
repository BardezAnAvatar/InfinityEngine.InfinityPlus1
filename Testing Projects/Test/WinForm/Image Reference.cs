using System;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Represents a class to represent the image key and its file path</summary>
    public class ImageReference : IComparable
    {
        #region Fields
        /// <summary>Exposes the path to the bitmap file</summary>
        public String ImageDescription { get; set; }

        /// <summary>Represents the unique key to the bitmap in device memory.</summary>
        public Int32 RenderKey { get; set; }

        /// <summary>X coordinate to render at</summary>
        public Int64 RenderOriginX { get; set; }

        /// <summary>Y coordinate to render at</summary>
        public Int64 RenderOriginY { get; set; }
        
        /// <summary>Represents an order to the collection when sorted</summary>
        public Int32 Order { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ImageReference() { }

        /// <summary>Default constructor</summary>
        public ImageReference(String path, Int32 key)
        {
            this.ImageDescription = path;
            this.RenderKey = key;
            this.RenderOriginX = 0;
            this.RenderOriginY = 0;
        }

        /// <summary>Default constructor</summary>
        public ImageReference(String path, Int32 key, Int32 order) : this (path, key)
        {
            this.Order = order;
        }
        #endregion


        #region Overridden Methods
        /// <summary>Overrides the ToString() method, returning the file path.</summary>
        /// <returns>BitmapPath</returns>
        public override String ToString()
        {
            return this.ImageDescription;
        }
        #endregion


        #region Compare methods
        /// <summary>Compares this instance to any given Object</summary>
        /// <param name="comparable">Object reference to compare to</param>
        /// <returns>An Int32 comparison</returns>
        public Int32 CompareTo(Object comparable)
        {
            Int32 comparison;

            if (comparable is ImageReference)
                comparison = ImageReference.Compare(this, comparable as ImageReference);
            else
                comparison = -1;

            return comparison;
        }

        /// <summary>Comparison method for two ImageReferences</summary>
        /// <param name="left">Left ImageReference to compare</param>
        /// <param name="right">Right ImageReference to compare</param>
        /// <returns>An Int32 comparison</returns>
        public static Int32 Compare(ImageReference left, ImageReference right)
        {
            Int32 comparison;

            if (left == right)      //same reference
                comparison = 0;
            else if (left == null)  //not both null
                comparison = -1;
            else if (right == null) //not both null
                comparison = 1;
            else if (left.Order == right.Order)
                comparison = String.Compare(left.ImageDescription, right.ImageDescription);
            else
                comparison = left.Order < right.Order ? -1 : 1;

            return comparison;
        }
        #endregion
    }
}