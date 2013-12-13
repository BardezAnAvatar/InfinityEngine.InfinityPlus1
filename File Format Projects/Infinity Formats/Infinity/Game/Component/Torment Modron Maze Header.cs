using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents the header fields of the Modron Maze save data</summary>
    public class TormentModronMazeHeader
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 56;
        #endregion


        #region Fields
        /// <summary>Dimension (width) of the maze</summary>
        public UInt32 SizeX { get; set; }

        /// <summary>Dimension (height) of the maze</summary>
        public UInt32 SizeY { get; set; }

        /// <summary>Coordinate (width) of Nordom's room in the maze</summary>
        public UInt32 NodromX { get; set; }

        /// <summary>Coordinate (height) of Nordom's room in the maze</summary>
        public UInt32 NodromY { get; set; }

        /// <summary>Coordinate (width) of the Main Hall in the maze</summary>
        public UInt32 MainHallX { get; set; }

        /// <summary>Coordinate (height) of the Main Hall in the maze</summary>
        public UInt32 MainHallY { get; set; }

        /// <summary>Coordinate (width) of the foyer in the maze</summary>
        public UInt32 FoyerX { get; set; }

        /// <summary>Coordinate (height) of the foyer in the maze</summary>
        public UInt32 FoyerY { get; set; }

        /// <summary>Coordinate (width) of the Engine Room in the maze</summary>
        public UInt32 EngineRoomX { get; set; }

        /// <summary>Coordinate (height) of the Engine Room in the maze</summary>
        public UInt32 EngineRoomY { get; set; }

        /// <summary>Count of traps in the maze</summary>
        public UInt32 TrapCount { get; set; }

        /// <summary>Indicates that the maze has been initialized</summary>
        public UInt32 Initialized { get; set; } //set to 1

        public UInt32 Unknown1 { get; set; }

        public UInt32 Unknown2 { get; set; }
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

            Byte[] buffer = ReusableIO.BinaryRead(input, TormentModronMazeHeader.StructSize);

            this.SizeX = ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.SizeY = ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.NodromX = ReusableIO.ReadUInt32FromArray(buffer, 8);
            this.NodromY = ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.MainHallX = ReusableIO.ReadUInt32FromArray(buffer, 16);
            this.MainHallY = ReusableIO.ReadUInt32FromArray(buffer, 20);
            this.FoyerX = ReusableIO.ReadUInt32FromArray(buffer, 24);
            this.FoyerY = ReusableIO.ReadUInt32FromArray(buffer, 28);
            this.EngineRoomX = ReusableIO.ReadUInt32FromArray(buffer, 32);
            this.EngineRoomY = ReusableIO.ReadUInt32FromArray(buffer, 36);
            this.TrapCount = ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.Initialized = ReusableIO.ReadUInt32FromArray(buffer, 44);
            this.Unknown1 = ReusableIO.ReadUInt32FromArray(buffer, 48);
            this.Unknown2 = ReusableIO.ReadUInt32FromArray(buffer, 52);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.SizeX, output);
            ReusableIO.WriteUInt32ToStream(this.SizeY, output);
            ReusableIO.WriteUInt32ToStream(this.NodromX, output);
            ReusableIO.WriteUInt32ToStream(this.NodromY, output);
            ReusableIO.WriteUInt32ToStream(this.MainHallX, output);
            ReusableIO.WriteUInt32ToStream(this.MainHallY, output);
            ReusableIO.WriteUInt32ToStream(this.FoyerX, output);
            ReusableIO.WriteUInt32ToStream(this.FoyerY, output);
            ReusableIO.WriteUInt32ToStream(this.EngineRoomX, output);
            ReusableIO.WriteUInt32ToStream(this.EngineRoomY, output);
            ReusableIO.WriteUInt32ToStream(this.TrapCount, output);
            ReusableIO.WriteUInt32ToStream(this.Initialized, output);
            ReusableIO.WriteUInt32ToStream(this.Unknown1, output);
            ReusableIO.WriteUInt32ToStream(this.Unknown2, output);
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

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Maze Header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Size X"));
            builder.Append(this.SizeX);
            builder.Append(StringFormat.ToStringAlignment("Size Y"));
            builder.Append(this.SizeY);
            builder.Append(StringFormat.ToStringAlignment("Nodrom X"));
            builder.Append(this.NodromX);
            builder.Append(StringFormat.ToStringAlignment("Nodrom Y"));
            builder.Append(this.NodromY);
            builder.Append(StringFormat.ToStringAlignment("Main Hall X"));
            builder.Append(this.MainHallX);
            builder.Append(StringFormat.ToStringAlignment("Main Hall Y"));
            builder.Append(this.MainHallY);
            builder.Append(StringFormat.ToStringAlignment("Foyer X"));
            builder.Append(this.FoyerX);
            builder.Append(StringFormat.ToStringAlignment("Foyer Y"));
            builder.Append(this.FoyerY);
            builder.Append(StringFormat.ToStringAlignment("Engine Room X"));
            builder.Append(this.EngineRoomX);
            builder.Append(StringFormat.ToStringAlignment("Engine Room Y"));
            builder.Append(this.EngineRoomY);
            builder.Append(StringFormat.ToStringAlignment("Trap Count"));
            builder.Append(this.TrapCount);
            builder.Append(StringFormat.ToStringAlignment("Initialized"));
            builder.Append(this.Initialized);
            builder.Append(StringFormat.ToStringAlignment("Unknown # 1"));
            builder.Append(this.Unknown1);
            builder.Append(StringFormat.ToStringAlignment("Unknown # 2"));
            builder.Append(this.Unknown2);

            return builder.ToString();
        }
        #endregion
    }
}