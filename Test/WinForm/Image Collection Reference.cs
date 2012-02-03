using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Descriptive information for a ListBox control to display for a collection of images</summary>
    public class ImageCollectionReference
    {
        #region Fields
        /// <summary>Exposes the path to the bitmap file</summary>
        public String ImageDescription { get; set; }

        /// <summary>Represents the underlying collection of images.</summary>
        public List<ImageReference> ImageList { get; set; }

        /// <summary>Represents an order to the collection when sorted</summary>
        public Int32 Order { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ImageCollectionReference()
        {
            this.ImageList = new List<ImageReference>();
        }

        /// <summary>Default constructor</summary>
        public ImageCollectionReference(String path, Int32 order) : this()
        {
            this.ImageDescription = path;
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

        /// <summary>Compares this instance to any given Object</summary>
        /// <param name="comparable">Object reference to compare to</param>
        /// <returns>An Int32 comparison</returns>
        public Int32 CompareTo(Object comparable)
        {
            Int32 comparison;

            if (comparable is ImageCollectionReference)
                comparison = ImageCollectionReference.Compare(this, comparable as ImageCollectionReference);
            else
                comparison = -1;

            return comparison;
        }

        /// <summary>Comparison method for two ImageCollectionReferences</summary>
        /// <param name="left">Left ImageCollectionReference to compare</param>
        /// <param name="right">Right ImageCollectionReference to compare</param>
        /// <returns>An Int32 comparison</returns>
        public static Int32 Compare(ImageCollectionReference left, ImageCollectionReference right)
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
    }
}