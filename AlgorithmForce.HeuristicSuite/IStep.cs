using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IStep<TKey, TStep>
    {
        TKey Key { get; }

        bool IsValidStep { get; }

        int Depth { get; set; }

        IStep<TKey, TStep> PreviousStep { get; set; }
    }
}