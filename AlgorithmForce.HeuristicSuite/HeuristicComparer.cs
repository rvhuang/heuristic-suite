using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class HeuristicComparer<TKey> : IHeuristicComparer<TKey>
    {
        #region Fields

        private readonly TKey goal;
        private readonly Func<TKey, double> estimationH;

        #endregion

        #region Properties

        public HeuristicFunctionPreference Preference
        {
            get; set;
        }

        public TKey Goal
        {
            get { return this.goal; }
        }

        #endregion

        public HeuristicComparer(TKey goal, Func<TKey, double> estimationH)
        {
            if (goal == null) throw new ArgumentNullException(nameof(goal));
            if (estimationH == null) throw new ArgumentNullException(nameof(estimationH));

            this.goal = goal;
            this.estimationH = estimationH;
        }
        
        public int Compare(TKey x, TKey y)
        {
            return Comparer<double>.Default.Compare(this.EstimateH(x), this.EstimateH(y));
        }

        public int Compare(IStep<TKey> x, IStep<TKey> y)
        {
            return this.Compare(x, y, this.Preference);
        }

        public double EstimateH(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return this.estimationH(key);
        }
    }
}
