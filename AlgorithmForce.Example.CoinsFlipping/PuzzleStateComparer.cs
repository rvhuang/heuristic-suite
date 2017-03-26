using System.Linq;

namespace AlgorithmForce.Example.CoinsFlipping
{
    using HeuristicSuite;

    class PuzzleStateComparer : HeuristicComparer<bool[]>
    {
        public static bool[] DefaulGoal = new bool[10] { true, true, true, true, true, true, true, true, true, true, };

        public PuzzleStateComparer()
            : base(DefaulGoal, EstimateH)
        {
        }

        private static double EstimateH(bool[] coins)
        {
            return coins.Count(coin => coin) + GetContinuity(coins);
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