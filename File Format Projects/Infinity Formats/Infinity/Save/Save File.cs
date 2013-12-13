using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Save
{
    /// <summary>Represents a single Infinity Engine save file, containing living/changing resources</summary>
    public class SaveFile : InfinityFormat
    {
        #region Fields
        /// <summary>Collection of saved resources</summary>
        public List<SavedResource> Resources { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            this.Resources = new List<SavedResource>();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            while (input.Position != input.Length)
            {
                SavedResource resource = new SavedResource();
                resource.Read(input);
                this.Resources.Add(resource);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            base.Write(output);

            foreach (SavedResource resource in this.Resources)
                resource.Write(output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Save game:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(this.Signature);
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(this.Version);

            for (Int32 index = 0; index < this.Resources.Count; ++index)
                builder.Append(this.Resources[index].ToString());

            return builder.ToString();
        }
        #endregion
    }
}