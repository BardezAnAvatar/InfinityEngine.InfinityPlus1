using System;
namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Globals
{
    /// <summary>
    ///     This public class represents a resource reference within the Infinity Engine Game files.
    ///     It is typically an 8-byte string in official Infinity Engine games.
    /// </summary>
    public class ResourceReference
    {
        /// <summary>This member represents the resource reference string</summary>
        private String resRef;

        /// <summary>This member represents the type of resource referenced</summary>
        private ResourceType type;

        /// <summary>This public property gets or sets the resource reference string</summary>
        public String ResRef
        {
            get { return this.resRef; }
            set { this.resRef = value; }
        }

        /// <summary>This public property gets or sets the NULL-terminated representation of the resource reference string</summary>
        public String ZResRef
        {
            get { return ZString.GetZString(this.resRef); }
            set { this.resRef = ZString.GetZString(value); }
        }

        /// <summary>This member represents the type of resource1 referenced</summary>
        public ResourceType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
    }
}