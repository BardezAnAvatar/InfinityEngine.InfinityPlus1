using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DCT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.ExternalFormats.Image.Mathematics.DCT
{
    /// <summary>
    ///     This is a test class for JpegJfifParserTest and is intended
    ///     to contain all JpegJfifParserTest Unit Tests
    /// </summary>
    [TestClass()]
    public class JpegJfifParserTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
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


        /// <summary>A test for InverseDiscreteCosineTransformSlow</summary>
        [TestMethod]
        [DeploymentItem("InfinityPlus1.Files.dll")]
        public void InverseDiscreteCosineTransformSlowTest()
        {
            List<Int32> x = new List<Int32>();
            
            x.Add(-416);    x.Add(-33); x.Add(-60); x.Add(32);  x.Add(48);  x.Add(-40); x.Add(0);  x.Add(0);
            x.Add(0);       x.Add(-24); x.Add(-56); x.Add(19);  x.Add(26);  x.Add(0);   x.Add(0);  x.Add(0);
            x.Add(-42);     x.Add(13);  x.Add(80);  x.Add(-24); x.Add(-40); x.Add(0);   x.Add(0);  x.Add(0);
            x.Add(-42);     x.Add(17);  x.Add(44);  x.Add(-29); x.Add(0);   x.Add(0);   x.Add(0);  x.Add(0);
            x.Add(18);      x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);  x.Add(0);
            x.Add(0);       x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);  x.Add(0);
            x.Add(0);       x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);  x.Add(0);
            x.Add(0);       x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);   x.Add(0);  x.Add(0);

            SummationDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFloat(x);

            Boolean matchesExpected =
                (x[0] > -67 && x[0] < -65) &&
                (x[1] > -64 && x[1] < -62) &&
                (x[2] > -72 && x[2] < -70) &&
                (x[3] > -69 && x[3] < -67) &&
                (x[4] > -57 && x[4] < -55) &&
                (x[5] > -66 && x[5] < -64) &&
                (x[6] > -69 && x[6] < -67) &&
                (x[7] > -47 && x[7] < -45) &&
                (x[8] > -71 && x[8] < -69) &&
                (x[9] > -74 && x[9] < -72) &&
                (x[10] > -73 && x[10] < -71) &&
                (x[11] > -47 && x[11] < -45);

            Assert.AreEqual(matchesExpected, true);
        }


        /// <summary>A test for InverseDiscreteCosineTransformSlow</summary>
        [TestMethod]
        [DeploymentItem("InfinityPlus1.Files.dll")]
        public void ForwardDiscreteCosineTransformSlowTest()
        {
            List<Double> z, a;
            Int32[] x, y;
            x = new Int32[] 
            {
                //-76,     -73, -67, -62, -58, -67, -64, -55,   //from WIKI
                //-65,     -69, -73, -38, -19, -43, 59,  -56,
                //-66,     -69, -60, -15, 16,  -24, -62, -55,
                //-65,     -70, -57, -6,  26,  -22, -58, -59,
                //-61,     -67, -60, -24, -2,  -40, -60, -58,
                //-49,     -63, -68, -58, -51, -60, -70, -53,
                //-43,     -57, -64, -69, -73, -67, -63, -45,
                //-41,     -49, -59, -60, -63, -52, -50, -34,
                //10, 9, 	8, 	8, 	8, 	8, 	7, 	6,      //from first block, integer clamped
                //10, 9, 	8, 	8, 	8, 	8, 	7, 	6, 
                //10, 9, 	8, 	8, 	8, 	8, 	7, 	6, 
                //9, 	8, 	8, 	7, 	8, 	8, 	7, 	6, 
                //8, 	7, 	7, 	7, 	8, 	8, 	7, 	6, 
                //8, 	7, 	6, 	7, 	8, 	8, 	7, 	6, 
                //7, 	6, 	6, 	6, 	7, 	8, 	7, 	6, 
                //7, 	6, 	6, 	6, 	7, 	8, 	7, 	7, 
                6, 	5, 	5, 	5, 	5, 	5, 	4, 	3, 
                6, 	5, 	5, 	5, 	5, 	5, 	4, 	3, 
                6, 	5, 	5, 	5, 	5, 	5, 	4, 	3, 
                5, 	5, 	5, 	4, 	5, 	5, 	4, 	3, 
                5, 	4, 	4, 	4, 	5, 	5, 	4, 	3, 
                5, 	4, 	3, 	4, 	5, 	5, 	4, 	3, 
                4, 	3, 	3, 	3, 	4, 	5, 	4, 	3, 
                4, 	3, 	3, 	3, 	4, 	5, 	4, 	4, 

            };
            y = new Int32[64];
            Array.Copy(x, 0, y, 0, 64);

            //Double[] x, y;
            //x = new Double[] 
            //{
            //    //9.62899999999999, 	8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999,   //normal YPbPr
            //    //9.62899999999999, 	8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //9.62899999999999, 	8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //7.62899999999999, 	6.62900000000002, 	6.62900000000002, 	6.62900000000002, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //7.62899999999999, 	6.62900000000002, 	5.62899999999999, 	6.62900000000002, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //6.62900000000002, 	5.62899999999999, 	5.62899999999999, 	5.62899999999999, 	6.62900000000002, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //6.62900000000002, 	5.62899999999999, 	5.62899999999999, 	5.62899999999999, 	6.62900000000002, 	7.62899999999999, 	6.62900000000002, 	6.62900000000002, 

            //    6.26176862745098, 	5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    6.26176862745098, 	5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    6.26176862745098, 	5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    4.54412156862745, 	3.68529803921567, 	3.68529803921567, 	3.68529803921567, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    4.54412156862745, 	3.68529803921567, 	2.82647450980392, 	3.68529803921567, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    3.68529803921567, 	2.82647450980392, 	2.82647450980392, 	2.82647450980392, 	3.68529803921567, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    3.68529803921567, 	2.82647450980392, 	2.82647450980392, 	2.82647450980392, 	3.68529803921567, 	4.54412156862745, 	3.68529803921567, 	3.68529803921567, 
            //};
            //y = new Double[64];
            //Array.Copy(x, 0, y, 0, 64);

            z = SummationDiscreteCosineTransformationFloat.ForwardDiscreteCosineTransformFloat(x);
            a = SummationDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFloat(z);

            Boolean matchesExpected =
                (x[0]  > -416 && x[0]  < -416) &&
                (x[1]  >  -33 && x[1]  < -33) &&
                (x[2]  >  -60 && x[2]  < -60) &&
                (x[3]  >  -32 && x[3]  < -32) &&
                (x[4]  >  -48 && x[4]  < -48) &&
                (x[5]  >  -40 && x[5]  < -40) &&
                (x[6]  >    0 && x[6]  <  0) &&
                (x[7]  >    0 && x[7]  <  0) &&
                (x[8]  >    0 && x[8]  <  0) &&
                (x[9]  >  -24 && x[9]  < -24) &&
                (x[10] >  -40 && x[10] < -40) &&
                (x[11] >    0 && x[11] < -0);

            Assert.AreEqual(matchesExpected, true);
        }



        /// <summary>A test for InverseDiscreteCosineTransformSlow</summary>
        [TestMethod]
        [DeploymentItem("InfinityPlus1.Files.dll")]
        public void IDCTFastTest()
        {
            List<Double> z, a;
            Int32[] x, y;
            x = new Int32[] 
            {
                -76, -73, -67, -62, -58, -67, -64, -55,   //from WIKI
                -65, -69, -73, -38, -19, -43,  59, -56,
                -66, -69, -60, -15,  16, -24, -62, -55,
                -65, -70, -57,  -6,  26, -22, -58, -59,
                -61, -67, -60, -24,  -2, -40, -60, -58,
                -49, -63, -68, -58, -51, -60, -70, -53,
                -43, -57, -64, -69, -73, -67, -63, -45,
                -41, -49, -59, -60, -63, -52, -50, -34,
            };
            y = new Int32[64];
            Array.Copy(x, 0, y, 0, 64);

            //Double[] x, y;
            //x = new Double[] 
            //{
            //    //9.62899999999999, 	8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999,   //normal YPbPr
            //    //9.62899999999999, 	8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //9.62899999999999, 	8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //8.62899999999999, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //7.62899999999999, 	6.62900000000002, 	6.62900000000002, 	6.62900000000002, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //7.62899999999999, 	6.62900000000002, 	5.62899999999999, 	6.62900000000002, 	7.62899999999999, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //6.62900000000002, 	5.62899999999999, 	5.62899999999999, 	5.62899999999999, 	6.62900000000002, 	7.62899999999999, 	6.62900000000002, 	5.62899999999999, 
            //    //6.62900000000002, 	5.62899999999999, 	5.62899999999999, 	5.62899999999999, 	6.62900000000002, 	7.62899999999999, 	6.62900000000002, 	6.62900000000002, 

            //    6.26176862745098, 	5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    6.26176862745098, 	5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    6.26176862745098, 	5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    5.4029450980392, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    4.54412156862745, 	3.68529803921567, 	3.68529803921567, 	3.68529803921567, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    4.54412156862745, 	3.68529803921567, 	2.82647450980392, 	3.68529803921567, 	4.54412156862745, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    3.68529803921567, 	2.82647450980392, 	2.82647450980392, 	2.82647450980392, 	3.68529803921567, 	4.54412156862745, 	3.68529803921567, 	2.82647450980392, 
            //    3.68529803921567, 	2.82647450980392, 	2.82647450980392, 	2.82647450980392, 	3.68529803921567, 	4.54412156862745, 	3.68529803921567, 	3.68529803921567, 
            //};
            //y = new Double[64];
            //Array.Copy(x, 0, y, 0, 64);

            z = SummationDiscreteCosineTransformationFloat.ForwardDiscreteCosineTransformFloat(x);
            a = SummationDiscreteCosineTransformationFloat.InverseDiscreteCosineTransformFloat(z);

            List<Int32> fdct = new List<Int32>(), idctInt;
            for (Int32 i = 0; i < z.Count; ++i)
                fdct.Add(Convert.ToInt32(z[i]));

            idctInt = LoefflerDiscreteCosineTransformationInteger.InverseDiscreteCosineTransformInteger(fdct);


            Boolean matchesExpected = false;
            for (Int32 i = 0; i < 64; ++i)
            {
                matchesExpected = idctInt[i] > x[i] - 2 && idctInt[i] < x[i] + 2;   //within an error of 1

                if (!matchesExpected)
                    break;
            }

            Assert.AreEqual(matchesExpected, true);
        }
    }
}