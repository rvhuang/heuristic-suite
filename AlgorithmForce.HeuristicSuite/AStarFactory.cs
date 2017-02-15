using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class AStarFactory
    {
        public static AStar<TStep> Create<TStep>()
            where TStep : IStep<TStep>, IEquatable<TStep>, IComparable<TStep>
        {
            return new AStar<TStep>();
        }

        public static AStar<TStep> Create<TStep>(IEqualityComparer<TStep> ec)
            where TStep : IStep<TStep>, IComparable<TStep>
        {
            if (ec == null) throw new ArgumentNullException("ec");

            return new AStar<TStep>(ec);
        }

        public static AStar<TStep> Create<TStep>(IComparer<TStep> c)
            where TStep : IStep<TStep>, IEquatable<TStep>
        {
            if (c == null) throw new ArgumentNullException("c");

            return new AStar<TStep>(c);
        }

        public static AStar<TStep> Create<TStep>(IComparer<TStep> c, IEqualityComparer<TStep> ec)
            where TStep : IStep<TStep>
        {
            if (c == null) throw new ArgumentNullException("c");
            if (ec == null) throw new ArgumentNullException("ec");

            return new AStar<TStep>(c, ec);
        }
    }
}
