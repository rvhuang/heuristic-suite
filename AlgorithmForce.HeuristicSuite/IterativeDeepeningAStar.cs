using System;
using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class IterativeDeepeningAStar<TKey, TStep> : HeuristicSearch<TKey, TStep>
        where TStep : IStep<TKey>
    {
        #region Fields

        private int max = 1024;

        #endregion

        #region Properties

        public int MaxNumberOfLoopTimes
        {
            get { return this.max; }
            set { this.max = value; }
        }

        #endregion

        #region Constructor

        public IterativeDeepeningAStar()
        {
            base.HeuristicFunctionPreference = HeuristicFunctionPreference.HFirst;
        }

        #endregion

        #region Override

        protected override TStep ExecuteCore(TStep from, TStep goal, IHeuristicComparer<TKey, TStep> sc)
        {
            var counter = 0;
            var bound = from;
            
            while (counter <= max)
            {
                var t = Search(from, bound, goal, new RecursionState(this, sc));

                if (t.Flag == RecursionFlag.Found) return t.Step;
                if (t.Flag == RecursionFlag.NotFound) return default(TStep);

                bound = t.Step;
                counter++;
            }
            return default(TStep);
        }

        #endregion
        
        #region Core

        private RecursionResult Search(TStep node, TStep bound, TStep goal, RecursionState state)
        {
            if (state.StepComparer.Compare(node, bound) > 0)
                return RecursionResult.Create(RecursionFlag.InProgress, node);

            if (base.EqualityComparer.Equals(node.Key, goal.Key))
            {
                goal.Depth = node.Depth;
                goal.PreviousStep = node.PreviousStep;

                return RecursionResult.Create(RecursionFlag.Found, node);
            }

            var min = default(TStep);
            var hasMin = false;
            
            foreach (var succ in state.GetNextSteps(node))
            {
                var t = Search(succ, bound, goal, state);

                if (t.Flag == RecursionFlag.Found) return t;
                if (t.Flag == RecursionFlag.NotFound) continue;
                if (!hasMin || state.StepComparer.Compare(t.Step, min) < 0)
                {
                    min = t.Step;
                    hasMin = true;
                }
            }
            return RecursionResult.Create(hasMin ? RecursionFlag.InProgress : RecursionFlag.NotFound, min);
        }

        #endregion 
    }

    public class IterativeDeepeningAStar<TKey> : IterativeDeepeningAStar<TKey, Step<TKey>>
    {
        public IterativeDeepeningAStar() { }

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
}
