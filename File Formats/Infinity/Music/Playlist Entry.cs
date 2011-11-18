using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Music
{
    /// <summary>Represents a line-item in the playlist</summary>
    /// <remarks>The exist special silence ACM files. These will need to be special cases.</remarks>
    public class PlaylistEntry //: IInfinityFormat //this is text, so no IInfinityFormat
    {
        #region Fields
        /// <summary>The file siffix to play as well as the entry's identifier referenced by other entries</summary>
        /// <value>Pulled from the first 10 characters</value>
        /// <remarks>Can be a special value, indicating a root silence file, varies on the game</remarks>
        protected String entry;

        /// <summary>The file siffix of the next entry to play</summary>
        /// <remarks>If this value is 'empty', it will move on to the next entry</remarks>
        /// <value>Pulled from the second 10 characters</value>
        protected String next;

        /// <summary>File to play next if the playlist is interrupted (i.e.: Battle ending or starting)</summary>
        /// <value>Pulled from the characters after 20</value>
        protected String interruptTag;
        #endregion

        #region Properties
        /// <summary>The file siffix to play as well as the entry's identifier referenced by other entries</summary>
        /// <value>Pulled from the first 10 characters</value>
        /// <remarks>Can be a special value, indicating a root silence file, varies on the game</remarks>
        public String Entry
        {
            get
            {
                String value = this.entry;
                if (value != null)
                    value = this.entry.Trim();
                return value;
            }
            set
            {
                if (value != null)
                    value = value.Trim();

                this.entry = value;
            }
        }

        /// <summary>Next entry to play</summary>
        /// <remarks>If this value is 'empty', it will move on to the next entry</remarks>
        /// <value>Pulled from the second 10 characters</value>
        public String Next
        {
            get
            {
                String value = this.next;
                if (value != null)
                    value = this.next.Trim();
                return value;
            }
            set
            {
                if (value != null)
                    value = value.Trim();

                this.next = value;
            }
        }

        /// <summary>File to play next if the playlist is interrupted (i.e.: Battle ending or starting)</summary>
        /// <value>Pulled from the characters after 20</value>
        public String InterruptTag
        {
            get
            {
                String value = this.interruptTag;
                if (value != null && value.StartsWith("@TAG "))
                    value = value.Substring(5, value.Length - 5);

                return value;
            }
            set { this.interruptTag = value; }
        }

        /// <summary>Entry used for output, restricted to exactly 10 characters in length</summary>
        protected String OutputEntry
        {
            get { return PlaylistEntry.FormatToCharacterLength10(this.entry); }
        }

        /// <summary>Next entry used for output, restricted to exactly 10 characters in length</summary>
        protected String OutputNext
        {
            get { return PlaylistEntry.FormatToCharacterLength10(this.next); }
        }

        /// <summary>Next entry used for output, restricted to exactly 10 characters in length</summary>
        protected String OutputInterruptTag
        {
            get { return String.IsNullOrEmpty(this.interruptTag) ? String.Empty : ("@TAG " + this.InterruptTag); }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public PlaylistEntry() { }

        /// <summary>Definition constructor</summary>
        /// <param name="playFile">ACM file to start with</param>
        /// <param name="nextFile">Next file to play if uniterrupted</param>
        /// <param name="interrupt">Next file to play if interrupted</param>
        public PlaylistEntry(String playFile, String nextFile, String interrupt)
        {
            this.Entry = playFile;
            this.Next = nextFile;
            this.InterruptTag = interrupt;
        }
        #endregion

        #region I/O methods
        /// <summary>This public method reads file format data structure from the input stream. Reads the whole data structure.</summary>
        /// <param name="input">StreamReader to read from.</param>
        public void Read(StreamReader input)
        {
            String[] entries = this.ReadEntryStrings(input.ReadLine());
            this.Entry = entries[0];
            this.Next = entries[1];
            this.InterruptTag = entries[2];

            //if (line != null)
            //{
            //    this.Entry = line.Substring(0, Math.Min(line.Length, 10));

            //    if (line.Length > 10)
            //    {
            //        this.Next = line.Substring(10, Math.Min(10, (line.Length - 10)));

            //        if (line.Length > 20)
            //            this.InterruptTag = line.Substring(20, line.Length - 20);
            //    }
            //}
        }

        /// <summary>This public method writes the file format data structure to the output stream.</summary>
        /// <param name="output">StreamWriter to write to</param>
        public void Write(StreamWriter output)
        {
            output.Write(this.OutputEntry);
            output.Write(this.OutputNext);
            output.WriteLine(this.OutputInterruptTag);
        }
        #endregion

        /// <summary>Limits a value to 10 characters, outputting exactly 10, padding with spaces as necessary</summary>
        /// <param name="source">Source String to adjust</param>
        /// <returns>A 10-character string containing the (possibly truncated) contents of source</returns>
        protected static String FormatToCharacterLength10(String source)
        {
            String value = String.Format("{0, -10}", (source ?? String.Empty));
            if (value.Length > 10)
                value = value.Substring(0, 10);

            return value;
        }

        /// <summary>Overridden method that will generate a human-readable representation of the contents of this Playlist Entry</summary>
        /// <returns>A string suitable for display to a human for readability</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.OutputEntry);
            builder.Append('|');
            builder.Append(this.OutputNext);
            builder.Append('|');
            builder.Append(this.OutputInterruptTag);
            return builder.ToString();
        }

        /// <summary>Parses a line passed into the Playlist entry</summary>
        /// <param name="line">Expected to be a single-lined String</param>
        /// <returns>A String array of items three values in length, the entry, the next and the interrupt.</returns>
        protected String[] ReadEntryStrings(String line)
        {
            //Get the string tokens
            List<String> playlistEntryTokens = PlaylistEntry.GetEntryStringTokens(line);

            //now you have a list of arbitrary length. Could be 0 to infinity, really.

            String[] returnEntries = new String[3] { String.Empty, String.Empty, String.Empty };

            for (Int32 i = 0; i < playlistEntryTokens.Count; ++i)
            {
                //assume the string cannot be null. where would we be adding a null string above?

                //copy first entry
                if (i == 0 && playlistEntryTokens[i].StartsWith("#"))
                    break;
                else if (i == 0 && !playlistEntryTokens[i].StartsWith("#")) //entry
                    returnEntries[0] = playlistEntryTokens[i];
                else if (i > 0 && playlistEntryTokens[i].StartsWith("@")) //tag
                {
                    ++i;    //look at next entry
                    if (i < playlistEntryTokens.Count && !playlistEntryTokens[i].StartsWith("#"))
                        returnEntries[2] = playlistEntryTokens[i];
                    else
                        break;
                }
                else    //in between interrupt and entry
                {
                    if (!playlistEntryTokens[i].StartsWith("#"))
                        returnEntries[1] = playlistEntryTokens[i];  //should keep overwriting itself, ya?
                    else
                        break;
                }
            }

            return returnEntries;
        }

        /// <summary>Gets the String tokens from the line read</summary>
        /// <param name="line">Line to parse</param>
        /// <returns>A List of String tokens read</returns>
        private static List<String> GetEntryStringTokens(String line)
        {
            List<String> playlistEntryTokens = new List<String>();

            //parse the line
            if (line != null)
            {
                StringBuilder item = new StringBuilder();

                //Analyze each character
                for (Int32 i = 0; i < line.Length; ++i)
                {
                    if (Char.IsWhiteSpace(line[i]) && item.Length > 0)
                    {
                        playlistEntryTokens.Add(item.ToString());
                        item = new StringBuilder();
                    }
                    else if (!Char.IsWhiteSpace(line[i]))    //not whitspace
                        item.Append(line[i]);
                }

                //add if anything still in the buffer (did not end on whitespace)
                if (item.Length > 0)
                    playlistEntryTokens.Add(item.ToString());
            }

            return playlistEntryTokens;
        }
    }
}