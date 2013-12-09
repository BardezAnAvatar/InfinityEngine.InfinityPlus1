using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Bardez.Projects.BasicStructures.Win32;
using Bardez.Projects.DirectX.Direct2D;
using Bardez.Projects.Factories;
using Bardez.Projects.Multimedia.MediaBase.Data.Pixels;
using PixelEnums = Bardez.Projects.Multimedia.MediaBase.Data.Pixels.Enums;
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.Multimedia.MediaBase.Render.Image;

namespace Bardez.Projects.InfinityPlus1.Output.Visual.Renderers.Direct2D
{
    /// <summary>Represents an image renderer that is a Forms User Control, using Direct2D to draw.</summary>
    public class Direct2dRendererUserControl : UserControl, IRendererImage
    {
        #region Fields
        /// <summary>Represents a Win32 HWND render target for Direct2D</summary>
        /// <remarks>Do not reference directly</remarks>
        private ControlRenderTarget ctrlRenderTarget;

        /// <summary>Represents a drawing buffer</summary>
        /// <remarks>Do not reference directly</remarks>
        private BitmapRenderTarget bmpRenderTarget;

        /// <summary>Represents the key to accessing the currently set key</summary>
        protected Int32 currentFrameKey;

        /// <summary>Image scaling factor. Used to zoom the image.</summary>
        protected Single imageScalingFactor;

        /// <summary>Represents the factory to create Direct2D objects</summary>
        protected Factory factory;

        /// <summary>Represents the resource collection of Direct2D bitmaps renderable</summary>
        protected List<Bardez.Projects.DirectX.Direct2D.Bitmap> renderResourceCollection;

        /// <summary>Represents a buffer drawing command lock</summary>
        private Object controlBufferLock;

        /// <summary>Represents a paint/render drawing command lock</summary>
        private Object controlPaintRenderLock;

        /// <summary>Private lock not accessable by other memory</summary>
        private Object resourceLock;

        /// <summary>Flag indicating whether there is a bending drawing operation or not</summary>
        protected Boolean isDrawing;
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


        #region Events
        /// <summary>Event that occurs when the renderer has finished rendering the provided data. Used to signal expiration and ready for disposal.</summary>
        protected event EventHandler finishedRendering;
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Direct2dRendererUserControl()
        {
            this.currentFrameKey = -1;
            this.InitializeControlDirect2D();

            this.factory = new Factory(FactoryType.MultiThreaded, DebugLevel.Information);
            this.renderResourceCollection = new List<Bardez.Projects.DirectX.Direct2D.Bitmap>();

            //locking objects
            this.controlBufferLock = new Object();
            this.controlPaintRenderLock = new Object();
            this.resourceLock = new Object();

            //flag
            this.isDrawing = false;
        }

