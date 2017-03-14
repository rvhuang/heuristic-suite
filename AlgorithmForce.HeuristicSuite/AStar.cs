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

        private HeuristicFunctionPreference preference = HeuristicFunctionPreference.Average;
        private SolutionFindingMode mode = SolutionFindingMode.DefaultIfNotFound;

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
            set { this._equalityComparer = value == null ? EqualityComparer<TKey>.Default : value; }
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

        public SolutionFindingMode FindingMode
        {
            get { return this.mode; }
            set
            {
                if (Enum.IsDefined(typeof(SolutionFindingMode), value))
                    this.mode = value;
                else
                    throw new ArgumentException("Not a defined value.", "HeuristicFunctionPreference");
            }
        }

        #endregion

        #region Constructor

        static AStar()
        {
#if PORTABLE 
            if (typeof(INextStepFactory<TKey, TStep>).GetTypeInfo().IsAssignableFrom(typeof(TStep).GetTypeInfo()))
                DefaultNextStepFactory = step => (step as INextStepFactory<TKey, TStep>).GetNextSteps();
            else
                DefaultNextStepFactory = step => Enumerable.Empty<TStep>();
#else
            if (typeof(INextStepFactory<TKey, TStep>).GetTypeInfo().IsAssignableFrom(typeof(TStep)))
                DefaultNextStepFactory = step => (step as INextStepFactory<TKey, TStep>).GetNextSteps();
            else
                DefaultNextStepFactory = step => Enumerable.Empty<TStep>();
#endif
            DefaultStepValidityChecker = step => step.IsValidStep;
        }

        public AStar()
        {
        }

        #endregion

        #region Execute 

        public TStep Execute(TStep from, TStep goal)
        {
            return this.ExecuteCore(from, goal, this._nextStepsFactory, this._comparer, this._equalityComparer, this.mode);
        }

        public TStep ExecuteWith(TStep from, TKey goalState)
        {
            if (goalState == null) throw new ArgumentNullException("goalState");

            return this.ExecuteCore(from, new Step<TKey>(goalState), this._nextStepsFactory, this._comparer, this._equalityComparer, this.mode);
        }

        #endregion

        #region Core

        private TStep ExecuteCore(TStep from, IStep<TKey> goal, Func<TStep, IEnumerable<TStep>> nextStepsFactory, IComparer<TKey> c, IEqualityComparer<TKey> ec, SolutionFindingMode mode)
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
                {
                    if (goal is TStep)
                    {
                        goal.Depth = current.Depth;
                        goal.PreviousStep = current.PreviousStep;

                        return (TStep)goal;
                    }
                    return current;
                }

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
            switch (mode)
            {
                case SolutionFindingMode.DefaultIfNotFound:
                    return default(TStep);

                case SolutionFindingMode.ClosestIfNotFound:
                    return closed.OrderBy(kvp => kvp.Key, sc.KeyComparer).FirstOrDefault().Value;

                case SolutionFindingMode.LatestIfNotFound:
                    return closed.LastOrDefault().Value;
            }
            return default(TStep);
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

    public enum SolutionFindingMode
    {
        DefaultIfNotFound,

        ClosestIfNotFound,

        LatestIfNotFound,
    }
}