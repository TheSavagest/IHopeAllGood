using System;
using MasterProject.Extensions;
using static System.Array;

namespace MasterProject.Solutions
{
    public sealed class Point : IPoint<Point>
    {
        public double Fitness { get; set; }
        public double Value { get; set; }
        public double[] Coordinates { get; set; }

        public Point()
        {
            Fitness = double.NaN;
            Value = double.NaN;
            Coordinates = Empty<double>();
        }
        
        public Point DeepClone()
        {
            return new()
            {
                Fitness = Fitness,
                Value = Value,
                Coordinates = Coordinates.Copy()
            };
        }
    }
}