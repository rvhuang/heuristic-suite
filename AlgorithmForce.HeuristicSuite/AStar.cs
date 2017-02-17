using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Func<TStep, IEnumerable<TStep>> _nextStepsFactory;
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
            set { this._nextStepsFactory = value; }
        }

        public Func<TStep, bool> StepValidityChecker // This is optional
        {
            get { return this._stepValidityChecker; }
            set { this._stepValidityChecker = value == null ? DefaultStepValidityChecker : value; }
        }

        #endregion

        #region Constructor

        internal AStar()
            : this(Comparer<TKey>.Default, EqualityComparer<TKey>.Default)
        {
        }

        internal AStar(IEqualityComparer<TKey> ec)
            : this(Comparer<TKey>.Default, ec)
        {
        }

        internal AStar(IComparer<TKey> c)
            : this(c, EqualityComparer<TKey>.Default)
        {
        }

        internal AStar(IComparer<TKey> c, IEqualityComparer<TKey> ec)
        {
            this._c = c;
            this._ec = ec;
        }

        #endregion

        #region Methods

        public TStep Execute(TStep startAt, TStep goal)
        {
            if (this._nextStepsFactory == null)
                throw new InvalidOperationException("Property NextStepFactory is null.");

            var open = new SortedList<TKey, TStep>(this._c);
            var closed = new Dictionary<TKey, TStep>(this._ec);

            open.Add(startAt.Key, startAt);

            while (open.Count > 0)
            {
#if DEBUG
                Debug.WriteLine("Open:");
                Debug.WriteLine(string.Join(Environment.NewLine, open));
                Debug.WriteLine("Closed:");
                Debug.WriteLine(string.Join(Environment.NewLine, closed));
                Debug.WriteLine("-------");
#endif
                var current = open.First().Value;

                open.Remove(current.Key);
                closed.Add(current.Key, current);

                if (this._ec.Equals(current.Key, goal.Key))
                    return current;

                foreach (var next in this._nextStepsFactory(current))
                {
                    if (!IsValidStep(next)) continue;
                    if (closed.ContainsKey(next.Key)) continue;

                    next.PreviousStep = current;

                    var prior = default(TStep);
                    //                                            score is updated and better than prior.
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
            return step != null && step.IsValidStep && this._stepValidityChecker(step);
        }

        #endregion
    }
}