using System;
using System.Text;

using Bardez.Projects.Configuration.Ini.Component;

namespace Bardez.Projects.Configuration.Ini.Line
{
    public class IniPropertyLine : IniLineBase
    {
        /// <summary>Exposure of the property name</summary>
        public String Key
        {
            get { return this.GetKey(); }

            set
            {
                String key = String.Empty;

                if (value != null)
                {
                    key = value;
                    this.line = String.Concat(key, "=", this.GetValue());
                }
                else
                    this.line = String.Concat(key, "=");
            }
        }

        /// <summary>Exposure of the property value</summary>
        public String Value
        {
            get { return this.GetValue(); }

            set
            {
                String val = String.Empty;

                if (value != null)
                {
                    val = value;
                    this.line = String.Concat(this.GetKey(), "=", val);
                }
                else
                    this.line = String.Concat("=" + val);
            }
        }

        /// <summary>Default constructor</summary>
        public IniPropertyLine()
        {
        }

        /// <summary>Default constructor</summary>
        public IniPropertyLine(String line)
        {
            this.line = line;
        }


        /// <summary>Gets the key of the property line</summary>
        /// <returns>String of the key</returns>
        protected String GetKey()
        {
            String key = null;

            if (this.line != null)
            {
                key = String.Empty;
                String lineTrim = IniHelper.TrimComment(line);

                Int32 index = lineTrim.IndexOf('=');
                if (index > 0)
                    key = lineTrim.Substring(0, index).Trim();
                else
                    key = lineTrim.Trim();
            }

            return key;
        }

        /// <summary>Gets the value of the property line</summary>
        /// <returns>String of the value</returns>
        protected String GetValue()
        {
            String value = null;

            if (this.line != null)
            {
                value = String.Empty;
                String lineTrim = IniHelper.TrimComment(line);

                Int32 index = lineTrim.IndexOf('=');
                if (index > -1 && index < lineTrim.Length)
                    value = this.line.Substring(index + 1, (this.line.Length - index) - 1).Trim();
                else
                    value = lineTrim.Trim();
            }

            return value;
        }
    }
}