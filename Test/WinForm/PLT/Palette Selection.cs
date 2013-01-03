using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    /// <summary>User control to expose an updatable 8-color palette</summary>
    public partial class PaletteSelection : UserControl
    {
        #region Fields
        /// <summary>Event that indicates that the palette has changed</summary>
        protected event Action paletteChanged;

        /// <summary>Manager for the palette BMPs that exposes them as palettes</summary>
        protected PaletteReader paletteReader;

        /// <summary>Array indicating the selected palette numbers</summary>
        protected Int32[] selectedPalettes;
        #endregion


        #region Properties
        /// <summary>Exposes the seleced color palettes</summary>
        public Palette[] Palettes
        {
            get
            {
                return new Palette[]
                {
                    this.paletteReader.Palette256[this.selectedPalettes[0]],
                    this.paletteReader.Palette256[this.selectedPalettes[1]],
                    this.paletteReader.Palette256[this.selectedPalettes[2]],
                    this.paletteReader.Palette256[this.selectedPalettes[3]],
                    this.paletteReader.Palette256[this.selectedPalettes[4]],
                    this.paletteReader.Palette256[this.selectedPalettes[5]],
                    this.paletteReader.Palette256[this.selectedPalettes[6]],
                    this.paletteReader.Palette256[this.selectedPalettes[7]],
                };
            }
        }

        /// <summary>Event that indicates that the palette has changed</summary>
        public event Action PaletteChanged
        {
            add { this.paletteChanged += value; }
            remove { this.paletteChanged -= value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public PaletteSelection()
        {
            this.InitializeComponent();

            this.selectedPalettes = new Int32[8];

            //read palettes
            this.paletteReader = new PaletteReader();
            this.paletteReader.InitializePalettes();
        }
        #endregion


        #region Event handlers
        /// <summary>Event handler for clicking the first color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor1_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor1, 0);
        }

        /// <summary>Event handler for clicking the second color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor2_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor2, 1);
        }

        /// <summary>Event handler for clicking the third color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor3_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor3, 2);
        }

        /// <summary>Event handler for clicking the fourth color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor4_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor4, 3);
        }

        /// <summary>Event handler for clicking the fifth color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor5_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor5, 4);
        }

        /// <summary>Event handler for clicking the sixth color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor6_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor6, 5);
        }

        /// <summary>Event handler for clicking the sevenh color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor7_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor7, 6);
        }

        /// <summary>Event handler for clicking the eighth color button</summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Paremeters for the event</param>
        protected void btnColor8_Click(Object sender, EventArgs e)
        {
            this.PickColor(this.btnColor8, 7);
        }
        #endregion


        #region Helpers
        /// <summary>Helper method to select colors for the palette</summary>
        /// <param name="button">Button to assign the color to</param>
        /// <param name="paletteIndex">Index of the palette to set</param>
        protected void PickColor(Button button, Int32 paletteIndex)
        {
            if (button != null)
            {
                Color original = button.BackColor;
                PaletteSelector selector = new PaletteSelector();
                selector.ShowDialog();
                button.BackColor = selector.SelectedPaletteColor;

                this.selectedPalettes[paletteIndex] = selector.PaletteNumber;

                //if a new color, raise the palette changed event
                if (button.BackColor != original)
                    this.paletteChanged();
            }
        }
        #endregion
    }
}