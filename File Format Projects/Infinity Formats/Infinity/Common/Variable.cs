using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common
{
    /// <summary>Represents a saved variable</summary>
    public class Variable : IInfinityFormat
    {
        #region Constants
        /// <summary>The size on one variable on disk.</summary>
        public const Int32 StructSize = 84;
        #endregion


        #region Fields
        /// <summary>Represents the variable name</summary>
        /// <remarks>32 bytes in length</remarks>
        public ZString Name { get; set; }

        /// <summary>Type of variable this represents</summary>
        public VariableType Type { get; set; }

        /// <summary>Type of resource referenced, if any</summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>Represents the DWORD value of the variable</summary>
        public UInt32 DWORDValue { get; set; }

        /// <summary>Represents the integer value of the variable</summary>
        public Int32 IntegerValue { get; set; }

        /// <summary>8-byte floating point value</summary>
        public Double FloatingPointValue { get; set; }

        /// <summary>Script name value</summary>
        public ZString ScriptNameValue { get; set; }

        /// <summary>32 bytes of padding after the value. Probably intended for script name.</summary>
        public Byte[] Padding { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Name = new ZString();
            this.ScriptNameValue = new ZString();
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

            Byte[] buffer = ReusableIO.BinaryRead(input, Variable.StructSize);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish, 32);
            this.Type = (VariableType)ReusableIO.ReadUInt16FromArray(buffer, 32);
            this.ResourceType = (ResourceType)ReusableIO.ReadUInt16FromArray(buffer, 34);
            this.DWORDValue = ReusableIO.ReadUInt32FromArray(buffer, 36);
            this.IntegerValue = ReusableIO.ReadInt32FromArray(buffer, 40);
            this.FloatingPointValue = ReusableIO.ReadDoubleFromArray(buffer, 44);
            this.ScriptNameValue.Source = ReusableIO.ReadStringFromByteArray(buffer, 52, CultureConstants.CultureCodeEnglish, 32);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
            ReusableIO.WriteUInt16ToStream((UInt16)this.Type, output);
            ReusableIO.WriteUInt16ToStream((UInt16)this.ResourceType, output);
            ReusableIO.WriteUInt32ToStream(this.DWORDValue, output);
            ReusableIO.WriteInt32ToStream(this.IntegerValue, output);
            ReusableIO.WriteDoubleToStream(this.FloatingPointValue, output);
            ReusableIO.WriteStringToStream(this.ScriptNameValue.Source, output, CultureConstants.CultureCodeEnglish, false, 32);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Format("'{0}'", this.Name.Value));
            builder.Append(StringFormat.ToStringAlignment("Variable type"));
            builder.Append(this.Type.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Resource type"));
            builder.Append(this.ResourceType.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("DWORD Value"));
            builder.Append(this.DWORDValue);
            builder.Append(StringFormat.ToStringAlignment("Integer Value"));
            builder.Append(this.IntegerValue);
            builder.Append(StringFormat.ToStringAlignment("Floating point Value"));
            builder.Append(this.FloatingPointValue);
            builder.Append(StringFormat.ToStringAlignment("Script Name Value"));
            builder.Append(String.Format("'{0}'", this.ScriptNameValue.Value));

            return builder.ToString();
        }
        #endregion
    }
}