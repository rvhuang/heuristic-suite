using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

namespace AlgorithmForce.HeuristicSuite
{
    public class DiscreteHeuristicComparer<TKey, TStep> : IHeuristicComparer<TKey, TStep>
        where TStep : IStep<TKey>
    {
        #region Fields

        private readonly IComparer<TKey> _keyComparer;
        private readonly Comparison<TStep> _comparison;
        private readonly HeuristicFunctionPreference _preference;

        #endregion

        #region Properties

        public IComparer<TKey> KeyComparer
        {
            get { return this._keyComparer; }
        }

        public HeuristicFunctionPreference Preference
        {
            get { return this._preference; }
        }

        #endregion

        #region Constructor

        public DiscreteHeuristicComparer(IComparer<TKey> keyComparer, HeuristicFunctionPreference preference)
        {
            this._keyComparer = keyComparer == null ? Comparer<TKey>.Default : keyComparer;
            this._preference = preference;

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

        #endregion

        #region Methods

        public int Compare(TStep a, TStep b)
        {
            if (a != null && b == null) return -1;
            if (a == null && b != null) return 1;
            if (a == null && b == null) return 0;

            return this._comparison(a, b);
        }
        
        private int AverageComparison(TStep a, TStep b)
        {
            var keyComparing = _keyComparer.Compare(a.Key, b.Key); // H(x)
            var depthComparing = DistanceHelper.Int32Comparer.Compare(a.Depth, b.Depth); // G(x)
#if DEBUG
            Debug.WriteLine("({0}) A: {1}\t B: {2} K: {3} D: {4}", nameof(DiscreteHeuristicComparer<TKey, TStep>), a, b, keyComparing, depthComparing);
#endif
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

        #endregion
    }

    public enum HeuristicFunctionPreference
    {
        Average,

        HFirst,

        GFirst,
    }
}
