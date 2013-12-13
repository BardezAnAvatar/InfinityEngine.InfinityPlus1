using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component
{
    /// <summary>Represents a projectile trap set by the party</summary>
    public class ProjectileTrap : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 28;
        #endregion


        #region Fields
        /// <summary>Projectile resource</summary>
        public ResourceReference Projectile { get; set; }

        /// <summary>Offset to the effect block for this trap</summary>
        public Int32 OffsetEffectBlock { get; set; }

        /// <summary>Size of the effect block for this trap</summary>
        public Int16 SizeEffectBlock { get; set; }

        /// <summary>Value indicating the type of missile</summary>
        /// <remarks>Matches to MISSILE.IDS</remarks>
        public Int16 MissileType { get; set; }

        /// <summary>Ticks until next trigger check</summary>
        public Int16 TicksUntilCheck { get; set; }

        /// <summary>Number of triggers still remaining</summary>
        public Int16 TriggersRemaining { get; set; }

        /// <summary>Location of the trap</summary>
        public Point Location { get; set; }

        /// <summary>Z co-ordinate of the trap</summary>
        public UInt16 LocationZ { get; set; }

        /// <summary>Type targeted by this trap</summary>
        /// <remarks>Matches to EA.IDS</remarks>
        public Byte EnemyAllyTarget { get; set; }

        /// <summary>Party member number that created this trap</summary>
        public Byte PartyMemberCreated { get; set; }

        /* additional byte indicating caster id? In-memory only? */
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Projectile = new ResourceReference();
            this.Location = new Point();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, ProjectileTrap.StructSize);

            this.Projectile.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.OffsetEffectBlock = ReusableIO.ReadInt32FromArray(buffer, 8);
            this.SizeEffectBlock = ReusableIO.ReadInt16FromArray(buffer, 12);
            this.MissileType = ReusableIO.ReadInt16FromArray(buffer, 14);
            this.TicksUntilCheck = ReusableIO.ReadInt16FromArray(buffer, 16);
            this.TriggersRemaining = ReusableIO.ReadInt16FromArray(buffer, 18);
            this.Location.X = ReusableIO.ReadUInt16FromArray(buffer, 20);
            this.Location.Y = ReusableIO.ReadUInt16FromArray(buffer, 22);
            this.LocationZ = ReusableIO.ReadUInt16FromArray(buffer, 24);
            this.EnemyAllyTarget = buffer[26];
            this.PartyMemberCreated = buffer[27];
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Projectile.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteInt32ToStream(this.OffsetEffectBlock, output);
            ReusableIO.WriteInt16ToStream(this.SizeEffectBlock, output);
            ReusableIO.WriteInt16ToStream(this.MissileType, output);
            ReusableIO.WriteInt16ToStream(this.TicksUntilCheck, output);
            ReusableIO.WriteInt16ToStream(this.TriggersRemaining, output);
            ReusableIO.WriteUInt16ToStream(this.Location.X, output);
            ReusableIO.WriteUInt16ToStream(this.Location.Y, output);
            ReusableIO.WriteUInt16ToStream(this.LocationZ, output);
            output.WriteByte(this.EnemyAllyTarget);
            output.WriteByte(this.PartyMemberCreated);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
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
        /// <param name="entryIndex">Known spells entry #</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 entryIndex)
        {
            return StringFormat.ReturnAndIndent(String.Format("Projectile trap # {0}:", entryIndex), 0) + this.GetStringRepresentation();
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "Projectile trap:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the data structure's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Projectile resource"));
            builder.Append(String.Format("'{0}'", this.Projectile.ResRef));
            builder.Append(StringFormat.ToStringAlignment("Offset to the Effect block"));
            builder.Append(this.OffsetEffectBlock);
            builder.Append(StringFormat.ToStringAlignment("Size of the Effect block"));
            builder.Append(this.SizeEffectBlock);
            builder.Append(StringFormat.ToStringAlignment("Missile type"));
            builder.Append(this.MissileType);
            builder.Append(StringFormat.ToStringAlignment("Ticks until next check"));
            builder.Append(this.TicksUntilCheck);
            builder.Append(StringFormat.ToStringAlignment("Triggers remaining"));
            builder.Append(this.TriggersRemaining);
            builder.Append(StringFormat.ToStringAlignment("Location"));
            builder.Append(this.Location.ToString());
            builder.Append(StringFormat.ToStringAlignment("Z co-ordinate"));
            builder.Append(this.LocationZ);
            builder.Append(StringFormat.ToStringAlignment("Enemy/Ally target"));
            builder.Append(this.EnemyAllyTarget);
            builder.Append(StringFormat.ToStringAlignment("Creator party member number "));
            builder.Append(this.PartyMemberCreated);

            return builder.ToString();
        }
        #endregion
    }
}