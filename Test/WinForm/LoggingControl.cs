using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Control intended to display loggin messages raised by another class</summary>
    public partial class LoggingControl : UserControl
    {
        private delegate void VoidStringParameterInvoke(String param);

        /// <summary>Default constructor</summary>
        public LoggingControl()
        {
            InitializeComponent();
        }

        /// <summary>Posts a message to this control</summary>
        /// <param name="message">Message to be posted</param>
        public virtual void PostMessage(String message)
        {
            this.AddMessageControl(message);
        }

        /// <summary>Adds a new TextBox control to the panel containing output</summary>
        /// <param name="text">Text of te new control to add</param>
        protected virtual void AddMessageControl(String text)
        {
            if (this.InvokeRequired)
                this.Invoke(new VoidStringParameterInvoke(this.AddMessageControl), new Object[] { text });
            else
            {
                TextBox messageControl = new TextBox();
                messageControl.Text = text;
                messageControl.BorderStyle = BorderStyle.FixedSingle;
                messageControl.ReadOnly = true;
                messageControl.Multiline = true;
                messageControl.Dock = DockStyle.Top;
                messageControl.WordWrap = true;
                messageControl.Font = new Font("Consolas", 8.25f, FontStyle.Regular);

                Size ofInterest = TextRenderer.MeasureText(text, messageControl.Font);
                Size interest = TextRenderer.MeasureText(text, messageControl.Font, messageControl.Size);

                if (this.pnlControlParent.Controls.Count == 0)
                    messageControl.Top = 0;
                else
                {
                    Control previous = this.pnlControlParent.Controls[this.pnlControlParent.Controls.Count - 1];
                    messageControl.Top = previous.Top + previous.Height + 2;
                }

                Size output = this.CalculateDimensionsConstrainedByWidth(text, messageControl.Font, this.pnlControlParent.Size);
                messageControl.Size = output;

                this.pnlControlParent.Controls.Add(messageControl);
            }
        }

        /// <summary>Calculates the appropriate dimensions of a control, given the text to be assigned</summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="font">Font in which the text will be rendered</param>
        /// <param name="parent">Parent control's size</param>
        /// <returns>New dimensions for the child control</returns>
        /// <remarks>Does not account for worpwrap rendering, where a word will skip to the next line if not enough width remaining</remarks>
        protected virtual Size CalculateDimensionsConstrainedByWidth(String text, Font font, Size parent)
        {
            return this.CalculateDimensionsConstrainedByWidth(text, font, parent, null);
        }

        /// <summary>Calculates the appropriate dimensions of a control, given the text to be assigned</summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="font">Font in which the text will be rendered</param>
        /// <param name="parent">Parent control's size</param>
        /// <param name="initial">Optional, initial size of the control.</param>
        /// <returns>New dimensions for the child control</returns>
        /// <remarks>Does not account for worpwrap rendering, where a word will skip to the next line if not enough width remaining</remarks>
        protected virtual Size CalculateDimensionsConstrainedByWidth(String text, Font font, Size parent, Size? initial)
        {
            Size calc;  //TextRenderer result

            if (initial == null)
                calc = TextRenderer.MeasureText(text, font);
            else
                calc = TextRenderer.MeasureText(text, font, initial.Value, TextFormatFlags.WordBreak);

            //get base dimensions, with some padding
            Int32 baseHeight = calc.Height + 4; //padding between lines
            Int32 baseWidth = parent.Width - 6; //padding around control = 3

            //calculate output dimensions
            Size output = new Size();
            output.Width = baseWidth;
            output.Height = ((calc.Width / baseWidth) + (calc.Width % baseWidth > 0 ? 1 : 0)) * baseHeight;
            output.Height += 2; //margin = 6, but trailing line's space = 4

            return output;
        }

        /// <summary>Handler for resizing the control</summary>
        /// <param name="e">Event arguments for the resize operation</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            for (Int32 index = 0; index < this.pnlControlParent.Controls.Count; ++index)
            {
                Control c = this.pnlControlParent.Controls[index];
                c.Size = this.CalculateDimensionsConstrainedByWidth(c.Text, c.Font, pnlControlParent.Size);

                //set control top position, since controls are docked to the top
                if (index == 0)
                    c.Top = 0;
                else
                {
                    Control previous = this.pnlControlParent.Controls[this.pnlControlParent.Controls.Count - 1];
                    c.Top = previous.Top + previous.Height + 2;
                }
            }
        }

        /// <summary>Clears the logged message controls</summary>
        public virtual void ClearControls()
        {
            this.pnlControlParent.Controls.Clear();

            //this.logOutput.Controls.Clear();  //only detaches the controls, not removing them altogether
            for (Int32 index = (this.pnlControlParent.Controls.Count - 1); index > -1; --index)
                this.pnlControlParent.Controls.RemoveAt(index);
        }
    }
}