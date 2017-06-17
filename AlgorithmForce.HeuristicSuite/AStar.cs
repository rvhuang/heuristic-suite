﻿using System;
using System.Collections.Generic;
using System.Linq;

#if DEBUG
using System.Diagnostics;
#endif

namespace AlgorithmForce.HeuristicSuite
{
    public class AStar<TKey, TStep> : HeuristicSearch<TKey, TStep>
        where TStep : IStep<TKey>
    {
        #region Fields

        private SolutionFindingMode mode = SolutionFindingMode.DefaultIfNotFound;

        #endregion

        #region Properties

        public SolutionFindingMode FindingMode
        {
            get { return this.mode; }
            set
            {
                if (Enum.IsDefined(typeof(SolutionFindingMode), value))
                    this.mode = value;
                else
                    throw new ArgumentException("Not a defined value.", nameof(this.FindingMode));
            }
        }

        #endregion

        #region Constructor

        public AStar() { }

        #endregion

        #region Core

        protected override TStep ExecuteCore(TStep from, TStep goal, IHeuristicComparer<TKey, TStep> sc)
        {
            var open = new List<TStep>();
            var closed = new Dictionary<TKey, TStep>(base.EqualityComparer);

            open.Add(from);

            while (open.Any())
            {
#if DEBUG
                Debug.WriteLine("Open:");
                Debug.WriteLine(string.Join(Environment.NewLine, open));
                Debug.WriteLine("Closed:");
                Debug.WriteLine(string.Join(Environment.NewLine, closed.Values));
                Debug.WriteLine("-------");
#endif
                var current = open.First();
                var hasNext = false;

                if (base.EqualityComparer.Equals(current.Key, goal.Key))
                {
                    goal.Depth = current.Depth;
                    goal.PreviousStep = current.PreviousStep;

                    return current;
                }

                open.RemoveAt(0);
                closed.Add(current.Key, current);

                foreach (var next in base.NextStepsFactory(current))
                {
                    if (!IsValidStep(next)) continue;
                    if (closed.ContainsKey(next.Key)) continue;
                    if (!open.Any(step => base.EqualityComparer.Equals(next.Key, step.Key)))
                    {
                        next.PreviousStep = current;
                        next.Depth = current.Depth + 1;

                        open.Add(next);
                        hasNext = true;
                    }
                }
                if (hasNext) open.Sort(sc);
            }
            return HandleSolutionNotFound(closed, sc);
        }

        #endregion

        #region Others

        protected virtual TStep HandleSolutionNotFound(IDictionary<TKey, TStep> closed, IHeuristicComparer<TKey, TStep> sc)
        {
            switch (this.mode)
            {
                case SolutionFindingMode.DefaultIfNotFound:
                    return default(TStep);

                case SolutionFindingMode.ClosestIfNotFound:
                    return closed.OrderByDescending(kvp => kvp.Key, sc.KeyComparer).FirstOrDefault().Value;

                case SolutionFindingMode.LatestIfNotFound:
                    return closed.OrderBy(kvp => kvp.Value, sc).FirstOrDefault().Value;
            }
            return default(TStep);
        }

        #endregion

    }

    public class AStar<TKey> : AStar<TKey, Step<TKey>>
    {
        public AStar() { }

        public Step<TKey> Execute(TKey initKey, TKey goalKey)
        {
            return this.Execute(initKey, goalKey, Comparer<TKey>.Default);
        }

        public Step<TKey> Execute(TKey initKey, TKey goalKey, IComparer<TKey> comparer)
        {
            if (initKey == null) throw new ArgumentNullException(nameof(initKey));
            if (goalKey == null) throw new ArgumentNullException(nameof(goalKey));

            return base.Execute(new Step<TKey>(initKey), new Step<TKey>(goalKey), comparer);
        }
    }

    public enum SolutionFindingMode
    {
        DefaultIfNotFound,

        ClosestIfNotFound,

        LatestIfNotFound,
    }
}