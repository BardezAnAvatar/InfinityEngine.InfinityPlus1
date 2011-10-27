using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Bardez.Projects.Configuration.Ini.Line;

namespace Bardez.Projects.Configuration.Ini.Component
{
    public class IniSection
    {
        #region Members
        /// <summary>Ini file declaration line</summary>
        protected IniSectionLine declaration;

        /// <summary>List of lines to output</summary>
        protected List<IniLineBase> lines;
        #endregion

        #region Properties
        /// <summary>Ini file declaration line</summary>
        public IniSectionLine Declaration
        {
            get { return this.declaration; }
            set { this.declaration = value; }
        }

        /// <summary>Ini file declaration line</summary>
        public String Name
        {
            get { return this.declaration.Name; }
            set { this.declaration.Name = value; }
        }

        /// <summary>List of lines to output</summary>
        public List<IniLineBase> Lines
        {
            get { return this.lines; }
            set { this.lines = value; }
        }

        /// <summary>Exposure of Properties underneath this section</summary>
        public List<IniPropertyLine> PropertyLines
        {
            get
            {
                List<IniPropertyLine> propertyLines = new List<IniPropertyLine>();

                foreach (IniLineBase line in this.lines)
                    if (line.GetType() == typeof(IniPropertyLine))
                        propertyLines.Add(line as IniPropertyLine);

                return propertyLines;
            }
        }

        /// <summary>Exposure of Properties underneath this section as a simpler NameValueCollection</summary>
        public NameValueCollection Properties
        {
            get
            {
                NameValueCollection nvc = new NameValueCollection();

                foreach (IniPropertyLine line in this.PropertyLines)
                    nvc.Add(line.Key, line.Value);

                return nvc;
            }
        }
        #endregion

        /// <summary>Default constructor</summary>
        public IniSection()
        {
            this.declaration = new IniSectionLine();
            this.lines = new List<IniLineBase>();
        }

        /// <summary>Default constructor</summary>
        public IniSection(IniSectionLine section)
        {
            this.declaration = section;
            this.lines = new List<IniLineBase>();
        }

        /// <summary>Default constructor</summary>
        public IniSection(String line)
        {
            this.declaration = new IniSectionLine(line);
            this.lines = new List<IniLineBase>();
        }

        /// <summary>Default constructor</summary>
        public IniSection(String line, String name)
        {
            this.declaration = new IniSectionLine(line, name);
            this.lines = new List<IniLineBase>();
        }
    }
}
