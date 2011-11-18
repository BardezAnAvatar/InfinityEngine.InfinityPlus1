using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.ReusableCode;
using Bardez.Projects.Win32.Audio;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Music
{
    /// <summary>Represents a MUS playlist file of ACM media</summary>
    public class Playlist : IInfinityFormat
    {
        #region Fields/Propertes
        /// <summary>Exposes a WaveFormatEx instance for playback, storage, etc.</summary>
        public WaveFormatEx WaveFormat { get; set; }

        /// <summary>Indicates whether or not the playlist is currently logicaly playing</summary>
        public Boolean IsPlaying { get; set; }

        /// <summary>Indicates whether or not the playlist playback has been interrupted</summary>
        public Boolean Interrupted { get; set; }

        /// <summary>Index into entries indicating the present Entry</summary>
        protected Int32 presentIndex;

        /// <summary>Lock on the instance fo stuff like setting interrupts, etc.</summary>
        protected Object instanceLock;

        /// <summary>Dictionary of decoded acm audio files' samples</summary>
        protected Dictionary<String, Byte[]> fileSamples;

        /// <summary>Playlist prefix (prefix of files with thi</summary>
        public String PlaylistName { get; set; }

        /// <summary>List of playlist entries</summary>
        /// <remarks>The List Count will also indicate the number of entries so plays a dual function</remarks>
        public List<PlaylistEntry> Entries { get; set; }

        /// <summary>Represents the remainder of data within the file, comments, essentially</summary>
        public String TrailingData { get; set; }

        /// <summary>Accesses the root file path to start from.</summary>
        /// <value>Represents to a root directory that would contain this playlist and the silence audio file.</value>
        public String RootFilePath { get; set; }

        /// <summary>Accesses the number of entries within this playlist</summary>
        public Int32 EntryCount
        {
            get { return (this.Entries == null) ? 0 : this.Entries.Count; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public Playlist()
        {
            this.instanceLock = new Object();
            this.IsPlaying = false;     //the playlist is not playing
            this.Interrupted = false;   //the playlist is not playing
        }

        /// <summary>Initializes the playlist data</summary>
        public void Initialize()
        {
            this.Entries = new List<PlaylistEntry>();
            this.fileSamples = new Dictionary<String, Byte[]>();
            this.presentIndex = -1;
        }
        #endregion

        #region IInfinityFormat I/O methods
        /// <summary>This public method reads file format data structure from the input stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the input stream, after the header has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();
            StreamReader reader = new StreamReader(input);
            String line = reader.ReadLine();

            if (line == null)
                throw new InvalidDataException("Could not read the playlist prefix. Value was null.");
            else
            {
                this.PlaylistName = line.Trim();

                line = reader.ReadLine();
                if (line == null)
                    throw new InvalidDataException("Could not read the playlist count. Value was null.");
                else
                {
                    Int32 itemCount = -1;
                    try
                    {
                        itemCount = Int32.Parse(line);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException(String.Format("Could not read the playlist entry count. Value found was '{0}'.", line), ex);
                    }

                    //read all the playlist entries
                    for (Int32 i = 0; i < itemCount; ++i)
                    {
                        PlaylistEntry pe = new PlaylistEntry();
                        pe.Read(reader);
                        this.Entries.Add(pe);
                    }
                }
            }
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            StreamWriter writer = new StreamWriter(output);
            writer.WriteLine(this.PlaylistName);    //prefix
            writer.WriteLine(this.EntryCount);      //count of entries
            for (Int32 entryIndex = 0; entryIndex < this.EntryCount; ++entryIndex)
            {
                this.Entries[entryIndex].Write(writer);
            }

            if (this.TrailingData != null)
                writer.WriteLine(this.TrailingData);

            writer.Flush();
        }
        #endregion

        #region Playlist Interfacing
        /// <summary>Gets the Byte[] of the next audio sequence</summary>
        /// <returns>PCM samples data</returns>
        public Byte[] GetNext()
        {
            lock (this.instanceLock)
            {
                if (this.presentIndex < 0)
                    this.presentIndex = 0;

                //get pesent entry
                PlaylistEntry entry = this.Entries[presentIndex];

                String sampleFile = null;
                if (this.Interrupted)
                    sampleFile = entry.InterruptTag;
                else if (!String.IsNullOrEmpty(entry.Next))
                {
                    sampleFile = entry.Next;
                    this.presentIndex = this.Entries.FindIndex(playlist => playlist.Entry == entry.Next);
                }
                else
                {
                    this.presentIndex++;    //move onto the next entry
                    if (this.presentIndex >= this.EntryCount)   //if there is no 'next', loop back to the first entry
                        this.presentIndex = 0;

                    sampleFile = this.Entries[this.presentIndex].Entry;
                }

                Byte[] returnArray;
                if (this.fileSamples.ContainsKey(sampleFile))
                    returnArray = this.fileSamples[sampleFile];
                else
                    returnArray = new Byte[0];

                return returnArray;
            }
        }

        /// <summary>Sets the next index to be a</summary>
        public void Interrupt()
        {
            lock (this.instanceLock)
            {
                this.Interrupted = true;
            }
        }

        /// <summary>Takes he initialized playlist and sets it to be ready for playback</summary>
        public void StartPlayList()
        {
            lock (this.instanceLock)
            {
                this.presentIndex = -1;
                this.Interrupted = false;
            }
        }
        #endregion

        #region AudioFile Reading
        /// <summary>Opens an ACM file for each ACM audio file and stores its sample data</summary>
        public void ReadPlayListItems()
        {
            this.fileSamples = new Dictionary<String, Byte[]>();    //re-initialize
            List<String> files = this.GetPlayListFileSuffix();

            //now read and load them all
            foreach (String suffix in files)
            {
                String fileName = this.GetPlaylistEntryFilename(suffix);
                if (File.Exists(fileName))
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        AcmAudioFile acmFile = new AcmAudioFile();
                        acmFile.Read(stream);
                        this.fileSamples.Add(suffix, acmFile.GetSampleData());

                        if (this.WaveFormat == null)
                            this.WaveFormat = acmFile.GetWaveFormat();
                    }
                }
                else
                    this.fileSamples.Add(suffix, new Byte[0]);
            }

            //set indexes
            this.presentIndex = -1;
        }

        /// <summary>Gets a de-duplicated List of Strings indicating CM file paths</summary>
        /// <returns>A List of acm file paths</returns>
        protected List<String> GetPlayListFileSuffix()
        {
            List<String> files = new List<String>();
            foreach (PlaylistEntry entry in this.Entries)
            {
                if (entry != null)
                {
                    if (!String.IsNullOrEmpty(entry.Entry) && !files.Contains(entry.Entry))
                        files.Add(entry.Entry);

                    if (!String.IsNullOrEmpty(entry.Next) && !files.Contains(entry.Next))
                        files.Add(entry.Next);

                    if (!String.IsNullOrEmpty(entry.InterruptTag) && !files.Contains(entry.InterruptTag))
                        files.Add(entry.InterruptTag);
                }
            }

            return files;
        }

        /// <summary>Gets a filename for a playlist playback ACM item</summary>
        /// <param name="file">File to get the path for. Can be a suffix or a special blank file</param>
        /// <returns>Appendable string to get an appropriate file system accessor path</returns>
        protected String GetPlaylistEntryFilename(String file)
        {
            String path = null;

            switch (file)
            {
                case "SPC":
                case "SPC1":
                    path = file;
                    break;
                case "MX0000A":
                    path = "MX0000\\MX0000A";
                    break;
                case "MX9000A":
                    path = "MX9000A\\MX9000A";
                    break;
                default:
                    path = this.PlaylistName + "\\" + this.PlaylistName + file;
                    break;
            }

            path = this.RootFilePath + "\\" + path + ".acm";
            return path;
        }
        #endregion

        #region ToString()
        /// <summary>Overridden method that will generate a human-readable representation of the contents of this Playlist Entry</summary>
        /// <returns>A string suitable for display to a human for readability</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StringFormat.ToStringAlignment("Playlist name"));
            sb.Append(this.PlaylistName);
            sb.Append(StringFormat.ToStringAlignment("Playlist entry count"));
            sb.AppendLine(this.EntryCount.ToString());

            sb.Append(String.Format("{0, -10}|", "Entry"));
            sb.Append(String.Format("{0, -10}|", "Next"));
            sb.AppendLine("Interrupt");

            foreach (PlaylistEntry entry in this.Entries)
                sb.AppendLine(entry.ToString());

            sb.AppendLine(this.TrailingData);

            return sb.ToString();
        }
        #endregion
    }
}