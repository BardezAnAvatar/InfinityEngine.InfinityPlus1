using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityPlus1.Files
{
    /// <summary>This public class represents a resource reference within the Infinity Engine Game giles. It is typically an 8-byte string in official Infinity Engine games</summary>
    public class ResourceReference
    {
        /// <summary>This member represents the resource reference string</summary>
        private String resRef;

        /// <summary>This public property gets or sets the resource reference string</summary>
        public String ResRef
        {
            get { return resRef; }
            set { resRef = value; }
        }
    }
}