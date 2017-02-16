using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlgorithmForce.HeuristicSuite
{
    class StepCollection<TKey, TStep> : KeyedCollection<TKey, TStep> 
        where TStep : IStep<TKey, TStep>
    {
        public StepCollection(IEqualityComparer<TKey> ec)
            : base(ec)
        {
        }

        protected override TKey GetKeyForItem(TStep item)
        {
            return item.Key;
        }
    }
}