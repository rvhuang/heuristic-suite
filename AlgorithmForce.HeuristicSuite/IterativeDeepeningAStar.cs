﻿using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

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

        protected override TStep ExecuteCore(TStep from, IStep<TKey> goal, IComparer<TKey> c)
        {
            var counter = 0;
            var bound = from;
            
            while (counter <= max)
            {
                var t = Search(from, bound, goal, new RecursionState(this, c));

                if (t.Flag == RecursionFlag.Found) return t.Step;
                if (t.Flag == RecursionFlag.NotFound) return default(TStep);

                bound = t.Step;
                counter++;
            }
            return default(TStep);
        }

        #endregion
        
        #region Core

        private RecursionResult Search(TStep node, TStep bound, IStep<TKey> goal, RecursionState state)
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

    public class IterativeDeepeningAStar<TStep> : IterativeDeepeningAStar<TStep, TStep>
        where TStep : IStep<TStep>
    {
    }
}
