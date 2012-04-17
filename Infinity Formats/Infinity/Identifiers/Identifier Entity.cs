using System;
using System.Globalization;
using System.Text;

using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Identifiers
{
    /// <summary>Represents a single entry for an IDS file</summary>
    public class IdentifierEntity
    {
        #region Fields
        /// <summary>String representing the string description of the Identifier</summary>
        public String Description { get; set; }

        /// <summary>String representing what the description is stranslated into or from</summary>
        public String Translation { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the Integer value of the translation, unless inappropriate</summary>
        /// <remarks>Should work for most situations, but null for IWD's PREFAB.IDS</remarks>
        public Int32? Numeric
        {
            get
            {
                Int32 value;
                Boolean num = Int32.TryParse(this.Translation, out value);
                if (!num)
                    num = Int32.TryParse(this.Translation.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);

                if (num)
                    return value;
                else
                    return null;
            }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="id">Value translated as the key (non-unique)</param>
        /// <param name="description">Value of the description</param>
        public IdentifierEntity(String id, String description)
        {
            this.Translation = id.Trim();
            this.Description = description.Trim();
        }
        #endregion


        #region ToString methods
        /// <summary>Gets a human-readable String representation of this entity</summary>
        /// <returns>A human-readable String representation of this entity</returns>
        public override String ToString()
        {
            String representation = null;

            if (this.Numeric == null)
                representation = String.Concat(this.Translation, "=", this.Description);
            else
                representation = String.Format("{0,-12} {1}", this.Translation, this.Description);

            return representation;
        }
        #endregion
    }
}