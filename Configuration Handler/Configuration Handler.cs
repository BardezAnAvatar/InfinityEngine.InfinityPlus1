using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;

namespace Bardez.Projects.Configuration
{
    /// <summary>Configuration wrapper class that merges multiple *.config values into one collection</summary>
    /// <remarks>Not static, I think, because of .NET 1.1 limitations</remarks>
    public class ConfigurationHandler
    {
        private static Dictionary<String, KeyValueConfigurationElement> AppSettings;
        private static Dictionary<String, ConnectionStringSettings> ConnectionStrings;
        private static Boolean isNotInitialized = true;

        public static KeyValueConfigurationElement GetSetting(String Key)
        {
            if (isNotInitialized || AppSettings == null)
                Initialize();

            KeyValueConfigurationElement result;

            try
            {
                result = AppSettings[Key];
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static String GetSettingValue(String Key)
        {
            if (isNotInitialized || AppSettings == null)
                Initialize();

            String result;

            try
            {
                result = AppSettings[Key].Value;
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static Boolean GetBoolSettingValue(String Key, Boolean Default)
        {
            String result;
            Boolean boolValue = Default;

            if (isNotInitialized || AppSettings == null)
                Initialize();

            try { result = AppSettings[Key].Value; }
            catch { result = null; }

            if (null != result)
                boolValue = GetBoolValue(result, Default);

            return boolValue;
        }

        /// <summary>Gets a connection string tied to the String key</summary>
        /// <param name="Key">String to look up</param>
        /// <returns>Connection String associated with the key</returns>
        public static String GetConnectionString(String Key)
        {
            if (isNotInitialized || ConnectionStrings == null)
                Initialize();

            return ConnectionStrings[Key].ConnectionString;
        }

        /// <summary>Gets a boolean value for a string value passed in</summary>
        /// <param name="Value">String to evaluate</param>
        /// <param name="Default">
        ///     Boolean value to default to in the evnet that the string does not match
        ///     a Boolean value.
        /// </param>
        /// <returns>A Boolean value representing either the value, evaluated, or the default passed in</returns>
        private static Boolean GetBoolValue(String Value, Boolean Default)
        {
            Boolean boolValue;
            switch (Value.ToUpper())
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
                default:
                    boolValue = Default;
                    break;
            }

            return boolValue;
        }

        /// <summary>Initializes the configuration handler static class</summary>
        private static void Initialize()
        {
            AppSettings = new Dictionary<String, KeyValueConfigurationElement>();
            ConnectionStrings = new Dictionary<String, ConnectionStringSettings>();

            try
            {
                String readMachineConfig = ConfigurationManager.AppSettings["TrustedApplication"];
                if(readMachineConfig != null)
                    readMachineConfig = readMachineConfig.ToUpper();

                if (GetBoolValue(readMachineConfig, false))
                {
                    //Read the Machine.config file
                    System.Configuration.Configuration machine;
                    machine = ConfigurationManager.OpenMachineConfiguration();
                    CopyKeys(machine, AppSettings, ConnectionStrings);
                }

                //Read the app.config file or web.config file
                LoadAppConfig();

                //look for <machinename>.config
                LoadMachineNameConfig();

                //look for <domain>.config
                LoadDomainNameConfig();
            }
            catch
            {
                AppSettings = new Dictionary<String, KeyValueConfigurationElement>();
                ConnectionStrings = new Dictionary<String, ConnectionStringSettings>();
                CopyKeys(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings, AppSettings, ConnectionStrings);
            }

            //Now I has all m'keys...
            isNotInitialized = false;
        }

        /// <summary>Copies keys from one configuration document element to the Settings and connectionstrings key/value Dictionaries</summary>
        /// <param name="Config">Configuration document to read</param>
        /// <param name="Settings">Dictionary to copy to</param>
        /// <param name="ConnectionStrings">ConnectionString Dictionary to copy to</param>
        private static void CopyKeys
                (
                    System.Configuration.Configuration Config,
                    Dictionary<String, KeyValueConfigurationElement> Settings,
                    Dictionary<String, ConnectionStringSettings> ConnectionStrings
                )
        {
            //read all the settings
            foreach (KeyValueConfigurationElement pair in Config.AppSettings.Settings)
                Settings[pair.Key] = Config.AppSettings.Settings[pair.Key];

            foreach (ConnectionStringSettings key in Config.ConnectionStrings.ConnectionStrings)
                ConnectionStrings[key.Name] = Config.ConnectionStrings.ConnectionStrings[key.Name];
        }

        private static void CopyKeys
                (
                    NameValueCollection AppSettings, ConnectionStringSettingsCollection ConnStrings,
                    Dictionary<String, KeyValueConfigurationElement> Settings,
                    Dictionary<String, ConnectionStringSettings> ConnectionStrings
                )
        {
            KeyValueConfigurationElement tempElement;
            KeyValueConfigurationCollection collection = new KeyValueConfigurationCollection();
            //read all the settings
            foreach (String key in AppSettings.Keys)
            {
                tempElement  = new KeyValueConfigurationElement(key, AppSettings[key]);
                collection.Add(tempElement);
            }
            foreach (String key in AppSettings.Keys)
                Settings[key] = collection[key];

            foreach (ConnectionStringSettings key in ConnStrings)
                ConnectionStrings[key.Name] = ConnStrings[key.Name];
        }

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

        private static void LoadDomainNameConfig()
        {
            if (System.Web.HttpContext.Current != null)
            {
                String path = GetAssemblyPath();

                String config = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"];

                if(config.IndexOf(':') > -1)
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