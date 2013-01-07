using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Coding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Bardez.Projects.InfinityPlus1.UnitTesting.Control_Objects;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Interpretation;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.InfinityFormats.External.Interplay.MVE.Component.Coding
{
    /// <summary>
    ///     This is a test class for VideoCoding8BitTest and is intended
    ///     to contain all VideoCoding8BitTest Unit Tests
    /// </summary>
    [TestClass]
    public class VideoCoding8BitTest
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


        /// <summary>A test for DecodeCopyBlock</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeCopyBlockTest()
        {
            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test = new Byte[64];
            Byte[] control = new Byte[64];
            Byte[] data = new Byte[0];
            Int32 dataRemain = 0;
            Int32 x = 0, y = 0;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic.DecodeBlock(control, 0, data, ref dataRemain, ref x, ref y);

            //create test object
            SetDecodingMap sdm = new SetDecodingMap(1);
            sdm.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.Copy };
            VideoData vd = new VideoData(0);
            vd.Data = new Byte[0];
            VideoOpcodeStream stream = new VideoOpcodeStream();
            MveVideoFrame frame = new MveVideoFrame(sdm, vd);

            VideoCoding8Bit_Accessor target = new VideoCoding8Bit_Accessor(8, 8, stream);

            //test the test logic
            target.DecodeCopyBlock(0, 0, previousData, test);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test, control));
        }

        /// <summary>A test for DecodeCopyNewFrameBottom</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeCopyNewFrameBottomTest()
        {
            Int32 dataSize = 40 * 40;

            Byte[] previousData = this.CreateRandomSource(40, 40);
            Byte[] test = new Byte[dataSize];
            Byte[] control = new Byte[dataSize];
            //ensure all data are the same
            Array.Copy(previousData, test, dataSize);
            Array.Copy(previousData, control, dataSize);
            Byte[] data = new Byte[] { 230 };
            Int32 x = 16, y = 0;
            Int64 position = 0L;
            Int32 dataRemain = data.Length - (Int32)position;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic = new MveVideoCoder8SuppliedLogic(40, 40, previousData);
            controlLogic.DecodeBlock(control, 2, data, ref dataRemain, ref x, ref y);

            //create test object
            SetDecodingMap sdm = new SetDecodingMap(1);
            sdm.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.CopyBottom };
            VideoData vd = new VideoData(0);
            vd.Data = new Byte[0];
            VideoOpcodeStream stream = new VideoOpcodeStream();
            MveVideoFrame frame = new MveVideoFrame(sdm, vd);

            VideoCoding8Bit_Accessor target = new VideoCoding8Bit_Accessor(40, 40, stream);

            //test the test logic
            target.DecodeCopyNewFrameBottom(2, 0, data, ref position, test);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test, control));
        }

        /// <summary>A test for DecodeCopyNewFrameTop</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeCopyNewFrameTopTest()
        {
            Int32 dataSize = 40 * 40;

            Byte[] previousData = this.CreateRandomSource(40, 40);
            Byte[] test = new Byte[dataSize];
            Byte[] control = new Byte[dataSize];
            //ensure all data are the same
            Array.Copy(previousData, test, dataSize);
            Array.Copy(previousData, control, dataSize);
            Byte[] data = new Byte[] { 0 };
            Int32 x = 16, y = 8;
            Int64 position = 0L;
            Int32 dataRemain = data.Length - (Int32)position;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic = new MveVideoCoder8SuppliedLogic(40, 40, previousData);
            controlLogic.DecodeBlock(control, 3, data, ref dataRemain, ref x, ref y);

            //create test object
            SetDecodingMap sdm = new SetDecodingMap(1);
            sdm.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.CopyTop };
            VideoData vd = new VideoData(0);
            vd.Data = new Byte[0];
            VideoOpcodeStream stream = new VideoOpcodeStream();
            MveVideoFrame frame = new MveVideoFrame(sdm, vd);

            VideoCoding8Bit_Accessor target = new VideoCoding8Bit_Accessor(40, 40, stream);

            //test the test logic
            target.DecodeCopyNewFrameTop(2, 1, data, ref position, test);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test, control));
        }

        /// <summary>A test for DecodeCopyPreviousFrameClose</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeCopyPreviousFrameCloseTest()
        {
            Int32 dataSize = 40 * 40;

            Byte[] previousData = this.CreateRandomSource(40, 40);
            Byte[] test = new Byte[dataSize];
            Byte[] control = new Byte[dataSize];
            //ensure all data are the same
            Array.Copy(previousData, test, dataSize);
            Array.Copy(previousData, control, dataSize);
            Byte[] data = new Byte[] { 198 };
            Int32 x = 24, y = 24;
            Int64 position = 0L;
            Int32 dataRemain = data.Length - (Int32)position;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic = new MveVideoCoder8SuppliedLogic(40, 40, previousData);
            controlLogic.DecodeBlock(control, 4, data, ref dataRemain, ref x, ref y);

            //create test object
            SetDecodingMap sdm = new SetDecodingMap(1);
            sdm.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.CopyPrevious };
            VideoData vd = new VideoData(0);
            vd.Data = new Byte[0];
            VideoOpcodeStream stream = new VideoOpcodeStream();
            MveVideoFrame frame = new MveVideoFrame(sdm, vd);

            VideoCoding8Bit_Accessor target = new VideoCoding8Bit_Accessor(40, 40, stream);

            //test the test logic
            target.DecodeCopyPreviousFrameClose(3, 3, data, ref position, previousData, test);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test, control));
        }

        /// <summary>A test for DecodeCopyPreviousFrameDistant</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeCopyPreviousFrameDistantTest()
        {
            Int32 dataSize = 64 * 64;

            Byte[] previousData = this.CreateRandomSource(64, 64);
            Byte[] test = new Byte[dataSize];
            Byte[] control = new Byte[dataSize];
            //ensure all data are the same
            Array.Copy(previousData, test, dataSize);
            Array.Copy(previousData, control, dataSize);
            Byte[] data = new Byte[] { 232, 18 };
            Int32 x = 24, y = 16;
            Int64 position = 0L;
            Int32 dataRemain = data.Length - (Int32)position;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic = new MveVideoCoder8SuppliedLogic(64, 64, previousData);
            controlLogic.DecodeBlock(control, 5, data, ref dataRemain, ref x, ref y);

            //create test object
            SetDecodingMap sdm = new SetDecodingMap(1);
            sdm.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.CopyPreviousLarge };
            VideoData vd = new VideoData(0);
            vd.Data = new Byte[0];
            VideoOpcodeStream stream = new VideoOpcodeStream();
            MveVideoFrame frame = new MveVideoFrame(sdm, vd);

            VideoCoding8Bit_Accessor target = new VideoCoding8Bit_Accessor(64, 64, stream);

            //test the test logic
            target.DecodeCopyPreviousFrameDistant(3, 2, data, ref position, previousData, test);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test, control));
        }

        /// <summary>A test for DecodeDither2Colors</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeDither2ColorsTest()
        {
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test = new Byte[dataSize];
            Byte[] control = new Byte[dataSize];
            Byte[] data = this.CreateRandomSource(2);
            Int32 x = 0, y = 0;
            Int64 position = 0L;
            Int32 dataRemain = data.Length - (Int32)position;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic.DecodeBlock(control, 0x0F, data, ref dataRemain, ref x, ref y);

            //create test object
            SetDecodingMap sdm = new SetDecodingMap(1);
            sdm.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.CopyBottom };
            VideoData vd = new VideoData(0);
            vd.Data = new Byte[0];
            VideoOpcodeStream stream = new VideoOpcodeStream();
            MveVideoFrame frame = new MveVideoFrame(sdm, vd);

            VideoCoding8Bit_Accessor target = new VideoCoding8Bit_Accessor(8, 8, stream);

            //test the test logic
            target.DecodeDither2Colors(0, 0, data, ref position, test);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test, control));
        }

        /// <summary>A test for DecodeFourColorIndecesQuadrantsOrHalves</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeFourColorIndecesQuadrantsOrHalvesTest()
        {
            /* Quadrants */
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test = new Byte[dataSize];
            Byte[] control = new Byte[dataSize];
            Byte[] data = this.CreateRandomSource(32);
            data[0] = 31;
            data[1] = 120;
            Int32 x = 0, y = 0;
            Int64 position = 0L;
            Int32 dataRemain = data.Length - (Int32)position;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic.DecodeBlock(control, 0x0A, data, ref dataRemain, ref x, ref y);

            //create test object
            SetDecodingMap sdm = new SetDecodingMap(1);
            sdm.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitFieldQuadrantsOrHalves2BitsPerPixel };
            VideoData vd = new VideoData(0);
            vd.Data = new Byte[0];
            VideoOpcodeStream stream = new VideoOpcodeStream();
            MveVideoFrame frame = new MveVideoFrame(sdm, vd);

            VideoCoding8Bit_Accessor target = new VideoCoding8Bit_Accessor(8, 8, stream);

            //test the test logic
            target.DecodeFourColorIndecesQuadrantsOrHalves(0, 0, data, ref position, test);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test, control));


            /* Vertical halves */
            Byte[] test2 = new Byte[dataSize];
            Byte[] control2 = new Byte[dataSize];
            Byte[] data2 = this.CreateRandomSource(24);
            data2[0] = 123;
            data2[1] = 0;
            data2[2] = 33;
            data2[3] = 120;
            data2[12] = 123;
            data2[13] = 0;

            Int32 x2 = 0, y2 = 0;
            Int64 position2 = 0L;
            Int32 dataRemain2 = data2.Length - (Int32)position2;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic2 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic2.DecodeBlock(control2, 0x0A, data2, ref dataRemain2, ref x2, ref y2);

            //create test object
            SetDecodingMap sdm2 = new SetDecodingMap(1);
            sdm2.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitFieldQuadrantsOrHalves2BitsPerPixel };
            VideoData vd2 = new VideoData(0);
            vd2.Data = new Byte[0];
            VideoOpcodeStream stream2 = new VideoOpcodeStream();
            MveVideoFrame frame2 = new MveVideoFrame(sdm2, vd2);

            VideoCoding8Bit_Accessor target2 = new VideoCoding8Bit_Accessor(8, 8, stream2);

            //test the test logic
            target2.DecodeFourColorIndecesQuadrantsOrHalves(0, 0, data2, ref position2, test2);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test2, control2));


            /* Horizontal halves */
            Byte[] test3 = new Byte[dataSize];
            Byte[] control3 = new Byte[dataSize];
            Byte[] data3 = this.CreateRandomSource(24);
            data3[0] = 123;
            data3[1] = 0;
            data3[2] = 0;
            data3[3] = 120;
            data3[12] = 0;
            data3[13] = 123;

            Int32 x3 = 0, y3 = 0;
            Int64 position3 = 0L;
            Int32 dataRemain3 = data3.Length - (Int32)position3;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic3 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic3.DecodeBlock(control3, 0x0A, data3, ref dataRemain3, ref x3, ref y3);

            //create test object
            SetDecodingMap sdm3 = new SetDecodingMap(1);
            sdm3.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitFieldQuadrantsOrHalves2BitsPerPixel };
            VideoData vd3 = new VideoData(0);
            vd3.Data = new Byte[0];
            VideoOpcodeStream stream3 = new VideoOpcodeStream();
            MveVideoFrame frame3 = new MveVideoFrame(sdm3, vd3);

            VideoCoding8Bit_Accessor target3 = new VideoCoding8Bit_Accessor(8, 8, stream3);

            //test the test logic
            target3.DecodeFourColorIndecesQuadrantsOrHalves(0, 0, data3, ref position3, test3);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test3, control3));
        }

        /// <summary>A test for DecodeFourColorIndecesRowsOrRectangles</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeFourColorIndecesRowsOrRectanglesTest()
        {
            /* 1x, 2y rectangle rows*/
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test1 = new Byte[dataSize];
            Byte[] control1 = new Byte[dataSize];
            Byte[] data1 = this.CreateRandomSource(12);
            data1[0] = 33;
            data1[1] = 0;
            data1[2] = 35;
            data1[3] = 34;
            Int32 x1 = 0, y1 = 0;
            Int64 position1 = 0L;
            Int32 dataRemain1 = data1.Length - (Int32)position1;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic1 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic1.DecodeBlock(control1, 9, data1, ref dataRemain1, ref x1, ref y1);

            //create test object
            SetDecodingMap sdm1 = new SetDecodingMap(1);
            sdm1.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitField2BitsPerPixel };
            VideoData vd1 = new VideoData(0);
            vd1.Data = new Byte[0];
            VideoOpcodeStream stream1 = new VideoOpcodeStream();
            MveVideoFrame frame1 = new MveVideoFrame(sdm1, vd1);

            VideoCoding8Bit_Accessor target1 = new VideoCoding8Bit_Accessor(8, 8, stream1);

            //test the test logic
            target1.DecodeFourColorIndecesRowsOrRectangles(0, 0, data1, ref position1, test1);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test1, control1));



            /* 2x, 1y rectangle rows*/
            Byte[] test2 = new Byte[dataSize];
            Byte[] control2 = new Byte[dataSize];
            Byte[] data2 = this.CreateRandomSource(12);
            data2[0] = 33;
            data2[1] = 0;
            data2[2] = 120;
            data2[3] = 121;
            Int32 x2 = 0, y2 = 0;
            Int64 position2 = 0L;
            Int32 dataRemain2 = data2.Length - (Int32)position2;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic2 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic2.DecodeBlock(control2, 9, data2, ref dataRemain2, ref x2, ref y2);

            //create test object
            SetDecodingMap sdm2 = new SetDecodingMap(1);
            sdm2.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitField2BitsPerPixel };
            VideoData vd2 = new VideoData(0);
            vd2.Data = new Byte[0];
            VideoOpcodeStream stream2 = new VideoOpcodeStream();
            MveVideoFrame frame2 = new MveVideoFrame(sdm2, vd2);

            VideoCoding8Bit_Accessor target2 = new VideoCoding8Bit_Accessor(8, 8, stream2);

            //test the test logic
            target2.DecodeFourColorIndecesRowsOrRectangles(0, 0, data2, ref position2, test2);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test2, control2));



            /* 2x, 2y square rows*/
            Byte[] test3 = new Byte[dataSize];
            Byte[] control3 = new Byte[dataSize];
            Byte[] data3 = this.CreateRandomSource(8);
            data3[0] = 0;
            data3[1] = 123;
            data3[2] = 35;
            data3[3] = 33;
            Int32 x3 = 0, y3 = 0;
            Int64 position3 = 0L;
            Int32 dataRemain3 = data3.Length - (Int32)position3;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic3 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic3.DecodeBlock(control3, 9, data3, ref dataRemain3, ref x3, ref y3);

            //create test object
            SetDecodingMap sdm3 = new SetDecodingMap(1);
            sdm3.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitField2BitsPerPixel };
            VideoData vd3 = new VideoData(0);
            vd3.Data = new Byte[0];
            VideoOpcodeStream stream3 = new VideoOpcodeStream();
            MveVideoFrame frame3 = new MveVideoFrame(sdm3, vd3);

            VideoCoding8Bit_Accessor target3 = new VideoCoding8Bit_Accessor(8, 8, stream3);

            //test the test logic
            target3.DecodeFourColorIndecesRowsOrRectangles(0, 0, data3, ref position3, test3);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqualAccountForDisplacementBug(test3, control3));



            /* 1x, 1y square rows*/
            Byte[] test4 = new Byte[dataSize];
            Byte[] control4 = new Byte[dataSize];
            Byte[] data4 = this.CreateRandomSource(20);
            data4[0] = 123;
            data4[1] = 0;
            data4[2] = 0;
            data4[3] = 120;
            Int32 x4 = 0, y4 = 0;
            Int64 position4 = 0L;
            Int32 dataRemain4 = data4.Length - (Int32)position4;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic4 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic4.DecodeBlock(control4, 9, data4, ref dataRemain4, ref x4, ref y4);

            //create test object
            SetDecodingMap sdm4 = new SetDecodingMap(1);
            sdm4.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitField2BitsPerPixel };
            VideoData vd4 = new VideoData(0);
            vd4.Data = new Byte[0];
            VideoOpcodeStream stream4 = new VideoOpcodeStream();
            MveVideoFrame frame4 = new MveVideoFrame(sdm4, vd4);

            VideoCoding8Bit_Accessor target4 = new VideoCoding8Bit_Accessor(8, 8, stream4);

            //test the test logic
            target4.DecodeFourColorIndecesRowsOrRectangles(0, 0, data4, ref position4, test4);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test4, control4));
        }

        /// <summary>A test for DecodeRawIndeces</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeRawIndecesTest()
        {
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test1 = new Byte[dataSize];
            Byte[] control1 = new Byte[dataSize];
            Byte[] data1 = this.CreateRandomSource(64);
            Int32 x1 = 0, y1 = 0;
            Int64 position1 = 0L;
            Int32 dataRemain1 = data1.Length - (Int32)position1;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic1 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic1.DecodeBlock(control1, 0x0B, data1, ref dataRemain1, ref x1, ref y1);

            //create test object
            SetDecodingMap sdm1 = new SetDecodingMap(1);
            sdm1.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.Raw };
            VideoData vd1 = new VideoData(0);
            vd1.Data = new Byte[0];
            VideoOpcodeStream stream1 = new VideoOpcodeStream();
            MveVideoFrame frame1 = new MveVideoFrame(sdm1, vd1);

            VideoCoding8Bit_Accessor target1 = new VideoCoding8Bit_Accessor(8, 8, stream1);

            //test the test logic
            target1.DecodeRawIndeces(0, 0, data1, ref position1, test1);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test1, control1));
        }

        /// <summary>A test for DecodeRawIndeces2PixelSquares</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeRawIndeces2PixelSquaresTest()
        {
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test1 = new Byte[dataSize];
            Byte[] control1 = new Byte[dataSize];
            Byte[] data1 = this.CreateRandomSource(16);
            Int32 x1 = 0, y1 = 0;
            Int64 position1 = 0L;
            Int32 dataRemain1 = data1.Length - (Int32)position1;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic1 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic1.DecodeBlock(control1, 0x0C, data1, ref dataRemain1, ref x1, ref y1);

            //create test object
            SetDecodingMap sdm1 = new SetDecodingMap(1);
            sdm1.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.Raw2Square };
            VideoData vd1 = new VideoData(0);
            vd1.Data = new Byte[0];
            VideoOpcodeStream stream1 = new VideoOpcodeStream();
            MveVideoFrame frame1 = new MveVideoFrame(sdm1, vd1);

            VideoCoding8Bit_Accessor target1 = new VideoCoding8Bit_Accessor(8, 8, stream1);

            //test the test logic
            target1.DecodeRawIndeces2PixelSquares(0, 0, data1, ref position1, test1);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test1, control1));
        }

        /// <summary>A test for DecodeRawIndeces4PixelSquares</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeRawIndeces4PixelSquaresTest()
        {
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test1 = new Byte[dataSize];
            Byte[] control1 = new Byte[dataSize];
            Byte[] data1 = this.CreateRandomSource(4);
            Int32 x1 = 0, y1 = 0;
            Int64 position1 = 0L;
            Int32 dataRemain1 = data1.Length - (Int32)position1;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic1 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic1.DecodeBlock(control1, 0x0D, data1, ref dataRemain1, ref x1, ref y1);

            //create test object
            SetDecodingMap sdm1 = new SetDecodingMap(1);
            sdm1.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.Raw4Square };
            VideoData vd1 = new VideoData(0);
            vd1.Data = new Byte[0];
            VideoOpcodeStream stream1 = new VideoOpcodeStream();
            MveVideoFrame frame1 = new MveVideoFrame(sdm1, vd1);

            VideoCoding8Bit_Accessor target1 = new VideoCoding8Bit_Accessor(8, 8, stream1);

            //test the test logic
            target1.DecodeRawIndeces4PixelSquares(0, 0, data1, ref position1, test1);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test1, control1));
        }

        /// <summary>A test for DecodeRawIndecesSolidColor</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeRawIndecesSolidColorTest()
        {
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test1 = new Byte[dataSize];
            Byte[] control1 = new Byte[dataSize];
            Byte[] data1 = this.CreateRandomSource(1);
            Int32 x1 = 0, y1 = 0;
            Int64 position1 = 0L;
            Int32 dataRemain1 = data1.Length - (Int32)position1;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic1 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic1.DecodeBlock(control1, 0x0E, data1, ref dataRemain1, ref x1, ref y1);

            //create test object
            SetDecodingMap sdm1 = new SetDecodingMap(1);
            sdm1.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.Raw8Square };
            VideoData vd1 = new VideoData(0);
            vd1.Data = new Byte[0];
            VideoOpcodeStream stream1 = new VideoOpcodeStream();
            MveVideoFrame frame1 = new MveVideoFrame(sdm1, vd1);

            VideoCoding8Bit_Accessor target1 = new VideoCoding8Bit_Accessor(8, 8, stream1);

            //test the test logic
            target1.DecodeRawIndecesSolidColor(0, 0, data1, ref position1, test1);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test1, control1));
        }

        /// <summary>A test for DecodeTwoToneIndecesQuadrantsOrHalves</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeTwoToneIndecesQuadrantsOrHalvesTest()
        {
            /* Quadrants */
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test1 = new Byte[dataSize];
            Byte[] control1 = new Byte[dataSize];
            Byte[] data1 = this.CreateRandomSource(16);
            data1[0] = 0;
            data1[1] = 33;
            Int32 x1 = 0, y1 = 0;
            Int64 position1 = 0L;
            Int32 dataRemain1 = data1.Length - (Int32)position1;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic1 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic1.DecodeBlock(control1, 8, data1, ref dataRemain1, ref x1, ref y1);

            //create test object
            SetDecodingMap sdm1 = new SetDecodingMap(1);
            sdm1.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitFieldQuadrants4or8Pixel };
            VideoData vd1 = new VideoData(0);
            vd1.Data = new Byte[0];
            VideoOpcodeStream stream1 = new VideoOpcodeStream();
            MveVideoFrame frame1 = new MveVideoFrame(sdm1, vd1);

            VideoCoding8Bit_Accessor target1 = new VideoCoding8Bit_Accessor(8, 8, stream1);

            //test the test logic
            target1.DecodeTwoToneIndecesQuadrantsOrHalves(0, 0, data1, ref position1, test1);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test1, control1));



            /* Horizontalhalves */
            Byte[] test2 = new Byte[dataSize];
            Byte[] control2 = new Byte[dataSize];
            Byte[] data2 = this.CreateRandomSource(12);
            data2[0] = 123;
            data2[1] = 0;
            data2[6] = 0;
            data2[7] = 120;

            Int32 x2 = 0, y2 = 0;
            Int64 position2 = 0L;
            Int32 dataRemain2 = data2.Length - (Int32)position2;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic2 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic2.DecodeBlock(control2, 8, data2, ref dataRemain2, ref x2, ref y2);

            //create test object
            SetDecodingMap sdm2 = new SetDecodingMap(1);
            sdm2.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitFieldQuadrants4or8Pixel };
            VideoData vd2 = new VideoData(0);
            vd2.Data = new Byte[0];
            VideoOpcodeStream stream2 = new VideoOpcodeStream();
            MveVideoFrame frame2 = new MveVideoFrame(sdm2, vd2);

            VideoCoding8Bit_Accessor target2 = new VideoCoding8Bit_Accessor(8, 8, stream2);

            //test the test logic
            target2.DecodeTwoToneIndecesQuadrantsOrHalves(0, 0, data2, ref position2, test2);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test2, control2));



            /* Vertical halves */
            Byte[] test3 = new Byte[dataSize];
            Byte[] control3 = new Byte[dataSize];
            Byte[] data3 = this.CreateRandomSource(12);
            data3[0] = 123;
            data3[1] = 0;
            data3[6] = 1;
            data3[7] = 0;

            Int32 x3 = 0, y3 = 0;
            Int64 position3 = 0L;
            Int32 dataRemain3 = data3.Length - (Int32)position3;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic3 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic3.DecodeBlock(control3, 8, data3, ref dataRemain3, ref x3, ref y3);

            //create test object
            SetDecodingMap sdm3 = new SetDecodingMap(1);
            sdm3.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitFieldQuadrants4or8Pixel };
            VideoData vd3 = new VideoData(0);
            vd3.Data = new Byte[0];
            VideoOpcodeStream stream3 = new VideoOpcodeStream();
            MveVideoFrame frame3 = new MveVideoFrame(sdm3, vd3);

            VideoCoding8Bit_Accessor target3 = new VideoCoding8Bit_Accessor(8, 8, stream3);

            //test the test logic
            target3.DecodeTwoToneIndecesQuadrantsOrHalves(0, 0, data3, ref position3, test3);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test3, control3));
        }

        /// <summary>A test for DecodeTwoToneIndecesRowOrSquare</summary>
        [TestMethod]
        [DeploymentItem("Infinity Formats.dll")]
        public void DecodeTwoToneIndecesRowOrSquareTest()
        {
            /* 1x, 1y rectangle rows*/
            Int32 dataSize = 8 * 8;

            Byte[] previousData = this.CreateRandomSource(8, 8);
            Byte[] test1 = new Byte[dataSize];
            Byte[] control1 = new Byte[dataSize];
            Byte[] data1 = this.CreateRandomSource(10);
            data1[0] = 52;
            data1[1] = 250;
            Int32 x1 = 0, y1 = 0;
            Int64 position1 = 0L;
            Int32 dataRemain1 = data1.Length - (Int32)position1;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic1 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic1.DecodeBlock(control1, 7, data1, ref dataRemain1, ref x1, ref y1);

            //create test object
            SetDecodingMap sdm1 = new SetDecodingMap(1);
            sdm1.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitField2PixelRowOr2Square };
            VideoData vd1 = new VideoData(0);
            vd1.Data = new Byte[0];
            VideoOpcodeStream stream1 = new VideoOpcodeStream();
            MveVideoFrame frame1 = new MveVideoFrame(sdm1, vd1);

            VideoCoding8Bit_Accessor target1 = new VideoCoding8Bit_Accessor(8, 8, stream1);

            //test the test logic
            target1.DecodeTwoToneIndecesRowOrSquare(0, 0, data1, ref position1, test1);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqual(test1, control1));



            /* 2x, 2y square rows*/
            Byte[] test3 = new Byte[dataSize];
            Byte[] control3 = new Byte[dataSize];
            Byte[] data3 = this.CreateRandomSource(4);
            data3[0] = 152;
            data3[1] = 12;
            Int32 x3 = 0, y3 = 0;
            Int64 position3 = 0L;
            Int32 dataRemain3 = data3.Length - (Int32)position3;

            //create and decode with control logic
            MveVideoCoder8SuppliedLogic controlLogic3 = new MveVideoCoder8SuppliedLogic(8, 8, previousData);
            controlLogic3.DecodeBlock(control3, 7, data3, ref dataRemain3, ref x3, ref y3);

            //create test object
            SetDecodingMap sdm3 = new SetDecodingMap(1);
            sdm3.BlockEncoding = new BlockEncodingMethod[1] { BlockEncodingMethod.BitField2PixelRowOr2Square };
            VideoData vd3 = new VideoData(0);
            vd3.Data = new Byte[0];
            VideoOpcodeStream stream3 = new VideoOpcodeStream();
            MveVideoFrame frame3 = new MveVideoFrame(sdm3, vd3);

            VideoCoding8Bit_Accessor target3 = new VideoCoding8Bit_Accessor(8, 8, stream3);

            //test the test logic
            target3.DecodeTwoToneIndecesRowOrSquare(0, 0, data3, ref position3, test3);

            //evaluate
            Assert.IsTrue(this.AreBinaryArraysEqualAccountForDisplacementBug(test3, control3));
        }


        #region Dummy objects generation
        protected Byte[] CreateRandomSource(Int32 height, Int32 width)
        {
            Byte[] data = new Byte[height * width];

            Random randGen = new Random(DateTime.Now.Millisecond);
            randGen.NextBytes(data);

            return data;
        }

        protected Byte[] CreateRandomSource(Int32 count)
        {
            Byte[] data = new Byte[count];

            Random randGen = new Random(DateTime.Now.Millisecond);
            randGen.NextBytes(data);

            return data;
        }
        #endregion


        #region compare output
        protected virtual Boolean AreBinaryArraysEqual(Byte[] a, Byte[] b)
        {
            Boolean equal = false;

            if (a == null && b == null)
                equal = true;
            else if (a.Length == b.Length)
            {
                Boolean foundNotEqual = false;

                for (Int32 i = 0; i < a.Length; ++i)
                    if (a[i] != b[i])
                    {
                        foundNotEqual = true;
                        break;
                    }

                equal = !foundNotEqual;
            }

            return equal;
        }

        protected virtual Boolean AreBinaryArraysEqualAccountForDisplacementBug(Byte[] a, Byte[] b)
        {
            Boolean equal = false;

            if (a == null && b == null)
                equal = true;
            else if (a.Length == b.Length)
            {
                Boolean foundNotEqual = false;

                for (Int32 i = 0; i < a.Length; i += 4)
                {
                    if (a[i] != b[i] || a[i+1] != b[i+2] || a[i+2] != b[i+1] || a[i+3] != b[i+3])
                    {
                        foundNotEqual = true;
                        break;
                    }
                }

                equal = !foundNotEqual;
            }

            return equal;
        }
        #endregion
    }
}
