using System;
using System.Text.RegularExpressions;

namespace Bardez.Projects.Configuration.Ini.Component
{
    public static class IniHelper
    {
        /// <summary>Trims the comments then surrounding whitespace from value of a string</summary>
        /// <param name="input">A string to be trimmed</param>
        /// <returns>The string, minus its comments and surrounding whitespace</returns>
        /// <remarks>This needs to be tokenized, probably and unfortunately. Semicolons and comments inside quotes and brackets are valid, I guess</remarks>
        //[Obsolete("Remove the C-style comments, as they differ from Section def and are misleading", true)]
        public static String TrimComment(String input)
        {
            String result = input;

            //Int32 index = result.IndexOf("//");
            //if (index > -1)
            //    result = result.Substring(0, index);

            //Semicolons (;) at the beginning of the line indicate a comment. Comment lines are ignored.
            if (result.StartsWith(";"))
                result = String.Empty;

            //Some software supports the use of the pound sign (#) as an alternative to the semicolon for indicating comments
            if (result.StartsWith("#"))
                result = String.Empty;

            // apparently there is also a pseudo C-style comment of /words/, without the asterisks
            //Regex regx = new Regex("/[^.]*/");
            //if (regx.IsMatch(result))
            //    result = regx.Replace(result, String.Empty);

            return result.Trim();
        }
    }
}