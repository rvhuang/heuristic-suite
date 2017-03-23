using System.Collections.Generic;
using System.Linq;

#if DEBUG
using System.Diagnostics;
#endif

namespace AlgorithmForce.Example.CoinsFlipping
{
    class PuzzleStateComparer : Comparer<bool[]>
    {
        public static IReadOnlyList<bool> Goal = new bool[10] { true, true, true, true, true, true, true, true, true, true, };

        public override int Compare(bool[] x, bool[] y)
        {
            if (x == null) return 1;
            if (y == null) return -1;

            var countXHeads = x.Count(coin => coin); // higher is better 
            var countYHeads = y.Count(coin => coin);
            var continuityX = GetContinuity(x); // higher is better
            var continuityY = GetContinuity(y);

#if DEBUG
            Debug.WriteLine("X: {0}{1}", string.Join(", ", x.Select(coin => coin ? "Head" : "Tail")), string.Empty);
            Debug.WriteLine("countXHeads: {0}, continuityX: {1}, total: {2}", countXHeads, continuityX, countXHeads + continuityX);
            Debug.WriteLine("Y: {0}{1}", string.Join(", ", y.Select(coin => coin ? "Head" : "Tail")), string.Empty);
            Debug.WriteLine("countYHeads: {0}, continuityY: {1}, total: {2}", countYHeads, continuityY, countYHeads + continuityY);
#endif
            return Comparer<int>.Default.Compare(countYHeads + continuityY, countXHeads + continuityX);
        }

        private static int GetContinuity(bool[] coins)
        {
            var finalScore = 0;
            var currentScore = 0;

            for (var i = 1; i < coins.Length; i++)
            {
                if (coins[i - 1] && coins[i])
                    currentScore += 1;
                else
                {
                    if (finalScore < currentScore)
                        finalScore = currentScore;

                    currentScore = 0;
                }
            }

            if (finalScore < currentScore)
                finalScore = currentScore;

            return finalScore;
        }
    }
}