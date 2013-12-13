using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Store.Components
{
    /// <summary>Structure representing a item available by a store for purchase</summary>
    public class AvailableItemPst : AvailableItem
    {
        /// <summary>Binary size of the struct on disk</summary>
        public new static Int32 StructSize
        {
            get { return 88; }
        }

        #region Members
        /// <summary>String reference who, when read, should include a scripting condition</summary>
        protected StringReference trigger;

        /// <summary>Unknown 56 bytes (unused?)</summary>
        protected Byte[] unknown;
        #endregion

        #region Properties
        /// <summary>tring reference who, when read, should include a scripting condition</summary>
        public StringReference Trigger
        {
            get { return this.trigger; }
            set { this.trigger = value; }
        }

        /// <summary>Unknown 56 bytes (unused?)</summary>
        public Byte[] Unknown
        {
            get { return this.unknown; }
            set { this.unknown = value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public AvailableItemPst() : base()
        {
            this.trigger = null;
            this.unknown = null;
        }
 
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();
            this.trigger = new StringReference();
            this.unknown = new Byte[56];
        }
        #endregion

        #region Abstract IO methods
        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            base.ReadBody(input);
            Byte[] itemForSale = ReusableIO.BinaryRead(input, 60);

            this.trigger.StringReferenceIndex = ReusableIO.ReadInt32FromArray(itemForSale, 0);
            Array.Copy(itemForSale, 4, this.unknown, 0, 56);
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);
            ReusableIO.WriteInt32ToStream(trigger.StringReferenceIndex, output);
            output.Write(this.unknown, 0, 56);
        }
        #endregion

        #region ToString() Helpers
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.GetStringRepresentation());
            builder.Append(StringFormat.ToStringAlignment("Trigger StrRef"));
            builder.Append(this.trigger.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Unknown"));
            builder.Append(StringFormat.ByteArrayToHexString(this.unknown));

            return builder.ToString();
        }
        #endregion
    }
}