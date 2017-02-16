using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class StepExtensions
    {
        public static IEnumerable<IStep<TKey, TStep>> Enumerate<TKey, TStep>(this IStep<TKey, TStep> step)
        {
            if (step == null)
                yield break;
            do
            {
                yield return step;
                step = step.PreviousStep;
            }
            while (step != null);
        }

        public static IStep<TKey, TStep> Reverse<TKey, TStep>(this IStep<TKey, TStep> step) 
        { 
            var prior = default(IStep<TKey, TStep>);
            var current = step;
            var next = default(IStep<TKey, TStep>);

            while (current != null)
            {
                prior = current.PreviousStep;
                current.PreviousStep = next;
                next = current;
                current = prior;
            }
            return next;
        }
    }
}
