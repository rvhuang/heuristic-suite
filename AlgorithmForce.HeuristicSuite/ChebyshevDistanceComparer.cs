using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public class ChebyshevDistanceComparer : IGoalOrientedComparer<Point2DInt32>, IGoalOrientedComparer<Point2DInt64>
    {
        private readonly Point2DInt32 goal2dInt32;
        private readonly Point2DInt64 goal2dInt64;

        Point2DInt64 IGoalOrientedComparer<Point2DInt64>.Goal
        {
            get { return goal2dInt64; }
        }

        Point2DInt32 IGoalOrientedComparer<Point2DInt32>.Goal
        {
            get { return goal2dInt32; }
        }

        public ChebyshevDistanceComparer(Point2DInt32 goal)
        {
            this.goal2dInt32 = goal;
        }

        public ChebyshevDistanceComparer(Point2DInt64 goal)
        {
            this.goal2dInt64 = goal;
        }

        public int Compare(Point2DInt32 a, Point2DInt32 b)
        {
            var distanceA =
                DistanceHelper.GetChebyshevDistance(a.X, a.Y, goal2dInt32.X, goal2dInt32.Y);
            var distanceB =
                DistanceHelper.GetChebyshevDistance(b.X, b.Y, goal2dInt32.X, goal2dInt32.Y);

            return DistanceHelper.Int32Comparer.Compare(distanceA, distanceB);
        }

        public int Compare(Point2DInt64 a, Point2DInt64 b)
        {
            var distanceA =
                DistanceHelper.GetChebyshevDistance(a.X, a.Y, goal2dInt64.X, goal2dInt64.Y);
            var distanceB =
                DistanceHelper.GetChebyshevDistance(b.X, b.Y, goal2dInt64.X, goal2dInt64.Y);

            return DistanceHelper.Int64Comparer.Compare(distanceA, distanceB);
        }
    }
}
