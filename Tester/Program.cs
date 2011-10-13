using System;

using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.InfinityPlus1.Test.BiowareIndexFileFormat.Biff1;
using Bardez.Projects.InfinityPlus1.Test.Character;
using Bardez.Projects.InfinityPlus1.Test.ChitinKey;
using Bardez.Projects.InfinityPlus1.Test.Creature;
using Bardez.Projects.InfinityPlus1.Test.Effect;
using Bardez.Projects.InfinityPlus1.Test.Item;
using Bardez.Projects.InfinityPlus1.Test.Spell;
using Bardez.Projects.InfinityPlus1.Test.Store;
using Bardez.Projects.InfinityPlus1.Test.TextLocationKey;
using Bardez.Projects.InfinityPlus1.Test.TwoDimensionalArray;

namespace Bardez.Projects.InfinityPlus1.Tester
{
    /// <summary>Main program class/entry point object.</summary>
    internal class Program
    {
        /// <summary>Driving main method application entry point</summary>
        /// <param name="args">String Array of application arguments</param>
        internal static void Main(String[] args)
        {
            ITester tester = new Biography2Test();
            tester.Test();

            Console.Write("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}