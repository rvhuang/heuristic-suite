using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class HeuristicComparer<TKey, TStep> : Comparer<TStep>, IHeuristicComparer<TKey, TStep>
        where TStep : IStep<TKey>
    {
        #region Fields

        private readonly Func<TKey, double> estimation;
        private readonly HeuristicFunctionPreference preference;
        private readonly IComparer<TKey> keyComparer;

        #endregion

        #region Properties

        public HeuristicFunctionPreference Preference
        {
            get { return this.preference; }
        }

        public IComparer<TKey> KeyComparer
        {
            get { return this.keyComparer; }
        }

        #endregion

        #region Constructor

        public HeuristicComparer(Func<TKey, double> estimation, HeuristicFunctionPreference preference)
        {
            this.estimation = estimation ?? throw new ArgumentNullException(nameof(estimation));
            this.preference = preference;
            this.keyComparer = Comparer<TKey>.Create(this.KeyComparison);
        }

        #endregion

        #region Method

        public double Estimate(TKey key)
        {
            return this.estimation(key);
        }

        #endregion

        #region Comparer<T> Method

        public override int Compare(TStep x, TStep y)
        {
            return Compare(x, y, this.estimation, this.preference);
        }

        protected int KeyComparison(TKey x, TKey y)
        {
            return DistanceHelper.DoubleComparer.Compare(estimation(x), estimation(y));
        }

        #endregion

        #region Others

        private static int Compare(IStep<TKey> a, IStep<TKey> b, Func<TKey, double> estimation,
            HeuristicFunctionPreference preference)
        {
            if (a != null && b == null) return -1;
            if (a == null && b != null) return 1;
            if (a == null && b == null) return 0;

            var hValueA = estimation(a.Key);
            var hValueB = estimation(b.Key);
            var estimationA = hValueA + a.Depth; // H(n) + G(n)
            var estimationB = hValueB + b.Depth;

            var result = DistanceHelper.DoubleComparer.Compare(estimationA, estimationB);

            if (result != 0) return result;

            switch (preference)
            {
                case HeuristicFunctionPreference.GFirst:
                    return DistanceHelper.Int32Comparer.Compare(a.Depth, b.Depth);

                case HeuristicFunctionPreference.HFirst:
                    return DistanceHelper.DoubleComparer.Compare(hValueA, hValueB);
            }
            return result;
        }

        #endregion
    }
}