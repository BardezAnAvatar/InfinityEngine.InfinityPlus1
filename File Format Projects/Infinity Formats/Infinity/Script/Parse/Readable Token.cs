using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse
{
    /// <summary>Contains a two-character token and a flag indicating whether it can be lower case</summary>
    public class ReadableToken
    {
        #region Fields
        /// <summary>The token being targeted</summary>
        public String Token { get; set; }

        /// <summary>Flag inticating whether the lower case is allowable</summary>
        public Boolean AllowLowerCase { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="token">The token being targeted</param>
        /// <param name="lower">Flag inticating whether the lower case is allowable</param>
        public ReadableToken(String token, Boolean lower)
        {
            this.Token = token;
            this.AllowLowerCase = lower;
        }
        #endregion
    }
}