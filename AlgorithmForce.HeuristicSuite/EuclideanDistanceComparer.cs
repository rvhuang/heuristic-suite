
namespace AlgorithmForce.HeuristicSuite
{
    public class EuclideanDistanceComparer : IGoalOrientedComparer<Point2DInt32>, IGoalOrientedComparer<Point2DInt64>
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

        public EuclideanDistanceComparer(Point2DInt32 goal)
        {
            this.goal2dInt32 = goal;
        }

        public EuclideanDistanceComparer(Point2DInt64 goal)
        {
            this.goal2dInt64 = goal;
        }

        public int Compare(Point2DInt32 a, Point2DInt32 b)
        {
            var distanceA =
                DistanceHelper.GetEuclideanDistance(a.X, a.Y, goal2dInt32.X, goal2dInt32.Y);
            var distanceB =
                DistanceHelper.GetEuclideanDistance(b.X, b.Y, goal2dInt32.X, goal2dInt32.Y);

            return DistanceHelper.DoubleComparer.Compare(distanceA, distanceB);
        }

        public int Compare(Point2DInt64 a, Point2DInt64 b)
        {
            var distanceA =
                DistanceHelper.GetEuclideanDistance(a.X, a.Y, goal2dInt64.X, goal2dInt64.Y);
            var distanceB =
                DistanceHelper.GetEuclideanDistance(b.X, b.Y, goal2dInt64.X, goal2dInt64.Y);

            return DistanceHelper.DoubleComparer.Compare(distanceA, distanceB);
        }
    }
}