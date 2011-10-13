using System;
using System.Configuration;

namespace Bardez.Projects.Configuration
{
    public class KeyValueConfigurationElementImpersonator
    {
        /// <summary>Settings key</summary>
        protected String key;

        /// <summary>Value of the setting</summary>
        protected String value;

        #region Properties
        /// <summary>Settings key</summary>
        public String Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>Value of the setting</summary>
        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        #endregion

        /// <summary>Default constructor</summary>
        /// <param name="key">Settings key</param>
        /// <param name="value">Value of the setting</param>
        public KeyValueConfigurationElementImpersonator(String key, String value)
        {
            this.key = key;
            this.value = value;
        }

        /// <summary>Constructor that copies from a source KeyValueConfigurationElement</summary>
        /// <param name="source">Source KeyValueConfigurationElement to copy the key and value from</param>
        public KeyValueConfigurationElementImpersonator(KeyValueConfigurationElement source)
        {
            this.key = source.Key;
            this.value = source.Value;
        }
    }
}