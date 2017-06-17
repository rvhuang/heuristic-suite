using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if DEBUG
using System.Diagnostics;
#endif

namespace AlgorithmForce.HeuristicSuite
{
    public abstract class HeuristicSearch<TKey, TStep>
        where TStep : IStep<TKey>
    {
        #region Fields

        public static readonly Func<TStep, IEnumerable<TStep>> DefaultNextStepFactory;
        public static readonly Func<TStep, bool> DefaultStepValidityChecker;

        private Func<TStep, IEnumerable<TStep>> _nextStepsFactory = DefaultNextStepFactory;
        private Func<TStep, bool> _stepValidityChecker = DefaultStepValidityChecker;

        private IEqualityComparer<TKey> _equalityComparer = EqualityComparer<TKey>.Default;
        private HeuristicFunctionPreference preference = HeuristicFunctionPreference.Average;

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

        #endregion

        #region Constructor

        static HeuristicSearch()
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

        protected HeuristicSearch() { }

        #endregion

        #region Methods

        public TStep Execute(TStep from, TStep goal)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            return this.ExecuteCore(from, goal, new StepComparer<TKey, TStep>(Comparer<TKey>.Default, this.preference));
        }
        
        public TStep Execute(TStep from, TStep goal, IComparer<TKey> comparer)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            return this.ExecuteCore(from, goal, new StepComparer<TKey, TStep>(comparer, this.preference));
        }

        public TStep Execute(TStep from, TStep goal, Func<TKey, double> estimation)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            return this.ExecuteCore(from, goal, new HeuristicComparer<TKey, TStep>(estimation, this.preference));
        }

        public TStep Execute(TStep from, TStep goal, Func<TKey, TKey, double> estimationFromGoal)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (goal == null) throw new ArgumentNullException(nameof(goal));

            return this.ExecuteCore(from, goal, new HeuristicComparer<TKey, TStep>((key) => estimationFromGoal(key, goal.Key), this.preference));
        }

        #endregion

        #region To Be Implemented

        protected abstract TStep ExecuteCore(TStep from, TStep goal, IHeuristicComparer<TKey, TStep> c);

        #endregion

        #region Others

        public bool IsValidStep(TStep step)
        {
            if (step == null)
                return false;

            if (step.PreviousStep != null && this._equalityComparer.Equals(step.Key, step.PreviousStep.Key))
                return false;

            if (!step.IsValidStep)
                return false;

            if (!this._stepValidityChecker(step))
                return false;

            return true;
        }
        
        #endregion

        #region Recursion State

        protected class RecursionState
        {
            private readonly HeuristicSearch<TKey, TStep> owner;
            private readonly IComparer<TStep> sc;
            private readonly ISet<TKey> visited;

            public IComparer<TStep> StepComparer
            {
                get { return this.sc; }
            }

            public RecursionState(HeuristicSearch<TKey, TStep> owner, IComparer<TStep> sc)
            {
                this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
                this.sc = sc ?? Comparer<TStep>.Default;
                this.visited = new HashSet<TKey>(owner.EqualityComparer);
            }

            public IEnumerable<TStep> GetNextSteps(TStep node)
            {
                foreach (var succ in this.owner.NextStepsFactory(node))
                {
                    if (!owner.IsValidStep(succ)) continue;
                    if (!this.visited.Add(succ.Key)) continue;
#if DEBUG
                    Debug.WriteLine("({0}) Depth: {1}\t Step: {2}\t", nameof(GetNextSteps), node.Depth + 1, succ.Key);
#endif
                    succ.Depth = node.Depth + 1;
                    succ.PreviousStep = node;

                    yield return succ;
                }
            }
        }

        protected enum RecursionFlag
        {
            Found,

            InProgress,

            NotFound,
        }

        protected struct RecursionResult
        {
            public RecursionFlag Flag
            {
                get; private set;
            }

            public TStep Step
            {
                get; private set;
            }

            public static RecursionResult Create(RecursionFlag flag, TStep step)
            {
#if DEBUG
                Debug.WriteLine("({0}) Step: {1}\t Prev: {2}\t Flag: {3}",
                    nameof(RecursionResult), step, step != null ? step.PreviousStep : step, flag);
#endif
                return new RecursionResult() { Flag = flag, Step = step };
            }

            public override string ToString()
            {
                return string.Format("Step: {0}\t Flag: {1}", this.Step, this.Flag);
            }
        }

        #endregion
    }
}