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
            var max = new Point2DInt64(20, 20);
            var startAt = new Point2DInt64(5, 0);
            var goal = new Point2DInt64(11, 18);
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

            #endregion

            var aStar = AStarFactory.Create<Point2DInt64, Step>(new ChebyshevDistanceComparer(goal));

            aStar.NextStepsFactory = step => step.GetNextSteps();
            aStar.StepValidityChecker = step => !obstacles.Contains(step.Position);

            var result = aStar.Execute(new Step(startAt, max, 1), new Step(goal, max, 1)).Enumerate().ToArray();

            for (var y = 0; y < max.Y; y++)
            {
                for (var x = 0; x < max.X; x++)
                {
                    var point = new Point2DInt64(x, y);

                    if (obstacles.Contains(point))
                        Console.Write("_X");
                    else if (point.Equals(startAt))
                        Console.Write("_V");
                    else if (point.Equals(goal))
                        Console.Write("_V");
                    else if (result.Any(step => step.Key.Equals(point)))
                        Console.Write("_O");
                    else
                        Console.Write("__");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Total steps: {0}", result.Count());
            Console.ReadKey(true);
        }
    }
}
