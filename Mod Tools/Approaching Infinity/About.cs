using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bardez.Projects.InfinityPlus1.Tools.ApproachingInfinity
{
    /// <summary>The "About" window form</summary>
    public partial class About : Form
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public About()
        {
            InitializeComponent();
        }
        #endregion


        #region Event Handlers
        /// <summary>OK button event handler</summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">Arguments to the event</param>
        protected void btnDone_Click(Object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>Page load event handler</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event arguments</param>
        protected void About_Load(Object sender, EventArgs e)
        {
            this.lblVersion.Text = String.Format("Version {0}", BuildDetails.Version);
            this.lblVersion.Left = (this.Width - this.lblVersion.Width) / 2;
        }
        #endregion
    }
}