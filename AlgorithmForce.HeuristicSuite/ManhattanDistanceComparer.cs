
namespace AlgorithmForce.HeuristicSuite
{
    public class ManhattanDistanceComparer : IGoalOrientedComparer<Point2DInt32>, IGoalOrientedComparer<Point2DInt64> 
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

        public ManhattanDistanceComparer(Point2DInt32 goal)
        {
            this.goal2dInt32 = goal;
        }

        public ManhattanDistanceComparer(Point2DInt64 goal)
        {
            this.goal2dInt64 = goal;
        }

        public double GetScore(Point2DInt32 a)
        {
            return DistanceHelper.GetManhattanDistance(a.X, a.Y, goal2dInt32.X, goal2dInt32.Y);
        }

        public double GetScore(Point2DInt64 a)
        {
            return DistanceHelper.GetManhattanDistance(a.X, a.Y, goal2dInt64.X, goal2dInt64.Y);
        }

        public int Compare(Point2DInt32 a, Point2DInt32 b)
        {
            var distanceA =
                DistanceHelper.GetManhattanDistance(a.X, a.Y, goal2dInt32.X, goal2dInt32.Y);
            var distanceB =
                DistanceHelper.GetManhattanDistance(b.X, b.Y, goal2dInt32.X, goal2dInt32.Y);

            return DistanceHelper.Int32Comparer.Compare(distanceA, distanceB);
        }

        public int Compare(Point2DInt64 a, Point2DInt64 b)
        {
            var distanceA =
                DistanceHelper.GetManhattanDistance(a.X, a.Y, goal2dInt64.X, goal2dInt64.Y);
            var distanceB =
                DistanceHelper.GetManhattanDistance(b.X, b.Y, goal2dInt64.X, goal2dInt64.Y);

            return DistanceHelper.Int64Comparer.Compare(distanceA, distanceB);
        }
    }
}