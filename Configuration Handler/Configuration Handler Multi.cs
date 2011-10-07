using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;

namespace Bardez.Projects.Configuration
{
    /// <summary>Configuration wrapper class that merges multiple *.config values into one collection</summary>
    /// <remarks>
    ///     This configuration handler will deal with multiple key/value pairs, returing a list if using GetValues or the latest instance if using GetValue.
    ///     If there are multiple values in one level preceeding, the handler will merge them. If there are multiple values in the overriding, it will merge.
    ///     If, however, each level has a single value, it will overwrite.
    /// 
    ///     This configuration handler supports a hierarchical approach to multiple *.config files
    ///     it proceeds as follows, with the bottom being the last in, complimenting and overriding its predecessors
    ///     <list type="number">
    ///         <item>
    ///             <term>machine.config</term>        
    ///             <description>The machine.config file installed with the .NET framework</description>
    ///         </item>
    ///         <item>
    ///             <term>application-level *.config</term>        
    ///             <description>The web.config or app.config file within the application directory</description>
    ///         </item>
    ///         <item>
    ///             <term>Machine Name *.config</term>        
    ///             <description>A *.config file the application directory that has the machine's name as the filename</description>
    ///         </item>
    ///         <item>
    ///             <term>Domain Name *.config</term>        
    ///             <description>A *.config file the application directory that has the application's calling domain name as the filename</description>
    ///         </item>
    ///     </list>
    ///     
    ///     In the case of Direct Logic Solutions, after which I have modelled this design, this would be as follows:
    ///     machine.config, web.config, DLWEB12.config, reg.hasbro.com.config
    ///     
    ///     This way, machine.config values are loaded, generic web settings are, then any overriding server settings, followed by domain-testing-level settings.
    /// </remarks>
    [Obsolete("Built-in sections do not return multiple values for a given key.", true)]
    public static class ConfigurationHandlerMulti
    {
        #region Members
        /// <summary>Dictionary of KeyValueConfigurationElement objects</summary>
        private static Dictionary<String, List<KeyValueConfigurationElement>> AppSettings;

        /// <summary>Dictionary of ConnectionStringSetting objects</summary>
        private static Dictionary<String, List<ConnectionStringSettings>> ConnectionStrings;

        /// <summary>Boolean indicating whether or not the Configuration Handler is initialized</summary>
        private static Boolean isNotInitialized = true;
        #endregion

        #region Initialization
        /// <summary>Initializes the configuration handler static class</summary>
        private static void Initialize()
        {
            AppSettings = new Dictionary<String, List<KeyValueConfigurationElement>>();
            ConnectionStrings = new Dictionary<String, List<ConnectionStringSettings>>();

            try
            {
                String readMachineConfig = ConfigurationManager.AppSettings["TrustedApplication"];
                if (readMachineConfig != null)
                    readMachineConfig = readMachineConfig.ToUpper();

                if (GetBoolValue(readMachineConfig, false))
                {
                    //Read the Machine.config file
                    System.Configuration.Configuration machine = ConfigurationManager.OpenMachineConfiguration();
                    CopyKeys(machine, AppSettings, ConnectionStrings);
                }

                //Read the app.config file or web.config file
                LoadAppConfig();

                //look for <machinename>.config
                LoadMachineNameConfig();

                //look for <domain>.config
                LoadDomainNameConfig();
            }
            catch   //try to just load off of configuration manager
            {
                AppSettings = new Dictionary<String, List<KeyValueConfigurationElement>>();
                ConnectionStrings = new Dictionary<String, List<ConnectionStringSettings>>();
                CopyKeys(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings, AppSettings, ConnectionStrings);
            }

            //Now I has all m'keys...
            isNotInitialized = false;
        }
        #endregion

