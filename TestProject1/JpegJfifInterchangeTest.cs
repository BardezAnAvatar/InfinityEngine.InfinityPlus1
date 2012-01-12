using Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for JpegJfifInterchangeTest and is intended
    ///to contain all JpegJfifInterchangeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JpegJfifInterchangeTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        
        /// <summary>
        ///A test for UnZigZag
        ///</summary>
        [TestMethod()]
        [DeploymentItem("InfinityPlus1.Files.dll")]
        public void UnZigZagTest()
        {
            JpegJfifInterchange_Accessor target = new JpegJfifInterchange_Accessor();
            List<Int32> zigZagList = new List<Int32>();

            zigZagList.Add(1); zigZagList.Add(2); zigZagList.Add(9); zigZagList.Add(17); zigZagList.Add(10); zigZagList.Add(3); zigZagList.Add(4); zigZagList.Add(11);
            zigZagList.Add(18); zigZagList.Add(25); zigZagList.Add(33); zigZagList.Add(26); zigZagList.Add(19); zigZagList.Add(12); zigZagList.Add(5); zigZagList.Add(6);
            zigZagList.Add(13); zigZagList.Add(20); zigZagList.Add(27); zigZagList.Add(34); zigZagList.Add(41); zigZagList.Add(49); zigZagList.Add(42); zigZagList.Add(35);
            zigZagList.Add(28); zigZagList.Add(21); zigZagList.Add(14); zigZagList.Add(7); zigZagList.Add(8); zigZagList.Add(15); zigZagList.Add(22); zigZagList.Add(29);
            zigZagList.Add(36); zigZagList.Add(43); zigZagList.Add(50); zigZagList.Add(57); zigZagList.Add(58); zigZagList.Add(51); zigZagList.Add(44); zigZagList.Add(37);
            zigZagList.Add(30); zigZagList.Add(23); zigZagList.Add(16); zigZagList.Add(24); zigZagList.Add(31); zigZagList.Add(38); zigZagList.Add(45); zigZagList.Add(52);
            zigZagList.Add(59); zigZagList.Add(60); zigZagList.Add(53); zigZagList.Add(46); zigZagList.Add(39); zigZagList.Add(32); zigZagList.Add(40); zigZagList.Add(47);
            zigZagList.Add(54); zigZagList.Add(61); zigZagList.Add(62); zigZagList.Add(55); zigZagList.Add(48); zigZagList.Add(56); zigZagList.Add(63); zigZagList.Add(64); 
            
            target.UnZigZag(zigZagList);

            Boolean worked = false;
            for (Int32 i = 1; i < 64; ++i)
            {
                worked = (zigZagList[i] == zigZagList[i - 1] + 1);
                if (!worked)
                    break;
            }

            Assert.AreEqual(true, worked);
        }
    }
}
