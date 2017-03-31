using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IStep<TKey>
    {
        TKey Key { get; }

        bool IsValidStep { get; }

        int Depth { get; set; }

        IStep<TKey> PreviousStep { get; set; }
    }

    public interface INextStepFactory<TKey, TStep>
            where TStep : IStep<TKey>
    {
        IEnumerable<TStep> GetNextSteps();
    }
}