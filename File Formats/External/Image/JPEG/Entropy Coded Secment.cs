using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents a list of entropy-coded MCUs</summary>
    public class EntropyCodedSegment
    {
        //contains multiple minimum coded units
        #region Field(s)
        /// <summary>Represents the list of MCUs</summary>
        protected List<MimimumCodedUnit> mimimumCodedUnits;
        #endregion


        #region Properties
        /// <summary>Exposes the list of MCUs</summary>
        public List<MimimumCodedUnit> MimimumCodedUnits
        {
            get { return this.mimimumCodedUnits; }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public EntropyCodedSegment()
        {
            this.Initialize();
        }

        /// <summary>Initializes Lists</summary>
        protected virtual void Initialize()
        {
            this.mimimumCodedUnits = new List<MimimumCodedUnit>();
        }
        #endregion
    }
}