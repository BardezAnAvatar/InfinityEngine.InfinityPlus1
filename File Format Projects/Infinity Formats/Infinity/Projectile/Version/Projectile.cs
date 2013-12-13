using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Components;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Version
{
    /// <summary>Representation of a *.PRO file</summary>
    public class Projectile_v1 : IInfinityFormat
    {
        #region Fields
        /// <summary>Projectile header</summary>
        public ProjectileHeader Header { get; set; }

        /// <summary>Animation of the projectile</summary>
        public ProjectileAnimation Animation { get; set; }

        /// <summary>Area effect of the projectile</summary>
        /// <remarks>May not be instantiated</remarks>
        public AreaEffect AreaEffect { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new ProjectileHeader();
            this.Animation = new ProjectileAnimation();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new ProjectileHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);

            if (this.Header.Type == ProjectileType.SingleTarget || this.Header.Type == ProjectileType.AreaOfEffect)
            {
                this.Animation.Read(input);

                if (this.Header.Type == ProjectileType.AreaOfEffect)
                {
                    this.AreaEffect = new AreaEffect();
                    this.AreaEffect.Read(input);
                }
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.Header.Write(output);

            if (this.Header.Type == ProjectileType.SingleTarget || this.Header.Type == ProjectileType.AreaOfEffect)
            {
                this.Animation.Write(output);

                if (this.Header.Type == ProjectileType.AreaOfEffect)
                    this.AreaEffect.Write(output);
            }
        }
        #endregion
        

        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(this.GenerateTypeDescriptionString());
            builder.AppendLine(this.Header.ToString());
            builder.AppendLine(this.Animation.ToString());

            if (this.AreaEffect != null)
                builder.AppendLine(this.AreaEffect.ToString());

            return builder.ToString();
        }

        /// <summary>Generates a description String of this Type</summary>
        /// <returns>A description String of this Type</returns>
        protected virtual String GenerateTypeDescriptionString()
        {
            return "Projectile:";
        }
        #endregion
    }
}