using System;
using System.Drawing;
using System.Windows.Forms;

using Direct2D = Bardez.Projects.DirectX.Direct2D;
using ExternalPixelEnums = Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Pixels.Enums;
using Bardez.Projects.DirectX.Direct2D;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Enums;
using Bardez.Projects.Win32;

namespace Bardez.Projects.InfinityPlus1.Output.Visual
{
    /// <summary>Represents a rendering target for Direct2D.</summary>
    /// <remarks>
    ///     To use this control, an external component will need to supply bitmap data. This does not need to be a GDI+ Bitmap class.
    ///     However, the container for this control will need to push data into this control.
    ///     So, in the case of a movie player (MVE), we'd see the following model:
    ///         * decompress a frame.
    ///         * push frame to control
    ///         * invoke Invalidate
    ///             * control will render the bitmap
    ///         * sleep just a little bit
    ///         * go back to first step
    ///        
    ///     Realistically, I would probably like to have a resources controller that creates the factory elsewhere and lets me instantiate
    ///     bitmaps left and right and I just keep instance references in my game controller.
    /// </remarks>
    public class Direct2dRenderControl : VisualRenderControl
    {
        #region Fields
        /// <summary>Represents a Win32 HWND render target for Direct2D</summary>
        protected ControlRenderTarget ctrlRenderTarget;

        /// <summary>Represents a drawing buffer</summary>
        protected BitmapRenderTarget bmpRengerTarget;

        /// <summary>Represents a buffer drawing command lock</summary>
        private Object controlBufferLock;

        /// <summary>Represents a paint/render drawing command lock</summary>
        private Object controlPaintRenderLock;

        /// <summary>Represents the key to accessing the currently set key</summary>
        protected Int32 currentFrameKey;
        #endregion


