using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bardez.Projects.InfinityPlus1.UnitTesting
{
    /// <summary>Static class containing helpful methods for unit testing</summary>
    internal static class Helper
    {
        /// <summary>Gets the value as a string from the TestContext</summary>
        /// <param name="name">Name of the value to retrieve</param>
        /// <param name="context">TestContext to read from</param>
        /// <returns>The value of the value, or null if it was empty</returns>
        internal static String GetContextString(String name, TestContext context)
        {
            String value = null;

            if (context.DataRow.Table.Columns.Contains(name))
            {
                if (context.DataRow.Table.Columns[name].DataType == typeof(String))
                    value = context.DataRow[name] as String;
                else
                    value = context.DataRow[name].ToString();
            }

            return value;
        }

        /// <summary>Gets the value as a nullable integer from the TestContext</summary>
        /// <param name="name">Name of the value to retrieve</param>
        /// <param name="context">TestContext to read from</param>
        /// <returns>The value of the value, or null if it was empty</returns>
        internal static Int32? GetContextInt32(String name, TestContext context)
        {
            Int32? val = null;

            String value = Helper.GetContextString(name, context);
            if (value != null)
                val = Int32.Parse(value);

            return val;
        }

        /// <summary>Gets the value as a nullable unsigned integer from the TestContext</summary>
        /// <param name="name">Name of the value to retrieve</param>
        /// <param name="context">TestContext to read from</param>
        /// <returns>The value of the value, or null if it was empty</returns>
        internal static UInt32? GetContextUInt32(String name, TestContext context)
        {
            UInt32? val = null;

            String value = Helper.GetContextString(name, context);
            if (value != null)
                val = UInt32.Parse(value);

            return val;
        }


        /// <summary>Gets the value as a nullable boolean from the TestContext</summary>
        /// <param name="name">Name of the value to retrieve</param>
        /// <param name="context">TestContext to read from</param>
        /// <returns>The value of the value, or null if it was empty</returns>
        internal static Boolean? GetContextBoolean(String name, TestContext context)
        {
            Boolean? val = null;

            String value = Helper.GetContextString(name, context);
            if (value != null)
                val = Boolean.Parse(value);

            return val;
        }
    }
}
