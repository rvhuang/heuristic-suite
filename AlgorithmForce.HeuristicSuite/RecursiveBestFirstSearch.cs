using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.HeuristicSuite
{
    public class RecursiveBestFirstSearch<TKey, TStep> : HeuristicSearch<TKey, TStep>
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

        protected override TStep ExecuteCore(TStep from, TStep goal, IHeuristicComparer<TKey, TStep> sc)
        {
            return Search(from, from, goal, new RecursionState(this, sc)).Step;
        }

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

            var nexts = state.GetNextSteps(node).ToList();

            if (!nexts.Any()) return RecursionResult.Create(RecursionFlag.NotFound, default(TStep));

            while (true)
            {
                nexts.Sort(state.StepComparer);

                var best = nexts.ElementAt(0);

                if (state.StepComparer.Compare(best, bound) > 0)
                    return RecursionResult.Create(RecursionFlag.InProgress, best);

                var alternative = state.StepComparer.Min(nexts.ElementAtOrDefault(1), bound);
                var result = Search(best, alternative, goal, state);

                if (result.Flag == RecursionFlag.NotFound || result.Flag == RecursionFlag.Found)
                    return result;

                nexts.Add(result.Step);
            }
        }
    }

    public class RecursiveBestFirstSearch<TKey> : RecursiveBestFirstSearch<TKey, Step<TKey>>
    {
        public RecursiveBestFirstSearch() { }

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
