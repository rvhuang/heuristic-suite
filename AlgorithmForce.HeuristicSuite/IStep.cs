using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IStep<out TStep>
    {
        bool IsValidStep { get; }

        IEnumerable<TStep> GetNextSteps();
    }
}
