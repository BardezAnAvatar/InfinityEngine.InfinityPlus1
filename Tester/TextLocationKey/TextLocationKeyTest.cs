﻿using System;
using System.Text;
using InfinityPlus1.Files;

namespace InfinityPlus1.Tester.TextLocationKey
{
    public class TextLocationKeyTest : ITester
    {
        /// <summary>Testing object</summary>
        protected Files.TextLocationKey tlkFile;

        /// <summary>ITester Interface method</summary>
        public void Test()
        {
            String path;
            //path = @"D:\Games\Forgotten Realms\Planescape - Torment\Dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\Planescape - Torment\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\Baldurs Gate Trilogy\dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\tosc.fr.dialog\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\tosc.fr.dialog\dialog.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\bg.de.dialog\dialogF.tlk";
            //path = @"D:\Games\Forgotten Realms\tlk-files\bg.de.dialog\dialog.tlk";

            path = @"D:\Games\Icewind Dale\dialog.tlk";

            Test(path);
        }

        /// <summary>Tests the testing object</summary>
        /// <param name="filePath">String describing the location of a TLK file</param>
        public void Test(String filePath)
        {
            Console.WriteLine("Initializing at " + DateTime.Now.TimeOfDay.ToString() + ":");
            tlkFile = new Files.TextLocationKey();
            tlkFile.TlkPath = filePath;
            Console.WriteLine("     Reading at " + DateTime.Now.TimeOfDay.ToString() + ":");
            tlkFile.ReadFile();

            //Console.Write(tlkFile.ToString()); that will take far too long...
        }
    }
}