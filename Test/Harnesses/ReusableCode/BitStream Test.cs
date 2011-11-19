using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.ReusableCode;
using Bardez.Projects.InfinityPlus1.Test;

namespace Bardez.Projects.InfinityPlus1.Test.ReusableCode
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.ReusableCode.BitStream class.</summary>
    public class BitStreamTest : ITester
    {
        protected BitStream bitStream;

        public BitStream BitStreamExposure
        {
            get { return this.bitStream; }
        }

        public void Test()
        {
            this.bitStream = new BitStream();
            //this.bitStream.Data = new Byte[] { 255, 127, 63, 31, 15, 7, 3, 1, 0 };
            //Byte byte1, byte2, byte3, byte4, byte5, byte6;

            //byte1 = this.bitStream.ReadByte(3); //should be value 7, position 3
            //byte2 = this.bitStream.ReadByte(7); //should be value 127, position 10
            //byte3 = this.bitStream.ReadByte(7); //should be value 95, position 17
            //byte4 = this.bitStream.ReadByte(6); //should be value 31, position 23
            //byte5 = this.bitStream.ReadByte(6); //should be value 62, position 29
            //byte6 = this.bitStream.ReadByte(8); //should be value 120, position 3

            this.bitStream.Data = new Byte[] { 0xA6, 0x06, 0x60, 0xBE, 0x2F, 0x6C, 0x1F, 0x34, 0x6C, 0xAB, 0xEB };
            Byte byte1, byte2, byte3, byte4, byte5, byte6, byte7, byte8, byte9;

            byte1 = this.bitStream.ReadByte(4); //should be value 6 //pwr
            byte2 = this.bitStream.ReadByte(8); //should be value 106  //cnt 1
            byte3 = this.bitStream.ReadByte(8); //should be value 0  //cnt 2
            byte4 = this.bitStream.ReadByte(5); //should be value 6
            byte5 = this.bitStream.ReadByte(6); //should be value 31
            byte6 = this.bitStream.ReadByte(6); //should be value 31
            byte7 = this.bitStream.ReadByte(6); //should be value 33
            byte8 = this.bitStream.ReadByte(6); //should be value 45
            byte9 = this.bitStream.ReadByte(6); //should be value 15
        }
    }
}