        #region Get configuration values
        /// <summary>Getshe latest configuration element read that is associated with the key</summary>
        /// <param name="key">String to look up</param>
        /// <returns>The latest read configuration element associated with the key</returns>
        public static KeyValueConfigurationElement GetSetting(String key)
        {
            if (isNotInitialized || AppSettings == null)
                Initialize();

            KeyValueConfigurationElement result = null;
            
            if (AppSettings.ContainsKey(key))
            {
                List<KeyValueConfigurationElement> settings = AppSettings[key];
                result = settings[settings.Count - 1];
            }

            return result;
        }

        /// <summary>Gets the most recet String value associated with a key from merged config files</summary>
        /// <param name="key">String to look up</param>
        /// <returns>The latest read string associated with the key</returns>
        public static String GetSettingValue(String key)
        {
            if (isNotInitialized || AppSettings == null)
                Initialize();

            String result;

            if (AppSettings.ContainsKey(key))
            {
                List<KeyValueConfigurationElement> settings = AppSettings[key];
                result = settings[settings.Count - 1].Value;
            }
            else
                result = null;

            return result;
        }

        /// <summary>Gets all String values associated with a key from merged config files</summary>
        /// <param name="key">String to look up</param>
        /// <returns>String values matched to the key</returns>
        public static List<String> GetSettingValues(String key)
        {
            if (isNotInitialized || AppSettings == null)
                Initialize();

            List<String> result;

            if (AppSettings.ContainsKey(key))
            {
                List<KeyValueConfigurationElement> settings = AppSettings[key];
                result = new List<String>();

                foreach (KeyValueConfigurationElement element in settings)
                    result.Add(element.Value);
            }
            else
                result = null;

            return result;
        }

        /// <summary>Evaluates a config setting as a Boolean</summary>
        /// <param name="key">String to look up</param>
        /// <param name="defaultValue">Default value if not found or could not parse</param>
        /// <returns>Default if unable to find or cannot parse, otherwise the matched value to a bolean</returns>
        public static Boolean GetBoolSettingValue(String key, Boolean defaultValue)
        {
            String result;
            Boolean boolValue = defaultValue;

            if (isNotInitialized || AppSettings == null)
                Initialize();

            result = GetSettingValue(key);

            if (null != result)
                boolValue = GetBoolValue(result, defaultValue);

            return boolValue;
        }

        /// <summary>Gets a connection string tied to the String key</summary>
        /// <param name="key">String to look up</param>
        /// <returns>Connection String associated with the key</returns>
        public static String GetConnectionString(String key)
        {
            if (isNotInitialized || ConnectionStrings == null)
                Initialize();

            String result = null;

            if (ConnectionStrings.ContainsKey(key))
            {
                List<ConnectionStringSettings> settings = ConnectionStrings[key];
                result = settings[settings.Count - 1].ConnectionString;
            }

            return result;
        }

        /// <summary>Gets a connection string tied to the String key</summary>
        /// <param name="key">String to look up</param>
        /// <returns>Connection String associated with the key</returns>
        public static List<String> GetConnectionStrings(String key)
        {
            if (isNotInitialized || ConnectionStrings == null)
                Initialize();

            List<String> result = null;

            if (ConnectionStrings.ContainsKey(key))
            {
                List<ConnectionStringSettings> settings = ConnectionStrings[key];
                result = new List<String>();

                foreach (ConnectionStringSettings element in settings)
                    result.Add(element.ConnectionString);
            }

            return result;
        }
        #endregion

        /// <summary>Gets a boolean value for a string value passed in</summary>
        /// <param name="value">String to evaluate</param>
        /// <param name="defaultValue">
        ///     Boolean value to default to in the evnet that the string does not match
        ///     a Boolean value.
        /// </param>
        /// <returns>A Boolean value representing either the value, evaluated, or the default passed in</returns>
        private static Boolean GetBoolValue(String value, Boolean defaultValue)
        {
            Boolean boolValue = defaultValue;
            if (value != null)
                switch (value.ToUpper())
                {
                    case "TRUE":
                    case "ON":
                    case "1":
                    case "YES":
                        boolValue = true;
                        break;
                    case "FALSE":
                    case "OFF":
                    case "0":
                    case "NO":
                        boolValue = false;
                        break;
                }

            return boolValue;
        }
        
