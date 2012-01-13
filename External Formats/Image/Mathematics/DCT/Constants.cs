using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.Mathematics.DCT
{
    /// <summary>Represents a repository of DCT pre-computed constants</summary>
    public static class Constants
    {
        public const Double RootHalf = 0.70710678118654757;     //Math.Sqrt(0.5);
        public const Double RootTwo = 1.4142135623730951;       //Math.Sqrt(2.0);

        //k * cos(n * pi / 16) or k * sin(n * pi / 16); k = root2; n=1,3,6; cos/sin and n represent the variable name
        public const Double RotateC6 = 0.54119610014619712;     //Constants.RootTwo * Math.Cos(6.0 * Math.PI / 16.0);
        public const Double RotateC1 = 1.3870398453221475;      //Constants.RootTwo * Math.Cos(1.0 * Math.PI / 16.0);
        public const Double RotateC3 = 1.1758756024193588;      //Constants.RootTwo * Math.Cos(3.0 * Math.PI / 16.0);
        public const Double RotateS6 = 1.3065629648763766;      //Constants.RootTwo * Math.Sin(6.0 * Math.PI / 16.0);
        public const Double RotateS1 = 0.275899379282943;       //Constants.RootTwo * Math.Sin(1.0 * Math.PI / 16.0);
        public const Double RotateS3 = 0.78569495838710213;     //Constants.RootTwo * Math.Sin(3.0 * Math.PI / 16.0);
    }
}