using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.Utility
{
    public static class Extension
    {
        /// <summary>Extension method of System.Collections.Generic.List to clone all class, clonable references in the list.</summary>
        /// <typeparam name="T">Type of object that is a class and implements IDeepCloneable.</typeparam>
        /// <param name="reference">List to clone.</param>
        /// <returns>A deeply cloned list.</returns>
        public static List<T> Clone<T>(this List<T> reference) where T : class, IDeepCloneable
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
        public static Boolean Equals<T>(this List<T> reference, List<T> compare)
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
    }
}