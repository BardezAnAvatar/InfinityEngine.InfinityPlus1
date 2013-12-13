using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Bardez.Projects.WinForms.Controls
{
    /// <summary>A tab control that you can close from the tab itself</summary>
    /// <remarks>Based off of code from http://www.codeproject.com/Articles/20050/FireFox-like-Tab-Control </remarks>
    public class ClosableTabControl : TabControl
    {
        #region Constants
        /// <summary>Represents the amount of padding to allow the 'button' drawn</summary>
        protected static readonly Int32 PaddingCloseInner = 2;

        /// <summary>Amount of padding surrounding UI elements on the tab</summary>
        protected static readonly Int32 PaddingArea = 2;

        /// <summary>Amount of padding surrounding UI elements on the tab</summary>
        protected static readonly Int32 PaddingCloseOuter = 3;
        #endregion


        #region Fields
        /// <summary>Flag indicating whether or not to confirm when closing a tab</summary>
        protected Boolean confirmClose;
        #endregion


        #region Properties
        /// <summary>Flag indicating whether or not to confirm when closing a tab</summary>
        public Boolean ConfirmClose
        {
            get { return this.confirmClose; }
            set { this.confirmClose = value; }
        }
        #endregion


        #region Local Events
        /// <summary>Event that occurs when a tab is closed</summary>
        protected event Action<Object, TabCloseEventArgs> closeTab;
        #endregion


        #region Exposed Events
        /// <summary>Event that occurs when a tab is closed</summary>
        public event Action<Object, TabCloseEventArgs> CloseTab
        {
            add { this.closeTab += value; }
            remove { this.closeTab -= value; }
        }
        #endregion

        
        #region Construction
        /// <summary>Default constructor</summary>
        public ClosableTabControl() : base()
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.closeTab += this.ReactToCloseTab;
        }
        #endregion


        #region Event Handlers
        /// <summary>Draws the item</summary>
        /// <param name="e">Drawing parameters</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //check if the bounds are valid
            if (e.Bounds != RectangleF.Empty)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                //iterate through tabs
                for (Int32 index = 0; index < this.TabCount; ++index)
                {
                    this.DrawTab(e.Graphics, index, index == this.SelectedIndex);
                }
            }
        }

        /// <summary>Reacts to a tab mouse click</summary>
        /// <param name="e">Mouse event parameters</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (! this.DesignMode)
            {
                RectangleF tabArea = this.GetTabRect(this.SelectedIndex);
                RectangleF closeButtonArea = this.GetCloseButtonRectangle(tabArea);

                if (closeButtonArea.Contains(e.X, e.Y))
                {
                    if (this.confirmClose)
                    {
                        String message = String.Format("You are about to close a tab (\"{0}\"). Are you sure you want to continue?", this.TabPages[this.SelectedIndex].Text.Trim());
                        DialogResult result = MessageBox.Show(message, "Confirm close", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                            return;
                    }

                    //Fire Event to Client
                    this.RaiseCloseTab(this.SelectedIndex);
                }
            }
        }

        /// <summary>Default tab close event handler</summary>
        /// <param name="e">Parameters associated with this event</param>
        protected virtual void OnCloseTab(TabCloseEventArgs e)
        {
            if (e != null && this.TabPages.Count > e.TabIndex)
                this.TabPages.RemoveAt(e.TabIndex);
        }

        /// <summary>Reacts to the close tab event</summary>
        /// <param name="sender">Object sending the message</param>
        /// <param name="e">Parameters associated with this event</param>
        protected void ReactToCloseTab(Object sender, TabCloseEventArgs e)
        {
            this.OnCloseTab(e);
        }

        /// <summary>Draws the tab</summary>
        /// <param name="graphics">Graphics object to draw with</param>
        /// <param name="tabIndex">Index of the tab to draw</param>
        /// <param name="tabActive">Flag indicating whether the tab to draw is the active tab</param>
        [Obsolete("This is the older gradient brush code, and it does not look right on Windows 7")]
        protected void DrawTabOriginalGradient(Graphics graphics, Int32 tabIndex, Boolean tabActive)
        {
            //built-in implicit operator
            RectangleF area = this.GetTabRect(tabIndex);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(area);

                using (LinearGradientBrush brush = new LinearGradientBrush(area, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical))
                {
                    ColorBlend blend = new ColorBlend(3);

                    if (tabActive)
                        blend.Colors = new Color[] { SystemColors.ControlLightLight, SystemColors.ControlLight, SystemColors.ControlDark, SystemColors.ControlLightLight };
                    else
                        blend.Colors = new Color[] { SystemColors.ControlLightLight, SystemColors.Control, SystemColors.ControlLight, SystemColors.Control };

                    blend.Positions = new Single[] { 0f, .4f, 0.5f, 1f };
                    brush.InterpolationColors = blend;

                    graphics.FillPath(brush, path);
                    using (Pen pen = new Pen(SystemColors.ActiveBorder))
                        graphics.DrawPath(pen, path);

                    //Draw Close Button
                    if (tabActive)
                        blend.Colors = new Color[] { Color.FromArgb(255, 231, 164, 152), Color.FromArgb(255, 231, 164, 152), Color.FromArgb(255, 197, 98, 79), Color.FromArgb(255, 197, 98, 79) };
                    else
                        blend.Colors = new Color[] { SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder };

                    brush.InterpolationColors = blend;

                    graphics.FillRectangle(brush, area.X + area.Width - 22, 4, area.Height - 3, area.Height - 5);
                    graphics.DrawRectangle(Pens.White, area.X + area.Width - 20, 6, area.Height - 8, area.Height - 9);

                    using (Pen pen = new Pen(Color.White, 2))
                    {
                        graphics.DrawLine(pen, area.X + area.Width - 16, 9, area.X + area.Width - 7, 17);
                        graphics.DrawLine(pen, area.X + area.Width - 16, 17, area.X + area.Width - 7, 9);
                    }
                }
            }

            using (SolidBrush brush = new SolidBrush(this.TabPages[tabIndex].ForeColor))
                graphics.DrawString(this.TabPages[tabIndex].Text, this.Font, brush, area);
        }

        /// <summary>Draws the tab</summary>
        /// <param name="graphics">Graphics object to draw with</param>
        /// <param name="area">Rectangle to draw with</param>
        /// <param name="tabIndex">Index of the tab to draw</param>
        /// <param name="tabActive">Flag indicating whether the tab to draw is the active tab</param>
        protected void DrawTab(Graphics graphics, Int32 tabIndex, Boolean tabActive)
        {
            //built-in implicit operator
            RectangleF area = this.GetTabRect(tabIndex);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(area);

                //Draw the tab
                Color tabColor = tabActive ? SystemColors.ControlLight : SystemColors.ControlDark;
                using (SolidBrush brush = new SolidBrush(tabColor))
                {
                    graphics.FillPath(brush, path);
                    using (Pen pen = new Pen(SystemColors.ActiveBorder))
                        graphics.DrawPath(pen, path);
                }
            }

            //Draw Close Button
            Color closeBoxColor = tabActive ? Color.IndianRed : Color.PaleVioletRed;
            
            //set up bounding box to fill
            RectangleF closeArea = this.GetCloseButtonRectangle(area);

            //draw the box
            using (SolidBrush brush = new SolidBrush(closeBoxColor))
                graphics.FillRectangle(brush, closeArea); //closeLocation.X, closeLocation.Y, closeSize.Width, closeSize.Height);

            //set up inner box
            RectangleF innerArea = this.GetCloseButtonInnerRectangle(closeArea);

            //interesting note: Do not dispose of existing Pens (i.e. using(Pen pen = Pen.White) is bad).

            //draw the rectangle within the close box
            graphics.DrawRectangle(Pens.White, innerArea.X, innerArea.Y, innerArea.Width, innerArea.Height);

            //draw the "X"
            Single xBegin = innerArea.X;
            Single xEnd = xBegin + innerArea.Width;
            Single yBeign = innerArea.Y;
            Single yEnd = yBeign + innerArea.Height;

            //draw the two crossing lines
            graphics.DrawLine(Pens.White, xBegin, yBeign, xEnd, yEnd);
            graphics.DrawLine(Pens.White, xBegin, yEnd, xEnd, yBeign);

            //Draw the text
            PointF textPoint = new PointF(area.X + ClosableTabControl.PaddingArea, area.Y + ClosableTabControl.PaddingArea);
            using (SolidBrush brush = new SolidBrush(this.TabPages[tabIndex].ForeColor))
                graphics.DrawString(this.TabPages[tabIndex].Text, this.Font, brush, textPoint);
        }
        #endregion


        #region Event Raising Methods
        /// <summary>Raises the Close tab event to subscribers</summary>
        /// <param name="index">Tab index to close</param>
        public void RaiseCloseTab(Int32 index)
        {
            if (this.closeTab != null)
                this.closeTab(this, new TabCloseEventArgs(index));
        }
        #endregion


        #region Helper methods
        /// <summary>Gets the Stream for the specified resource form the executing assembly</summary>
        /// <param name="filename">Name of the resource to retrieve</param>
        /// <returns>A Stream containing the data for the requested resource</returns>
        protected Stream GetContentFromResource(String filename)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream(filename);
            return stream;
        }
        #endregion


        #region Location Helpers
        /// <summary>Gets the rectangle describing where to draw the close button</summary>
        /// <param name="area">Area in which to draw the tab</param>
        /// <returns>The rectangle describing the drawing area</returns>
        protected RectangleF GetCloseButtonRectangle(RectangleF area)
        {
            Single width = area.Height - (2 * ClosableTabControl.PaddingCloseOuter);
            Single height = area.Height - (2 * ClosableTabControl.PaddingCloseOuter);
            Single x = area.X + area.Width - (width + ClosableTabControl.PaddingCloseOuter);
            Single y = ClosableTabControl.PaddingCloseOuter + ClosableTabControl.PaddingArea;

            RectangleF rectangle = new RectangleF(x, y, width, height);
            return rectangle;
        }

        /// <summary>Gets the rectangle describing where to draw the inner box of the close button</summary>
        /// <param name="outerArea">Close button area to draw within</param>
        /// <returns>The rectangle describing the inner drawing area</returns>
        protected RectangleF GetCloseButtonInnerRectangle(RectangleF outerArea)
        {
            Single width = outerArea.Width - (ClosableTabControl.PaddingCloseInner * 2);
            Single height = outerArea.Height - (ClosableTabControl.PaddingCloseInner * 2);
            Single x = outerArea.X + ClosableTabControl.PaddingCloseInner;
            Single y = outerArea.Y + ClosableTabControl.PaddingCloseInner;

            RectangleF innerBox = new RectangleF(x, y, width, height);
            return innerBox;
        }
        #endregion
    }
}