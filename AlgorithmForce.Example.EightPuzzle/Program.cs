using System;

namespace AlgorithmForce.Example.EightPuzzle
{
    using HeuristicSuite;

    public class Program
    {
        public static void Main(string[] args)
        {
            var engine = default(HeuristicSearch<BoardState, BoardState>);

            while (true)
            {
                // http://www.8puzzle.com/images/8_puzzle_start_state_a.png
                var initial = new BoardState(new[]
                {
                    new Point2DInt32(1, 2), // empty square 
                    new Point2DInt32(0, 1), // square 1
                    new Point2DInt32(0, 0), // square 2
                    new Point2DInt32(2, 0), // square 3
                    new Point2DInt32(2, 1), // square 4
                    new Point2DInt32(2, 2), // square 5
                    new Point2DInt32(1, 1), // square 6
                    new Point2DInt32(0, 2), // square 7
                    new Point2DInt32(1, 0), // square 8
                });

                // http://www.8puzzle.com/images/8_puzzle_goal_state_a.png
                var goal = new BoardState(new[]
                {
                    new Point2DInt32(1, 1), // empty square 
                    new Point2DInt32(0, 0), // square 1
                    new Point2DInt32(1, 0), // square 2
                    new Point2DInt32(2, 0), // square 3
                    new Point2DInt32(2, 1), // square 4
                    new Point2DInt32(2, 2), // square 5
                    new Point2DInt32(1, 2), // square 6
                    new Point2DInt32(0, 2), // square 7
                    new Point2DInt32(0, 1), // square 8
                });

                Console.WriteLine("A)-Star Search");
                Console.WriteLine("B)est First Search");
                Console.WriteLine("I)terative Deepening AStar Search");
                Console.Write("Select an algorithm: ");

                // Initial the engine.
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        engine = new AStar<BoardState>();
                        break;

                    case ConsoleKey.B:
                        engine = new BestFirstSearch<BoardState>();
                        break;

                    case ConsoleKey.I:
                        engine = new IterativeDeepeningAStar<BoardState>();
                        break;

                    default: continue;
                }
                Console.WriteLine();

                foreach (var step in engine.Execute(initial, goal, new BoardStateComparer(goal.Positions)).Reverse().Enumerate())
                {
                    Console.WriteLine("Step {0}:", step.Depth);
                    Console.WriteLine(step);
                    Console.WriteLine("----------------------");
                    Console.ReadKey(true);
                }
                Console.WriteLine("Press any key to continue or X to exit.");

                if (Console.ReadKey(true).Key == ConsoleKey.X) break;
            }
        }
    }
}