        #region Properties
        /// <summary>Indicates whether there is a frame set for this control</summary>
        protected Boolean HasFrameSet
        {
            get { return currentFrameKey > -1; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Direct2dRenderControl() : base()
        {
            // Build a factory
            this.controlBufferLock = new Object();
            this.controlPaintRenderLock = new Object();
            this.currentFrameKey = -1;
            this.InitializeControlDirect2D();
        }

        /// <summary>Initializes the Direct2D</summary>
        protected void InitializeControlDirect2D()
        {
            // Build options on the Control render target
            PixelFormat format = new PixelFormat(DXGI_ChannelFormat.FORMAT_B8G8R8A8_UNORM, AlphaMode.Unknown);  //32-bit color, pure alpha
            DpiResolution res = Direct2dResourceManager.Instance.Factory.GetDesktopDpi();
            RenderTargetProperties rtProp = new RenderTargetProperties(RenderTargetType.Default, format, res, RenderTargetUsage.GdiCompatible, DirectXVersion.DirectX9);

            //Build out control render target properties
            HwndRenderTargetProperties hwndProp = new HwndRenderTargetProperties(this.Handle, new SizeU(this.Size), PresentationOptions.RetainContents);

            // populate the Control rendering target
            ResultCode result = Direct2dResourceManager.Instance.Factory.CreateHwndRenderTarget(rtProp, hwndProp, out this.ctrlRenderTarget);

            // create a bitmap rendering target
            this.ctrlRenderTarget.CreateCompatibleRenderTarget(out this.bmpRengerTarget);
        }
        #endregion


        #region Destruction
        /// <summary>Disposal code; releases unmanaged resources</summary>
        /// <param name="disposing">True indicates to dispose managed resources</param>
        /// <remarks>Dispose()</remarks>
        protected override void Dispose(Boolean disposing)
        {
            lock (this.controlBufferLock)
            {
                if (this.bmpRengerTarget != null)
                {
                    this.bmpRengerTarget.Dispose();
                    this.bmpRengerTarget = null;
                }
            }

            lock (this.controlPaintRenderLock)
            {
                if (this.ctrlRenderTarget != null)
                {
                    this.ctrlRenderTarget.Dispose();
                    this.ctrlRenderTarget = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>Disposal</summary>
        /// <remarks>Finalize()</remarks>
        ~Direct2dRenderControl()
        {
            this.Dispose();
        }
        #endregion


        #region Event Raising
        /// <summary>Draws the output, then raises the paint event</summary>
        /// <param name="e">Painting Event arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            this.OnPaintBackground(e);

            //copy the bitmap
            Direct2D.Bitmap bmp;
            ResultCode result = this.bmpRengerTarget.GetBitmap(out bmp);

            using (bmp)
            {
                Direct2D.RectangleF rect = new Direct2D.RectangleF(e.ClipRectangle);

                //tell Direct2D that a paint is beginning
                this.ctrlRenderTarget.BeginDraw();

                // do the actual draw
                this.ctrlRenderTarget.DrawBitmap(bmp, rect, 1.0F, BitmapInterpolationMode.Linear, rect);
            } //dispose the newly acquired bitmap

            //tell Direct2D that a paint is ending
            result = this.ctrlRenderTarget.EndDraw();

            // do everything else; raise the Paint event
            base.OnPaint(e);
        }

        /// <summary>Overides the resize method</summary>
        /// <param name="e">Parameters for the resize event</param>
        protected override void OnResize(EventArgs e)
        {
            //Null check since resize fires before it is constructed, with the size event during parent's InitializeComponent
            if (this.ctrlRenderTarget != null)
            {
                lock (this.controlPaintRenderLock)
                {
                    //resize the existing control rendering target
                    this.ctrlRenderTarget.Resize(new SizeU(this.Size));
                }

                //I also need to resize the buffer, but can't. Instead, create a new one, then copy the existing one. Kind of lame.
                this.ResizeBitmapRenderTarget();

                // do everything else; raise the Resize event
                base.OnResize(e);

                this.Invalidate(new Rectangle(new Point(0, 0), this.Size));
            }
        }
        #endregion


        #region Drawing
        /// <summary>Resizes the bitmap rendering target buffer</summary>
        /// <remarks>Does not lock. The OnResize or other callers lock instead.</remarks>
        protected void ResizeBitmapRenderTarget()
        {
            lock (this.controlBufferLock)
            {
                using (BitmapRenderTarget bmpCurr = this.bmpRengerTarget)
                {
                    BitmapRenderTarget bmpNew;

                    // create a bitmap rendering target
                    ResultCode result = this.ctrlRenderTarget.CreateCompatibleRenderTarget(out bmpNew);

                    //Copy the contents
                    this.DuplicateDoubleBufferContents(bmpNew);

                    this.bmpRengerTarget = bmpNew;
                } //dispose the previous, now unused bitmap renderer
            }
        }

        /// <summary>Draws a Bitmap to the render buffer</summary>
        /// <param name="bmp">Direct2D bitmap to draw to the buffer.</param>
        protected void DrawBitmapToBuffer(Direct2D.Bitmap bmp)
        {
            //Determine the copy rectangle
            Direct2D.SizeF bmpSize = bmp.GetSize();
            Single width = bmpSize.Width > this.bmpRengerTarget.Size.Width ? this.bmpRengerTarget.Size.Width : bmpSize.Width;
            Single height = bmpSize.Height > this.bmpRengerTarget.Size.Height ? this.bmpRengerTarget.Size.Height : bmpSize.Height;

            Direct2D.RectangleF rect = new Direct2D.RectangleF(0, width, 0, height);

            //tell Direct2D to start the draw
            this.bmpRengerTarget.BeginDraw();

            //Flood-fill the rendering target with the existing control color
            this.FillBufferRenderTarget(this.bmpRengerTarget);

            // do the actual draw
            this.bmpRengerTarget.DrawBitmap(bmp, rect, 1.0F, BitmapInterpolationMode.Linear, rect);

            //tell Direct2D that a paint operation is ending
            ResultCode result = this.bmpRengerTarget.EndDraw();
        }

        /// <summary>Duplicates the bitmap behind the existing rendering target, and drawing it to a new one, discarding the current and setting the new.</summary>
        protected void DuplicateDoubleBufferContents(BitmapRenderTarget bmpNew)
        {
            Direct2D.Bitmap bmp = null;
            ResultCode result;

            if (this.HasFrameSet)
                bmp = Direct2dResourceManager.Instance.GetBitmapResource(this.currentFrameKey);
            else
                result = this.bmpRengerTarget.GetBitmap(out bmp);

            //begin the draw
            bmpNew.BeginDraw();

            //Fill
            this.FillBufferRenderTarget(bmpNew);

            //calculate the size to copy
            Direct2D.SizeF bmpSize = bmp.GetSize();
            Single width = bmpSize.Width > this.ctrlRenderTarget.Size.Width ? this.ctrlRenderTarget.Size.Width : bmpSize.Width;
            Single height = bmpSize.Height > this.ctrlRenderTarget.Size.Height ? this.ctrlRenderTarget.Size.Height : bmpSize.Height;

            //Determine the copy rectangle
            Direct2D.RectangleF rect = new Direct2D.RectangleF(0, width, 0, height);

            //Copy
            bmpNew.DrawBitmap(bmp, rect, 1.0F, BitmapInterpolationMode.Linear, rect);

            //conditionally disose the bitmap, don't if it is in the manager
            if (!this.HasFrameSet)
                bmp.Dispose();  //dispose the old render target bitmap just generated

            //tell Direct2D that a paint operation is ending
            result = bmpNew.EndDraw();
        }

        /// <summary>Discards the current buffer and replaces it with a blank one.</summary>
        protected void DiscardCurrentBuffer()
        {
            BitmapRenderTarget bmpNew;

            // create a bitmap rendering target
            ResultCode result = this.ctrlRenderTarget.CreateCompatibleRenderTarget(out bmpNew);
            
            bmpNew.BeginDraw();     //tell Direct2D to start the draw
            this.FillBufferRenderTarget(bmpNew);    //flood fill
            bmpNew.EndDraw();       //tell Direct2D that a paint operation is ending

            //replace the old buffer
            this.bmpRengerTarget = bmpNew;
        }

        /// <summary>Fills the buffer render target with the background color</summary>
        /// <param name="renderTarget">Bitmap render target to fill</param>
        protected void FillBufferRenderTarget(BitmapRenderTarget renderTarget)
        {
            //Get a solid brush
            SolidColorBrush brush = null;
            ResultCode result = this.bmpRengerTarget.CreateSolidColorBrush(new ColorF(this.BackColor), out brush);
            
            //flood fill
            renderTarget.FillRectangle(new Direct2D.RectangleF(new Rectangle(new Point(0, 0), this.Size)), brush);
            brush.Dispose(); //clean up device resource
        }
        #endregion


        #region Frame setting
        /// <summary>Posts a Frame resource to the resource manager and returns a unique key to access it.</summary>
        /// <param name="resource">Frame to be posted.</param>
        /// <returns>A unique Int32 key</returns>
        public override Int32 AddFrameResource(Frame resource)
        {
            //create the bitmap
            Direct2D.Bitmap bmp = null;
            BitmapProperties properties = new BitmapProperties(new PixelFormat(DXGI_ChannelFormat.FORMAT_B8G8R8A8_UNORM, AlphaMode.PreMultiplied), Direct2dResourceManager.Instance.Factory.GetDesktopDpi());
            SizeU dimensions = new SizeU(Convert.ToUInt32(resource.Pixels.Metadata.Width), Convert.ToUInt32(resource.Pixels.Metadata.Height));
            ResultCode result = this.bmpRengerTarget.CreateBitmap(dimensions, properties, out bmp);

            Byte[] data = resource.Pixels.GetPixelData(ExternalPixelEnums.PixelFormat.RGBA_B8G8R8A8, ScanLineOrder.TopDown, 0, 0);
            result = bmp.CopyFromMemory(new RectangleU(dimensions), data, Convert.ToUInt32(resource.Pixels.Metadata.Width * 4));

            return Direct2dResourceManager.Instance.AddFrameResource(bmp);
        }

        /// <summary>Sets the frame to be rendered to the User Control</summary>
        /// <param name="key">Frame key to set as current image</param>
        public override void SetRenderFrame(Int32 key)
        {
            if (key > -1)
                this.DrawBitmapToBuffer(Direct2dResourceManager.Instance.GetBitmapResource(key));
            else
                this.DiscardCurrentBuffer();

            this.currentFrameKey = key;
        }

        /// <summary>This method invokes the rendering. For use by the appliation to tell the control to change images on demand.</summary>
        public void Render()
        {
            this.Invalidate();
        }
        #endregion


        //TODO: slap something together that will allow for layered draws. Maybe a 2D List of Bitmaps and Rects?
    }
}