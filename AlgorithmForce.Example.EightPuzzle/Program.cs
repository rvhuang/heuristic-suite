using System;

namespace AlgorithmForce.Example.EightPuzzle
{
    using HeuristicSuite;

    public class Program
    {
        public static void Main(string[] args)
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

            var comparer = new BoardStateComparer(goal.Positions);
            var aStar = new AStar<BoardState>();

            foreach (var step in aStar.Execute(initial, goal, comparer).Reverse().Enumerate())
            {
                Console.WriteLine("Step {0}:", step.Depth);
                Console.WriteLine(step);
                Console.WriteLine("----------------------");
                Console.ReadKey(true);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}
