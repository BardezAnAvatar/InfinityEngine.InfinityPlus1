using System;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Globals
{
    /// <summary>Class representing a NULL-terminated string</summary>
    public class ZString
    {
        /// <summary>source string being stored</summary>
        protected String source;

        /// <summary>source string being stored</summary>
        public String Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        /// <summary>ZString string being stored</summary>
        public String Value
        {
            get { return GetZString(this.source); }
            set { this.source = GetZString(value); }
        }

        /// <summary>Truncates the string at the first found NULL character</summary>
        /// <param name="output">String output into the method</param>
        /// <returns>a String substring of the passed in string</returns>
        public static String GetZString(String input)
        {
            String output = input;

            if (input != null)
            {
                Int32 index;
                for (index = 0; index < input.Length; ++index)
                {
                    if (input[index] == Char.MinValue)
                        break;
                }

                if (index < input.Length)
                    output = input.Substring(0, index);
            }

            return output;
        }

        /// <summary>Overridden ToString() representation of the String</summary>
        /// <returns>The null-terminated string</returns>
        public override String ToString()
        {
            return this.Value;
        }
    }
}