using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class DistanceHelper
    {
        internal static readonly IEqualityComparer<long> Int64EqualityComparer = EqualityComparer<long>.Default;
        internal static readonly IComparer<long> Int64Comparer = Comparer<long>.Default;

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
