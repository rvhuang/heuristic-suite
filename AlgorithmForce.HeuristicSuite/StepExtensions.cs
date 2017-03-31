using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public static class StepExtensions
    {
        public static IEnumerable<IStep<TKey>> Enumerate<TKey>(this IStep<TKey> step)
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
        
        public static IStep<TKey> Reverse<TKey>(this IStep<TKey> step) 
        { 
            var prior = default(IStep<TKey>);
            var current = step;
            var next = default(IStep<TKey>);

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
