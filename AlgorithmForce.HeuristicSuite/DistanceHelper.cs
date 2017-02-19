using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class DistanceHelper
    {
        public static readonly IEqualityComparer<long> Int64EqualityComparer = EqualityComparer<long>.Default;
        public static readonly IEqualityComparer<int> Int32EqualityComparer = EqualityComparer<int>.Default;

        public static readonly IComparer<long> Int64Comparer = Comparer<long>.Default;
        public static readonly IComparer<int> Int32Comparer = Comparer<int>.Default;

        public static long GetManhattanDistance(long x1, long y1, long x2, long y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static long GetManhattanDistance(this Point2DInt64 a, Point2DInt64 b)
        {
            return GetManhattanDistance(a.X, a.Y, b.X, b.Y);
        }

        public static int GetManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static int GetManhattanDistance(this Point2DInt32 a, Point2DInt32 b)
        {
            return GetManhattanDistance(a.X, a.Y, b.X, b.Y);
        }

        public static long GetChebyshevDistance(long x1, long y1, long x2, long y2)
        {
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }

        public static long GetChebyshevDistance(this Point2DInt64 a, Point2DInt64 b)
        {
            return GetChebyshevDistance(a.X, a.Y, b.X, b.Y);
        }

        public static int GetChebyshevDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }

        public static int GetChebyshevDistance(this Point2DInt32 a, Point2DInt32 b)
        {
            return GetChebyshevDistance(a.X, a.Y, b.X, b.Y);
        }
    }
}