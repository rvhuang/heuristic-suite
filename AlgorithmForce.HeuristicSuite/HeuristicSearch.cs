using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        #endregion

        #region Methods

        public TStep Execute(TStep from, TStep goal)
        {
            return this.ExecuteCore(from, goal, Comparer<TKey>.Default);
        }

        public TStep ExecuteWith(TStep from, TKey goalState)
        {
            if (goalState == null) throw new ArgumentNullException("goalState");

            return this.ExecuteCore(from, new Step<TKey>(goalState), Comparer<TKey>.Default);
        }

        public TStep Execute(TStep from, TStep goal, IComparer<TKey> comparer)
        {
            return this.ExecuteCore(from, goal, comparer);
        }

        public TStep ExecuteWith(TStep from, TKey goalState, IComparer<TKey> comparer)
        {
            if (goalState == null) throw new ArgumentNullException("goalState");

            return this.ExecuteCore(from, new Step<TKey>(goalState), comparer);
        }

        #endregion

        #region To Be Implemented

        protected abstract TStep ExecuteCore(TStep from, IStep<TKey> goal, IComparer<TKey> c);

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

        public virtual IComparer<TStep> GetStepComparer(IComparer<TKey> c)
        {
            if (c is IGoalOrientedComparer<TKey>)
                return new GoalOrientedStepComparer<TKey, TStep>(c as IGoalOrientedComparer<TKey>, this.preference);
            else
                return new StepComparer<TKey, TStep>(c, this.preference);
        }

        #endregion
    }
}
