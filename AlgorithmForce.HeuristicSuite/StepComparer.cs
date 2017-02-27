using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    class StepComparer<TKey, TStep> : IComparer<TStep>
        where TStep : IStep<TKey>
    {
        private readonly IComparer<TKey> _keyComparer;
        private readonly Comparison<TStep> _comparison;

        public StepComparer(IComparer<TKey> keyComparer, HeuristicFunctionPreference preference)
        {
            this._keyComparer = keyComparer == null ? Comparer<TKey>.Default : keyComparer;

            switch (preference)
            {
                case HeuristicFunctionPreference.GFirst:
                    this._comparison = this.DepthFirstComparison;
                    break;

                case HeuristicFunctionPreference.HFirst:
                    this._comparison = this.KeyFirstComparison;
                    break;

                default:
                    this._comparison = this.AverageComparison;
                    break;
            }
        }

        public int Compare(TStep a, TStep b)
        {
            if (a == null) return b == null ? 0 : 1;
            if (a != null) return b != null ? this._comparison(a, b) : -1;

            return 0; // actually we won't reach here.
        }

        private int AverageComparison(TStep a, TStep b)
        {
            var keyComparing = _keyComparer.Compare(a.Key, b.Key); // H(x)
            var depthComparing = DistanceHelper.Int32Comparer.Compare(a.Depth, b.Depth); // G(x)

            if (keyComparing < 0 && depthComparing < 0)
                return -1;

            if (keyComparing > 0 && depthComparing > 0)
                return 1;

            if (keyComparing == 0 && depthComparing != 0)
                return depthComparing;

            if (keyComparing != 0 && depthComparing == 0)
                return keyComparing;

            return 0;
        }

        private int KeyFirstComparison(TStep a, TStep b)
        {
            var keyComparing = _keyComparer.Compare(a.Key, b.Key); // H(x)
            var depthComparing = DistanceHelper.Int32Comparer.Compare(a.Depth, b.Depth); // G(x)

            if (keyComparing < 0 && depthComparing < 0)
                return -1;

            if (keyComparing > 0 && depthComparing > 0)
                return 1;

            if (keyComparing == 0 && depthComparing == 0)
                return 0;

            if (keyComparing != 0)
                return keyComparing;
            else
                return depthComparing;
        }

        private int DepthFirstComparison(TStep a, TStep b)
        {
            var keyComparing = _keyComparer.Compare(a.Key, b.Key); // H(x)
            var depthComparing = DistanceHelper.Int32Comparer.Compare(a.Depth, b.Depth); // G(x)

            if (keyComparing < 0 && depthComparing < 0)
                return -1;

            if (keyComparing > 0 && depthComparing > 0)
                return 1;

            if (keyComparing == 0 && depthComparing == 0)
                return 0;

            if (depthComparing != 0)
                return depthComparing;
            else
                return keyComparing;
        }
    }

    public enum HeuristicFunctionPreference
    {
        Average,

        HFirst,

        GFirst,
    }
}
