using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AlgorithmForce.HeuristicSuite
{
    public class AStar<TKey, TStep>
        where TStep : IStep<TKey>
    {
        #region Fields

        public static readonly Func<TStep, IEnumerable<TStep>> DefaultNextStepFactory;
        public static readonly Func<TStep, bool> DefaultStepValidityChecker;

        private Func<TStep, IEnumerable<TStep>> _nextStepsFactory = DefaultNextStepFactory;
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

        static AStar()
        {
            if (typeof(INextStepFactory<TKey, TStep>).GetTypeInfo().IsAssignableFrom(typeof(TStep)))
                DefaultNextStepFactory = step => (step as INextStepFactory<TKey, TStep>).GetNextSteps();
            else
                DefaultNextStepFactory = step => Enumerable.Empty<TStep>();

            DefaultStepValidityChecker = step => step.IsValidStep;
        }

        public AStar()
        {
        }

        #endregion

        #region Execute 

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
            return this.ExecuteCore(from, goal, nextStepsFactory, c, ec, false);
        }

        #endregion

        #region Find Closest

        public TStep FindClosest(TStep from, TStep goal)
        {
            return this.FindClosest(from, goal, this._nextStepsFactory, this._comparer, this._equalityComparer);
        }

        public TStep FindClosest(TStep from, TStep goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory)
        {
            return this.FindClosest(from, goal, nextStepsFactory, this._comparer, this._equalityComparer);
        }

        public TStep FindClosest(TStep from, TStep goal, IComparer<TKey> c)
        {
            return this.FindClosest(from, goal, this._nextStepsFactory, c, this._equalityComparer);
        }

        public TStep FindClosest(TStep from, TStep goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c)
        {
            return this.FindClosest(from, goal, nextStepsFactory, c, this._equalityComparer);
        }

        public TStep FindClosest(TStep from, TStep goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c, IEqualityComparer<TKey> ec)
        {
            return this.ExecuteCore(from, goal, nextStepsFactory, c, ec, true);
        }

        #endregion

        #region Core

        private TStep ExecuteCore(TStep from, TStep goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c, IEqualityComparer<TKey> ec, bool closestIfNoSolution)
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

                if (closed.Comparer.Equals(current.Key, goal.Key))
                    return current;

                open.RemoveAt(0);
                closed.Add(current.Key, current);

                foreach (var next in nextStepsFactory(current))
                {
                    if (!IsValidStep(next)) continue;
                    if (closed.ContainsKey(next.Key)) continue;
                    if (!open.Any(step => closed.Comparer.Equals(next.Key, step.Key)))
                    {
                        next.PreviousStep = current;
                        next.Depth = current.Depth + 1;

                        open.Add(next);
                    }
                }
                open.Sort(sc);
            }

            return closestIfNoSolution ? closed.OrderBy(kvp => kvp.Key, c).FirstOrDefault().Value : default(TStep); // no solution
        }

        #endregion

        #region Others

        public bool IsValidStep(TStep step)
        {
            return step != null && step.IsValidStep && this._stepValidityChecker(step);
        }

        #endregion
    }

    public class AStar<TStep> : AStar<TStep, TStep>
        where TStep : IStep<TStep>
    {

    }
}