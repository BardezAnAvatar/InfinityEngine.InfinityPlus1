using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents the data structure of the Modron Maze within the GAM file</summary>
    public class TormentModronMaze : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of all data sub-structures on disk</summary>
        public const Int32 StructSize = 1720;

        /// <summary>Represents the count of areas within the maze </summary>
        public const Int32 MazeCount = 64;

        /// <summary>Represents a single of a single dimension of areas within the maze </summary>
        public const Int32 MazeDimension = 8;
        #endregion


        #region Fields
        /// <summary>Two-dimensional array of maze area entries</summary>
        public TormentModronMazeEntry[,] Entries { get; set; }

        /// <summary>Represents a header of information (but located at the end of the struct)</summary>
        public TormentModronMazeHeader Footer { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Entries = new TormentModronMazeEntry[TormentModronMaze.MazeDimension, TormentModronMaze.MazeDimension];

            for (Int32 height = 0; height < TormentModronMaze.MazeDimension; ++height)
                for (Int32 width = 0; width < TormentModronMaze.MazeDimension; ++width)
                    this.Entries[height, width] = new TormentModronMazeEntry();

            this.Footer = new TormentModronMazeHeader();
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            for (Int32 height = 0; height < TormentModronMaze.MazeDimension; ++height)
                for (Int32 width = 0; width < TormentModronMaze.MazeDimension; ++width)
                    this.Entries[height, width].Read(input);

            this.Footer.Read(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            for (Int32 height = 0; height < TormentModronMaze.MazeDimension; ++height)
                for (Int32 width = 0; width < TormentModronMaze.MazeDimension; ++width)
                    this.Entries[height, width].Write(output);

            this.Footer.Write(output);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (Int32 height = 0; height < TormentModronMaze.MazeDimension; ++height)
                for (Int32 width = 0; width < TormentModronMaze.MazeDimension; ++width)
                    builder.Append(this.Entries[height, width].ToString(height, width));

            builder.Append(this.Footer.ToString());

            return builder.ToString();
        }
        #endregion
    }
}