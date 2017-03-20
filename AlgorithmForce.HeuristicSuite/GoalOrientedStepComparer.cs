using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    class GoalOrientedStepComparer<TKey, TStep> : Comparer<TStep>, IComparer<IStep<TKey>>
         where TStep : IStep<TKey>
    {
        private readonly IGoalOrientedComparer<TKey> _comparer;
        private readonly HeuristicFunctionPreference _preference;

        public IGoalOrientedComparer<TKey> KeyComparer
        {
            get { return this._comparer; }
        }

        public GoalOrientedStepComparer(IGoalOrientedComparer<TKey> comparer, HeuristicFunctionPreference preference)
        {
            this._comparer = comparer;
            this._preference = preference;
        }

        public override int Compare(TStep a, TStep b)
        {
            return (this as IComparer<IStep<TKey>>).Compare(a, b);
        }

        int IComparer<IStep<TKey>>.Compare(IStep<TKey> a, IStep<TKey> b)
        {
            var scoreA = this._comparer.GetScore(a.Key);
            var scoreB = this._comparer.GetScore(b.Key);
            var comparing = DistanceHelper.DoubleComparer.Compare(scoreA + a.Depth, scoreB + b.Depth);

            if (comparing != 0)
                return comparing;

            switch (this._preference)
            {
                case HeuristicFunctionPreference.GFirst:
                    return DistanceHelper.Int32Comparer.Compare(a.Depth, b.Depth);

                case HeuristicFunctionPreference.HFirst:
                    return DistanceHelper.DoubleComparer.Compare(scoreA, scoreB);
            }
            return 0;
        }
    }
}
