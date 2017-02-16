using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IStep<TKey, TStep> 
    {
        TKey Key { get; }

        bool IsValidStep { get; }

        IStep<TKey, TStep> PreviousStep { get; set; }
    }
}
