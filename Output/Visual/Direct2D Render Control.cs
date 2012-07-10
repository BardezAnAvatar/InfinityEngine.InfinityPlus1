using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Direct2D = Bardez.Projects.DirectX.Direct2D;
using ExternalPixelEnums = Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.DirectX.Direct2D;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.Utility.DataContainers;
using Bardez.Projects.Win32;
using Bardez.Projects.ReusableCode;

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
        /*
        *   Locking orientation:
        *       There are two rendering targets: a GDI display and a bitmap back buffer.
        *       There are 5 'real' locking operations:
        *           Rendering to the GDI display (OnPaint)
        *           Rendering to the back buffer (DrawBitmapToBuffer, DiscardCurrentBuffer)
        *           Setting the current displayed frame (SetRenderFrame)
        *           Resource Freeing (FreeFrameResource)
        *           Resizing (OnResize)
        *
        *       Briefly, the overarching effects of these five are:
        *           Rendering
        *               Utilizes the buffer and the display
        *           Back Buffer
        *               Utilizes the buffer and if mid-render could affect this display
        *           Set Frame
        *               Sets the buffer's displayed image
        *           Resource Freeing
        *               Affects the buffer if a resource is refernced
        *           Resizing
        *               Resizes the render control and the bitmap, uses the frame set by set
        *           
        *       Locking plan:
        *           Resize should block set, free and back buffer
        *           Render should block back buffer, control
        *           Set frame should block resize and back buffer
        *           Free should block back buffer, 
        *           Back buffer should block rendering, setting, resizing, freeing
        *      
        *       Basically, lock everything at the process level, and not at the access level.
        */      

        #region Fields
        /// <summary>Represents a Win32 HWND render target for Direct2D</summary>
        private ControlRenderTarget ctrlRenderTarget;

        /// <summary>Represents a drawing buffer</summary>
        /// <remarks>Do not reference directly</remarks>
        private BitmapRenderTarget bmpRenderTarget;

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

        /// <summary>Exposes a wrapper for the bitmap render target</summary>
        protected BitmapRenderTarget BmpRenderTarget
        {
            get { return this.bmpRenderTarget; }
            set
            {
                lock (this.controlBufferLock)
                {
                    if (this.bmpRenderTarget != null)
                        this.bmpRenderTarget.Dispose();

                    this.bmpRenderTarget = value;
                }
            }
        }

        /// <summary>Represents a Win32 HWND render target for Direct2D</summary>
        protected ControlRenderTarget CtrlRenderTarget
        {
            get { return this.ctrlRenderTarget; }
            set
            {
                lock (this.controlPaintRenderLock)
                {
                    if (this.ctrlRenderTarget != null)
                        this.ctrlRenderTarget.Dispose();

                    this.ctrlRenderTarget = value;
                }
            }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Direct2dRenderControl() : base()
        {
            this.controlBufferLock = new Object();
            this.controlPaintRenderLock = new Object();

            this.currentFrameKey = -1;
            this.InitializeControlDirect2D();
        }

        /// <summary>Initializes the Direct2D</summary>
        protected void InitializeControlDirect2D()
        {
            //disable Windows background draw
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            // Build options on the Control render target
            PixelFormat format = new PixelFormat(DXGI_ChannelFormat.FORMAT_B8G8R8A8_UNORM, AlphaMode.Unknown);  //32-bit color, pure alpha
            DpiResolution res = Direct2dResourceManager.Instance.Factory.GetDesktopDpi();
            RenderTargetProperties rtProp = new RenderTargetProperties(RenderTargetType.Default, format, res, RenderTargetUsage.GdiCompatible, DirectXVersion.DirectX9);

            //Build out control render target properties
            HwndRenderTargetProperties hwndProp = new HwndRenderTargetProperties(this.Handle, new SizeU(this.Size), PresentationOptions.RetainContents);

            lock (this.controlPaintRenderLock)
            {
                // populate the Control rendering target
                ResultCode result = Direct2dResourceManager.Instance.Factory.CreateHwndRenderTarget(rtProp, hwndProp, out this.ctrlRenderTarget);

                lock (this.controlBufferLock)
                {
                    // create a bitmap rendering targets
                    this.CtrlRenderTarget.CreateCompatibleRenderTarget(out this.bmpRenderTarget);
                }
            }
        }
        #endregion


        #region Destruction
        /// <summary>Disposal code; releases unmanaged resources</summary>
        /// <param name="disposing">True indicates to dispose managed resources</param>
        /// <remarks>Dispose()</remarks>
        protected override void Dispose(Boolean disposing)
        {
            this.BmpRenderTarget = null;            //property disposes
            this.CtrlRenderTarget = null;           //property disposes
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
            lock (this.controlPaintRenderLock)
            {
                lock (this.controlBufferLock)
                {
                    this.SuspendLayout();

                    this.OnPaintBackground(e);

                    //copy the bitmap
                    Direct2D.Bitmap bmp;
                    ResultCode result;

                    result = this.BmpRenderTarget.GetBitmap(out bmp);

                    Direct2D.RectangleF rect = new Direct2D.RectangleF(e.ClipRectangle);

                    //tell Direct2D that a paint is beginning
                    this.CtrlRenderTarget.BeginDraw();

                    // do the actual draw
                    this.CtrlRenderTarget.DrawBitmap(bmp, rect, 1.0F, BitmapInterpolationMode.Linear, rect);

                    //tell Direct2D that a paint is ending
                    result = this.CtrlRenderTarget.EndDraw();

                    bmp.Dispose();  //dispose the newly acquired bitmap

                    if (result != ResultCode.Success_OK)
                        throw new ApplicationException(String.Format("Error encountered during draw: '{0}'", result.ToString()));

                    // do everything else; raise the Paint event
                    base.OnPaint(e);

                    this.ResumeLayout();
                }
            }
        }

        /// <summary>Overides the resize method</summary>
        /// <param name="e">Parameters for the resize event</param>
        protected override void OnResize(EventArgs e)
        {
            lock (this.controlPaintRenderLock)
            {
                lock (this.controlBufferLock)
                {
                    //Null check since resize fires before it is constructed, with the size event during parent's InitializeComponent
                    if (this.CtrlRenderTarget != null)
                    {
                        this.SuspendLayout();

                        //resize the existing control rendering target
                        this.CtrlRenderTarget.Resize(new SizeU(this.Size));

                        //I also need to resize the buffer, but can't. Instead, create a new one, then copy the existing one. Kind of lame.
                        this.ResizeBitmapRenderTarget();

                        // do everything else; raise the Resize event
                        base.OnResize(e);

                        //cause another draw
                        this.Invalidate(new Rectangle(new Point(0, 0), this.Size));

                        this.ResumeLayout();
                    }
                }
            }
        }

        /// <summary>Overridden Paint background method</summary>
        /// <param name="e">Paint event arguments</param>
        /// <remarks>
        ///     Made empty to avoid GDI and Direct2D writing to the same control; when moving the control around
        ///     or rendering at high speeds, the dreaded WinForms flicker was introduced.
        ///     The background painting can be found in the <see cref="FillBufferRenderTarget"/> method,
        ///     which fills the bitmap back buffer with the control's background color.
        /// </remarks>
        protected override void OnPaintBackground(PaintEventArgs e) { }
        #endregion


        #region Drawing
        /// <summary>Resizes the bitmap rendering target buffer</summary>
        /// <remarks>Does not lock the GDI render target. The OnResize or other callers lock instead.</remarks>
        protected void ResizeBitmapRenderTarget()
        {
            lock (this.controlPaintRenderLock)
            {
                lock (this.controlBufferLock)
                {
                    using (BitmapRenderTarget bmpCurr = this.BmpRenderTarget)
                    {
                        BitmapRenderTarget bmpNew;

                        // create a bitmap rendering target
                        ResultCode result = this.CtrlRenderTarget.CreateCompatibleRenderTarget(out bmpNew);

                        //Copy the contents
                        this.DuplicateDoubleBufferContents(bmpNew);

                        //Property disposes and locks
                        this.BmpRenderTarget = bmpNew;
                    } //dispose the previous, now unused bitmap renderer
                }
            }
        }

        /// <summary>Draws a Bitmap to the render buffer</summary>
        /// <param name="bmp">Direct2D bitmap to draw to the buffer.</param>
        protected void DrawBitmapToBuffer(Direct2D.Bitmap bmp, Point2dF origin)
        {
            lock (this.controlBufferLock)
            {
                //Determine the copy rectangle
                Direct2D.SizeF bmpSize = bmp.GetSize();

                Single width = bmpSize.Width > this.BmpRenderTarget.Size.Width ? this.BmpRenderTarget.Size.Width : bmpSize.Width;
                Single height = bmpSize.Height > this.BmpRenderTarget.Size.Height ? this.BmpRenderTarget.Size.Height : bmpSize.Height;

                Direct2D.RectangleF destRect = new Direct2D.RectangleF(origin.X, origin.X + width, origin.Y, origin.Y + height);
                Direct2D.RectangleF srcRect = new Direct2D.RectangleF(0.0F, width, 0.0F, height);

                //tell Direct2D to start the draw
                this.BmpRenderTarget.BeginDraw();

                //Flood-fill the rendering target with the existing control color
                this.FillBufferRenderTarget(this.BmpRenderTarget);

                // do the actual draw
                this.BmpRenderTarget.DrawBitmap(bmp, destRect, 1.0F, BitmapInterpolationMode.Linear, srcRect);

                //tell Direct2D that a paint operation is ending
                ResultCode result = this.BmpRenderTarget.EndDraw();
            }
        }

        /// <summary>Draws a Bitmap to the render buffer</summary>
        /// <param name="bmp">Direct2D bitmap to draw to the buffer.</param>
        protected void DrawBitmapToBuffer(Direct2D.Bitmap bmp)
        {
            this.DrawBitmapToBuffer(bmp, new Point2dF(0.0F, 0.0F));
        }

        /// <summary>Duplicates the bitmap behind the existing rendering target, and drawing it to a new one, discarding the current and setting the new.</summary>
        /// <remarks>Does not lock any references, as the outside method locks</remarks>
        protected void DuplicateDoubleBufferContents(BitmapRenderTarget bmpNew)
        {
            Direct2D.Bitmap bmp = null;
            ResultCode result = ResultCode.Success_OK;

            if (this.HasFrameSet)
                bmp = Direct2dResourceManager.Instance.GetBitmapResource(this.currentFrameKey);
            else
                result = this.BmpRenderTarget.GetBitmap(out bmp);

            //begin the draw
            bmpNew.BeginDraw();

            //Fill
            this.FillBufferRenderTarget(bmpNew);

            //calculate the size to copy
            Direct2D.SizeF bmpSize = bmp.GetSize();

            Single width = bmpSize.Width > this.CtrlRenderTarget.Size.Width ? this.CtrlRenderTarget.Size.Width : bmpSize.Width;
            Single height = bmpSize.Height > this.CtrlRenderTarget.Size.Height ? this.CtrlRenderTarget.Size.Height : bmpSize.Height;

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
            lock (this.controlBufferLock)
            {
                BitmapRenderTarget bmpNew;

                // create a bitmap rendering target
                ResultCode result = this.CtrlRenderTarget.CreateCompatibleRenderTarget(out bmpNew);

                bmpNew.BeginDraw();             //tell Direct2D to start the draw
                this.FillBufferRenderTarget(bmpNew);    //flood fill
                result = bmpNew.EndDraw();      //tell Direct2D that a paint operation is ending

                //property locks, so no lock here
                this.BmpRenderTarget = bmpNew;  //replace the old buffer
            }
        }

        /// <summary>Fills the buffer render target with the background color</summary>
        /// <param name="renderTarget">Bitmap render target to fill</param>
        protected void FillBufferRenderTarget(BitmapRenderTarget renderTarget)
        {
            //Get a solid brush
            SolidColorBrush brush = null;

            ResultCode result = renderTarget.CreateSolidColorBrush(new ColorF(this.BackColor), out brush);
            
            //flood fill
            renderTarget.FillRectangle(new Direct2D.RectangleF(new Rectangle(new Point(0, 0), this.Size)), brush);
            brush.Dispose(); //clean up device resource
        }
        #endregion


        #region Frame Resources
        /// <summary>Posts a Frame resource to the resource manager and returns a unique key to access it.</summary>
        /// <param name="resource">Frame to be posted.</param>
        /// <returns>A unique Int32 key</returns>
        public override Int32 AddFrameResource(Frame resource)
        {
            lock (this.controlBufferLock)
            {
                //create the bitmap
                BitmapProperties properties = new BitmapProperties(new PixelFormat(DXGI_ChannelFormat.FORMAT_B8G8R8A8_UNORM, AlphaMode.PreMultiplied), Direct2dResourceManager.Instance.Factory.GetDesktopDpi());
                SizeU dimensions = new SizeU(Convert.ToUInt32(resource.Pixels.Metadata.Width), Convert.ToUInt32(resource.Pixels.Metadata.Height));

                Direct2D.Bitmap bmp = null;
                ResultCode result = this.BmpRenderTarget.CreateBitmap(dimensions, properties, out bmp);

                if (result != ResultCode.Success_OK)
                    throw new ApplicationException(String.Format("Error creating a Direct2D Bitmap: {0}", result.ToString()));

                //get data and read to Byte array
                MemoryStream data = resource.Pixels.GetPixelData(ExternalPixelEnums.PixelFormat.RGBA_B8G8R8A8, ScanLineOrder.TopDown, 0, 0);
                Byte[] binData = data.ToArray();

                //submit byte array
                result = bmp.CopyFromMemory(new RectangleU(dimensions), binData, Convert.ToUInt32(resource.Pixels.Metadata.Width * 4));

                if (result != ResultCode.Success_OK)
                    throw new ApplicationException(String.Format("Error creating a Direct2D Bitmap: {0}", result.ToString()));

                return Direct2dResourceManager.Instance.AddFrameResource(bmp);
            }
        }

        /// <summary>Frees a Bitmap resource in the resource manager and Disposes of it.</summary>
        /// <param name="frameKey">Direct2D Bitmap key to be Disposed.</param>
        public override void FreeFrameResource(Int32 frameKey)
        {
            lock (this.controlBufferLock)
            {
                if (frameKey > -1)
                    Direct2dResourceManager.Instance.FreeFrameResource(frameKey);
            }
        }
        #endregion


        #region Frame Setting
        /// <summary>Sets the frame to be rendered to the User Control</summary>
        /// <param name="key">Frame key to set as current image</param>
        public override void SetRenderFrame(Int32 key)
        {
            this.SetRenderFrame(key, 0, 0);
        }

        /// <summary>Sets the frame to be rendered to the User Control</summary>
        /// <param name="key">Frame key to set as current image</param>
        /// <param name="originX">X coordinate to start drawing from</param>
        /// <param name="originY">Y coordinate to start drawing from</param>
        public override void SetRenderFrame(Int32 key, Int64 originX, Int64 originY)
        {
            lock (this.controlBufferLock)
            {
                if (key > -1)
                    this.DrawBitmapToBuffer(Direct2dResourceManager.Instance.GetBitmapResource(key), new Point2dF(Convert.ToSingle(originX), Convert.ToSingle(originY)));
                else
                    this.DiscardCurrentBuffer();

                this.currentFrameKey = key;
            }
        }

        /// <summary>This method invokes the rendering. For use by the appliation to tell the control to change images on demand.</summary>
        public void Render()
        {
            this.Invalidate();
        }
        
        /// <summary>Sets the frame to be rendered to the User Control and then renders it</summary>
        /// <param name="key">Frame key to set as current image</param>
        public virtual void SetRenderFrameAndRender(Int32 key)
        {
            this.SetRenderFrameAndRender(key, false);
        }

        /// <summary>Sets the frame to be rendered to the User Control and then renders it</summary>
        /// <param name="key">Frame key to set as current image</param>
        /// <param name="freePreviousFrame">
        ///     Flag indicating whether to dispose of the previous image set
        ///     (in the case of transient images, such as composite images for a game or high-frame video playback)
        /// </param>
        public virtual void SetRenderFrameAndRender(Int32 key, Boolean freePreviousFrame)
        {
            this.SetRenderFrameAndRender(key, 0, 0, freePreviousFrame);
        }

        /// <summary>Sets the frame to be rendered to the User Control and then renders it</summary>
        /// <param name="key">Frame key to set as current image</param>
        /// <param name="originX">X coordinate to start drawing from</param>
        /// <param name="originY">Y coordinate to start drawing from</param>
        public virtual void SetRenderFrameAndRender(Int32 key, Int64 originX, Int64 originY)
        {
            this.SetRenderFrameAndRender(key, originX, originY, false);
        }

        /// <summary>Sets the frame to be rendered to the User Control and then renders it</summary>
        /// <param name="key">Frame key to set as current image</param>
        /// <param name="originX">X coordinate to start drawing from</param>
        /// <param name="originY">Y coordinate to start drawing from</param>
        /// <param name="freePreviousFrame">
        ///     Flag indicating whether to dispose of the previous image set
        ///     (in the case of transient images, such as composite images for a game or high-frame video playback)
        /// </param>
        public virtual void SetRenderFrameAndRender(Int32 key, Int64 originX, Int64 originY, Boolean freePreviousFrame)
        {
            lock (this.controlBufferLock)
            {
                Int32 previousFrameKey = this.currentFrameKey;

                this.SetRenderFrame(key, originX, originY);
                this.Render();

                if (freePreviousFrame)
                    this.FreeFrameResource(previousFrameKey);
            }
        }
        #endregion


        //TODO: Design to allow for layered draws to the intermediate BitmapRenderTarget. Maybe a 2D List of Bitmaps and Rects?
    }
}