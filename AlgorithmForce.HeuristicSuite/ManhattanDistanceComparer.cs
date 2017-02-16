using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class ManhattanDistanceComparer : IComparer<Point2DInt64>
    {
        private readonly Point2DInt64 goal2d;

        public Point2DInt64 Point2DInt64Goal
        {
            get { return goal2d; }
        }

        public ManhattanDistanceComparer(Point2DInt64 goal)
        {
            this.goal2d = goal;
        }

        public int Compare(Point2DInt64 a, Point2DInt64 b)
        {
            var distanceA = DistanceHelper.GetManhattanDistance(goal2d.X, goal2d.Y, a.X, a.Y);
            var distanceB = DistanceHelper.GetManhattanDistance(goal2d.X, goal2d.Y, b.X, b.Y);

            return DistanceHelper.Int64Comparer.Compare(distanceA, distanceB);
        }
    }
}
