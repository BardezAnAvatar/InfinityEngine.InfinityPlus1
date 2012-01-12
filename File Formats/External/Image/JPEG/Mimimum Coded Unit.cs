using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Provides a common base for a mimumum coded unit, does nothing else.</summary>
    public abstract class MimimumCodedUnit
    {
    }

    /// <summary>Represents an MCU of sample blocks</summary>
    public class DctMcu : MimimumCodedUnit
    {
        #region Field(s)
        /// <summary>Represents the data units</summary>
        protected List<Int32[]> dataUnits;
        #endregion


        #region Properties
        /// <summary>Exposes the data units</summary>
        public List<Int32[]> DataUnits
        {
            get { return this.dataUnits; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public DctMcu()
        {
            this.Initialize();
        }

        /// <summary>Range constructor</summary>
        public DctMcu(Int32 startSelector, Int32 endSelector)
        {
            this.Initialize();

            Int32 distance = (endSelector - startSelector) + 1;
            this.dataUnits.Add(new Int32[distance]);    //zero-initialized
        }

        /// <summary>Initializes lists</summary>
        public virtual void Initialize()
        {
            this.dataUnits = new List<Int32[]>();
        }
        #endregion
    }

    /// <summary>Represents an MCU of samples</summary>
    public class SampleMcu : MimimumCodedUnit
    {
        #region Field(s)
        /// <summary>Represents the data units</summary>
        protected List<Byte> dataUnits;
        #endregion


        #region Properties
        /// <summary>Exposes the data units</summary>
        public List<Byte> DataUnits
        {
            get { return this.dataUnits; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public SampleMcu()
        {
            this.Initialize();
        }

        /// <summary>Initializes lists</summary>
        public virtual void Initialize()
        {
            this.dataUnits = new List<Byte>();
        }
        #endregion
    }
}