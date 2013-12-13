using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents a Planescape: Torment header</summary>
    public class TormentHeader : HeaderBase
    {
        #region Constants
        /// <summary>Constant representing the length of the GAM header divergent data structures</summary>
        private const Int32 TormentTrailerLength = 36;

        /// <summary>Constant representing the length of the GAM header divergent data structure's trailing padding</summary>
        private const Int32 TormentPaddingLength = 64;
        #endregion


        #region Fields
        /// <summary>Represents an offset to Maze data (Modron maze?)</summary>
        public Int32 MazeOffset { get; set; }

        /// <summary>Offset within the stream to the kill variables</summary>
        public Int32 VariableKillsOffset { get; set; }

        /// <summary>Count of the kill variables</summary>
        public UInt32 VariableKillsCount { get; set; }

        /// <summary>Offset to Bestiary GAM data</summary>
        public Int32 BestiaryOffset { get; set; }

        /// <summary>A Third area value, observed to be identical to the other two in most of my saves</summary>
        /// <remarks>
        ///     Original research:
        ///     My sav #30 inside the maze shows AR3006D as main and current; AR3006 as the third
        ///     Also, My save #72 shows main and current as AR1333; the third is AR13G3.
        ///     Also, My save #73 shows main and current as AR13FD; the third is AR13FD. (where I find Nordom?)
        ///     Also, My save #74 shows main and current as AR1342; the third is AR13G3.
        ///     Also, My save #75&76 shows main and current as AR13WZ; the third is AR13WZ. (where I slay the evil wizard?)
        ///     Also, My save #77 shows main and current as AR13FY; the third is AR13FY.
        ///     
        ///     The third does not have a corresponding area code, but it does match a MOS file.
        /// </remarks>
        public ResourceReference MapImage { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.MapImage = new ResourceReference(ResourceType.Mosaic);
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            this.ReadBaseHeader(input);

            Byte[] buffer = ReusableIO.BinaryRead(input, TormentHeader.TormentTrailerLength);

            this.MazeOffset = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.Reputation = ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.CurrentAreaCode.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 8, CultureConstants.CultureCodeEnglish);
            this.VariableKillsOffset = ReusableIO.ReadInt32FromArray(buffer, 16);
            this.VariableKillsCount = ReusableIO.ReadUInt32FromArray(buffer, 20);
            this.BestiaryOffset = ReusableIO.ReadInt32FromArray(buffer, 24);
            this.MapImage.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 28, CultureConstants.CultureCodeEnglish);
            this.Padding = ReusableIO.BinaryRead(input, TormentHeader.TormentPaddingLength);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);
            this.WriteBaseHeader(output);
            ReusableIO.WriteInt32ToStream(this.MazeOffset, output);
            ReusableIO.WriteUInt32ToStream(this.Reputation, output);
            ReusableIO.WriteStringToStream(this.CurrentAreaCode.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.VariableKillsOffset, output);
            ReusableIO.WriteUInt32ToStream(this.VariableKillsCount, output);
            ReusableIO.WriteInt32ToStream(this.BestiaryOffset, output);
            ReusableIO.WriteStringToStream(this.MapImage.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.Write(this.Padding, 0, this.Padding.Length);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the header</summary>
        /// <returns>A human-readable String representation of the header</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());
            builder.Append(StringFormat.ToStringAlignment("Offset to Maze data"));
            builder.Append(this.MazeOffset);
            builder.Append(StringFormat.ToStringAlignment("Reputation"));
            builder.Append(this.Reputation);
            builder.Append(StringFormat.ToStringAlignment("Current Area"));
            builder.Append(this.CurrentAreaCode.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Offset to kill variables"));
            builder.Append(this.VariableKillsOffset);
            builder.Append(StringFormat.ToStringAlignment("Count of kill variables"));
            builder.Append(this.VariableKillsCount);
            builder.Append(StringFormat.ToStringAlignment("Offset to bestiar entries"));
            builder.Append(this.BestiaryOffset);
            builder.Append(StringFormat.ToStringAlignment("Map resource"));
            builder.Append(this.MapImage.ZResRef);
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding));

            return builder.ToString();
        }
        #endregion
    }
}