        #region Copy and Merge configuration keys
        /// <summary>Copies keys from one configuration document element to the Settings and connectionstrings key/value Dictionaries</summary>
        /// <param name="Config">Configuration document to read</param>
        /// <param name="ApplicationSettings">Dictionary to copy to</param>
        /// <param name="ConnStrings">ConnectionString Dictionary to copy to</param>
        private static void CopyKeys
                (
                    System.Configuration.Configuration Config,
                    Dictionary<String, List<KeyValueConfigurationElement>> ApplicationSettings,
                    Dictionary<String, List<ConnectionStringSettings>> ConnStrings
                )
        {
            //temp lists
            Dictionary<String, List<KeyValueConfigurationElement>> tempSettings = new Dictionary<String, List<KeyValueConfigurationElement>>();
            Dictionary<String, List<ConnectionStringSettings>> tempConnStrings = new Dictionary<String, List<ConnectionStringSettings>>();

            //read all the settings
            foreach (KeyValueConfigurationElement pair in Config.AppSettings.Settings)
            {
                if (!tempSettings.ContainsKey(pair.Key))
                    tempSettings[pair.Key] = new List<KeyValueConfigurationElement>();

                tempSettings[pair.Key].Add(pair);
            }

            //merge all the settings
            CopySettings(tempSettings, ApplicationSettings);

            //read all the connection strings
            foreach (ConnectionStringSettings key in Config.ConnectionStrings.ConnectionStrings)
            {
                if (!tempConnStrings.ContainsKey(key.Name))
                    tempConnStrings[key.Name] = new List<ConnectionStringSettings>();

                tempConnStrings[key.Name].Add(key);
            }

            //merge all the connection strings
            CopyConnectionStrings(tempConnStrings, ConnStrings);
        }


        /// <summary>Copies keys from one NameValueCollection to the Settings and one ConnectionStringSettingsCollectionto the ConnectionStrings Dictionaries</summary>
        /// <param name="ApplicationSettings">NameValueCollection to copy from</param>
        /// <param name="ConnStrings">ConnectionStringSettingsCollection to copy from</param>
        /// <param name="SettingsDict">App Settings Dictionary to copy to</param>
        /// <param name="ConnectionStringsDict">ConnectionString Dictionary to copy to</param>
        private static void CopyKeys
            (NameValueCollection ApplicationSettings, ConnectionStringSettingsCollection ConnStrings,
                Dictionary<String, List<KeyValueConfigurationElement>> SettingsDict, Dictionary<String, List<ConnectionStringSettings>> ConnectionStringsDict)
        {
            //temp lists
            Dictionary<String, List<KeyValueConfigurationElement>> tempSettings = new Dictionary<String, List<KeyValueConfigurationElement>>();
            Dictionary<String, List<ConnectionStringSettings>> tempConnStrings = new Dictionary<String, List<ConnectionStringSettings>>();

            //read all the settings
            foreach (String key in ApplicationSettings.Keys)
            {
                String[] values = ApplicationSettings.GetValues(key);

                foreach (String value in values)
                {
                    if (!tempSettings.ContainsKey(key))
                        tempSettings[key] = new List<KeyValueConfigurationElement>();

                    tempSettings[key].Add(new KeyValueConfigurationElement(key, value));
                }
            }

            //merge all the settings
            CopySettings(tempSettings, SettingsDict);


            //read all the connection strings
            foreach (ConnectionStringSettings key in ConnStrings)
            {
                if (!tempConnStrings.ContainsKey(key.Name))
                    tempConnStrings[key.Name] = new List<ConnectionStringSettings>();

                tempConnStrings[key.Name].Add(key);
            }

            //merge all the connection strings
            CopyConnectionStrings(tempConnStrings, ConnectionStringsDict);
        }

