﻿using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.Example.CoinsFlipping
{
    using HeuristicSuite;

    class PuzzleStateComparer : Comparer<bool[]>
    {
        public PuzzleStateComparer()
        {
        }

        public override int Compare(bool[] x, bool[] y)
        {
            return DistanceHelper.DoubleComparer.Compare(Estimation(x), Estimation(y));
        }

        private static double Estimation(bool[] coins)
        {
            return 0 - (coins.Count(coin => coin) + GetContinuity(coins));
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