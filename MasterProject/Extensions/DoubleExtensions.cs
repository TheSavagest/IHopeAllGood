using static System.Math;

namespace MasterProject.Extensions
{
    public static class DoubleExtensions
    {
        private const double DoubleDelta = 1E-12;

        public static bool IsEqualWithDelta(double value1, double value2, double delta = DoubleDelta)
        {
            return Abs(value1 - value2) < delta;
        }
    }
}