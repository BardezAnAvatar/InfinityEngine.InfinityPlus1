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
    }
}