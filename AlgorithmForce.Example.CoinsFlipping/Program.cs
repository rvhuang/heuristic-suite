using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.Example.CoinsFlipping
{
    using HeuristicSuite;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This example demonstrates how the game AI solves the coins flipping puzzle.");
            Console.WriteLine("Only a pair of adjacent coins is allowed to be flipped over at same time.");
            Console.WriteLine("All ten coins that are head is the goal.");
            Console.WriteLine();
            Console.WriteLine("The puzzle is inspired by brilliant.org:");
            Console.WriteLine("https://brilliant.org/practice/flipping-pairs/?chapter=introduction-to-joy");
            Console.WriteLine();

            // true  -> head
            // false -> tail
            var goal = new bool[10] { true, true, true, true, true, true, true, true, true, true, };
            var init = new bool[10] { true, false, true, false, true, true, false, true, false, true };

            var engine = new AStar<bool[], Step<bool[]>>();

            engine.EqualityComparer = new SequenceEqualityComparer<bool>();
            engine.NextStepsFactory = GetNextSteps;

            var solution = engine.ExecuteWith(new Step<bool[]>(init), goal, new PuzzleStateComparer()).Reverse();

            foreach (var step in solution.Enumerate())
            {
                var coins = step.Key;

                Console.WriteLine("Step {0}: {1}", step.Depth, step.Depth == 0 ? "(Initial)" : string.Empty);
                Console.WriteLine(string.Join(", ", coins.Select(coin => coin ? "Head" : "Tail")));
            }
            Console.ReadKey(true);
        }

        static IEnumerable<Step<bool[]>> GetNextSteps(Step<bool[]> coins)
        {
            for (var i = 0; i < 9; i++)
            {
                var copied = coins.Key.ToArray();

                copied[i] = !copied[i];
                copied[i+ 1] = !copied[i + 1];

                yield return new Step<bool[]>(copied);
            }
        }

        static void Print(bool[] array)
        {
            Console.WriteLine(string.Join(", ", array.Select(coin => coin ? "Head" : "Tail")));
        }
    }
}