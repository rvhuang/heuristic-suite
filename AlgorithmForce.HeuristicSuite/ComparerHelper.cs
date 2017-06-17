using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class ComparerHelper
    {
        public static T Min<T>(this IComparer<T> comparer, T a, T b)
        {
            if (a == null) return b;
            if (b == null) return a;

            return (comparer != null ? comparer : Comparer<T>.Default).Compare(a, b) < 0 ? a : b;
        }

        public static T Max<T>(this IComparer<T> comparer, T a, T b)
        {
            if (a == null) return b;
            if (b == null) return a;

            return (comparer != null ? comparer : Comparer<T>.Default).Compare(a, b) > 0 ? a : b;
        }
    }
}