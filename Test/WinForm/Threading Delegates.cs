using System;

using Bardez.Projects.InfinityPlus1.Utility;

namespace Bardez.Projects.InfinityPlus1.Test.WinForm
{
    /// <summary>Delegate for threaded application, where Invoke is required</summary>
    public delegate void VoidInvoke();

    /// <summary>Delegate for threaded application, where Invoke is required</summary>
    public delegate void VoidInvokeParameterBoolean(Boolean parameter);

    /// <summary>Delegate for threaded application, where Invoke is required</summary>
    public delegate void VoidStringParameterInvoke(String param);

    /// <summary>Delegate for threaded application, where Invoke is required</summary>
    public delegate void VoidLogItemParameterInvoke(LogItem param);
}