        /// <summary>Initializes the Direct2D</summary>
        protected void InitializeControlDirect2D()
        {
            //disable Windows background draw
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            // Build options on the Control render target
            Bardez.Projects.DirectX.Direct2D.PixelFormat format = new Bardez.Projects.DirectX.Direct2D.PixelFormat(DXGI_ChannelFormat.FORMAT_B8G8R8A8_UNORM, AlphaMode.Unknown);  //32-bit color, pure alpha
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

            lock (this.resourceLock)
            {
                //dispose the bitmaps
                if (this.renderResourceCollection != null)
                {
                    for (Int32 i = 0; i < this.renderResourceCollection.Count; ++i)
                    {
                        if (this.renderResourceCollection[i] != null)
                        {
                            this.renderResourceCollection[i].Dispose();
                            this.renderResourceCollection[i] = null;
                        }
                    }

                    this.renderResourceCollection = null;
                }

                //dispose the factory
                if (this.factory != null)
                {
                    this.factory.Dispose();
                    this.factory = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>Disposal</summary>
        /// <remarks>Finalize()</remarks>
        ~Direct2dRendererUserControl()
        {
            this.Dispose();
        }
        #endregion


        #region IRendererImage Properties
        /// <summary>
        ///     This exposes the scaling factor of the renderer. It will scale the finished image, rendering
        ///     the scaled composite image in the middle anchor point.
        /// </summary>
        /// <value>Should be withing reasonable scaling. 1/16 to 16x?</value>
        public Single ScaleFactor
        {
            get { return this.imageScalingFactor; }
            set { this.imageScalingFactor = value; }
        }
        #endregion


        #region IRendererImage Events
        /// <summary>Event that occurs when the renderer has finished rendering the provided data. Used to signal expiration and ready for disposal.</summary>
        public event EventHandler FinishedRendering
        {
            add { this.finishedRendering += value; }
            remove { this.finishedRendering -= value; }
        }
        #endregion


        #region IRendererImage Methods
        #region Resource Management
        /// <summary>Posts an <see cref="IMultimediaImageFrame" /> resource to the renderer and returns a unique key to access it.</summary>
        /// <param name="resource"><see cref="IMultimediaImageFrame" /> to be submitted.</param>
        /// <returns>A unique UInt32 key</returns>
        public Int32 SubmitImageResource(IMultimediaImageFrame resource)
        {
            lock (this.controlBufferLock)
            {
                //create the bitmap
                BitmapProperties properties = new BitmapProperties(new Bardez.Projects.DirectX.Direct2D.PixelFormat(DXGI_ChannelFormat.FORMAT_B8G8R8A8_UNORM, AlphaMode.PreMultiplied), this.factory.GetDesktopDpi());
                SizeU dimensions = new SizeU(Convert.ToUInt32(resource.MetadataImage.Width), Convert.ToUInt32(resource.MetadataImage.Height));

                Bardez.Projects.DirectX.Direct2D.Bitmap bmp = null;
                ResultCode result = this.BmpRenderTarget.CreateBitmap(dimensions, properties, out bmp);

                if (result != ResultCode.Success_OK)
                    throw new ApplicationException(String.Format("Error creating a Direct2D Bitmap: {0}", result.ToString()));

                //get data and read to Byte array
                IPixelConverter converter = IPixelConverterFactory.GetIPixelConverter();
                MemoryStream data = resource.GetFormattedData(converter, PixelEnums.PixelFormat.RGBA_B8G8R8A8, PixelEnums.ScanLineOrder.TopDown, 0, 0);
                Byte[] binData = data.ToArray();

                //submit byte array
                result = bmp.CopyFromMemory(new RectangleU(dimensions), binData, Convert.ToUInt32(resource.MetadataImage.Width * 4));

                if (result != ResultCode.Success_OK)
                    throw new ApplicationException(String.Format("Error creating a Direct2D Bitmap: {0}", result.ToString()));

                lock (this.resourceLock)
                {
                    this.renderResourceCollection.Add(bmp);
                    return this.renderResourceCollection.Count - 1;
                }
            }
        }

        /// <summary>Frees a submitted resource and Disposes of it.</summary>
        /// <param name="key">Unique UInt32 key of the resource to be disposed</param>
        public void FreeImageResource(Int32 key)
        {
            lock (this.controlBufferLock)
            {
                if (key > -1)
                {
                    lock (this.resourceLock)
                    {
                        if (this.renderResourceCollection[key] != null)
                        {
                            this.renderResourceCollection[key].Dispose();
                            this.renderResourceCollection[key] = null;
                        }
                    }
                }
            }
        }
        #endregion


        #region Rendering operations
        /// <summary>Begins the drawing operation of the renderer</summary>
        public void StartDrawing()
        {
            if (!this.isDrawing)
            {
                this.isDrawing = true;
                this.DiscardCurrentBuffer();
            }
            else
                throw new InvalidOperationException("Cannot being drawing while another drawing operation has been started.");
        }

        /// <summary>Finishes the drawing operation of the renderer, and redners the image</summary>
        public void FinishDrawing()
        {
            if (this.isDrawing)
            {
                this.isDrawing = true;
                this.Invalidate();  //render the image to this control via Direct2D
            }
            else
                throw new InvalidOperationException("Cannot finish drawing while no drawing operation is pending.");
        }

        /// <summary>Draws the specified image to the renderer</summary>
        /// <param name="key">Unique UInt32 key of the resource to be drawn</param>
        /// <param name="originX">X coordinate to start drawing from</param>
        /// <param name="originY">Y coordinate to start drawing from</param>
        public void DrawImage(Int32 key, Int64 originX, Int64 originY)
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

        /// <summary>Draws the specified image to the renderer</summary>
        /// <param name="key">Unique UInt32 key of the resource to be drawn</param>
        public void DrawImage(Int32 key)
        {
            this.DrawImage(key, 0, 0);
        }

        /// <summary>Draws a line segment from one point to another</summary>
        /// <param name="start">Start point of the line segment</param>
        /// <param name="end">End point of the line segment</param>
        /// <param name="color">Color of the line segment to draw</param>
        /// <param name="width">Width, in pixels of the line segment to draw</param>
        /// <param name="style">Style of the line segment to draw</param>
        public void DrawLine(Point start, Point end, Color color, Single width, LineStyle style)
        {
            lock (this.controlBufferLock)
            {
                //tell Direct2D to start the draw
                this.BmpRenderTarget.BeginDraw();
                
                using (SolidColorBrush brush = this.BmpRenderTarget.CreateSolidColorBrush(new ColorF(color)))
                using (StrokeStyle strokeStyle = this.BuildStrokeStyle(style))
                    this.BmpRenderTarget.DrawLine(new Point2dF(start), new Point2dF(end), brush, width, strokeStyle);

                this.BmpRenderTarget.EndDraw();
            }
        }

        /// <summary>Draws an ellipse at a center point</summary>
        /// <param name="origin">Origin of the elliptical object to draw</param>
        /// <param name="radiusX">X-radius of the elliptoid</param>
        /// <param name="radiuxY">Y-radius of the elliptoid</param>
        /// <param name="color">Color of the elliptoid to draw</param>
        /// <param name="width">Width, in pixels of the elliptoid to draw</param>
        /// <param name="style">Style of the elliptoid to draw</param>
        public void DrawEllipse(Point origin, Single radiusX, Single radiuxY, Color color, Single width, LineStyle style)
        {
            Ellipse ellipse = new Ellipse(new Point2dF(origin), radiusX, radiuxY);

            lock (this.controlBufferLock)
            {
                //tell Direct2D to start the draw
                this.BmpRenderTarget.BeginDraw();

                using (SolidColorBrush brush = this.BmpRenderTarget.CreateSolidColorBrush(new ColorF(color)))
                using (StrokeStyle strokeStyle = this.BuildStrokeStyle(style))
                    this.BmpRenderTarget.DrawEllipse(ellipse, brush, width, strokeStyle);

                this.BmpRenderTarget.EndDraw();
            }
        }

        /// <summary>Draws a rectangle at a the specified points</summary>
        /// <param name="upperLeft">Upper-left point of the line segment</param>
        /// <param name="lowerRight">Lower-right point of the line segment</param>
        /// <param name="color">Color of the rectangle to draw</param>
        /// <param name="width">Width, in pixels of the rectangle to draw</param>
        /// <param name="style">Style of the rectangle to draw</param>
        public void DrawRectangle(Point upperLeft, Point lowerRight, Color color, Single width, LineStyle style)
        {
            DirectX.Direct2D.RectangleF rectangle = new DirectX.Direct2D.RectangleF(upperLeft.X, lowerRight.Y, upperLeft.Y, lowerRight.Y);

            lock (this.controlBufferLock)
            {
                //tell Direct2D to start the draw
                this.BmpRenderTarget.BeginDraw();

                using (SolidColorBrush brush = this.BmpRenderTarget.CreateSolidColorBrush(new ColorF(color)))
                using (StrokeStyle strokeStyle = this.BuildStrokeStyle(style))
                    this.BmpRenderTarget.DrawRectangle(rectangle, brush, width, strokeStyle);

                this.BmpRenderTarget.EndDraw();
            }
        }
        #endregion
        #endregion


        #region Event Raising
        /// <summary>Raises the FinishedRendering event</summary>
        protected void RaiseFinishedRendering()
        {
            this.RaiseFinishedRendering(new EventArgs());
        }

        /// <summary>Raises the FinishedRendering event</summary>
        /// <param name="args">Parameters to the event being raised</param>
        protected void RaiseFinishedRendering(EventArgs args)
        {
            this.finishedRendering(this, args);
        }
        #endregion


        #region Direct2D Drawing Methods
        /// <summary>Discards the current buffer and replaces it with a blank one.</summary>
        protected void DiscardCurrentBuffer()
        {
            lock (this.controlBufferLock)
            {
                BitmapRenderTarget bmpNew;

                // create a bitmap rendering target
                ResultCode result = this.CtrlRenderTarget.CreateCompatibleRenderTarget(out bmpNew);

                bmpNew.BeginDraw();                     //tell Direct2D to start the draw
                this.FillBufferRenderTarget(bmpNew);    //flood fill
                result = bmpNew.EndDraw();              //tell Direct2D that a paint operation is ending

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
            renderTarget.FillRectangle(new Bardez.Projects.DirectX.Direct2D.RectangleF(new Rectangle(new Point(0, 0), this.Size)), brush);
            brush.Dispose(); //clean up device resource
        }

        /// <summary>Draws a Bitmap to the render buffer</summary>
        /// <param name="bmp">Direct2D bitmap to draw to the buffer.</param>
        protected void DrawBitmapToBuffer(Bardez.Projects.DirectX.Direct2D.Bitmap bmp, Point2dF origin)
        {
            lock (this.controlBufferLock)
            {
                //Determine the copy rectangle
                Bardez.Projects.DirectX.Direct2D.SizeF bmpSize = bmp.GetSize();

                Single width = bmpSize.Width > this.BmpRenderTarget.Size.Width ? this.BmpRenderTarget.Size.Width : bmpSize.Width;
                Single height = bmpSize.Height > this.BmpRenderTarget.Size.Height ? this.BmpRenderTarget.Size.Height : bmpSize.Height;

                Bardez.Projects.DirectX.Direct2D.RectangleF destRect = new Bardez.Projects.DirectX.Direct2D.RectangleF(origin.X, origin.X + width, origin.Y, origin.Y + height);
                Bardez.Projects.DirectX.Direct2D.RectangleF srcRect = new Bardez.Projects.DirectX.Direct2D.RectangleF(0.0F, width, 0.0F, height);

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
        #endregion


        #region Helper Methods
        /// <summary>Builds a Stroke style based off of the input LineStyle info</summary>
        /// <param name="style">Input data from which to build the D2D StrokeStyle</param>
        /// <returns>The constructed StrokeStyle</returns>
        private StrokeStyle BuildStrokeStyle(LineStyle style)
        {
            StrokeStyle d2dStyle = null;

            StrokeStyleProperties properties = null;

            //set up the properties
            switch (style)
            {
                case LineStyle.Solid:
                    properties = new StrokeStyleProperties(CapStyle.Flat, CapStyle.Flat, CapStyle.Flat, LineJoint.Round, 10.0F, DashStyle.Solid, 0.0F);
                    break;
                case LineStyle.Dotted:
                    properties = new StrokeStyleProperties(CapStyle.Flat, CapStyle.Flat, CapStyle.Flat, LineJoint.Round, 10.0F, DashStyle.Dot, 0.0F);
                    break;
                case LineStyle.DashPixel2:
                case LineStyle.DashPixel3:
                case LineStyle.DashPixel4:
                case LineStyle.DashPixel5:
                case LineStyle.DashPixel6:
                case LineStyle.DashPixel7:
                    properties = new StrokeStyleProperties(CapStyle.Flat, CapStyle.Flat, CapStyle.Flat, LineJoint.Round, 10.0F, DashStyle.Custom, 0.0F);
                    break;
            }

            Single[] dashpoints = null;

            //set up the dashes
            switch (style)
            {
                case LineStyle.DashPixel2:
                    dashpoints = new Single[] { 2.0F };
                    break;
                case LineStyle.DashPixel3:
                    dashpoints = new Single[] { 3.0F };
                    break;
                case LineStyle.DashPixel4:
                    dashpoints = new Single[] { 4.0F };
                    break;
                case LineStyle.DashPixel5:
                    dashpoints = new Single[] { 5.0F };
                    break;
                case LineStyle.DashPixel6:
                    dashpoints = new Single[] { 6.0F };
                    break;
                case LineStyle.DashPixel7:
                    dashpoints = new Single[] { 7.0F };
                    break;

                case LineStyle.Solid:
                case LineStyle.Dotted:
                default:
                    dashpoints = null;
                    break;
            }

            ResultCode result = this.factory.CreateStrokeStyle(properties, dashpoints, out d2dStyle);

            if (result != ResultCode.Success_OK)
                throw new ApplicationException(String.Format("Encountered an error when creating a StrokeStyle on the factory: received ResultCode of {0} with a label of \"{1}\".", result, result.ToString()));

            return d2dStyle;
        }
        #endregion
    }
}