using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class ChebyshevDistanceComparer : IComparer<Point2DInt64>
    {
        private readonly Point2DInt64 goal;
        
        public Point2DInt64 Goal
        {
            get { return goal; }
        }

        public ChebyshevDistanceComparer(Point2DInt64 goal)
        {
            this.goal = goal;
        }

        public int Compare(Point2DInt64 a, Point2DInt64 b)
        {
            var distanceA =
                DistanceHelper.GetChebyshevDistance(a.X, a.Y, goal.X, goal.Y);
            var distanceB =
                DistanceHelper.GetChebyshevDistance(b.X, b.Y, goal.X, goal.Y);

            return DistanceHelper.Int64Comparer.Compare(distanceA, distanceB);
        }
    }
}
