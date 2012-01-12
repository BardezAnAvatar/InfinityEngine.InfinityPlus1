using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey
{
    /// <summary>This class is a representation of the Chitin.key file</summary>
    public class ChitinKey1 : IDeepCloneable, IInfinityFormat
    {
        #region Fields
        /// <summary>This property exposes the chitin.key Header information.</summary>
        public ChitinKeyHeader Header { get; set; }

        /// <summary>This property exposes the collection of BIF entries.</summary>
        public List<ChitinKeyBifEntry> EntriesBif { get; set; }

        /// <summary>This property exposes the collection of resource1 entries.</summary>
        public List<ChitinKeyResourceEntry> EntriesResource { get; set; }

        /// <summary>This property exposes the flag which indicates whether or not to read the string references into memory.</summary>
        public Boolean StoreBifNamesInMemory { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public ChitinKey1() : this(true) { }

        /// <summary>Constructor setting whether to store BIFF names in memory</summary>
        /// <param name="StoreInMemory">Boolean indicating whether or not to store BIF name strings in memory when read</param>
        public ChitinKey1(Boolean StoreInMemory)
        {
            this.Header = null;
            this.EntriesBif = null;
            this.EntriesResource = null;
            this.StoreBifNamesInMemory = StoreInMemory;
        }

        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new ChitinKeyHeader();
            this.EntriesBif = new List<ChitinKeyBifEntry>();
            this.EntriesResource = new List<ChitinKeyResourceEntry>();
        }
        #endregion

        #region IO method implemetations
        /// <summary>Reads the chitin.key file using the provided stream</summary>
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
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new ChitinKeyHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();          //reset data instances

            this.Header.Read(input);       //read Header

            //seek to the BIFF entries
            ReusableIO.SeekIfAble(input, this.Header.OffsetBif, SeekOrigin.Begin);

            //read the BIFF entries
            for (Int32 i = 0; i < this.Header.CountBif; ++i)
            {
                ChitinKeyBifEntry bifEntry = new ChitinKeyBifEntry();
                bifEntry.Read(input);
                this.EntriesBif.Add(bifEntry);  //add to the collection
            }

            //seek to the resource1 entries
            ReusableIO.SeekIfAble(input, this.Header.OffsetResource, SeekOrigin.Begin);

            //read the resource1 entries
            for (Int32 i = 0; i < this.Header.CountResource; ++i)
            {
                ChitinKeyResourceEntry resource = new ChitinKeyResourceEntry();
                resource.Read(input);
                this.EntriesResource.Add(resource);  //add to the collection
            }

            //read BIFF names?
            if (this.StoreBifNamesInMemory)
            {
                for (Int32 i = 0; i < this.EntriesBif.Count; ++i)
                {
                    if (this.EntriesBif[i] != null)
                        this.EntriesBif[i].ReadBifName(input, "en-US");
                }
            }
        }

        /// <summary>This method takes in a cloned ChitinKey object and writes its BIF name strings to the output stream</summary>
        /// <param name="Output">Stream to write to</param>
        /// <param name="clone">Cloned instance of ChitinKey object</param>
        /// <param name="Offset">Int64 holding the current offset within the file, increased by the string sizes and string padding after call</param>
        /// <param name="padding">Number of bytes with skip forward after each string is written</param>
        /// <param name="sectionPadding">Number of bytes to skip forward before writing the strings</param>
        protected void WriteBifNames(Stream Output, ChitinKey1 clone, ref Int64 Offset, UInt32 padding, UInt32 sectionPadding)
        {
            Offset += sectionPadding;

            //write out each string
            for (Int32 i = 0; i < this.EntriesBif.Count; ++i)
            {
                ReusableIO.SeekIfAble(Output, Offset, SeekOrigin.Begin);
                ReusableIO.WriteStringToStream(clone.EntriesBif[i].BifFileName.Source, Output, "en-US", true);
                Offset += clone.EntriesBif[i].LengthBifFileName + padding;
            }
        }

        /// <summary>This method clones the current ChitinKey object and writes it to the destination file, maintaining the structure targeting reverse compatability.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            //middle names, no padding
            this.Write(output, ChitinKeyBifStringsLocation.BetweenBifAndResource, 0U, 0U);
        }

        /// <summary>This method clones the current ChitinKey object and writes it to the destination file.</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="bifNameLocation">Enumerator describing where to write the BIF name strings</param>
        /// <param name="bifStringPadding">Unsigned integer describing the number of bytes worth of padding between BIF name strings</param>
        /// <param name="sectionPadding">Unsigned integer describing the number of bytes worth of padding between KEY sections</param>
        public virtual void Write(Stream output, ChitinKeyBifStringsLocation bifNameLocation, UInt32 bifStringPadding, UInt32 sectionPadding)
        {
            //Clone key file to write to disk
            ChitinKey1 clone = this.Clone() as ChitinKey1;

            //a logical first pass, to calculate offsets
            Int64 Offset = this.ComputeChitinKeyOffsets(clone, bifNameLocation, bifStringPadding, sectionPadding);
            
            //We now have the determined file length, Offset. Now, write the file. Since the file may skip around a bit, buffer it to the length of offset.
            using (MemoryStream buffer = new MemoryStream(Convert.ToInt32(Offset)))
            {
                this.WriteChitinKeyToBuffer(buffer, clone, bifNameLocation, bifStringPadding, sectionPadding);

                //write buffer to file, close buffer and file
                buffer.Seek(0L, SeekOrigin.Begin);
                output.Write(buffer.GetBuffer(), 0, Convert.ToInt32(Offset));
            }
        }

        /// <summary>This method takes in a cloned ChitinKey object and writes it to the destination buffer.</summary>
        /// <param name="buffer">Stream to write the ChitinKey data to</param>
        /// <param name="clone">Cloned instance of ChitinKey object</param>
        /// <param name="bifNameLocation">Enumerator describing where to write the BIF name strings</param>
        /// <param name="bifStringPadding">Unsigned integer describing the number of bytes worth of padding between BIF name strings</param>
        /// <param name="sectionPadding">Unsigned integer describing the number of bytes worth of padding between KEY sections</param>
        protected virtual void WriteChitinKeyToBuffer(Stream buffer, ChitinKey1 clone, ChitinKeyBifStringsLocation bifNameLocation, UInt32 bifStringPadding, UInt32 sectionPadding)
        {
            //write cloned Header
            clone.Header.Write(buffer);

            //reset offset
            Int64 Offset = 24;

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BeforeBifEntries)
                this.WriteBifNames(buffer, clone, ref Offset, bifStringPadding, sectionPadding);

            //move to bif offsets
            if (clone.Header.OffsetBif < Offset + sectionPadding)
                clone.Header.OffsetBif = Convert.ToUInt32(Offset + sectionPadding);

            //seek
            Offset = clone.Header.OffsetBif;
            ReusableIO.SeekIfAble(buffer, Offset, SeekOrigin.Begin);

            //now write each BIFF entry
            foreach (ChitinKeyBifEntry entry in clone.EntriesBif)
                entry.Write(buffer);

            Offset += (12 * clone.EntriesBif.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BetweenBifAndResource)
                this.WriteBifNames(buffer, clone, ref Offset, bifStringPadding, sectionPadding);

            //write resource1 entries
            if (clone.Header.OffsetResource < Offset + sectionPadding)
                clone.Header.OffsetResource = Convert.ToUInt32(Offset + sectionPadding);

            //seek
            Offset = clone.Header.OffsetResource;
            ReusableIO.SeekIfAble(buffer, Offset, SeekOrigin.Begin);

            //now write each resource1 entry
            foreach (ChitinKeyResourceEntry entry in clone.EntriesResource)
                entry.Write(buffer);

            Offset += (14 * clone.EntriesResource.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.TrailResource)
                this.WriteBifNames(buffer, clone, ref Offset, bifStringPadding, sectionPadding);
        }
        #endregion

        #region Helpers
        /// <summary>This method clears the Header, the BIF entries and the resource1 entries</summary>
        public void Clear()
        {
            this.Header = null;
            this.EntriesBif = new List<ChitinKeyBifEntry>();
            this.EntriesResource = new List<ChitinKeyResourceEntry>();
        }

        /// <summary>This method takes in a cloned ChitinKey object and updates its offsets and also computes the destination file size.</summary>
        /// <param name="clone">Cloned instance of ChitinKey object</param>
        /// <param name="bifNameLocation">Enumerator describing where to write the BIF name strings</param>
        /// <param name="bifStringPadding">Unsigned integer describing the number of bytes worth of padding between BIF name strings</param>
        /// <param name="sectionPadding">Unsigned integer describing the number of bytes worth of padding between KEY sections</param>
        /// <returns>An Int64 containing the destination file length, after all offsets have been updated.</returns>
        protected Int64 ComputeChitinKeyOffsets(ChitinKey1 clone, ChitinKeyBifStringsLocation bifNameLocation, UInt32 bifStringPadding, UInt32 sectionPadding)
        {
            //logically write Header
            Int64 Offset = 24;

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BeforeBifEntries)
                this.ComputeBifNameOffsets(clone, ref Offset, bifStringPadding, sectionPadding);

            //move to bif offsets
            if (clone.Header.OffsetBif < Offset + sectionPadding)
                clone.Header.OffsetBif = Convert.ToUInt32(Offset + sectionPadding);

            Offset = clone.Header.OffsetBif;

            //now compute each BIFF entry
            Offset += (12 * clone.EntriesBif.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BetweenBifAndResource)
                this.ComputeBifNameOffsets(clone, ref Offset, bifStringPadding, sectionPadding);

            //compute resource1 entries
            if (clone.Header.OffsetResource < Offset + sectionPadding)
                clone.Header.OffsetResource = Convert.ToUInt32(Offset + sectionPadding);

            Offset = clone.Header.OffsetResource;

            //now compute each resource1 entry
            Offset += (14 * clone.EntriesResource.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.TrailResource)
                this.ComputeBifNameOffsets(clone, ref Offset, bifStringPadding, sectionPadding);

            return Offset;
        }

        /// <summary>This method takes in a cloned ChitinKey object and updates its BIF name offsets and also computes the destination file size after each such string.</summary>
        /// <param name="clone">Cloned instance of ChitinKey object</param>
        /// <param name="Offset">Int64 holding the current logical offset within the file, increased by the string sizes and string padding after call</param>
        /// <param name="padding">Number of bytes with which to append to the offset after each string's offset is computed</param>
        /// <param name="sectionPadding">Number of bytes to prepend to the offset before writing the strings</param>
        protected void ComputeBifNameOffsets(ChitinKey1 clone, ref Int64 Offset, UInt32 padding, UInt32 sectionPadding)
        {
            Offset += sectionPadding;

            //write out each string
            for (Int32 i = 0; i < this.EntriesBif.Count; ++i)
            {
                clone.EntriesBif[i].OffsetBifFileName = Convert.ToUInt32(Offset);
                clone.EntriesBif[i].LengthBifFileName = Convert.ToUInt16(ReusableIO.WriteStringToByteArray(clone.EntriesBif[i].BifFileName.Source, "en-US").Length /*+ 1*/); //it already returns a NUL-padding
                Offset += clone.EntriesBif[i].LengthBifFileName + padding;
            }
        }

        /// <summary>Performs a deep copy of the object and returns another, separate instace of it.</summary>
        /// <returns>A deeply copied separate instance clone of the insance called from.</returns>
        public IDeepCloneable Clone()
        {
            ChitinKey1 clone = new ChitinKey1();
            clone.Header = this.Header.Clone() as ChitinKeyHeader;
            clone.EntriesBif = this.EntriesBif.Clone<ChitinKeyBifEntry>();
            clone.EntriesResource = this.EntriesResource.Clone<ChitinKeyResourceEntry>();
            clone.StoreBifNamesInMemory = this.StoreBifNamesInMemory;

            return clone;
        }
        #endregion

        #region ToString(...)
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("TextLocationKeyHeader:");
            builder.Append(StringFormat.ToStringAlignment("Header"));
            builder.Append(this.Header.ToString());
            builder.Append(StringFormat.ToStringAlignment("StoreBifNamesInMemory"));
            builder.Append(this.StoreBifNamesInMemory);

            builder.AppendLine(StringFormat.ToStringAlignment("BIF entries"));
            foreach (ChitinKeyBifEntry entry in this.EntriesBif)
                builder.Append(entry.ToString());

            builder.Append(StringFormat.ToStringAlignment("Resource entries"));
            foreach (ChitinKeyResourceEntry entry in this.EntriesResource)
                builder.Append(entry.ToString());

            return builder.ToString();
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <param name="detail">Boolean indicating whether or not to print full detail</param>
        /// <returns>A String containing the member data line by line</returns>
        public String ToString(Boolean detail)
        {
            String data;
            if (detail)
                data = this.ToString();
            else
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("TextLocationKeyHeader:");
                builder.Append(StringFormat.ToStringAlignment("Header"));
                builder.Append(this.Header.ToString());
                builder.Append(StringFormat.ToStringAlignment("StoreBifNamesInMemory"));
                builder.Append(this.StoreBifNamesInMemory);

                builder.Append(StringFormat.ToStringAlignment("BIFF entries"));
                for (Int32 i = 0; i < this.EntriesBif.Count; ++i)
                {
                    builder.Append(this.EntriesBif[i].ToString(false));
                    Int32 count = this.EntriesResource.Where(x => x.ResourceLocator.BifIndex == i).Count();
                    builder.Append(StringFormat.ToStringAlignment("Resource count"));
                    builder.Append(count);
                }

                builder.Append(StringFormat.ReturnAndIndent("Resource type entries:", 0));

                var z = from resource in this.EntriesResource
                        group resource by resource.ResourceType
                        into grouping
                        orderby grouping.Count() descending
                        select new { grouping.Key, Count = grouping.Count() };

                foreach (var x in z)
                {
                    builder.Append(StringFormat.ToStringAlignment("Type"));
                    builder.Append(String.Format("{0,-19}", x.Key.ToString("G")));
                    builder.Append(StringFormat.ToStringAlignment("Count"));
                    builder.AppendLine(x.Count.ToString());
                }

                data = builder.ToString();
            }

            return data;
        }
        #endregion
    }
}