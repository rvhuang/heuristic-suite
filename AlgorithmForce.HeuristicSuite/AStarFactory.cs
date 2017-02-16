using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class AStarFactory
    {
        public static AStar<TKey, TStep> Create<TKey, TStep>(Func<TStep, IEnumerable<TStep>> nextStepsFactory)
            where TKey : IEquatable<TKey>, IComparable<TKey>
            where TStep : IStep<TKey, TStep>
        {
            if (nextStepsFactory == null) throw new ArgumentNullException("nextStepsFactory");

            return new AStar<TKey, TStep>(nextStepsFactory);
        }

        public static AStar<TKey, TStep> Create<TKey, TStep>(Func<TStep, IEnumerable<TStep>> nextStepsFactory, IEqualityComparer<TKey> ec)
            where TKey : IComparable<TKey>
            where TStep : IStep<TKey, TStep>
        {
            if (nextStepsFactory == null) throw new ArgumentNullException("nextStepsFactory");
            if (ec == null) throw new ArgumentNullException("ec");

            return new AStar<TKey, TStep>(nextStepsFactory, ec);
        }

        public static AStar<TKey, TStep> Create<TKey, TStep>(Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c)
            where TKey : IEquatable<TKey>
            where TStep : IStep<TKey, TStep>
        {
            if (nextStepsFactory == null) throw new ArgumentNullException("nextStepsFactory");
            if (c == null) throw new ArgumentNullException("c");

            return new AStar<TKey, TStep>(nextStepsFactory, c);
        }

        public static AStar<TKey, TStep> Create<TKey, TStep>(Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c, IEqualityComparer<TKey> ec)
            where TStep : IStep<TKey, TStep>
        {
            if (nextStepsFactory == null) throw new ArgumentNullException("nextStepsFactory");
            if (c == null) throw new ArgumentNullException("c");
            if (ec == null) throw new ArgumentNullException("ec");

            return new AStar<TKey, TStep>(nextStepsFactory, c, ec);
        }
    }
}