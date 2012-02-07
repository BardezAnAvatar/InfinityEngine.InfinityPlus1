using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.Utility
{
    /// <summary>Enum for result of comparing two files</summary>
    public enum FileCompareResult
    {
        FileDoesNotExist,
        DifferingLength,
        ContentDiffers,
        Equivalent,
    }

    /// <summary>Utility class for comparing two files</summary>
    public static class FileCompare
    {
        /// <summary>Compares two files, passed in as file path strings</summary>
        /// <param name="leftFilePath">left file to compare</param>
        /// <param name="rightFilePath">right file to compare</param>
        /// <returns>A FileCompareResult of two files' comparison</returns>
        public static FileCompareResult CompareFilesWithResult(String leftFilePath, String rightFilePath)
        {
            FileCompareResult result = FileCompareResult.FileDoesNotExist;

            //files exist?
            if (File.Exists(leftFilePath) && File.Exists(rightFilePath))
            {
                //file size
                FileInfo fil = new FileInfo(leftFilePath), fir = new FileInfo(rightFilePath);

                if (fil.Length != fir.Length)
                    result = FileCompareResult.DifferingLength;
                else //if (fil.Length == fir.Length)
                {
                    Boolean matching = false;

                    using (FileStream fsL = new FileStream(leftFilePath, FileMode.Open, FileAccess.Read))
                    using (FileStream fsR = new FileStream(rightFilePath, FileMode.Open, FileAccess.Read))
                    {
                        //perform the comparison
                        do
                            matching = fsL.ReadByte() == fsR.ReadByte();
                        while (fsL.Position < fsL.Length && matching);
                    }

                    result = matching ? FileCompareResult.Equivalent : FileCompareResult.ContentDiffers;
                }
            }

            return result;
        }

        /// <summary>Compares two files, passed in as file path strings</summary>
        /// <param name="leftFilePath">left file to compare</param>
        /// <param name="rightFilePath">right file to compare</param>
        /// <returns>A Boolean indicating the binary equivalence of the two files' comparison</returns>
        public static Boolean CompareFiles(String leftFilePath, String rightFilePath)
        {
            return FileCompare.CompareFilesWithResult(leftFilePath, rightFilePath) == FileCompareResult.Equivalent;
        }
    }
}