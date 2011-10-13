using System;
using System.Configuration;

namespace Bardez.Projects.Configuration
{
    public class ConnectionStringSettingsImpersonator
    {
        /// <summary>Name key of the connection string</summary>
        protected String name;

        /// <summary>Value of the connection string</summary>
        protected String connectionString;

        #region Properties
        /// <summary>Name key of the connection string</summary>
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public String ConnectionString
        {
            get { return this.connectionString; }
            set { this.connectionString = value; }
        }
        #endregion

        /// <summary>Default constructor</summary>
        /// <param name="name">Name key of the connection string</param>
        /// <param name="connectionString">Value of the connection string</param>
        public ConnectionStringSettingsImpersonator(String name, String connectionString)
        {
            this.name = name;
            this.connectionString = connectionString;
        }

        /// <summary>Constructor that copies from a source ConnectionStringSettings</summary>
        /// <param name="source">ConnectionStringSettings to copy connection string from</param>
        public ConnectionStringSettingsImpersonator(ConnectionStringSettings source)
        {
            this.name = source.Name;
            this.connectionString = source.ConnectionString;
        }
    }
}