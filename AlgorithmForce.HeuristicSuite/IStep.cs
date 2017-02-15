using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IStep : IEquatable<IStep>, IComparable<IStep>
    {
        bool IsValidStep { get; }

        IEnumerable<TStep> GetNextSteps<TStep>();
    }
}
