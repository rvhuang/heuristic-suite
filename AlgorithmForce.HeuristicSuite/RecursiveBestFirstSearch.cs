using System.Collections.Generic;
using System.Linq;

#if DEBUG
using System.Diagnostics;
#endif

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

        protected override TStep ExecuteCore(TStep from, IStep<TKey> goal, IComparer<TKey> c)
        {
            return Search(from, from, goal, new State(this, c)).Step;
        }

        private Result Search(TStep node, TStep bound, IStep<TKey> goal, State state)
        {
            if (state.StepComparer.Compare(node, bound) > 0)
                return Result.Create(Flag.InProgress, node);

            if (base.EqualityComparer.Equals(node.Key, goal.Key))
            {
                goal.Depth = node.Depth;
                goal.PreviousStep = node.PreviousStep;

                return Result.Create(Flag.Found, node);
            }

            var nexts = state.GetNextSteps(node).ToList();

            if (!nexts.Any()) return Result.Create(Flag.NotFound, default(TStep));

            while (true)
            {
                nexts.Sort(state.StepComparer);

                var best = nexts.ElementAt(0);

                if (state.StepComparer.Compare(best, bound) > 0)
                    return Result.Create(Flag.InProgress, best);

                var alternative = state.StepComparer.Min(nexts.ElementAtOrDefault(1), bound);
                var result = Search(best, alternative, goal, state);

                if (result.Flag == Flag.NotFound || result.Flag == Flag.Found)
                    return result;
                
                nexts.Add(result.Step);
            }
        }

        #region Others

        class State
        {
            private readonly HeuristicSearch<TKey, TStep> owner;
            private readonly IComparer<TStep> sc;
            private readonly ISet<TKey> visited;

            public IComparer<TStep> StepComparer
            {
                get { return this.sc; }
            }

            public State(HeuristicSearch<TKey, TStep> owner, IComparer<TKey> c)
            {
                this.owner = owner;
                this.sc = owner.GetStepComparer(c);
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

        enum Flag
        {
            Found,

            InProgress,

            NotFound,
        }

        struct Result
        {
            public Flag Flag
            {
                get; private set;
            }

            public TStep Step
            {
                get; private set;
            }

            public static Result Create(Flag flag, TStep step)
            {
#if DEBUG
                Debug.WriteLine("({0}) Step: {1}\t Prev: {2}\t Flag: {3}",
                    nameof(Result), step, step != null ? step.PreviousStep : step, flag);
#endif
                return new Result() { Flag = flag, Step = step };
            }

            public override string ToString()
            {
                return string.Format("Step: {0}\t Flag: {1}", this.Step, this.Flag);
            }
        }

        #endregion
    }

    public class RecursiveBestFirstSearch<TStep> : RecursiveBestFirstSearch<TStep, TStep>
        where TStep : IStep<TStep>
    {
    }
}
