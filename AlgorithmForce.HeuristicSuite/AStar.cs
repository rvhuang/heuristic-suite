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
            if (startAt == null)
                throw new ArgumentNullException("startAt");

            if (goal == null)
                throw new ArgumentNullException("goal");

            if (this._nextStepsFactory == null)
                throw new InvalidOperationException("Property NextStepFactory is null.");

            var sc = new StepComparer<TKey, TStep>(this._c, StepComparerMode.DepthFirst);
            var open = new List<TStep>();
            var closed = new Dictionary<TKey, TStep>(this._ec);

            open.Add(startAt);

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

                open.RemoveAt(0);
                closed.Add(current.Key, current);

                if (this._ec.Equals(current.Key, goal.Key))
                    return current;

                foreach (var next in this._nextStepsFactory(current))
                {
                    if (closed.ContainsKey(next.Key)) continue;
                    if (!IsValidStep(next)) continue;
                    if (!open.Any(step => this._ec.Equals(next.Key, step.Key)))
                    {
                        next.PreviousStep = current;
                        next.Depth = current.Depth + 1;
                        
                        open.Add(next);
                    }
                }
                open.Sort(sc);
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