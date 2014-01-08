using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represents the collection of flags for the user interface</summary>
    /// <remarks>Text Window size has three states, 2 bits</remarks>
    [Flags]
    public enum GuiFlags : uint /* UInt32 */
    {
        [Description("Party AI scripts are enables")]
        PartyScriptsEnabled     = 0x01,
        [Description("Text window size medium")]
        TextWindowMedium         = 0x02,
        [Description("text window size large")]
        TextWindowLarge         = 0x04,
        [Description("A dialog is running")]
        DialogRunning           = 0x08,
        [Description("GUI is hidden")]
        HideGui                 = 0x10,
        [Description("(left) GUI panel is hidden")]
        HideUserActionsPanel    = 0x20,
        [Description("(right) GUI panel is hidden")]
        HidePortraitsPanel      = 0x40,
        [Description("Map notes are hidden")]
        HideMapNotes            = 0x80,
    }
}
