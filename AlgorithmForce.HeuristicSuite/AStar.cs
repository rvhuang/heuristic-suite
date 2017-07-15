using System;
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

        #region Methods

        public AStarSolutionDetail<TKey, TStep> FindSolutionDetail(TStep from, TStep goal)
        {
            return this.FindSolutionDetail(from, goal, Comparer<TKey>.Default);
        }

        public AStarSolutionDetail<TKey, TStep> FindSolutionDetail(TStep from, TStep goal, IComparer<TKey> comparer)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            return this.ExecuteDetailed(from, goal, new DiscreteHeuristicComparer<TKey, TStep>(comparer, base.HeuristicFunctionPreference));
        }

        public AStarSolutionDetail<TKey, TStep> FindSolutionDetail(TStep from, TStep goal, Func<TKey, double> estimation)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            return this.ExecuteDetailed(from, goal, new HeuristicComparer<TKey, TStep>(estimation, base.HeuristicFunctionPreference));
        }

        public AStarSolutionDetail<TKey, TStep> FindSolutionDetail(TStep from, TStep goal, Func<TKey, TKey, double> estimationFromGoal)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            return this.ExecuteDetailed(from, goal, new HeuristicComparer<TKey, TStep>((key) => estimationFromGoal(key, goal.Key), base.HeuristicFunctionPreference));
        }

        #endregion

        #region Core

        protected override TStep ExecuteCore(TStep from, TStep goal, IHeuristicComparer<TKey, TStep> sc)
        {
            var open = new List<TStep>();
            var closed = new Dictionary<TKey, TStep>(base.EqualityComparer);

            open.Add(from);

            while (open.Count > 0)
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

        protected AStarSolutionDetail<TKey, TStep> ExecuteDetailed(TStep from, TStep goal, IHeuristicComparer<TKey, TStep> sc)
        {
            var open = new List<TStep>();
            var closed = new Dictionary<TKey, TStep>(base.EqualityComparer);

            open.Add(from);

            while (open.Count > 0)
            {
                var current = open.First();
                var hasNext = false;

                if (base.EqualityComparer.Equals(current.Key, goal.Key))
                {
                    goal.Depth = current.Depth;
                    goal.PreviousStep = current.PreviousStep;

                    return AStarSolutionDetail<TKey, TStep>.Create(from, current, open, closed.Values);
                }

                open.RemoveAt(0);
                OnRemovedFromOpenList(open, closed.Values, current);

                closed.Add(current.Key, current);
                OnAddedToClosedList(open, closed.Values, current);

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

                        OnAddedToOpenList(open, closed.Values, next);
                    }
                }
                if (hasNext) open.Sort(sc);
            }
            return AStarSolutionDetail<TKey, TStep>.Create(from, HandleSolutionNotFound(closed, sc), open, closed.Values);
        }

        #endregion

        #region Monitors 

#if PORTABLE 
        protected virtual void OnAddedToOpenList(IEnumerable<TStep> open, IEnumerable<TStep> closed, TStep step) { }

        protected virtual void OnRemovedFromOpenList(IEnumerable<TStep> open, IEnumerable<TStep> closed, TStep step) { }

        protected virtual void OnAddedToClosedList(IEnumerable<TStep> open, IEnumerable<TStep> closed, TStep step) { }
#else
        protected virtual void OnAddedToOpenList(IReadOnlyCollection<TStep> open, IReadOnlyCollection<TStep> closed, TStep step) { }

        protected virtual void OnRemovedFromOpenList(IReadOnlyCollection<TStep> open, IReadOnlyCollection<TStep> closed, TStep step) { }

        protected virtual void OnAddedToClosedList(IReadOnlyCollection<TStep> open, IReadOnlyCollection<TStep> closed, TStep step) { }
#endif

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

        public Step<TKey> FindSolution(TKey initKey, TKey goalKey)
        {
            return this.FindSolution(initKey, goalKey, Comparer<TKey>.Default);
        }

        public Step<TKey> FindSolution(TKey initKey, TKey goalKey, IComparer<TKey> comparer)
        {
            if (initKey == null) throw new ArgumentNullException(nameof(initKey));
            if (goalKey == null) throw new ArgumentNullException(nameof(goalKey));

            return base.FindSolution(new Step<TKey>(initKey), new Step<TKey>(goalKey), comparer);
        }
    }

    public sealed class AStarSolutionDetail<TKey, TStep>
        where TStep : IStep<TKey>
    {
        #region Properties

        public TStep From { get; private set; }

        public TStep Goal { get; private set; }

        public IReadOnlyCollection<TStep> OpenList { get; private set; }

        public IReadOnlyCollection<TStep> ClosedList { get; private set; }

        #endregion

        #region Constructor

        internal AStarSolutionDetail() { }

        #endregion

        #region Others

#if PORTABLE
        internal static AStarSolutionDetail<TKey, TStep> Create(TStep from, TStep goal,
            ICollection<TStep> open, ICollection<TStep> closed)
        {
            return new AStarSolutionDetail<TKey, TStep>()
            {
                From = from,
                Goal = goal,
                OpenList = open.ToArray(),
                ClosedList = open.ToArray()
            };
        }
#else
        internal static AStarSolutionDetail<TKey, TStep> Create(TStep from, TStep goal, 
            IReadOnlyCollection<TStep> open, IReadOnlyCollection<TStep> closed)
        {
            return new AStarSolutionDetail<TKey, TStep>()
            {
                From = from,
                Goal = goal,
                OpenList = open,
                ClosedList = closed
            };
        }
#endif

        #endregion
    }

    public enum SolutionFindingMode
    {
        DefaultIfNotFound,

        ClosestIfNotFound,

        LatestIfNotFound,
    }
}