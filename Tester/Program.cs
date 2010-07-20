using System;
using InfinityPlus1.Tester.TextLocationKey;

namespace InfinityPlus1.Tester
{
    class Program
    {
        static void Main(String[] args)
        {
            TextLocationKeyTest tester = new TextLocationKeyTest();
            tester.Test();

            Console.Write("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}