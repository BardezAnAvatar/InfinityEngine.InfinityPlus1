using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.Utility.DataContainers
{
    /// <summary>Wrapping class for a <see cref="Stream"/> data source</summary>
    public class StreamDataContainer : IDataContainer
    {
        #region Fields
        /// <summary>Data source for this container</summary>
        protected Stream DataSource { get; set; }

        /// <summary>Represents the initial position of the Stream</summary>
        protected Int64 InitialPosition { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the length of the contained data</summary>
        public Int32 Length
        {
            get { return Convert.ToInt32(this.DataSource.Length - this.DataSource.Position); }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="source">Source <see cref="Stream" /> data source</param>
        public StreamDataContainer(Stream source)
        {
            this.DataSource = source;
            this.InitialPosition = source.Position;
        }
        #endregion


        #region Destruction
        /// <summary>Disposes object references</summary>
        public void Dispose()
        {
            if (this.DataSource != null)
            {
                this.DataSource.Dispose();
                this.DataSource = null;
            }
        }
        #endregion


        #region Data Accessors
        /// <summary>Exposes an array of the data</summary>
        /// <returns>A Byte array of the data being represented</returns>
        /// <remarks>This method may consume underlying data</remarks>
        public Byte[] GetArray()
        {
            Byte[] buffer = new Byte[this.Length];
            this.DataSource.Read(buffer, 0, this.Length);
            return buffer;
        }

        /// <summary>Exposes a Stream representing the underlying data</summary>
        /// <returns>A <see cref="Stream"/> object representing the underlying data</returns>
        public Stream GetStream()
        {
            return this.DataSource;
        }

        /// <summary>eads a number of bytes from the underlying data at the specified offset</summary>
        /// <param name="offset">Offset from which to read</param>
        /// <param name="length">Length to read</param>
        /// <returns>Small Byte[] of data read</returns>
        public Byte[] ReadBytes(Int32 offset, Int32 length)
        {
            ReusableIO.SeekIfAble(this.DataSource, this.InitialPosition + offset, SeekOrigin.Begin);

            Byte[] data = new Byte[length];
            this.DataSource.Read(data, 0, length);

            return data;
        }

        /// <summary>eads a number of bytes from the underlying data at the specified offset</summary>
        /// <param name="offset">Offset from which to read</param>
        /// <returns>Small Byte[] of data read</returns>
        public Byte ReadByte(Int32 offset)
        {
            ReusableIO.SeekIfAble(this.DataSource, this.InitialPosition + offset, SeekOrigin.Begin);

            Int32 datum = this.DataSource.ReadByte();

            if (datum < 0)
                throw new EndOfStreamException(String.Format("Value read ({0}) was not valid fora Byte.", datum));

            return (Byte)datum;
        }
        #endregion
    }
}