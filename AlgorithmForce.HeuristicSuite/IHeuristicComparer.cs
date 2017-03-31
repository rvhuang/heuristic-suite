using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IHeuristicComparer<TKey> : IComparer<TKey>, IComparer<IStep<TKey>>
    {
        HeuristicFunctionPreference Preference { get; set; }

        TKey Goal { get; }

        double EstimateH(TKey key);
    }
}