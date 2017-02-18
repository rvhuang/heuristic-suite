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
        public static readonly Func<TStep, IEnumerable<TStep>> DefaultNextStepFactory = step => Enumerable.Empty<TStep>();

        private Func<TStep, IEnumerable<TStep>> _nextStepsFactory;
        private Func<TStep, bool> _stepValidityChecker = DefaultStepValidityChecker;

        private IComparer<TKey> _comparer = Comparer<TKey>.Default;
        private IEqualityComparer<TKey> _equalityComparer = EqualityComparer<TKey>.Default;

        private HeuristicFunctionPreference preference;

        #endregion

        #region Properties
         
        public Func<TStep, IEnumerable<TStep>> NextStepsFactory
        {
            get { return this._nextStepsFactory; }
            set { this._nextStepsFactory = value == null ? DefaultNextStepFactory : value; }
        }

        public Func<TStep, bool> StepValidityChecker
        {
            get { return this._stepValidityChecker; }
            set { this._stepValidityChecker = value == null ? DefaultStepValidityChecker : value; }
        }

        public IComparer<TKey> Comparer
        {
            get { return this._comparer; }
            set { this._comparer = value == null ? Comparer<TKey>.Default : value; }
        }

        public IEqualityComparer<TKey> EqualityComparer
        {
            get { return this._equalityComparer; }
            set { this._equalityComparer = value == null ? EqualityComparer<TKey>.Default : value ; }
        }

        public HeuristicFunctionPreference HeuristicFunctionPreference
        {
            get { return this.preference; }
            set
            {
                if (Enum.IsDefined(typeof(HeuristicFunctionPreference), value))
                    this.preference = value;
                else
                    throw new ArgumentException("Not a defined value.", "HeuristicFunctionPreference");
            }
        }

        #endregion

        #region Constructor

        public AStar()
        { 
        }

        #endregion

        #region Methods
        
        public TStep Execute(TStep from, TStep goal)
        {
            return this.Execute(from, goal, this._nextStepsFactory, this._comparer, this._equalityComparer);
        }

        public TStep Execute(TStep from, TStep goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory)
        {
            return this.Execute(from, goal, nextStepsFactory, this._comparer, this._equalityComparer);
        }

        public TStep Execute(TStep from, TStep goal, IComparer<TKey> c)
        {
            return this.Execute(from, goal, this._nextStepsFactory, c, this._equalityComparer);
        }

        public TStep Execute(TStep from, TStep goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c)
        {
            return this.Execute(from, goal, nextStepsFactory, c, this._equalityComparer);
        }
        
        public TStep Execute(TStep from, TStep goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c, IEqualityComparer<TKey> ec)
        {
            if (from == null) throw new ArgumentNullException("from");
            if (goal == null) throw new ArgumentNullException("goal");
            if (nextStepsFactory == null) throw new ArgumentNullException("nextStepsFactory");

            var sc = new StepComparer<TKey, TStep>(c, this.preference);
            var open = new List<TStep>();
            var closed = new Dictionary<TKey, TStep>(ec);
            
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

                open.RemoveAt(0);
                closed.Add(current.Key, current);

                if (closed.Comparer.Equals(current.Key, goal.Key))
                    return current;

                foreach (var next in nextStepsFactory(current))
                {
                    if (closed.ContainsKey(next.Key)) continue;
                    if (!IsValidStep(next)) continue;
                    if (!open.Any(step => closed.Comparer.Equals(next.Key, step.Key)))
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