        /// <summary>Copies configuration elements from the source into the destination, merging or setting as appropriate</summary>
        /// <param name="source">KeyValueConfigurationElement source</param>
        /// <param name="destination">KeyValueConfigurationElement depository</param>
        private static void CopySettings(Dictionary<String, List<KeyValueConfigurationElement>> source, Dictionary<String, List<KeyValueConfigurationElement>> destination)
        {
            //merge all the settings
            foreach (String key in source.Keys)
            {
                if (destination.ContainsKey(key) && destination[key] != null && (destination[key].Count > 1 || source[key].Count > 1))
                    destination[key].AddRange(source[key]);     //add or merge multiple
                else
                    destination[key] = source[key];             //drop the current for the new or set it entirely
            }
        }

        /// <summary>Copies configuration elements from the source into the destination, merging or setting as appropriate</summary>
        /// <param name="source">ConnectionStringSettings source</param>
        /// <param name="destination">ConnectionStringSettings depository</param>
        private static void CopyConnectionStrings(Dictionary<String, List<ConnectionStringSettings>> source, Dictionary<String, List<ConnectionStringSettings>> destination)
        {
            //merge all the settings
            foreach (String key in source.Keys)
            {
                if (destination.ContainsKey(key) && destination[key] != null && (destination[key].Count > 1 || source[key].Count > 1))
                    destination[key].AddRange(source[key]);     //add or merge multiple
                else
                    destination[key] = source[key];             //drop the current for the new or set it entirely
            }
        }
        #endregion

        #region Load config files
        /// <summary>Finds and loads a [Machine Name].config file</summary>
        private static void LoadMachineNameConfig()
        {
            String path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            String config = Environment.MachineName;
            path = path + "\\" + config + ".config";
            if (System.IO.File.Exists(path))
            {
                ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                map.ExeConfigFilename = path;
                System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                //System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(path);
                CopyKeys(configuration, AppSettings, ConnectionStrings);
            }
        }

        /// <summary>Finds and loads a [Domain Name].config file</summary>
        private static void LoadDomainNameConfig()
        {
            if (System.Web.HttpContext.Current != null)
            {
                String path = GetAssemblyPath();

                String config = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"];

                if (config.IndexOf(':') > -1)
                    config = config.Substring(0, config.IndexOf(':'));

                path = path + "\\" + config + ".config";
                if (System.IO.File.Exists(path))
                {
                    ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                    map.ExeConfigFilename = path;
                    System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                    //System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(path);
                    CopyKeys(configuration, AppSettings, ConnectionStrings);
                }
            }
        }

        /// <summary>Finds and loads a [Web or App].config file</summary>
        private static void LoadAppConfig()
        {
            System.Configuration.Configuration configuration;

            if (System.Web.HttpContext.Current != null)
            {
                String path = GetAssemblyPath();

                path = path + "\\web.config";
                if (System.IO.File.Exists(path))
                {
                    String webPath = System.IO.Path.GetDirectoryName(System.Web.HttpContext.Current.Request.FilePath).Replace('\\', '/');
                    configuration = WebConfigurationManager.OpenWebConfiguration(webPath);
                    CopyKeys(configuration, AppSettings, ConnectionStrings);
                }
            }
            else
            {
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                CopyKeys(configuration, AppSettings, ConnectionStrings);
            }
        }
        #endregion

        /// <summary>Gets the executing assembly path</summary>
        /// <returns>A String to the executing assembly path</returns>
        private static String GetAssemblyPath()
        {
            String path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            if (path.Substring(0, 8).ToLower() == "file:///")
            {
                path = path.Substring(8).Replace('/', '\\');

                //remove trailing paths.
                path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(path));
            }

            return path;
        }
    }
}