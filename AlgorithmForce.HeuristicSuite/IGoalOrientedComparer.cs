using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public interface IGoalOrientedComparer<T> : IComparer<T>
    {
        T Goal { get; }

        double GetScore(T obj);
    }
}
