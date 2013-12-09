using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;

using Bardez.Projects.Configuration.Ini.Component;
using Bardez.Projects.Configuration.Ini.Line;

namespace Bardez.Projects.Configuration.Ini
{
    /// <summary>*.ini configuration file class representation</summary>
    /// <remarks>
    ///     The reason this class retains the source isbecause I want to retain the ability to re-generate the source file when doing output.
    /// </remarks>
    public class IniConfiguration
    {
        #region Fields
        /// <summary>List of sections</summary>
        protected List<IniSection> sections;

        /// <summary>Culture expected to read ini files from</summary>
        protected CultureInfo culture;
        #endregion


        #region Properties
        /// <summary>List of sections</summary>
        public List<IniSection> SectionsList
        {
            get { return this.sections; }
            set { this.sections = value; }
        }

        /// <summary>Culture expected to read ini files from</summary>
        public CultureInfo Culture
        {
            get { return this.culture; }
            set { this.culture = value; }
        }

        /// <summary>Exposes the sections as a dictionary of NameValueCollections</summary>
        public Dictionary<String, Dictionary<String, IniPropertyLine>> Sections
        {
            get { return this.NormalizeSections(); }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public IniConfiguration()
        {
            this.InitializeSections();
            this.culture = new CultureInfo("en-us");
        }

        /// <summary>Constructor that reads from a stream</summary>
        /// <param name="input">Input Stream to read from</param>
        public IniConfiguration(Stream input) : this()
        {
            this.Read(input);
            this.culture = new CultureInfo("en-us");
        }

        /// <summary>Constructor that reads from a stream</summary>
        /// <param name="path">Input file path to read from</param>
        public IniConfiguration(String path) : this()
        {
            this.Read(path);
            this.culture = new CultureInfo("en-us");
        }

        /// <summary>Constructor that reads from a stream</summary>
        /// <param name="culture">Culture expected to be reading from</param>
        public IniConfiguration(CultureInfo culture) : this()
        {
            this.culture = culture;
        }

        /// <summary>Initializes the sections list</summary>
        protected void InitializeSections()
        {
            this.sections = new List<IniSection>();
        }
        #endregion


        #region Read
        /// <summary>Reads the *.ini file from the input stream</summary>
        /// <param name="input">Input Stream to read from</param>
        public void Read(Stream input)
        {
            StreamReader reader = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding(this.culture.TextInfo.ANSICodePage);
                reader = new StreamReader(input, encoding);
                this.ReadFromReader(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        /// <summary>Reads the *.ini file from the input stream</summary>
        /// <param name="path">Input file path to read from</param>
        public void Read(String path)
        {
            StreamReader reader = null;
            try
            {
                if (File.Exists(path))
                {
                    Encoding encoding = Encoding.GetEncoding(this.culture.TextInfo.ANSICodePage);
                    reader = new StreamReader(path, encoding);
                    this.ReadFromReader(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        /// <summary>Reads the configuration section from a StreamReader</summary>
        /// <param name="reader">StreamReader object to read from</param>
        protected void ReadFromReader(StreamReader reader)
        {
            //re-boot
            this.InitializeSections();

            IniSection section = null;

            String line = null;
            while ( (line = reader.ReadLine()) != null)
            {
                String tmpLine = IniHelper.TrimComment(line);

                if (IsComment(tmpLine))
                {
                    if (section == null)
                        section = new IniSection(null, String.Empty);

                    IniCommentLine comment = new IniCommentLine(line);
                    section.Lines.Add(comment);
                }
                else if (IsProperty(tmpLine))
                {
                    IniPropertyLine comment = new IniPropertyLine(line);
                    section.Lines.Add(comment);
                }
                else if (IsSection(tmpLine))
                {
                    if (section != null)
                        this.sections.Add(section);

                    section = new IniSection(line);
                }
            }

            if (section != null)
                this.sections.Add(section);
        }
        #endregion


        #region Command & Control
        /// <summary>Normalizes the underlaying sections and Line items to a dictionary of IniPropertyLines</summary>
        /// <returns>A dictionary of IniPropertyLines</returns>
        public Dictionary<String, Dictionary<String, IniPropertyLine>> NormalizeSections()
        {
            Dictionary<String, Dictionary<String, IniPropertyLine>> dict = new Dictionary<String, Dictionary<String, IniPropertyLine>>();

            foreach (IniSection section in this.sections)
            {
                if (!dict.ContainsKey(section.Name))
                    dict[section.Name] = new Dictionary<String, IniPropertyLine>();

                foreach (IniPropertyLine pl in section.PropertyLines)
                {
                    if (dict[section.Name].ContainsKey(pl.Key))
                        dict[section.Name][pl.Key] = pl;
                    else
                        dict[section.Name].Add(pl.Key, pl);
                }
            }

            return dict;
        }
        #endregion


        #region Static methods
        /// <summary>Determines if the line should be parsed as a comment</summary>
        /// <param name="line">String to be evaluated</param>
        /// <returns>A Boolean indicating whether the line looks like a comment</returns>
        public static Boolean IsComment(String line)
        {
            Boolean comment = false;

            if (line == null || line.Trim() == String.Empty || line.Trim().StartsWith(";"))
                comment = true;

            return comment;
        }

        /// <summary>Determines if the line should be parsed as a property</summary>
        /// <param name="line">String to be evaluated</param>
        /// <returns>A Boolean indicating whether the line looks like a property</returns>
        public static Boolean IsProperty(String line)
        {
            Boolean property = false;

            //has an equals sign in the line (although an equals sign could be in the section definition, I suppose
            if (line != null && line.Trim().Contains("=") && !IsSection(line))
                property = true;

            return property;
        }

        /// <summary>Determines if the line should be parsed as a section</summary>
        /// <param name="line">String to be evaluated</param>
        /// <returns>A Boolean indicating whether the line looks like a section</returns>
        /// <remarks>If the file is invalid and it *would* be a section being assigned to a value like a property that ends with a ], you have more problems to worry about.</remarks>
        public static Boolean IsSection(String line)
        {
            Boolean section = false;

            //has brackets beginning and ending the line
            if (line != null && line.Trim().StartsWith("[") && line.Trim().IndexOf("]") > 0)
                section = true;

            return section;
        }
        #endregion
    }
}