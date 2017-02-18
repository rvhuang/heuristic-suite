using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.Example.PathFinding
{
    using HeuristicSuite;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Define map border and obstacles.  
            var border = new Point2DInt64(20, 20);
            var obstacles = new HashSet<Point2DInt64>();

            #region Map Data

            obstacles.Add(new Point2DInt64(2, 8));
            obstacles.Add(new Point2DInt64(3, 8));
            obstacles.Add(new Point2DInt64(4, 8));
            obstacles.Add(new Point2DInt64(5, 8));
            obstacles.Add(new Point2DInt64(6, 8));
            obstacles.Add(new Point2DInt64(3, 10));
            obstacles.Add(new Point2DInt64(4, 10));
            obstacles.Add(new Point2DInt64(5, 10));
            obstacles.Add(new Point2DInt64(6, 10));
            obstacles.Add(new Point2DInt64(7, 10));
            obstacles.Add(new Point2DInt64(8, 10));
            obstacles.Add(new Point2DInt64(9, 10));
            obstacles.Add(new Point2DInt64(10, 10));
            obstacles.Add(new Point2DInt64(11, 11));
            obstacles.Add(new Point2DInt64(11, 12));
            obstacles.Add(new Point2DInt64(11, 13));
            obstacles.Add(new Point2DInt64(11, 14));
            obstacles.Add(new Point2DInt64(11, 15));
            obstacles.Add(new Point2DInt64(12, 15));
            obstacles.Add(new Point2DInt64(13, 15));
            obstacles.Add(new Point2DInt64(14, 15));
            obstacles.Add(new Point2DInt64(15, 15));

            #endregion

            // Initial the engine. 
            var aStar = new AStar<Point2DInt64, Step>();
            // Tell the engine how to get next steps. 
            aStar.NextStepsFactory = step => step.GetNextSteps();
            // Tell the engine how to check if there is any obstacle in the position.
            aStar.StepValidityChecker = step => !obstacles.Contains(step.Position);

            var fromPos = Point2DInt64.Zero;
            var goalPos = Point2DInt64.Zero;

            while (true)
            {
                #region Read From and To from input

                Console.WriteLine("Tell the engine where to start: (example: 5, 0)");

                var startStr = Console.ReadLine();

                Console.WriteLine("Tell the engine where to end: (example: 10, 18)");

                var endStr = Console.ReadLine();

                try
                {
                    var array1 = startStr.Split(',').Select(long.Parse).ToArray();
                    var array2 = endStr.Split(',').Select(long.Parse).ToArray();

                    fromPos = new Point2DInt64(array1[0], array1[1]);
                    goalPos = new Point2DInt64(array2[0], array2[1]);
                }
                catch
                {
                    continue;
                }

                #endregion

                var stepUnit = 1;
                var from = new Step(fromPos, border, stepUnit);
                var goal = new Step(goalPos, border, stepUnit);

                // Compare two positions and the goal position with Manhattan Distance.
                // ChebyshevDistanceComparer is also available.
                var comparer = new ManhattanDistanceComparer(goal.Position);

                // Get result and draw the map! 
                var path = aStar.Execute(from, goal, comparer).Enumerate().ToArray();
                
                for (var y = 0; y < border.Y; y++)
                {
                    for (var x = 0; x < border.X; x++)
                    {
                        var point = new Point2DInt64(x, y);

                        if (obstacles.Contains(point))
                            Console.Write(" X ");
                        else if (point.Equals(from.Position))
                            Console.Write(" V ");
                        else if (point.Equals(goal.Position))
                            Console.Write(" V ");
                        else if (path.Any(step => step.Key.Equals(point)))
                            Console.Write(" O ");
                        else
                            Console.Write(" - ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("Total steps: {0}. Press any key to continue or 'X' to exit...", path.Count());

                if (Console.ReadKey(true).Key != ConsoleKey.X)
                    Console.Clear();
                else
                    break;
            }
        }
    }
}
