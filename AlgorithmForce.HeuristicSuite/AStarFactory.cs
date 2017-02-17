using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class AStarFactory
    {
        public static AStar<TKey, TStep> Create<TKey, TStep>()
            where TKey : IEquatable<TKey>, IComparable<TKey>
            where TStep : IStep<TKey, TStep>
        {
            return new AStar<TKey, TStep>();
        }

        public static AStar<TKey, TStep> Create<TKey, TStep>(IEqualityComparer<TKey> ec)
            where TKey : IComparable<TKey>
            where TStep : IStep<TKey, TStep>
        {
            if (ec == null) throw new ArgumentNullException("ec");

            return new AStar<TKey, TStep>( ec);
        }

        public static AStar<TKey, TStep> Create<TKey, TStep>(IComparer<TKey> c)
            where TKey : IEquatable<TKey>
            where TStep : IStep<TKey, TStep>
        {
            if (c == null) throw new ArgumentNullException("c");

            return new AStar<TKey, TStep>( c);
        }

        public static AStar<TKey, TStep> Create<TKey, TStep>(IComparer<TKey> c, IEqualityComparer<TKey> ec)
            where TStep : IStep<TKey, TStep>
        {
            if (c == null) throw new ArgumentNullException("c");
            if (ec == null) throw new ArgumentNullException("ec");

            return new AStar<TKey, TStep>( c, ec);
        }
    }
}