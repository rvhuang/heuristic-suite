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
			Console.WriteLine("Please enter inital state: (H: Head, T: Tail)");
			
			// true  -> head
            // false -> tail
            var init = new bool[10];
            var goal = new bool[10] { true, true, true, true, true, true, true, true, true, true, };;
			
			for (var i = 0; i < 10; i++)
				init[i] = Console.ReadKey().Key == ConsoleKey.H;
			
            Console.WriteLine();
			
			var engine = new AStar<bool[]>();

            engine.EqualityComparer = new SequenceEqualityComparer<bool>();
            engine.NextStepsFactory = GetNextSteps;

            var solution = engine.ExecuteWith(new Step<bool[]>(init), goal, new PuzzleStateComparer()).Reverse();
			var prev = default(IStep<bool[]>);

            foreach (var step in solution.Enumerate())
            {
                Console.Write("Step {0}: ", step.Depth);
				
				if (prev == null)
					Console.Write(string.Concat(step.Key.Select(coin => coin ? " H " : " T ")));
                else
				{
					for (var i = 0; i < 10; i++)
					{
						// find the difference and give different color
						Console.ForegroundColor = step.Key[i] == prev.Key[i] ? ConsoleColor.Gray : ConsoleColor.Green;
						Console.Write(step.Key[i] ? " H " : " T ");
						Console.ForegroundColor = ConsoleColor.Gray;
					}
				}
				
				prev = step;
				Console.WriteLine(" {0}", step.Depth == 0 ? "(Initial)" : string.Empty);
            }
            Console.ReadKey(true);
        }

        static IEnumerable<Step<bool[]>> GetNextSteps(Step<bool[]> coins)
        {
            for (var i = 0; i < 9; i++)
            {
                var copied = coins.Key.ToArray();

                copied[i] = !copied[i];
                copied[i + 1] = !copied[i + 1];

                yield return new Step<bool[]>(copied);
            }
        }
    }
}