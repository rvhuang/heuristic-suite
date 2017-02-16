using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.HeuristicSuite
{
    public class AStar<TKey, TStep>
        where TStep : IStep<TKey, TStep>
    {
        #region Fields

        public static readonly Func<TStep, bool> DefaultStepValidityChecker = step => step.IsValidStep;

        private readonly IComparer<TKey> _c;
        private readonly IEqualityComparer<TKey> _ec;
        private readonly Func<TStep, IEnumerable<TStep>> _nextStepsFactory;

        private Func<TStep, bool> _stepValidityChecker = DefaultStepValidityChecker;

        #endregion

        #region Properties

        public IComparer<TKey> Comparer
        {
            get { return this._c; }
        }

        public IEqualityComparer<TKey> EqualityComparer
        {
            get { return this._ec; }
        }

        public Func<TStep, IEnumerable<TStep>> NextStepsFactory
        {
            get { return this._nextStepsFactory; }
        }

        public Func<TStep, bool> StepValidityChecker // This is optional
        {
            get { return this._stepValidityChecker; }
            set { this._stepValidityChecker = value == null ? DefaultStepValidityChecker : value; }
        }

        #endregion

        #region Constructor

        internal AStar(Func<TStep, IEnumerable<TStep>> nextStepsFactory)
            : this(nextStepsFactory, Comparer<TKey>.Default, EqualityComparer<TKey>.Default)
        {
        }

        internal AStar(Func<TStep, IEnumerable<TStep>> nextStepsFactory, IEqualityComparer<TKey> ec)
            : this(nextStepsFactory, Comparer<TKey>.Default, ec)
        {
        }

        internal AStar(Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c)
            : this(nextStepsFactory, c, EqualityComparer<TKey>.Default)
        {
        }

        internal AStar(Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c, IEqualityComparer<TKey> ec)
        {
            this._nextStepsFactory = nextStepsFactory;
            this._c = c;
            this._ec = ec;
        }

        #endregion

        #region Methods

        public TStep Execute(TStep startAt, TStep goal)
        {
            var open = new SortedList<TKey, TStep>(this._c);
            var closed = new StepCollection<TKey, TStep>(this._ec);

            open.Add(goal.Key, goal);

            while (open.Count > 0)
            {
                var current = open.First().Value;

                closed.Add(current);

                if (this._ec.Equals(current.Key, goal.Key))
                    return current;

                foreach (var next in this._nextStepsFactory(current))
                {
                    if (!IsValidStep(next)) continue;
                    if (closed.Contains(next)) continue;

                    next.PreviousStep = current;

                    var prior = default(TStep);
                    //                                            next has better score
                    if (!open.TryGetValue(next.Key, out prior) || this._c.Compare(next.Key, prior.Key) < 0)
                    {
                        if (prior != null)
                            open.Remove(prior.Key);

                        open.Add(next.Key, next);
                    }
                }
            }
            return default(TStep); // no solution
        }

        public bool IsValidStep(TStep step)
        {
            return step != null && this._stepValidityChecker(step);
        }

        #endregion
    }
}