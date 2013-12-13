using System;

namespace Bardez.Projects.WinForms.Controls
{
    /// <summary>Tab closing event arguments</summary>
    /// <remarks>Based off of code from http://www.codeproject.com/Articles/20050/FireFox-like-Tab-Control </remarks>
    public class TabCloseEventArgs : EventArgs
    {
        #region Fields
        /// <summary>Index of the tab to close</summary>
        public Int32 TabIndex { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="tabIndex">Index of the tab to close</param>
        public TabCloseEventArgs(Int32 tabIndex)
        {
            this.TabIndex = tabIndex;
        }
        #endregion
    }
}