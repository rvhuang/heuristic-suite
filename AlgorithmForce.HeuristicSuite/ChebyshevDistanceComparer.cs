using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class ChebyshevDistanceComparer : IComparer<Point2DInt64>
    {
        private readonly Point2DInt64 g;

        public Point2DInt64 GoalInt64
        {
            get { return g; }
        }

        public ChebyshevDistanceComparer(Point2DInt64 goal)
        {
            this.g = goal;
        }

        public int Compare(Point2DInt64 a, Point2DInt64 b)
        {
            var distanceA = DistanceHelper.GetChebyshevDistance(g.X, g.Y, a.X, a.Y);
            var distanceB = DistanceHelper.GetChebyshevDistance(g.X, g.Y, b.X, b.Y);

            return DistanceHelper.Int64Comparer.Compare(distanceA, distanceB);
        }
    }
}
