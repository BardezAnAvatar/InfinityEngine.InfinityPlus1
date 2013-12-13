using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DCT
{
    /// <summary>Represents a repository of DCT pre-computed constants</summary>
    public static class Constants
    {
        public const Double RootHalfD = 0.70710678118654757;    //Math.Sqrt(0.5);
        public const Double RootTwoD = 1.4142135623730951;      //Math.Sqrt(2.0);
        public const Int32 RootHalfI = 8119;                    //Math.Sqrt(0.5) * 11482 = 8119.000061583932620124;
        public const Int32 RootHalfShift = 11482;               //Math.Sqrt(0.5) * 11482 = 8119.000061583932620124;

        //k * cos(n * pi / 16) or k * sin(n * pi / 16); k = root2; n=1,3,6; cos/sin and n represent the variable name
        public const Double RotateC6D = 0.54119610014619712;    //Constants.RootTwo * Math.Cos(6.0 * Math.PI / 16.0);
        public const Double RotateC1D = 1.3870398453221475;     //Constants.RootTwo * Math.Cos(1.0 * Math.PI / 16.0);
        public const Double RotateC3D = 1.1758756024193588;     //Constants.RootTwo * Math.Cos(3.0 * Math.PI / 16.0);
        public const Double RotateS6D = 1.3065629648763766;     //Constants.RootTwo * Math.Sin(6.0 * Math.PI / 16.0);
        public const Double RotateS1D = 0.275899379282943;      //Constants.RootTwo * Math.Sin(1.0 * Math.PI / 16.0);
        public const Double RotateS3D = 0.78569495838710213;    //Constants.RootTwo * Math.Sin(3.0 * Math.PI / 16.0);

        //Attempt to clamp these constants above to a lossy integer value that can be faster.
        //37 seems to be the lowest constant with a decent low average remainder (0.245657911) when performing modulus 1 on a range of 1 - 111
        //261 comes out to 0.210361661
        //1044 comes out to 0.174779976
        //1530 comes out to 0.095223527
        public const Int32 RotateC6I = 828;     //Constants.RootTwo * Math.Cos(6.0 * Math.PI / 16.0) * 1530;
        public const Int32 RotateC1I = 2122;    //Constants.RootTwo * Math.Cos(1.0 * Math.PI / 16.0) * 1530;
        public const Int32 RotateC3I = 1799;    //Constants.RootTwo * Math.Cos(3.0 * Math.PI / 16.0) * 1530;
        public const Int32 RotateS6I = 1999;    //Constants.RootTwo * Math.Sin(6.0 * Math.PI / 16.0) * 1530;
        public const Int32 RotateS1I = 422;     //Constants.RootTwo * Math.Sin(1.0 * Math.PI / 16.0) * 1530;
        public const Int32 RotateS3I = 1202;    //Constants.RootTwo * Math.Sin(3.0 * Math.PI / 16.0) * 1530;
        public const Int32 RotateDivisor = 1530;
    }
}