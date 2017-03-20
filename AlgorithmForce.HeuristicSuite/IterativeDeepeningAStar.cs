using System.Collections.Generic;

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
                var t = Search(from, bound, goal, new State(this, c));

                if (t.Flag == Flag.Found) return t.Step;
                if (t.Flag == Flag.NotFound) return default(TStep);

                bound = t.Step;
                counter++;
            }
            return default(TStep);
        }

        #endregion
        
        #region Core

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

            var min = default(TStep);
            var hasMin = false;
            
            foreach (var succ in state.GetNextSteps(node))
            {
                var t = Search(succ, bound, goal, state);

                if (t.Flag == Flag.Found) return t;
                if (t.Flag == Flag.NotFound) continue;
                if (!hasMin || state.StepComparer.Compare(t.Step, min) < 0)
                {
                    min = t.Step;
                    hasMin = true;
                }
            }
            return Result.Create(hasMin ? Flag.InProgress : Flag.NotFound, min);
        }

        #endregion

        #region Others

        class State
        {
            private readonly IterativeDeepeningAStar<TKey, TStep> owner;
            private readonly IComparer<TStep> sc;
            private readonly ISet<TKey> visited;
            
            public IComparer<TStep> StepComparer
            {
                get { return this.sc; }
            }

            public State(IterativeDeepeningAStar<TKey, TStep> owner, IComparer<TKey> c)
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

            NotFound,

            InProgress,
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

    public class IterativeDeepeningAStar<TStep> : IterativeDeepeningAStar<TStep, TStep>
        where TStep : IStep<TStep>
    {
    }
}
