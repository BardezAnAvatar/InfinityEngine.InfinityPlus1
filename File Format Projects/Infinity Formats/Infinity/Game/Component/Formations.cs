using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents the selected formations and those displayed as selectable</summary>
    public class Formations : IInfinityFormat
    {
        #region Fields
        /// <summary>Indicates the formation index currently selected</summary>
        public Int16 SelectedIndex { get; set; }

        /// <summary>Represents the 5 formations selectable by the User Interface</summary>
        public Formation[] UserInterface { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.UserInterface = new Formation[5];
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

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="buffer">Input buffer to read from</param>
        /// <param name="position">Position in the buffer to start reading from</param>
        public virtual void Read(Byte[] buffer, Int32 position)
        {
            this.ReadBody(buffer, position);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 12);

            this.SelectedIndex = ReusableIO.ReadInt16FromArray(buffer, 0);
            this.UserInterface[0] = (Formation)ReusableIO.ReadInt16FromArray(buffer, 2);
            this.UserInterface[1] = (Formation)ReusableIO.ReadInt16FromArray(buffer, 4);
            this.UserInterface[2] = (Formation)ReusableIO.ReadInt16FromArray(buffer, 6);
            this.UserInterface[3] = (Formation)ReusableIO.ReadInt16FromArray(buffer, 8);
            this.UserInterface[4] = (Formation)ReusableIO.ReadInt16FromArray(buffer, 10);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="buffer">Input buffer to read from</param>
        /// <param name="position">Position in the buffer to start reading from</param>
        public virtual void ReadBody(Byte[] buffer, Int32 position)
        {
            this.Initialize();

            this.SelectedIndex = ReusableIO.ReadInt16FromArray(buffer, position + 0);
            this.UserInterface[0] = (Formation)ReusableIO.ReadInt16FromArray(buffer, position + 2);
            this.UserInterface[1] = (Formation)ReusableIO.ReadInt16FromArray(buffer, position + 4);
            this.UserInterface[2] = (Formation)ReusableIO.ReadInt16FromArray(buffer, position + 6);
            this.UserInterface[3] = (Formation)ReusableIO.ReadInt16FromArray(buffer, position + 8);
            this.UserInterface[4] = (Formation)ReusableIO.ReadInt16FromArray(buffer, position + 10);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteInt16ToStream(this.SelectedIndex, output);
            ReusableIO.WriteInt16ToStream((Int16)(this.UserInterface[0]), output);
            ReusableIO.WriteInt16ToStream((Int16)(this.UserInterface[1]), output);
            ReusableIO.WriteInt16ToStream((Int16)(this.UserInterface[2]), output);
            ReusableIO.WriteInt16ToStream((Int16)(this.UserInterface[3]), output);
            ReusableIO.WriteInt16ToStream((Int16)(this.UserInterface[4]), output);
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a human-readable String representation of the formations</summary>
        /// <returns>A human-readable String representation of the formations</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("Selected index"));
            builder.Append(this.SelectedIndex);
            builder.Append(StringFormat.ToStringAlignment("Formation # 1"));
            builder.Append(this.UserInterface[0].GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Formation # 2"));
            builder.Append(this.UserInterface[1].GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Formation # 3"));
            builder.Append(this.UserInterface[2].GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Formation # 4"));
            builder.Append(this.UserInterface[3].GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Formation # 5"));
            builder.Append(this.UserInterface[4].GetDescription());

            return builder.ToString();
        }
        #endregion
    }
}