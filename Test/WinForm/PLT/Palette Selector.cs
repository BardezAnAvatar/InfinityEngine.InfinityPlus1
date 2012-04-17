using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm.PLT
{
    /// <summary>This form is designed to be used as a pop-up/dialog window form</summary>
    public partial class PaletteSelector : Form
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public PaletteSelector()
        {
            this.InitializeComponent();
            this.PaletteNumber = 119;   //gray default
            this.SelectedPaletteColor = Color.Transparent;
        }
        #endregion

        
        #region Fields
        /// <summary>Exposes the selected palette number</summary>
        public Int32 PaletteNumber { get; set; }

        /// <summary>Selected palette color from the raised event</summary>
        public Color SelectedPaletteColor { get; set; }
        #endregion


        #region Event Handlers
        /// <summary>Event handler to attach to the user controls</summary>
        /// <param name="paletteSelector">AvailablePalette control raising the event</param>
        public void btnColorClick(AvailablePalette paletteSelector)
        {
            this.PaletteNumber = paletteSelector.PaletteNumber;
            this.SelectedPaletteColor = paletteSelector.Color;
            this.Close();
        }
        #endregion


        #region Helpers
        /// <summary>This method sets all the colors for the AvaiablePalette conrols.</summary>
        /// <remarks>Unused, kept for reference</remarks>
        protected void SetControlColors()
        {
            this.availablePalette000.Color = System.Drawing.ColorTranslator.FromHtml("#50443C");
            this.availablePalette001.Color = System.Drawing.ColorTranslator.FromHtml("#957140");
            this.availablePalette002.Color = System.Drawing.ColorTranslator.FromHtml("#794C18");
            this.availablePalette003.Color = System.Drawing.ColorTranslator.FromHtml("#BAAA71");
            this.availablePalette004.Color = System.Drawing.ColorTranslator.FromHtml("#A55020");
            this.availablePalette005.Color = System.Drawing.ColorTranslator.FromHtml("#999D8D");
            this.availablePalette006.Color = System.Drawing.ColorTranslator.FromHtml("#999D8D");
            this.availablePalette007.Color = System.Drawing.ColorTranslator.FromHtml("#716D61");
            this.availablePalette008.Color = System.Drawing.ColorTranslator.FromHtml("#8D6140");
            this.availablePalette009.Color = System.Drawing.ColorTranslator.FromHtml("#A17955");
            this.availablePalette010.Color = System.Drawing.ColorTranslator.FromHtml("#B2956D");
            this.availablePalette011.Color = System.Drawing.ColorTranslator.FromHtml("#C28171");
            this.availablePalette012.Color = System.Drawing.ColorTranslator.FromHtml("#D2A18D");
            this.availablePalette013.Color = System.Drawing.ColorTranslator.FromHtml("#C6AA99");
            this.availablePalette014.Color = System.Drawing.ColorTranslator.FromHtml("#AEA1A1");
            this.availablePalette015.Color = System.Drawing.ColorTranslator.FromHtml("#85A18D");
            this.availablePalette016.Color = System.Drawing.ColorTranslator.FromHtml("#B2A569");
            this.availablePalette017.Color = System.Drawing.ColorTranslator.FromHtml("#799DAE");
            this.availablePalette018.Color = System.Drawing.ColorTranslator.FromHtml("#3C507D");
            this.availablePalette019.Color = System.Drawing.ColorTranslator.FromHtml("#B24828");
            this.availablePalette020.Color = System.Drawing.ColorTranslator.FromHtml("#2C5924");
            this.availablePalette021.Color = System.Drawing.ColorTranslator.FromHtml("#595959");
            this.availablePalette022.Color = System.Drawing.ColorTranslator.FromHtml("#996D50");
            this.availablePalette023.Color = System.Drawing.ColorTranslator.FromHtml("#795D4C");
            this.availablePalette024.Color = System.Drawing.ColorTranslator.FromHtml("#9D6524");
            this.availablePalette025.Color = System.Drawing.ColorTranslator.FromHtml("#CAA15D");
            this.availablePalette026.Color = System.Drawing.ColorTranslator.FromHtml("#D2894C");
            this.availablePalette027.Color = System.Drawing.ColorTranslator.FromHtml("#858589");
            this.availablePalette028.Color = System.Drawing.ColorTranslator.FromHtml("#7D8185");
            this.availablePalette029.Color = System.Drawing.ColorTranslator.FromHtml("#4C3C34");
            this.availablePalette030.Color = System.Drawing.ColorTranslator.FromHtml("#656565");
            this.availablePalette031.Color = System.Drawing.ColorTranslator.FromHtml("#48858D");
            this.availablePalette032.Color = System.Drawing.ColorTranslator.FromHtml("#108544");
            this.availablePalette033.Color = System.Drawing.ColorTranslator.FromHtml("#C23461");
            this.availablePalette034.Color = System.Drawing.ColorTranslator.FromHtml("#71954C");
            this.availablePalette035.Color = System.Drawing.ColorTranslator.FromHtml("#DA8150");
            this.availablePalette036.Color = System.Drawing.ColorTranslator.FromHtml("#757520");
            this.availablePalette037.Color = System.Drawing.ColorTranslator.FromHtml("#595518");
            this.availablePalette038.Color = System.Drawing.ColorTranslator.FromHtml("#8D7161");
            this.availablePalette039.Color = System.Drawing.ColorTranslator.FromHtml("#715040");
            this.availablePalette040.Color = System.Drawing.ColorTranslator.FromHtml("#815D28");
            this.availablePalette041.Color = System.Drawing.ColorTranslator.FromHtml("#754424");
            this.availablePalette042.Color = System.Drawing.ColorTranslator.FromHtml("#7D7561");
            this.availablePalette043.Color = System.Drawing.ColorTranslator.FromHtml("#655548");
            this.availablePalette044.Color = System.Drawing.ColorTranslator.FromHtml("#954C6D");
            this.availablePalette045.Color = System.Drawing.ColorTranslator.FromHtml("#7D3055");
            this.availablePalette046.Color = System.Drawing.ColorTranslator.FromHtml("#AE2C1C");
            this.availablePalette047.Color = System.Drawing.ColorTranslator.FromHtml("#750000");
            this.availablePalette048.Color = System.Drawing.ColorTranslator.FromHtml("#BA7934");
            this.availablePalette049.Color = System.Drawing.ColorTranslator.FromHtml("#955018");
            this.availablePalette050.Color = System.Drawing.ColorTranslator.FromHtml("#9D8D00");
            this.availablePalette051.Color = System.Drawing.ColorTranslator.FromHtml("#756900");
            this.availablePalette052.Color = System.Drawing.ColorTranslator.FromHtml("#5D8538");
            this.availablePalette053.Color = System.Drawing.ColorTranslator.FromHtml("#406D28");
            this.availablePalette054.Color = System.Drawing.ColorTranslator.FromHtml("#1C5910");
            this.availablePalette055.Color = System.Drawing.ColorTranslator.FromHtml("#148D6D");
            this.availablePalette056.Color = System.Drawing.ColorTranslator.FromHtml("#0C6148");
            this.availablePalette057.Color = System.Drawing.ColorTranslator.FromHtml("#407595");
            this.availablePalette058.Color = System.Drawing.ColorTranslator.FromHtml("#1C487D");
            this.availablePalette059.Color = System.Drawing.ColorTranslator.FromHtml("#797189");
            this.availablePalette060.Color = System.Drawing.ColorTranslator.FromHtml("#504C6D");
            this.availablePalette061.Color = System.Drawing.ColorTranslator.FromHtml("#75616D");
            this.availablePalette062.Color = System.Drawing.ColorTranslator.FromHtml("#615555");
            this.availablePalette063.Color = System.Drawing.ColorTranslator.FromHtml("#8D8D8D");
            this.availablePalette064.Color = System.Drawing.ColorTranslator.FromHtml("#656565");
            this.availablePalette065.Color = System.Drawing.ColorTranslator.FromHtml("#4C504C");
            this.availablePalette066.Color = System.Drawing.ColorTranslator.FromHtml("#343438");
            this.availablePalette067.Color = System.Drawing.ColorTranslator.FromHtml("#F6C650");
            this.availablePalette068.Color = System.Drawing.ColorTranslator.FromHtml("#69D6FF");
            this.availablePalette069.Color = System.Drawing.ColorTranslator.FromHtml("#A5E26D");
            this.availablePalette070.Color = System.Drawing.ColorTranslator.FromHtml("#C6615D");
            this.availablePalette071.Color = System.Drawing.ColorTranslator.FromHtml("#C2E2FF");
            this.availablePalette072.Color = System.Drawing.ColorTranslator.FromHtml("#959195");
            this.availablePalette073.Color = System.Drawing.ColorTranslator.FromHtml("#14A14C");
            this.availablePalette074.Color = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            this.availablePalette075.Color = System.Drawing.ColorTranslator.FromHtml("#000000");
            this.availablePalette076.Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            this.availablePalette077.Color = System.Drawing.ColorTranslator.FromHtml("#00FF00");
            this.availablePalette078.Color = System.Drawing.ColorTranslator.FromHtml("#0000FF");
            this.availablePalette079.Color = System.Drawing.ColorTranslator.FromHtml("#BAB6BA");
            this.availablePalette080.Color = System.Drawing.ColorTranslator.FromHtml("#DA9100");
            this.availablePalette081.Color = System.Drawing.ColorTranslator.FromHtml("#C69D30");
            this.availablePalette082.Color = System.Drawing.ColorTranslator.FromHtml("#34A1CE");
            this.availablePalette083.Color = System.Drawing.ColorTranslator.FromHtml("#55555D");
            this.availablePalette084.Color = System.Drawing.ColorTranslator.FromHtml("#C68559");
            this.availablePalette085.Color = System.Drawing.ColorTranslator.FromHtml("#B6814C");
            this.availablePalette086.Color = System.Drawing.ColorTranslator.FromHtml("#75950C");
            this.availablePalette087.Color = System.Drawing.ColorTranslator.FromHtml("#714C24");
            this.availablePalette088.Color = System.Drawing.ColorTranslator.FromHtml("#BE5975");
            this.availablePalette089.Color = System.Drawing.ColorTranslator.FromHtml("#DE8185");
            this.availablePalette090.Color = System.Drawing.ColorTranslator.FromHtml("#917D10");
            this.availablePalette091.Color = System.Drawing.ColorTranslator.FromHtml("#755028");
            this.availablePalette092.Color = System.Drawing.ColorTranslator.FromHtml("#A17544");
            this.availablePalette093.Color = System.Drawing.ColorTranslator.FromHtml("#69594C");
            this.availablePalette094.Color = System.Drawing.ColorTranslator.FromHtml("#8D5944");
            this.availablePalette095.Color = System.Drawing.ColorTranslator.FromHtml("#AA8155");
            this.availablePalette096.Color = System.Drawing.ColorTranslator.FromHtml("#485D91");
            this.availablePalette097.Color = System.Drawing.ColorTranslator.FromHtml("#7599AE");
            this.availablePalette098.Color = System.Drawing.ColorTranslator.FromHtml("#998D69");
            this.availablePalette099.Color = System.Drawing.ColorTranslator.FromHtml("#956524");
            this.availablePalette100.Color = System.Drawing.ColorTranslator.FromHtml("#615548");
            this.availablePalette101.Color = System.Drawing.ColorTranslator.FromHtml("#B25040");
            this.availablePalette102.Color = System.Drawing.ColorTranslator.FromHtml("#79898D");
            this.availablePalette103.Color = System.Drawing.ColorTranslator.FromHtml("#406150");
            this.availablePalette104.Color = System.Drawing.ColorTranslator.FromHtml("#794C81");
            this.availablePalette105.Color = System.Drawing.ColorTranslator.FromHtml("#BEC6CE");
            this.availablePalette106.Color = System.Drawing.ColorTranslator.FromHtml("#AA8D8D");
            this.availablePalette107.Color = System.Drawing.ColorTranslator.FromHtml("#756961");
            this.availablePalette108.Color = System.Drawing.ColorTranslator.FromHtml("#CA9999");
            this.availablePalette109.Color = System.Drawing.ColorTranslator.FromHtml("#C67965");
            this.availablePalette110.Color = System.Drawing.ColorTranslator.FromHtml("#9D9DAE");
            this.availablePalette111.Color = System.Drawing.ColorTranslator.FromHtml("#D28969");
            this.availablePalette112.Color = System.Drawing.ColorTranslator.FromHtml("#717D65");
            this.availablePalette113.Color = System.Drawing.ColorTranslator.FromHtml("#AE9D95");
            this.availablePalette114.Color = System.Drawing.ColorTranslator.FromHtml("#855530");
            this.availablePalette115.Color = System.Drawing.ColorTranslator.FromHtml("#FFEE8D");
            this.availablePalette116.Color = System.Drawing.ColorTranslator.FromHtml("#DADADA");
            this.availablePalette117.Color = System.Drawing.ColorTranslator.FromHtml("#DADADA");
            this.availablePalette118.Color = System.Drawing.ColorTranslator.FromHtml("#DADADA");
            this.availablePalette119.Color = System.Drawing.ColorTranslator.FromHtml("#DADADA");
        }
        #endregion
    }
}
