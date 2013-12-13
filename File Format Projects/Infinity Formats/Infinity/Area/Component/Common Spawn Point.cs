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
    /// <summary>Spawn point for BG, BG2, PS:T and IWD</summary>
    public class CommonSpawnPoint : SpawnPointBase
    {
        #region Fields
        /// <summary>Spawn flags</summary>
        public SpawnFlags Flags { get; set; }
        #endregion


        #region IO method implemetations
        /// <summary>Reads the spawn method from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadSpawnMethod(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 2);
            this.Flags = (SpawnFlags)ReusableIO.ReadUInt16FromArray(buffer, 0);
        }

        /// <summary>Writes the spawn method to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        public override void WriteSpawnMethod(Stream output)
        {
            ReusableIO.WriteUInt16ToStream((UInt16)this.Flags, output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>Generates a human-readable String representing the spawn method</summary>
        /// <returns>A human-readable String representing the spawn method</returns>
        protected override String GenerateSpawnMethodString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Spawn flags (value)"));
            builder.Append((UInt16)this.Flags);
            builder.Append(StringFormat.ToStringAlignment("Spawn (enumeration)"));
            builder.Append(this.GetSpawnFlagsEnumerationString());

            return builder.ToString();
        }

        /// <summary>Gets a human-readable enumeration String of set Difficulty enumeration values</summary>
        /// <returns>A human-readable enumeration String of set Difficulty enumeration values</returns>
        protected String GetSpawnFlagsEnumerationString()
        {
            StringBuilder builder = new StringBuilder();

            StringFormat.AppendSubItem(builder, (this.Flags & SpawnFlags.HaltSpawn) == SpawnFlags.HaltSpawn, SpawnFlags.HaltSpawn.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & SpawnFlags.OneTime) == SpawnFlags.OneTime, SpawnFlags.OneTime.GetDescription());
            StringFormat.AppendSubItem(builder, (this.Flags & SpawnFlags.HasSpawned) == SpawnFlags.HasSpawned, SpawnFlags.HasSpawned.GetDescription());

            String result = builder.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}