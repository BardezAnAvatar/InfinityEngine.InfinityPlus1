using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize;
using Bardez.Projects.InfinityPlus1.Test.WinForm;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Generic control for Image types that contain multiple animations</summary>
    /// <typeparam name="ImageCollection">Image type that implements IAnimation and IInfinityFormat</typeparam>
    public abstract partial class HarnessAnimationCollectionTestControlBase<ImageCollection> : UserControl where ImageCollection : IAnimation, IInfinityFormat, new()
    {
        #region Constants
        /// <summary>Rendering key used to blank out an image</summary>
        protected const Int32 BlankRenderKey = -1;
        #endregion


        #region Fields
        /// <summary>Object reference to lock on for the User Interface</summary>
        private Object interfaceLock;

        /// <summary>Collection of decoded image paths and frame references</summary>
        protected List<AnimationCollectionReference> animationCollectionList { get; set; }

        /// <summary>Object reference to lock on for multithreaded decoding of images</summary>
        private Object animationCollectionLock;

        /// <summary>Rendering control background color</summary>
        protected Color RenderBackgroundColor
        {
            get { return this.direct2dRenderControl.BackColor; }
            set { this.direct2dRenderControl.BackColor = value; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public HarnessAnimationCollectionTestControlBase()
        {
            this.InitializeComponent();
            this.interfaceLock = new Object();
            this.animationCollectionLock = new Object();
        }
        #endregion


        #region UI event handlers
        /// <summary>Click event handler for the Initialize button. Loads a list of displayable bitmaps from the config file.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnInitialize_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                if (this.lstboxImages.Items.Count < 1)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LaunchImageDecoding));
            }
        }

        /// <summary>Click event hander for the Clear Display button. Clears the rendering control of its contents.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnClearDisplay_Click(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
                this.BlankDisplay();
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Sends a new bitmap index to the render target control.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxImages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                //clear existing output
                this.BlankDisplay();
                this.lstboxAnimationCollection.Items.Clear();   //Animations
                this.lstboxImageCollection.Items.Clear();       //Frames
                
                //populate new output list
                AnimationCollectionReference animColRef = this.lstboxImages.SelectedItem as AnimationCollectionReference;
                if (animColRef != null)
                    for (Int32 image = 0; image < animColRef.Animations.Count; ++image)
                        this.lstboxAnimationCollection.Items.Add(animColRef.Animations[image]);
            }
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Changes the animation collection list in the animation list box.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxAnimationCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                //clear existing output
                this.BlankDisplay();
                this.lstboxImageCollection.Items.Clear();
                
                //populate new output
                ImageCollectionReference imgColRef = this.lstboxAnimationCollection.SelectedItem as ImageCollectionReference;
                if (imgColRef != null)
                    for (Int32 image = 0; image < imgColRef.ImageList.Count; ++image)
                        this.lstboxImageCollection.Items.Add(imgColRef.ImageList[image]);
            }
        }

        /// <summary>Event handler for when the selected index of the listbox changes. Changes the image collection list in the image list box.</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void lstboxImageCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (this.interfaceLock)
            {
                ImageReference imgRef = this.lstboxImageCollection.SelectedItem as ImageReference;

                if (imgRef != null)
                    this.RenderDisplay(imgRef.RenderKey, imgRef.RenderOriginX, imgRef.RenderOriginY);
                else
                    this.BlankDisplay();
            }
        }

        /// <summary>Event handler for when the Choose background color button is clicked</summary>
        /// <param name="sender">Object senting the event</param>
        /// <param name="e">Standard EventArgs parameter e</param>
        protected virtual void btnChooseColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.AnyColor = true;
            cd.ShowDialog();
            this.RenderBackgroundColor = cd.Color;
        }
        #endregion


        #region Image decoding/loading
        /// <summary>Method that launches the decoding of the image from the config file</summary>
        /// <param name="stateInfo">WaitCallback state parameter</param>
        protected virtual void LaunchImageDecoding(Object stateInfo)
        {
            //load paths from the app.config
            IList<String> paths = this.GetPaths();
            this.animationCollectionList = new List<AnimationCollectionReference>();

            //spool up the strings
            for (Int32 pathIndex = 0; pathIndex < paths.Count; ++pathIndex)
            {
                AnimationCollectionReference col = new AnimationCollectionReference(paths[pathIndex], pathIndex);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DecodeSingleImageCollection), col);
            }

            //lock on the threads
            while (true)
            {
                Thread.Sleep(100);  //pause for a fraction of a second

                Boolean finished = false;

                lock (this.animationCollectionLock)
                    finished = (this.animationCollectionList.Count == paths.Count);

                if (finished)
                    break;
            }

            //sort the images
            lock (this.animationCollectionLock)    //lock, just to be safe
                this.animationCollectionList.Sort(AnimationCollectionReference.Compare);    //now sort, then add to the UI list

            //load them into the UI
            this.LoadDecodedImages();
        }

        /// <summary>Method to decode a single image file, in a multi-threaded environment</summary>
        /// <param name="animCollection">ImageCollectionReference containing String of a filepath, casted as an Object for use by ThreadPool</param>
        protected virtual void DecodeSingleImageCollection(Object animCollection)
        {
            //only proceed if there is a valid cast available
            if (animCollection is AnimationCollectionReference)
            {
                AnimationCollectionReference col = animCollection as AnimationCollectionReference;
                String path = col.ImageDescription; //description is the path

                //read the image
                ImageCollection image = new ImageCollection();
                using (FileStream fs = ReusableIO.OpenFile(path))
                    image.Read(fs);

                //loop through the frames in the image set
                for (Int32 frameIndex = 0; frameIndex < image.FrameCount; ++frameIndex)
                {
                    //et its frame data
                    Frame frame = image.GetFrame(frameIndex);

                    //submit a bitap
                    Int32 key;
                    lock (this.interfaceLock)
                        key = this.direct2dRenderControl.AddFrameResource(frame);

                    String imageName = String.Format("Frame # {0, 5}", frameIndex);

                    //create the ImageReference and add
                    ImageReference imgRef = new ImageReference(imageName, key, frameIndex);
                    imgRef.RenderOriginX = frame.OriginX;
                    imgRef.RenderOriginY = frame.OriginY;
                    this.SetFrameDetails(imgRef, image, frameIndex);    //allow for easy override with less intervention
                    col.FrameList.Add(imgRef);
                }

                //get the list of animations
                IList<IList<Int32>> animationFrames = image.GetFrameAnimations();

                //with the collection of image references, add the animations
                for (Int32 animIndex = 0; animIndex < animationFrames.Count; ++animIndex)
                {
                    ImageCollectionReference animation = new ImageCollectionReference();

                    for (Int32 imgIndex = 0; imgIndex < animationFrames[animIndex].Count; ++imgIndex)
                    {
                        Int32 frameIndex = animationFrames[animIndex][imgIndex];
                        ImageReference imgRef = null;
                        if (frameIndex > -1)
                            imgRef = col.FrameList[frameIndex];
                        else
                            imgRef = new ImageReference("Null index", HarnessAnimationCollectionTestControlBase<ImageCollection>.BlankRenderKey, imgIndex);
                        
                        animation.ImageList.Add(imgRef);
                    }

                    animation.ImageDescription = String.Format("Animation # {0, 4}", animIndex);
                    col.Animations.Add(animation);
                }

                //add the ImageCollectionReference to the main listbox collection
                lock (this.animationCollectionLock)
                    this.animationCollectionList.Add(col);
            }
        }

        /// <summary>Abstract method to load harness items</summary>
        protected virtual void LoadDecodedImages()
        {
            if (this.lstboxImages.InvokeRequired) //check if an invoke is required, call on UI thead
                this.lstboxImages.Invoke(new VoidInvoke(this.LoadDecodedImages));
            else    //good on existing thread
            {
                lock (this.animationCollectionLock)
                    foreach (AnimationCollectionReference reference in this.animationCollectionList)
                        this.lstboxImages.Items.Add(reference);
            }
        }
        #endregion


        #region Helper methods
        /// <summary>Sets the render Frame key to -1 and calls the render methood to the UI</summary>
        /// <param name="renderKey">Frame key used to set the rendering image</param>
        /// <remarks>Any locking must be done outside this method</remarks>
        protected virtual void RenderDisplay(Int32 renderKey)
        {
            this.RenderDisplay(renderKey, 0L, 0L);
        }
        /// <summary>Sets the render Frame key to -1 and calls the render methood to the UI</summary>
        /// <param name="renderKey">Frame key used to set the rendering image</param>
        /// <remarks>Any locking must be done outside this method</remarks>
        protected virtual void RenderDisplay(Int32 renderKey, Int64 renderX, Int64 renderY)
        {
            this.direct2dRenderControl.SetRenderFrame(renderKey, renderX, renderY);
            this.direct2dRenderControl.Render();
        }

        /// <summary>Blanks out the rendering key to the control</summary>
        /// <remarks>Any locking must be done outside this method</remarks>
        protected virtual void BlankDisplay()
        {
            this.RenderDisplay(HarnessAnimationCollectionTestControlBase<ImageCollection>.BlankRenderKey);
        }

        /// <summary>Gets the paths to test from the config file</summary>
        /// <returns>An IList of Strings for file paths</returns>
        protected abstract IList<String> GetPaths();

        /// <summary>Overridable method to set detail of a frame from within DecodeSingleImageCollection</summary>
        /// <param name="frame">ImageReference to set details for</param>
        /// <param name="container">ImageCollection format that contains animations and frames</param>
        /// <param name="frameIndex">Frame number setting the details for</param>
        protected abstract void SetFrameDetails(ImageReference frame, ImageCollection container, Int32 frameIndex);
        #endregion
    }
}