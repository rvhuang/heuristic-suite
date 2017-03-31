using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class HeuristicComparer<TKey> : IComparer<TKey>, IComparer<IStep<TKey>>
    {
        #region Fields

        private readonly Func<TKey, double> estimationH;

        #endregion

        #region Properties

        public HeuristicFunctionPreference Preference
        {
            get; set;
        }

        #endregion

        public HeuristicComparer(Func<TKey, double> estimationH)
        {
            if (estimationH == null) throw new ArgumentNullException(nameof(estimationH));

            this.estimationH = estimationH;
        }

        public int Compare(TKey x, TKey y)
        {
            return Comparer<double>.Default.Compare(this.estimationH(x), this.estimationH(y));
        }

        public int Compare(IStep<TKey> x, IStep<TKey> y)
        {
            return ComparerHelper.Compare(x, y, this.estimationH, this.Preference);
        }
    }
}