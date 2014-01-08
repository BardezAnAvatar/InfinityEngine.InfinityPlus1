using System;
using System.Collections.Generic;
using System.Linq;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Components;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse
{
    /// <summary>Represents a parameter and any whitespace leading up to it</summary>
    public class Parameter
    {
        #region Fields
        /// <summary>Collection of characters leading up to the parameter</summary>
        protected IList<Char> leadingWhiteSpace;

        /// <summary>Collection of characters composing the parameter</summary>
        protected IList<Char> parameter;
        #endregion
    }
}