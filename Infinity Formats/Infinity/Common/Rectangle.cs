using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common
{
    /// <summary>Represents a rectangle, stored as two Points</summary>
    public class Rectangle
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 8;
        #endregion


        #region Fields
        /// <summary>Top-left bounding point</summary>
        public Point TopLeft;

        /// <summary>Bottom-right bounding point</summary>
        public Point BottomRight;
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Rectangle()
        {
            this.TopLeft = new Point();
            this.BottomRight = new Point();
        }

        /// <summary>Definition constructor</summary>
        /// <param name="topLeft">Top left point</param>
        /// <param name="bottomRight">Bottom right point</param>
        public Rectangle(Point topLeft, Point bottomRight)
        {
            this.TopLeft = topLeft;
            this.BottomRight = bottomRight;
        }
        #endregion


        #region ToString overrides
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Top left Point"));
            builder.Append(this.TopLeft.ToString());
            builder.Append(StringFormat.ToStringAlignment("Bottom right Point"));
            builder.Append(this.BottomRight.ToString());

            return builder.ToString();
        }
        #endregion
    }
}