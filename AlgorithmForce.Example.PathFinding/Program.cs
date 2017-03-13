using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlgorithmForce.Example.PathFinding
{
    using HeuristicSuite;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Define map border and load map data.  
            var border = new Point2DInt32(20, 20);
            var mapData = LoadMapData();
            var fromPos = mapData.Item1;
            var goalPos = mapData.Item2;
            var obstacles = mapData.Item3;
            
            // Initial the engine. 
            var aStar = new AStar<Point2DInt32, Step>();
            // Tell the engine how to get next steps. 
            // aStar.NextStepsFactory = step => step.GetNextSteps();
            // Tell the engine how to check if there is any obstacle in the position.
            aStar.StepValidityChecker = step => !obstacles.Contains(step.Position);
            
            while (true)
            {
                Console.WriteLine("Select comparer:");
                Console.WriteLine("C)hebyshev Distance Comparer");
                Console.WriteLine("E)uclidean Distance Comparer");
                Console.WriteLine("M)anhattan Distance Comparer");

                // Compare two positions and the goal position with selected distance.
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.C:
                        aStar.Comparer = new ChebyshevDistanceComparer(goalPos);
                        break;

                    case ConsoleKey.E:
                        aStar.Comparer = new EuclideanDistanceComparer(goalPos);
                        break;

                    case ConsoleKey.M:
                        aStar.Comparer = new ManhattanDistanceComparer(goalPos);
                        break;

                    default: continue;
                }
                
                var stepUnit = 1;
                var from = new Step(fromPos, border, stepUnit);
                var goal = new Step(goalPos, border, stepUnit);

                // Get result and draw the map! 
                var path = aStar.Execute(from, goal).Enumerate().ToArray();
                
                for (var y = 0; y < border.Y; y++)
                {
                    for (var x = 0; x < border.X; x++)
                    {
                        var point = new Point2DInt32(x, y);

                        if (obstacles.Contains(point))
                            Console.Write(" X ");
                        else if (point.Equals(from.Position))
                            Console.Write(" F ");
                        else if (point.Equals(goal.Position))
                            Console.Write(" G ");
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

        public static Tuple<Point2DInt32, Point2DInt32, ISet<Point2DInt32>> LoadMapData()
        {
            var from = default(Point2DInt32);
            var goal = default(Point2DInt32);
            var obstacles = new HashSet<Point2DInt32>();
            var mapData = File.ReadAllLines("MapData.txt");

            for (int y = 0; y < mapData.Length; y++)
            {
                for (var x = 0; x < mapData[y].Length; x++)
                {
                    switch (mapData[y][x])
                    {
                        case 'F':
                            from = new Point2DInt32(x, y);
                            break;

                        case 'G':
                            goal = new Point2DInt32(x, y);
                            break;

                        case 'X':
                            obstacles.Add(new Point2DInt32(x, y));
                            break;
                    }
                }
            }
            return new Tuple<Point2DInt32, Point2DInt32, ISet<Point2DInt32>>(from, goal, obstacles);
        }
    }
}
