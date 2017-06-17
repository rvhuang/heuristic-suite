using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IHeuristicComparer<TKey, TStep> : IComparer<TStep>
        where TStep : IStep<TKey>
    {
        HeuristicFunctionPreference Preference { get; }

        IComparer<TKey> KeyComparer { get; }
    }
}