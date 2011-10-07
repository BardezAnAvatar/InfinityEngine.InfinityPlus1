using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.ChitinKey
{
    /// <summary>This class is a representation of the Chitin.key file</summary>
    public class ChitinKey1 : IDeepCloneable
    {
        #region Protected members
        /// <summary>This member contains the chitin.key header information.</summary>
        protected ChitinKeyHeader header;

        /// <summary>This member is the collection of BIF entries.</summary>
        protected List<ChitinKeyBifEntry> entriesBif;

        /// <summary>This member is the collection of resource1 entries.</summary>
        protected List<ChitinKeyResourceEntry> entriesResource;

        /// <summary>This flag indicates whether or not to read the string references into memory.</summary>
        protected Boolean storeBifNamesInMemory;
        
        /// <summary>This member stores the path of the chitin.key file</summary>
        protected String keyFilePath;
        #endregion

        #region Properties
        /// <summary>This property exposes the chitin.key header information.</summary>
        public ChitinKeyHeader Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        /// <summary>This property exposes the collection of BIF entries.</summary>
        public List<ChitinKeyBifEntry> EntriesBif
        {
            get { return this.entriesBif; }
            set { this.entriesBif = value; }
        }

        /// <summary>This property exposes the collection of resource1 entries.</summary>
        public List<ChitinKeyResourceEntry> EntriesResource
        {
            get { return this.entriesResource; }
            set { this.entriesResource = value; }
        }

        /// <summary>This property exposes the flag which indicates whether or not to read the string references into memory.</summary>
        public Boolean StoreBifNamesInMemory
        {
            get { return this.storeBifNamesInMemory; }
            set { this.storeBifNamesInMemory = value; }
        }

        /// <summary>This property exposes the path of the chitin.key file</summary>
        public String KeyFilePath
        {
            get { return this.keyFilePath; }
            set { this.keyFilePath = value; }
        }
        #endregion

        #region Constructors
        /// <summary>Default constructor</summary>
        public ChitinKey1()
            : this(true)
        {
        }

        /// <summary>Constructor setting whether to store BIF names in memory</summary>
        /// <param name="StoreInMemory">Boolean indicating whether or not to store BIF name strings in memory when read</param>
        public ChitinKey1(Boolean StoreInMemory)
            : this(StoreInMemory, null)
        {
        }

        /// <summary>Constructor setting whether to store BIF names in memory as well as the target chitin key path</summary>
        /// <param name="StoreInMemory">Boolean indicating whether or not to store BIF name strings in memory when read</param>
        /// <param name="FilePath">String representing the path to a chitin key file</param>
        public ChitinKey1(Boolean StoreInMemory, String FilePath)
        {
            this.header = new ChitinKeyHeader();
            this.entriesBif = new List<ChitinKeyBifEntry>();
            this.entriesResource = new List<ChitinKeyResourceEntry>();
            this.keyFilePath = FilePath;
            this.storeBifNamesInMemory = StoreInMemory;
        }
        #endregion

        /// <summary>Reads the chitin.key file using the locally defined file path</summary>
        public void Read()
        {
            using (Stream fileStream = ReusableIO.OpenFile(this.keyFilePath))
            {
                this.header.Read(fileStream);       //read header

                //seek to the BIF entries
                ReusableIO.SeekIfAble(fileStream, this.header.OffsetBif, SeekOrigin.Begin);

                //read the BIF entries
                for (Int32 i = 0; i < this.header.CountBif; ++i)
                {
                    ChitinKeyBifEntry bifEntry = new ChitinKeyBifEntry();
                    bifEntry.Read(fileStream);
                    this.entriesBif.Add(bifEntry);  //add to the collection
                }
                                
                //seek to the resource1 entries
                ReusableIO.SeekIfAble(fileStream, this.header.OffsetResource, SeekOrigin.Begin);

                //read the resource1 entries
                for (Int32 i = 0; i < this.header.CountResource; ++i)
                {
                    ChitinKeyResourceEntry resource = new ChitinKeyResourceEntry();
                    resource.Read(fileStream);
                    this.entriesResource.Add(resource);  //add to the collection
                }

                //read BIF names?
                if (this.storeBifNamesInMemory)
                {
                    for (Int32 i = 0; i < this.entriesBif.Count; ++i)
                    {
                        if (this.entriesBif[i] != null)
                        {
                            this.entriesBif[i].ReadBifName(fileStream, "en-US");
                        }
                    }
                }
            }
        }

        /// <summary>Reads the KEY file, assigning the key file path to the specified file path</summary>
        /// <param name="FilePath">String describing the path to the KEY file.</param>
        public void Read(String filePath)
        {
            this.keyFilePath = filePath;
            this.Read();
        }

        /// <summary>This method clears the header, the BIF entries and the resource1 entries</summary>
        public void Clear()
        {
            this.header = null;
            this.entriesBif = new List<ChitinKeyBifEntry>();
            this.entriesResource = new List<ChitinKeyResourceEntry>();
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
            for (Int32 i = 0; i < this.entriesBif.Count; ++i)
            {
                clone.entriesBif[i].OffsetBifFileName = Convert.ToUInt32(Offset);
                clone.entriesBif[i].LengthBifFileName = Convert.ToUInt16(ReusableIO.WriteStringToByteArray(clone.entriesBif[i].BifFileName, "en-US").Length /*+ 1*/); //it already returns a NUL-padding
                Offset += clone.entriesBif[i].LengthBifFileName + padding;
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
            for (Int32 i = 0; i < this.entriesBif.Count; ++i)
            {
                ReusableIO.SeekIfAble(Output, Offset, SeekOrigin.Begin);
                ReusableIO.WriteStringToStream(clone.entriesBif[i].BifFileName, Output, "en-US", true);
                Offset += clone.entriesBif[i].LengthBifFileName + padding;
            }
        }

        /// <summary>This method takes in a cloned ChitinKey object and updates its offsets and also computes the destination file size.</summary>
        /// <param name="clone">Cloned instance of ChitinKey object</param>
        /// <param name="bifNameLocation">Enumerator describing where to write the BIF name strings</param>
        /// <param name="bifStringPadding">Unsigned integer describing the number of bytes worth of padding between BIF name strings</param>
        /// <param name="sectionPadding">Unsigned integer describing the number of bytes worth of padding between KEY sections</param>
        /// <returns>An Int64 containing the destination file length, after all offsets have been updated.</returns>
        protected Int64 ComputeChitinKeyOffsets(ChitinKey1 clone, ChitinKeyBifStringsLocation bifNameLocation, UInt32 bifStringPadding, UInt32 sectionPadding)
        {
            //logically write header
            Int64 Offset = 24;

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BeforeBifEntries)
                this.ComputeBifNameOffsets(clone, ref Offset, bifStringPadding, sectionPadding);

            //move to bif offsets
            if (clone.header.OffsetBif < Offset + sectionPadding)
                clone.header.OffsetBif = Convert.ToUInt32(Offset + sectionPadding);

            Offset = clone.header.OffsetBif;

            //now compute each BIFF entry
            Offset += (12 * clone.entriesBif.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BetweenBifAndResource)
                this.ComputeBifNameOffsets(clone, ref Offset, bifStringPadding, sectionPadding);

            //compute resource1 entries
            if (clone.header.OffsetResource < Offset + sectionPadding)
                clone.header.OffsetResource = Convert.ToUInt32(Offset + sectionPadding);

            Offset = clone.header.OffsetResource;

            //now compute each resource1 entry
            Offset += (14 * clone.entriesResource.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.TrailResource)
                this.ComputeBifNameOffsets(clone, ref Offset, bifStringPadding, sectionPadding);

            return Offset;
        }

        /// <summary>This method takes in a cloned ChitinKey object and writes it to the destination buffer.</summary>
        /// <param name="buffer">Stream to write the ChitinKey data to</param>
        /// <param name="clone">Cloned instance of ChitinKey object</param>
        /// <param name="bifNameLocation">Enumerator describing where to write the BIF name strings</param>
        /// <param name="bifStringPadding">Unsigned integer describing the number of bytes worth of padding between BIF name strings</param>
        /// <param name="sectionPadding">Unsigned integer describing the number of bytes worth of padding between KEY sections</param>
        protected void WriteChitinKeyToBuffer(Stream buffer, ChitinKey1 clone, ChitinKeyBifStringsLocation bifNameLocation, UInt32 bifStringPadding, UInt32 sectionPadding)
        {
            //write cloned header
            clone.header.Write(buffer);

            //reset offset
            Int64 Offset = 24;

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BeforeBifEntries)
                this.WriteBifNames(buffer, clone, ref Offset, bifStringPadding, sectionPadding);

            //move to bif offsets
            if (clone.header.OffsetBif < Offset + sectionPadding)
                clone.header.OffsetBif = Convert.ToUInt32(Offset + sectionPadding);

            //seek
            Offset = clone.header.OffsetBif;
            ReusableIO.SeekIfAble(buffer, Offset, SeekOrigin.Begin);

            //now write each BIFF entry
            foreach (ChitinKeyBifEntry entry in clone.entriesBif)
                entry.Write(buffer);

            Offset += (12 * clone.entriesBif.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.BetweenBifAndResource)
                this.WriteBifNames(buffer, clone, ref Offset, bifStringPadding, sectionPadding);

            //write resource1 entries
            if (clone.header.OffsetResource < Offset + sectionPadding)
                clone.header.OffsetResource = Convert.ToUInt32(Offset + sectionPadding);

            //seek
            Offset = clone.header.OffsetResource;
            ReusableIO.SeekIfAble(buffer, Offset, SeekOrigin.Begin);

            //now write each resource1 entry
            foreach (ChitinKeyResourceEntry entry in clone.entriesResource)
                entry.Write(buffer);

            Offset += (14 * clone.entriesResource.Count);

            //do strings here?
            if (bifNameLocation == ChitinKeyBifStringsLocation.TrailResource)
                this.WriteBifNames(buffer, clone, ref Offset, bifStringPadding, sectionPadding);
        }

        /// <summary>This method clones the current ChitinKey object and writes it to the destination file.</summary>
        /// <param name="bifNameLocation">Enumerator describing where to write the BIF name strings</param>
        /// <param name="bifStringPadding">Unsigned integer describing the number of bytes worth of padding between BIF name strings</param>
        /// <param name="sectionPadding">Unsigned integer describing the number of bytes worth of padding between KEY sections</param>
        public void Write(ChitinKeyBifStringsLocation bifNameLocation, UInt32 bifStringPadding, UInt32 sectionPadding)
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
                using (FileStream destination = ReusableIO.OpenFile(this.keyFilePath, FileMode.Create, FileAccess.Write))
                {
                    buffer.Seek(0L, SeekOrigin.Begin);
                    destination.Write(buffer.GetBuffer(), 0, Convert.ToInt32(Offset));
                }
            }
        }

        /// <summary>This method clones the current ChitinKey object and writes it to the destination file, maintaining the structure targeting reverse compatability.</summary>
        public void Write()
        {
            //middle names, no padding
            this.Write(ChitinKeyBifStringsLocation.BetweenBifAndResource, 0U, 0U);
        }

        /// <summary>This method takes in a target file name, clones the current ChitinKey object and writes it to the destination file.</summary>
        /// <param name="filePath">String describing the path to the KEY file.</param>
        /// <param name="bifNameLocation">Enumerator describing where to write the BIF name strings</param>
        /// <param name="bifStringPadding">Unsigned integer describing the number of bytes worth of padding between BIF name strings</param>
        /// <param name="sectionPadding">Unsigned integer describing the number of bytes worth of padding between KEY sections</param>
        public void Write(String filePath, ChitinKeyBifStringsLocation bifNameLocation, UInt32 bifStringPadding, UInt32 sectionPadding)
        {
            this.keyFilePath = filePath;
            this.Write(bifNameLocation, bifStringPadding, sectionPadding);
        }

        /// <summary>
        ///     This method takes in a target file name (assigning the key file path to the specified file path), clones the current
        ///     ChitinKey object and writes it to the destination file, maintaining the structure targeting reverse compatability.
        /// </summary>
        /// <param name="filePath">String describing the path to the KEY file.</param>
        public void Write(String filePath)
        {
            //this is the call that one would make to ensure compatability
            this.Write(filePath, ChitinKeyBifStringsLocation.BetweenBifAndResource, 0U, 0U);
        }

        /// <summary>Performs a deep copy of the object and returns another, separate instace of it.</summary>
        /// <returns>A deeply copied separate instance clone of the insance called from.</returns>
        public IDeepCloneable Clone()
        {
            ChitinKey1 clone = new ChitinKey1();
            clone.header = this.header.Clone() as ChitinKeyHeader;
            clone.entriesBif = this.entriesBif.Clone<ChitinKeyBifEntry>();
            clone.entriesResource = this.entriesResource.Clone<ChitinKeyResourceEntry>();
            clone.storeBifNamesInMemory = this.storeBifNamesInMemory;
            clone.keyFilePath = this.keyFilePath;

            return clone;
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("TextLocationKeyHeader:");
            builder.Append("\n\tHeader:\n");
            builder.Append(this.header.ToString());
            builder.Append("\n\tkeyFilePath:            ");
            builder.Append(this.keyFilePath);
            builder.Append("\n\n\tstoreBifNamesInMemory:  ");
            builder.Append(this.storeBifNamesInMemory);

            builder.Append("\n\n\tBIF entries:\n");
            foreach (ChitinKeyBifEntry entry in this.entriesBif)
                builder.Append(entry.ToString());

            builder.Append("\n\n\tResource entries:\n");
            foreach (ChitinKeyResourceEntry entry in this.entriesResource)
                builder.Append(entry.ToString());
            builder.Append("\n\n");

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
                builder.Append("\n\tHeader:\n");
                builder.Append(this.header.ToString());
                builder.Append("\n\tkeyFilePath:            ");
                builder.Append(this.keyFilePath);
                builder.Append("\n\tstoreBifNamesInMemory:  ");
                builder.Append(this.storeBifNamesInMemory);

                builder.Append("\n\n\tBIF entries:\n");
                for (Int32 i = 0; i < this.entriesBif.Count; ++i)
                {
                    builder.Append(this.entriesBif[i].ToString(false));
                    Int32 count = this.entriesResource.Where(x => x.ResourceLocator.BifIndex == i).Count();
                    builder.Append("\tResource count:            ");
                    builder.Append(count);
                    builder.Append("\n\n\t");
                }

                builder.Append("\n\n\tResource entries:");
                //foreach (ChitinKeyResourceEntry entry in this.entriesResource)
                //{
                //    builder.Append(entry.ToString());
                //}
                var z = from resource in this.entriesResource
                        group resource by resource.ResourceType
                        into grouping
                        orderby grouping.Count() descending
                        select new { grouping.Key, Count = grouping.Count() };

                foreach (var x in z)
                {
                    builder.Append("\n\tType: ");
                    builder.Append(String.Format("{0,-19}", x.Key.ToString("G")));
                    builder.Append("\tCount: ");
                    builder.Append(x.Count);
                }

                ////curious:
                //builder.Append("\n\n\t\t\t*Curious*:");
                //builder.Append("\n\n\tInvalid Resource entries:\n");
                //var q = from resource1 in this.entriesResource
                //        where ((Int16)resource1.ResourceType) == 0
                //        select resource1;

                //foreach (ChitinKeyResourceEntry entry in q)
                //{
                //    builder.Append(entry.ToString());
                //    builder.Append(this.entriesBif[entry.BifIndex].ToString());
                //}

                builder.Append("\n\n");
                data = builder.ToString();
            }

            return data;
        }
    }
}