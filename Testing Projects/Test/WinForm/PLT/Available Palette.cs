using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    /// <summary>Represents a single selectable palette representative</summary>
    public partial class AvailablePalette : UserControl
    {
        #region Fields
        /// <summary>Event to raise when the control's button is clicked</summary>
        protected event Action<AvailablePalette> colorSelected;

        /// <summary>Gradient palette number</summary>
        protected Int32 paletteIndex;
        #endregion


        #region Properties
        /// <summary>Exposes the color of this gradient palette</summary>
        public Color Color
        {
            get { return this.btnSelect.BackColor; }
            set { this.btnSelect.BackColor = value; }
        }

        /// <summary>Exposes the name of this gradient palette</summary>
        public String ColorName
        {
            get { return this.txtbxPaletteName.Text; }
            set { this.txtbxPaletteName.Text = value; }
        }

        /// <summary>Exposes a palette number to raise when selected</summary>
        public Int32 PaletteNumber
        {
            get { return this.paletteIndex; }
            set
            {
                this.paletteIndex = value;
                this.labelPaletteNumber.Text = value.ToString();
            }
        }
        #endregion


        #region Events
        /// <summary>Exposes an event attachment point for the click event</summary>
        public event Action<AvailablePalette> ColorSelected
        {
            add { this.colorSelected += value; }
            remove { this.colorSelected -= value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public AvailablePalette()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true; //double-buffer
        }
        #endregion


        #region Event Handlers
        /// <summary>Event handler for when the color is selected</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Parameters to the event</param>
        protected void btnSelect_Click(Object sender, EventArgs e)
        {
            this.colorSelected(this);
        }
        #endregion
    }
}