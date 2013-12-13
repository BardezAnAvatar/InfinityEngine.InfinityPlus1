using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Repreents a single area within the Modron Maze</summary>
    /// <remarks>
    ///     All this information provided by Avenger
    ///     http://forums.gibberlings3.net/index.php?showtopic=21822
    /// </remarks>
    public class TormentModronMazeEntry
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 26;
        #endregion


        #region Fields
        /// <remarks>Special room?</remarks>
        public UInt32 Override { get; set; }

        /// <summary>Room is accessible (used in generation?)</summary>
        public UInt32 Accessible { get; set; }

        /// <summary>Area is used (used in generation?)</summary>
        public UInt32 Valid { get; set; }

        /// <summary>Traps in this room?</summary>
        public UInt32 Trapped { get; set; }

        /// <summary>Trap in this room</summary>
        public MazeTrap TrapType { get; set; }

        /// <summary>Walls in this room</summary>
        public MazeWalls Walls { get; set; }

        /// <remarks>Room has been visited?</remarks>
        public UInt32 Visited { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize() { }
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

            Byte[] buffer = ReusableIO.BinaryRead(input, TormentModronMazeEntry.StructSize);

            this.Override = ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.Accessible = ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.Valid = ReusableIO.ReadUInt32FromArray(buffer, 8);
            this.Trapped = ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.TrapType = (MazeTrap)ReusableIO.ReadUInt32FromArray(buffer, 16);
            this.Walls = (MazeWalls)ReusableIO.ReadUInt32FromArray(buffer, 20);
            this.Visited = ReusableIO.ReadUInt32FromArray(buffer, 22);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.Override, output);
            ReusableIO.WriteUInt32ToStream(this.Accessible, output);
            ReusableIO.WriteUInt32ToStream(this.Valid, output);
            ReusableIO.WriteUInt32ToStream(this.Trapped, output);
            ReusableIO.WriteUInt32ToStream((UInt32)this.TrapType, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Walls, output);
            ReusableIO.WriteUInt32ToStream(this.Visited, output);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            return this.GetStringRepresentation();
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="entryIndexA">First dimension entry #</param>
        /// <param name="entryIndexB">Second dimension entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndexA, Int32 entryIndexB)
        {
            return StringFormat.ReturnAndIndent(String.Format("Maze Entry # [{0}, {1}]:", entryIndexA, entryIndexB), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Maze Entry:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Override"));
            builder.Append(this.Override);
            builder.Append(StringFormat.ToStringAlignment("Accessible"));
            builder.Append(this.Accessible);
            builder.Append(StringFormat.ToStringAlignment("Valid"));
            builder.Append(this.Valid);
            builder.Append(StringFormat.ToStringAlignment("Trapped"));
            builder.Append(this.Trapped);
            builder.Append(StringFormat.ToStringAlignment("Trap Type (value)"));
            builder.Append((UInt32)this.TrapType);
            //builder.Append(StringFormat.ToStringAlignment("Trap Type (description)"));
            //builder.Append(this.TrapType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Walls (value)"));
            builder.Append((UInt16)this.Walls);
            builder.Append(StringFormat.ToStringAlignment("Walls (enumerated)"));
            builder.Append(this.GenerateWallsFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Visited"));
            builder.Append(this.Visited);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which GuiFlags flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GenerateWallsFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Walls & MazeWalls.East) == MazeWalls.East, MazeWalls.East.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Walls & MazeWalls.North) == MazeWalls.North, MazeWalls.North.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Walls & MazeWalls.South) == MazeWalls.South, MazeWalls.South.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Walls & MazeWalls.West) == MazeWalls.West, MazeWalls.West.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}