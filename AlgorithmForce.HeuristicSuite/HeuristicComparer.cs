using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public abstract class HeuristicComparer<TKey> : IHeuristicComparer<TKey>
    {
        public HeuristicFunctionPreference Preference { get; set; }

        public TKey Goal { get; private set; }

        protected HeuristicComparer(TKey goal)
        {
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            this.Goal = goal;
        }

        public int Compare(TKey x, TKey y)
        {
            return Comparer<double>.Default.Compare(this.EstimateH(x), this.EstimateH(y));
        }

        public int Compare(IStep<TKey> x, IStep<TKey> y)
        {
            return this.Compare(x, y, this.Preference);
        }

        public abstract double EstimateH(TKey key);
    }
}
