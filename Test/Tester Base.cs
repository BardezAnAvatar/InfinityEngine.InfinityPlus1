using System;

namespace Bardez.Projects.InfinityPlus1.Test
{
    #region Delegate declarations
    /// <summary>Delegate for posting an output message to the UI</summary>
    /// <param name="sender">Object sending the request</param>
    /// <param name="message">Message to send</param>
    public delegate void PostOutputMessage(Object sender, MessageEventArgs message);

    /// <summary>Event to raise for initializng the test class</summary>
    /// <param name="sender">Object sending/raising the request</param>
    /// <param name="e">Specific initialization event parameters</param>
    public delegate void InitializeTestClass(Object sender, EventArgs e);

    /// <summary>Event to raise for testing a specific value</summary>
    /// <param name="sender">Object sending/raising the request</param>
    /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
    public delegate void TestItem(Object sender, TestEventArgs testArgs);
    #endregion

    /// <summary>Base testing class, including events</summary>
    public abstract class TesterBase : ITester
    {
        #region Events
        /// <summary>Publically exposed event for attaching to, that the test class can communicate from</summary>
        private event PostOutputMessage postMessage;

        /// <summary>Protected event for raising an initialize method</summary>
        protected event InitializeTestClass Initialize;

        /// <summary>Protected event for raising a test case</summary>
        protected event TestItem TestInstance;

        /// <summary>Externally exposed event for handling an output event</summary>
        public event PostOutputMessage PostMessage
        {
            add { this.postMessage += value; }
            remove { this.postMessage -= value; }
        }
        #endregion

        #region Event Raising
        /// <summary>Raises an internal Initialize event</summary>
        /// <param name="caller">Object reference calling this method</param>
        public virtual void DoInitialize(Object caller)
        {
            this.Initialize(caller, null);
        }

        /// <summary>Raises an internal Initialize event</summary>
        /// <param name="caller">Object reference calling this method</param>
        /// <param name="testArgs">Message containing the item to test (usually a file path)</param>
        public virtual void DoTest(Object caller, TestEventArgs testArgs)
        {
            this.TestInstance(caller, testArgs);
        }

        /// <summary>Raises an internal Initialize event</summary>
        /// <param name="testArgs">Message containing the item to test (usually a file path)</param>
        protected virtual void DoTest(TestEventArgs testArgs)
        {
            this.TestInstance(this, testArgs);
        }

        /// <summary>Raises an internally controlled PostMessage event</summary>
        /// <param name="message">Message to send</param>
        protected virtual void DoPostMessage(MessageEventArgs message)
        {
            this.postMessage(this, message);
        }
        #endregion

        #region Event Handlers
        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected abstract void InitializeTestData(Object sender, EventArgs e);

        /// <summary>Event to raise for testing a specific value</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected abstract void TestCase(Object sender, TestEventArgs testArgs);
        #endregion

        #region Construction
        /// <summary>Initializes the test class, to be called in the constructor</summary>
        protected virtual void InitializeInstance()
        {
            this.Initialize += new InitializeTestClass(this.InitializeTestData);
            this.TestInstance += new TestItem(this.TestCase);
        }
        #endregion

        #region ITester Interface
        /// <summary>ITester interface, programmatically raises all tests</summary>
        public virtual void Test()
        {
            this.Initialize(this, null);  //raise initialization
            this.TestMulti();
        }

        /// <summary>Tests multiple instances of test cases</summary>
        protected virtual void TestMulti() { }
        #endregion
    }
}