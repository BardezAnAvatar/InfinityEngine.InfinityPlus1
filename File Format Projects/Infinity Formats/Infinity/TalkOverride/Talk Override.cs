using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkOverride
{
    /// <summary>Manager for Talk override header and table classes</summary>
    public class TalkOverride
    {
        #region Fields
        /// <summary>The strref header</summary>
        public TalkOverrideHeader Header { get; set; }

        /// <summary>The strref entries</summary>
        public TalkOverrideTable Table { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new TalkOverrideHeader();
            this.Table = new TalkOverrideTable();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="inputToh">Input stream to read header from</param>
        /// <param name="inputTot">Input stream to read table from</param>
        public virtual void Read(Stream inputToh, Stream inputTot)
        {
            this.ReadBody(inputToh, inputTot);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="inputToh">Input stream to read header from</param>
        /// <param name="inputTot">Input stream to read table from</param>
        public virtual void ReadBody(Stream inputToh, Stream inputTot)
        {
            this.Initialize();

            this.Header.Read(inputToh);
            this.Table.Read(inputTot);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="outputToh">Stream to write header to</param>
        /// <param name="outputTot">Stream to write table to</param>
        public virtual void Write(Stream outputToh, Stream outputTot)
        {
            this.Header.Write(outputToh);
            this.Table.Write(outputTot);
        }
        #endregion


        #region Interface
        /// <summary>Adds a string reference and its text to the talk overrides</summary>
        /// <param name="strref">String reference index to add</param>
        /// <param name="text">Text for the strref to add</param>
        public virtual void AddString(Int32 strref, String text)
        {
            Int32 offset = this.WriteStringToTable(text);

            //check for header entry
            TalkOverrideHeaderEntry entry = null;
            Boolean found = false;
            foreach (TalkOverrideHeaderEntry overridden in this.Header.Entries)
                if (overridden.StringReferenceIndex == strref)
                {
                    entry = overridden;
                    found = true;
                    break;
                }

            if (entry == null)
            {
                entry = new TalkOverrideHeaderEntry();
                entry.Initialize();
                entry.StringReferenceIndex = strref;
            }

            entry.AssociatedTextPresent = (text != String.Empty);
            entry.TableOffset = offset;

            if (!found)
                this.Header.Entries.Add(entry);
        }

        /// <summary>Gets a string from the talk override by its string reference index</summary>
        /// <param name="strref">Strref to lookup</param>
        /// <returns>String at the associated index</returns>
        public virtual String GetString(Int32 strref)
        {
            //check for header entry
            TalkOverrideHeaderEntry entry = null;
            foreach (TalkOverrideHeaderEntry overridden in this.Header.Entries)
                if (overridden.StringReferenceIndex == strref)
                {
                    entry = overridden;
                    break;
                }

            if (entry == null)
                throw new IndexOutOfRangeException("No such String Reference Index exists.");

            return this.GetStringFromTable(entry.TableOffset);
        }
        #endregion


        #region Helpers
        /// <summary>Retireves a concatenated String from the entries in the talk override table</summary>
        /// <param name="offset">Inital offset into the talk override table</param>
        /// <returns>The concatenated String</returns>
        protected String GetStringFromTable(Int32 offset)
        {
            Int32 index = offset / TalkOverrideTableEntry.StructSize;
            String strreffed;
            using (MemoryStream buffer = new MemoryStream(512))
            {
                do
                {
                    TalkOverrideTableEntry tableEntry = this.Table.Entries[index];
                    buffer.Write(tableEntry.Text, 0, 512);

                    if (tableEntry.ContinuedOffset == -1)
                        index = -1;
                    else
                        index = tableEntry.ContinuedOffset / TalkOverrideTableEntry.StructSize;
                }
                while (index > -1);

                Byte[] binBuffer = buffer.GetBuffer();
                strreffed = ReusableIO.ReadStringFromByteArray(binBuffer, 0, CultureConstants.CultureCodeEnglish, Convert.ToInt32(buffer.Length));
            }

            return ZString.GetZString(strreffed);
        }

        /// <summary>Writes a string to the talk override table</summary>
        /// <param name="strreffed">String to write</param>
        /// <returns>Offset into the table where the first String's entry begins</returns>
        protected Int32 WriteStringToTable(String strreffed)
        {
            Int32 initialOffset = this.Table.Entries.Count * TalkOverrideTableEntry.StructSize;
            Int32 previousOffset = -1;

            using (MemoryStream buffer = new MemoryStream(512))
            {
                ReusableIO.WriteStringToStream(strreffed, buffer, CultureConstants.CultureCodeEnglish, true);

                Int64 length = buffer.Length;

                do
                {
                    TalkOverrideTableEntry entry = new TalkOverrideTableEntry();
                    entry.Initialize();
                    entry.PreviousOffset = previousOffset;
                    entry.NextFreeRegion = 0;
                    
                    //get 512 bytes
                    Byte[] data = new Byte[512];
                    buffer.Read(data, 0, 512);
                    entry.Text = data;

                    //set previous to the current.
                    previousOffset = this.Table.Entries.Count * TalkOverrideTableEntry.StructSize;
                    
                    //add to table
                    this.Table.Entries.Add(entry);

                    length -= 512;

                    if (length > 0)
                        entry.ContinuedOffset = this.Table.Entries.Count * TalkOverrideTableEntry.StructSize;
                    else
                        entry.ContinuedOffset = -1;
                }
                while (length > 0);
            }

            return initialOffset;
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Talk Override:");

            foreach (TalkOverrideHeaderEntry entry in this.Header.Entries)
            {
                builder.Append(entry.ToString());
                if (entry.AssociatedTextPresent)
                {
                    builder.Append(StringFormat.ToStringAlignment("Associated String"));
                    builder.AppendLine(String.Format("'{0}'", this.GetStringFromTable(entry.TableOffset)));
                }
                builder.AppendLine(String.Empty);
            }

            return builder.ToString();
        }
        #endregion
    }
}