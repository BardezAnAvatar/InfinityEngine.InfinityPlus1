using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Bardez.Projects.WinForms.Controls
{
    /// <summary>A tab page that you can close from the tab itself</summary>
    public class ClosableTabPage : TabPage
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public ClosableTabPage() : base() { }

        /// <summary>Name constructor</summary>
        /// <param name="text">Text to give this page</param>
        public ClosableTabPage(String text) : base()
        {
            this.Text = text;
        }
        #endregion


        #region Properties
        /// <summary>Gets or sets the text to display on the tab</summary>
        public override String Text
        {
            get { return base.Text + "     "; }  //HACK, but functional
            set
            {
                base.Text = value;
                //Size size = this.Size;
                //size.Width += (this.Height * 2);
                //this.Size = size;
            }
        }
        #endregion
    }
}