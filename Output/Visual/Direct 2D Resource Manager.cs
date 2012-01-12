using System;
using System.Collections.Generic;

using Bardez.Projects.DirectX.Direct2D;
using Bardez.Projects.InfinityPlus1.Files.External.Image;

namespace Bardez.Projects.InfinityPlus1.Output.Visual
{
    //TODO: Add in a collection of Bitmaps (List) that provides a unique index back out.
    //  This will allow the application to submit a Frame to the resource Manager

    /// <summary>Represents a resource gateway to Direct2D data</summary>
    public class Direct2dResourceManager : IDisposable
    {
        #region Fields
        /// <summary>Private lock not accessable by other memory</summary>
        /// <remarks>Static instantiation</remarks>
        private static Object singletonLock = new Object();

        /// <summary>Singleton instance of Direct2dResourceManager</summary>
        private static Direct2dResourceManager singleton = null;

        /// <summary>Represents the factory to create Direct2D objects</summary>
        protected Factory factory;

        /// <summary>Represents the resource collection of Direct2D bitmaps renderable</summary>
        protected List<Bitmap> renderResourceCollection;

        /// <summary>Private lock not accessable by other memory</summary>
        private Object resourceLock;
        #endregion

        #region Properties
        /// <summary>Singleton instance accessor</summary>
        public static Direct2dResourceManager Instance
        {
            get
            {
                lock (Direct2dResourceManager.singletonLock)
                {
                    Direct2dResourceManager.Instantiate();
                    return Direct2dResourceManager.singleton;
                }
            }
        }

        /// <summary>Exposes the Direct2D factory object</summary>
        public Factory Factory
        {
            get { return this.factory; }
        }
        #endregion

        #region Construction
        /// <summary>Sets the singleton instance of the XAudio2 playback engine</summary>
        private static void Instantiate()
        {
            if (Direct2dResourceManager.singleton == null)
                Direct2dResourceManager.singleton = new Direct2dResourceManager();
        }

        /// <summary>Singleton constructor</summary>
        private Direct2dResourceManager()
        {
            // Build a factory
            this.factory = new Factory();
            this.renderResourceCollection = new List<Bitmap>();
            this.resourceLock = new Object();
        }
        #endregion

        #region Destruction
        /// <summary>Disposal code</summary>
        /// <remarks>Dispose()</remarks>
        public void Dispose()
        {
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

                //dispose the facotry
                if (this.factory != null)
                {
                    this.factory.Dispose();
                    this.factory = null;
                }
            }
        }

        /// <summary>Disposal</summary>
        /// <remarks>Finalize()</remarks>
        ~Direct2dResourceManager()
        {
            this.Dispose();
        }
        #endregion

        /// <summary>Posts a Bitmap resource to the resource manager and returns a unique key to access it.</summary>
        /// <param name="resource">Direct2D Bitmap to be posted.</param>
        /// <returns>A unique Int32 key</returns>
        public virtual Int32 AddFrameResource(Bitmap resource)
        {
            lock (this.resourceLock)
            {
                this.renderResourceCollection.Add(resource);
                return this.renderResourceCollection.Count - 1;
            }
        }

        //TODO: rethink the base resource manager class...
        /// <summary>Posts a Frame resource to the resource manager and returns a unique key to access it.</summary>
        /// <param name="resource">Frame to be posted.</param>
        /// <returns>A unique Int32 key</returns>
        public Int32 AddFrameResource(Frame resource)
        {
            throw new NotImplementedException("This is not implemented.");
        }

        /// <summary>Gets the bitmap resource from the specified key</summary>
        /// <param name="key">Key to look up the bitmap with</param>
        /// <returns>A Bitmap to reference</returns>
        public Bitmap GetBitmapResource(Int32 key)
        {
            lock (this.resourceLock)
            {
                return this.renderResourceCollection[key];
            }
        }
    }
}
