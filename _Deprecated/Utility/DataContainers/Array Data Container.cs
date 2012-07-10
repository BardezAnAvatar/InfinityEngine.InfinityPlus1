using System;
using System.IO;

namespace Bardez.Projects.Utility.DataContainers
{
    /// <summary>Wrapping class for a <see cref="Byte[]"/> data source</summary>
    public class ArrayDataConainer : IDataContainer
    {
        #region Fields
        /// <summary>Data source for this container</summary>
        protected Byte[] DataSource { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the length of the contained data</summary>
        public Int32 Length
        {
            get { return this.DataSource.Length; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="source">Source <see cref="Byte[]" /> data source</param>
        public ArrayDataConainer(Byte[] source)
        {
            this.DataSource = source;
        }
        #endregion


        #region Destruction
        /// <summary>Disposes object references</summary>
        public void Dispose()
        {
            if (this.DataSource != null)
                this.DataSource = null;
        }
        #endregion


        #region Data Accessors
        /// <summary>Exposes an array of the data</summary>
        /// <returns>A Byte array of the data being represented</returns>
        /// <remarks>This method may consume underlying data</remarks>
        public Byte[] GetArray()
        {
            return this.DataSource;
        }

        /// <summary>Exposes a Stream representing the underlying data</summary>
        /// <returns>A <see cref="Stream"/> object representing the underlying data</returns>
        public Stream GetStream()
        {
            return new MemoryStream(this.DataSource);
        }

        /// <summary>eads a number of bytes from the underlying data at the specified offset</summary>
        /// <param name="offset">Offset from which to read</param>
        /// <param name="length">Length to read</param>
        /// <returns>Small Byte[] of data read</returns>
        public Byte[] ReadBytes(Int32 offset, Int32 length)
        {
            Byte[] data = new Byte[length];
            Array.Copy(this.DataSource, offset, data, 0, length);
            return data;
        }

        /// <summary>eads a number of bytes from the underlying data at the specified offset</summary>
        /// <param name="offset">Offset from which to read</param>
        /// <returns>Small Byte[] of data read</returns>
        public Byte ReadByte(Int32 offset)
        {
            return this.DataSource[offset];
        }
        #endregion
    }
}