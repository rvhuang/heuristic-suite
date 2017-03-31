using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.HeuristicSuite
{
    public class BestFirstSearch<TKey, TStep> : HeuristicSearch<TKey, TStep>
        where TStep : IStep<TKey>
    {
        protected override TStep ExecuteCore(TStep from, TStep goal, IComparer<TKey> c)
        {
            var visited = new HashSet<TKey>(base.EqualityComparer);
            var nextSteps = new List<TStep>();

            nextSteps.Add(from);

            while (nextSteps.Any())
            {
                var best = nextSteps.First();
                var hasNext = false;

                if (base.EqualityComparer.Equals(best.Key, goal.Key))
                {
                    goal.Depth = best.Depth;
                    goal.PreviousStep = best.PreviousStep;

                    return best;
                }
                nextSteps.RemoveAt(0);

                foreach (var next in base.NextStepsFactory(best))
                {
                    if (!IsValidStep(next)) continue;
                    if (!visited.Add(next.Key)) continue;

                    next.Depth = best.Depth + 1;
                    next.PreviousStep = best;

                    nextSteps.Add(next);
                    hasNext = true;
                }
                if (hasNext) nextSteps.Sort((a, b) => c.Compare(a.Key, b.Key));
            }
            return default(TStep);
        }
    }

    public class BestFirstSearch<TKey> : BestFirstSearch<TKey, Step<TKey>>
    {
        public BestFirstSearch() { }

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