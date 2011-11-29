using System;
using System.Diagnostics;

using Bardez.Projects.InfinityPlus1.Files.External.Interplay.ACM;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.AmpitudeCodedModulation
{
    /// <summary>This class is a fairly legitimate unit test of all the amplitude and bitread conditions for "k12" - "k45"</summary>
    public class BitBlockTest : TesterBase
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public BitBlockTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        /// <remarks>No implementation due to lack of initialization to this test class</remarks>
        protected override void InitializeTestData(object sender, EventArgs e) { }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            this.TestAllAmplitudeReads();
            //this.TestBase3Code();
        }

        /// <summary>Tests all amplitude reads (all ACM basic amplitude functions)</summary>
        protected virtual void TestAllAmplitudeReads()
        {
            //1, 2
            this.TestSingleRead(0, 1, 2, 0);
            this.TestSingleRead(1, 1, 2, 1);
            this.TestSingleRead(2, 1, 2, 0);
            this.TestSingleRead(3, 1, 2, 3);

            //1, 3
            this.TestSingleRead(0, 1, 3, 0);
            this.TestSingleRead(1, 1, 3, 1);
            this.TestSingleRead(2, 1, 3, 0);
            this.TestSingleRead(3, 1, 3, 3);
            this.TestSingleRead(4, 1, 3, 0);
            this.TestSingleRead(5, 1, 3, 1);
            this.TestSingleRead(6, 1, 3, 0);
            this.TestSingleRead(7, 1, 3, 7);

            //2, 3
            this.TestSingleRead(0, 2, 3, 0);
            this.TestSingleRead(1, 2, 3, 1);
            this.TestSingleRead(2, 2, 3, 0);
            this.TestSingleRead(3, 2, 3, 3);
            this.TestSingleRead(4, 2, 3, 0);
            this.TestSingleRead(5, 2, 3, 5);
            this.TestSingleRead(6, 2, 3, 0);
            this.TestSingleRead(7, 2, 3, 7);

            //2, 4
            this.TestSingleRead(0,  2, 4, 0);
            this.TestSingleRead(1,  2, 4, 1);
            this.TestSingleRead(2,  2, 4, 0);
            this.TestSingleRead(3,  2, 4, 3);
            this.TestSingleRead(4,  2, 4, 0);
            this.TestSingleRead(5,  2, 4, 1);
            this.TestSingleRead(6,  2, 4, 0);
            this.TestSingleRead(7,  2, 4, 7);
            this.TestSingleRead(8,  2, 4, 0);
            this.TestSingleRead(9,  2, 4, 1);
            this.TestSingleRead(10, 2, 4, 0);
            this.TestSingleRead(11, 2, 4, 11);
            this.TestSingleRead(12, 2, 4, 0);
            this.TestSingleRead(13, 2, 4, 1);
            this.TestSingleRead(14, 2, 4, 0);
            this.TestSingleRead(15, 2, 4, 15);

            //3, 4
            this.TestSingleRead(0,  3, 4, 0);
            this.TestSingleRead(1,  3, 4, 1);
            this.TestSingleRead(2,  3, 4, 0);
            this.TestSingleRead(3,  3, 4, 3);
            this.TestSingleRead(4,  3, 4, 0);
            this.TestSingleRead(5,  3, 4, 5);
            this.TestSingleRead(6,  3, 4, 0);
            this.TestSingleRead(7,  3, 4, 7);
            this.TestSingleRead(8,  3, 4, 0);
            this.TestSingleRead(9,  3, 4, 1);
            this.TestSingleRead(10, 3, 4, 0);
            this.TestSingleRead(11, 3, 4, 11);
            this.TestSingleRead(12, 3, 4, 0);
            this.TestSingleRead(13, 3, 4, 5);
            this.TestSingleRead(14, 3, 4, 0);
            this.TestSingleRead(15, 3, 4, 15);

            //3, 5
            this.TestSingleRead(0,  3, 5, 0);
            this.TestSingleRead(1,  3, 5, 1);
            this.TestSingleRead(2,  3, 5, 0);
            this.TestSingleRead(3,  3, 5, 3);
            this.TestSingleRead(4,  3, 5, 0);
            this.TestSingleRead(5,  3, 5, 1);
            this.TestSingleRead(6,  3, 5, 0);
            this.TestSingleRead(7,  3, 5, 7);
            this.TestSingleRead(8,  3, 5, 0);
            this.TestSingleRead(9,  3, 5, 1);
            this.TestSingleRead(10, 3, 5, 0);
            this.TestSingleRead(11, 3, 5, 11);
            this.TestSingleRead(12, 3, 5, 0);
            this.TestSingleRead(13, 3, 5, 1);
            this.TestSingleRead(14, 3, 5, 0);
            this.TestSingleRead(15, 3, 5, 15);
            this.TestSingleRead(16, 3, 5, 0);
            this.TestSingleRead(17, 3, 5, 1);
            this.TestSingleRead(18, 3, 5, 0);
            this.TestSingleRead(19, 3, 5, 3);
            this.TestSingleRead(20, 3, 5, 0);
            this.TestSingleRead(21, 3, 5, 1);
            this.TestSingleRead(22, 3, 5, 0);
            this.TestSingleRead(23, 3, 5, 23);
            this.TestSingleRead(24, 3, 5, 0);
            this.TestSingleRead(25, 3, 5, 1);
            this.TestSingleRead(26, 3, 5, 0);
            this.TestSingleRead(27, 3, 5, 11);
            this.TestSingleRead(28, 3, 5, 0);
            this.TestSingleRead(29, 3, 5, 1);
            this.TestSingleRead(30, 3, 5, 0);
            this.TestSingleRead(31, 3, 5, 31);
            
            //4, 4
            this.TestSingleRead(0,  4, 4, 0);
            this.TestSingleRead(1,  4, 4, 1);
            this.TestSingleRead(2,  4, 4, 0);
            this.TestSingleRead(3,  4, 4, 3);
            this.TestSingleRead(4,  4, 4, 0);
            this.TestSingleRead(5,  4, 4, 5);
            this.TestSingleRead(6,  4, 4, 0);
            this.TestSingleRead(7,  4, 4, 7);
            this.TestSingleRead(8,  4, 4, 0);
            this.TestSingleRead(9,  4, 4, 9);
            this.TestSingleRead(10, 4, 4, 0);
            this.TestSingleRead(11, 4, 4, 11);
            this.TestSingleRead(12, 4, 4, 0);
            this.TestSingleRead(13, 4, 4, 13);
            this.TestSingleRead(14, 4, 4, 0);
            this.TestSingleRead(15, 4, 4, 15);
            
            //4, 5
            this.TestSingleRead(0,  4, 5, 0);
            this.TestSingleRead(1,  4, 5, 1);
            this.TestSingleRead(2,  4, 5, 0);
            this.TestSingleRead(3,  4, 5, 3);
            this.TestSingleRead(4,  4, 5, 0);
            this.TestSingleRead(5,  4, 5, 1);
            this.TestSingleRead(6,  4, 5, 0);
            this.TestSingleRead(7,  4, 5, 7);
            this.TestSingleRead(8,  4, 5, 0);
            this.TestSingleRead(9,  4, 5, 1);
            this.TestSingleRead(10, 4, 5, 0);
            this.TestSingleRead(11, 4, 5, 11);
            this.TestSingleRead(12, 4, 5, 0);
            this.TestSingleRead(13, 4, 5, 1);
            this.TestSingleRead(14, 4, 5, 0);
            this.TestSingleRead(15, 4, 5, 15);
            this.TestSingleRead(16, 4, 5, 0);
            this.TestSingleRead(17, 4, 5, 1);
            this.TestSingleRead(18, 4, 5, 0);
            this.TestSingleRead(19, 4, 5, 19);
            this.TestSingleRead(20, 4, 5, 0);
            this.TestSingleRead(21, 4, 5, 1);
            this.TestSingleRead(22, 4, 5, 0);
            this.TestSingleRead(23, 4, 5, 23);
            this.TestSingleRead(24, 4, 5, 0);
            this.TestSingleRead(25, 4, 5, 1);
            this.TestSingleRead(26, 4, 5, 0);
            this.TestSingleRead(27, 4, 5, 27);
            this.TestSingleRead(28, 4, 5, 0);
            this.TestSingleRead(29, 4, 5, 1);
            this.TestSingleRead(30, 4, 5, 0);
            this.TestSingleRead(31, 4, 5, 31);
        }

        /// <summary>Unit-tests a single read from the BitBlock</summary>
        /// <param name="datum">Datum byte to populate the datastream</param>
        /// <param name="amplitude">Amplitude bits read</param>
        /// <param name="maxBits">Maximum numer of bits to read</param>
        /// <param name="expectedValue">Expected value from the read</param>
        protected void TestSingleRead(Byte datum, Int32 amplitude, Int32 maxBits, Int32 expectedValue)
        {
            BitBlock block = BitBlock.Instance;
            block.BitDataStream = new BitStream(new Byte[] { datum });
            Int32 result = block.ReadAmplitudeBits(amplitude, maxBits);

            //replacement for Debug.Assert
            if (result != expectedValue)
                this.DoPostMessage(new MessageEventArgs(String.Format("Testing method with value {0}, amplitude {1} and bits of {2}; expected {3}, got {4}", datum, amplitude, maxBits, expectedValue, result), "Assertion", "TestSingleRead"));
        }

        /// <summary>Tests the base 3 data read</summary>
        [Obsolete("Used to debg a specific issue, no output")]
        protected void TestBase3Code()
        {
	        int x1, x2, x3;

            //int mul_3x3[3*3*3];
            //int mul_3x5[5*5*5]; 
            //int mul_2x11[11*11];

            int[] mul_3x3 =  new int[3*3*3];
            int[] mul_3x5 =  new int[5*5*5];
            int[] mul_2x11 = new int[11*11];

	        for (x3 = 0; x3 < 3; x3++)
		        for (x2 = 0; x2 < 3; x2++)
			        for (x1 = 0; x1 < 3; x1++)
				        mul_3x3[x1 + x2*3 + x3*3*3] =  x1 + (x2 << 4) + (x3 << 8);

	        for (x3 = 0; x3 < 5; x3++)
		        for (x2 = 0; x2 < 5; x2++)
			        for (x1 = 0; x1 < 5; x1++)
				        mul_3x5[x1 + x2*5 + x3*5*5] =  x1 + (x2 << 4) + (x3 << 8);
	        for (x2 = 0; x2 < 11; x2++)
		        for (x1 = 0; x1 < 11; x1++)
			        mul_2x11[x1 + x2*11] = x1 + (x2 << 4);

            // read only 26 bits?!?!
            // 000 = 0
            // 010 = 3
            // 020 = 6
            // 022 = 8
            // 100 = 9
            // 102 = 11
            // 110 = 12
            // 120 = 15
            // 200 = 18
            // 220 = 24
            // 222 = 26
            //1000 = 27
            //1010 = 30
            //1011 = 31

            int n1, n2, n3;
            int b = 26;

            n1 = (mul_3x3[b] & 0x0F) - 1;
            n2 = ((mul_3x3[b] >> 4) & 0x0F) - 1;
            n3 = ((mul_3x3[b] >> 8) & 0x0F) - 1;

            b = 13;

            n1 = (mul_3x3[b] & 0x0F) - 1;
            n2 = ((mul_3x3[b] >> 4) & 0x0F) - 1;
            n3 = ((mul_3x3[b] >> 8) & 0x0F) - 1;

            b = 0;

            n1 = (mul_3x3[b] & 0x0F) - 1;
            n2 = ((mul_3x3[b] >> 4) & 0x0F) - 1;
            n3 = ((mul_3x3[b] >> 8) & 0x0F) - 1;

            b = 11; // 102 == 11

            n1 = (mul_3x3[b] & 0x0F) - 1;
            n2 = ((mul_3x3[b] >> 4) & 0x0F) - 1;
            n3 = ((mul_3x3[b] >> 8) & 0x0F) - 1;


            Int32 digit1, digit2, digit3;
            b = 31;
            //convert to base 3
            digit1 = b % 3;
            digit2 = (b / 3) % 3;
            digit3 = (b / 9) % 3;

            b = 3;
            //convert to base 3
            digit1 = b % 3;
            digit2 = (b / 3) % 3;
            digit3 = (b / 9) % 3;

            b = 26;
            //convert to base 3
            digit1 = b % 3;
            digit2 = (b / 3) % 3;
            digit3 = (b / 9) % 3;
        }
    }
}