using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals
{
    /// <summary>
    ///     Overhaul, in their infinite wisdom (excellent pun) decided to drop the language code
    ///     from dialog.tlk files and just put everything in as english
    /// </summary>
    public enum LanguageCode
    {
        /// <summary>No value defined</summary>
        Undefined = -1,

        /// <summary>English</summary>
        [Description("English")]
        en_US = 0,

        /// <summary>French</summary>
        [Description("French")]
        fr_FR,

        /// <summary>Spanish</summary>
        [Description("Spanish")]
        es_ES,
        
        /// <summary>German</summary>
        [Description("German")]
        de_DE,

        /// <summary>Czech</summary>
        [Description("Czech")]
        cs_CZ,

        /// <summary>Italian</summary>
        [Description("Italian")]
        it_IT,

        /// <summary>Japanese</summary>
        [Description("Japanese")]
        ja_JP,

        /// <summary>Korean</summary>
        [Description("Korean")]
        ko_KR,

        /// <summary>Polish</summary>
        [Description("Polish")]
        pl_PL,

        /// <summary>Portuguese</summary>
        [Description("Portuguese")]
        pt_BR,

        /// <summary>Turkish</summary>
        [Description("Turkish")]
        tr_TR,

        /// <summary>Simplified Chinese</summary>
        [Description("Simplified Chinese")]
        zh_CH,
    }
}