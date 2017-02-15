using System;

namespace AlgorithmForce.HeuristicSuite
{
    public static class DistanceHelper
    {
        public static long GetManhattanDistance(long x1, long y1, long x2, long y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static long GetChebyshevDistance(long x1, long y1, long x2, long y2)
        {
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }
    }
}
