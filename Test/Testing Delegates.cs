using System;

namespace Bardez.Projects.InfinityPlus1.Test
{
    /// <summary>Delegate for posting an output message to the UI</summary>
    /// <param name="sender">Object sending the request</param>
    /// <param name="message">Message to send</param>
    public delegate void PostOutputMessage(Object sender, MessageEventArgs message);

    /// <summary>Event to raise for initializng the test class</summary>
    /// <param name="sender">Object sending/raising the request</param>
    /// <param name="e">Specific initialization event parameters</param>
    public delegate void InitializeTestClass(Object sender, EventArgs e);

    /// <summary>Event to raise for initializng the test class</summary>
    /// <param name="sender">Object sending/raising the request</param>
    /// <param name="e">Specific initialization event parameters</param>
    public delegate void EndInitializeTestClass(Object sender, EventArgs e);

    /// <summary>Event to raise for testing a specific value</summary>
    /// <param name="sender">Object sending/raising the request</param>
    /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
    public delegate void TestItem(Object sender, TestEventArgs testArgs);
}