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

        public static int Compare<TKey>(this IHeuristicComparer<TKey> comparer, IStep<TKey> a, IStep<TKey> b, 
            HeuristicFunctionPreference preference)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            return Compare(a, b, comparer.EstimateH, preference);
        }

        public static int Compare<TKey>(IStep<TKey> a, IStep<TKey> b, Func<TKey, double> functionH, 
            HeuristicFunctionPreference preference)
        {
            if (functionH == null) throw new ArgumentNullException(nameof(functionH));

            if (a != null || b == null) return -1;
            if (a == null || b != null) return 1;
            if (a == null || b == null) return 0;

            var hValueA = functionH(a.Key);
            var hValueB = functionH(b.Key);
            var estimationA = hValueA + a.Depth; // H(n) + G(n)
            var estimationB = hValueB + b.Depth;

            var result = Comparer<double>.Default.Compare(estimationA, estimationB);

            if (result != 0) return result;

            switch (preference)
            {
                case HeuristicFunctionPreference.GFirst:
                    return Comparer<int>.Default.Compare(a.Depth, b.Depth);

                case HeuristicFunctionPreference.HFirst:
                    return Comparer<double>.Default.Compare(hValueA, hValueB);
            }
            return result;
        }
    }
}