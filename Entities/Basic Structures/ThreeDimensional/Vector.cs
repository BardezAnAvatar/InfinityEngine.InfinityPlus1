using System;

namespace Bardez.Projects.BasicStructures.ThreeDimensional
{
    /// <summary>Represents a three dimensional vector</summary>
    /// <typeparam name="Primitive">Type represented by the vector (integer, float, etc.)</typeparam>
    public struct Vector<Primitive> where Primitive: struct
    {
        #region Fields
        /// <summary>X-coordinate of the vector</summary>
        private Primitive x;

        /// <summary>Y-coordinate of the vector</summary>
        private Primitive y;

        /// <summary>Z-coordinate of the vector</summary>
        private Primitive z;
        #endregion


        #region Properties
        /// <summary>X-coordinate of the vector</summary>
        public Primitive X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>Y-coordinate of the vector</summary>
        public Primitive Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        /// <summary>Z-coordinate of the vector</summary>
        public Primitive Z
        {
            get { return this.z; }
            set { this.z = value; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="x">X-coordinate of the vector</param>
        /// <param name="y">Y-coordinate of the vector</param>
        /// <param name="z">Z-coordinate of the vector</param>
        public Vector(Primitive x, Primitive y, Primitive z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        #endregion
    }
}