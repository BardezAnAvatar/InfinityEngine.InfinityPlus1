using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Dialog.Enums
{
    /// <summary>This enum represents the flags available to transition blocks</summary>
    [Flags]
    public enum TransitionFlags : uint /* UInt32 */
    {
        [Description("Transition is a text response")]
        HasText                     = 1U,

        [Description("Transition has a Trigger condition")]
        HasTrigger                  = 2U,

        [Description("Transition has an associated script action")]
        HasAction                   = 4U,

        [Description("Transition ends dialog")]
        TerminatesDialog            = 8U,

        [Description("Transition adds a Journal Entry")]
        AddsJournalEntry            = 16U,

        //Bit 5 unknown/unused

        [Description("Transition adds a Journal Quest Entry")]
        AddsJournalEntryQuest       = 64U,

        [Description("Transition removes a Journal Entry")]
        RemovesJournalEntry         = 128U,

        [Description("Transition removes a Journal Quest Entry")]
        RemovesJournalEntryQuest    = 256U
    }
}
