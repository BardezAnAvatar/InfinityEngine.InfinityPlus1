using System;
using System.Collections.Generic;

namespace Bardez.Projects.Utility
{
    public static class Extension
    {
        /// <summary>Extension method of System.Collections.Generic.List to clone all class, clonable references in the list.</summary>
        /// <typeparam name="T">Type of object that is a class and implements IDeepCloneable.</typeparam>
        /// <param name="reference">List to clone.</param>
        /// <returns>A deeply cloned list.</returns>
        public static List<T> Clone<T>(this IList<T> reference) where T : class, IDeepCloneable
        {
            List<T> clone = new List<T>();
            foreach (T type in reference)
            {
                clone.Add(type.Clone() as T);
            }

            return clone;
        }

        /// <summary>Extension method that performs a comparison of two lists</summary>
        /// <typeparam name="T">Generic List type</typeparam>
        /// <param name="reference">Left "this" compared List</param>
        /// <param name="compare">Right compared List</param>
        /// <returns>Boolean indicating equality</returns>
        public static Boolean Equals<T>(this IList<T> reference, IList<T> compare)
        {
            Boolean equal = false;

            try
            {
                if (reference != null && compare != null && reference.Count == compare.Count)
                {
                    Int32 index = 0;
                    do
                    {
                        equal = reference[index].Equals(compare[index]);
                        ++index;
                    }
                    while (equal && index < reference.Count);
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Extension method that performs GetHashCode on all list elements</summary>
        /// <typeparam name="T">Generic List type</typeparam>
        /// <param name="reference">"this" List instance</param>
        /// <returns>The computed hash code</returns>
        public static Int32 GetHashCode<T>(this IList<T> reference)
        {
            Int32 hashcode = 0;

            if (reference != null)
                foreach (T item in reference)
                    hashcode ^= item.GetHashCode();

            return hashcode;
        }
    }
}