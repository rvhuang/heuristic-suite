using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IStep<TKey, TStep>
    {
        TKey Key { get; }

        bool IsValidStep { get; }

        int Depth { get; set; }

        IStep<TKey, TStep> PreviousStep { get; set; }
    }

    internal class StepComparer<TKey, TStep> : IComparer<TStep>
        where TStep : IStep<TKey, TStep>
    {
        private readonly IComparer<TKey> _keyComparer;
        private readonly StepComparerMode _mode;
        private readonly Comparison<TStep> _comparison;

        public StepComparer(IComparer<TKey> keyComparer, StepComparerMode mode)
        {
            this._keyComparer = keyComparer;
            this._mode = mode;

            switch (mode)
            {
                case StepComparerMode.DepthFirst:
                    this._comparison = this.DepthFirstComparison;
                    break;

                default:
                    this._comparison = this.KeyFirstComparison;
                    break;
            }
        }

        public int Compare(TStep x, TStep y)
        {
            return this._comparison(x, y);
        }

        private int KeyFirstComparison(TStep x, TStep y)
        {
            var keyComparing = _keyComparer.Compare(x.Key, y.Key); // H(x)
            var depthComparing = DistanceHelper.Int32Comparer.Compare(x.Depth, y.Depth); // G(x)

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

        private int DepthFirstComparison(TStep x, TStep y)
        {
            var keyComparing = _keyComparer.Compare(x.Key, y.Key); // H(x)
            var depthComparing = DistanceHelper.Int32Comparer.Compare(x.Depth, y.Depth); // G(x)

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

    public enum StepComparerMode
    {
        KeyFirst,

        DepthFirst,
    }
}