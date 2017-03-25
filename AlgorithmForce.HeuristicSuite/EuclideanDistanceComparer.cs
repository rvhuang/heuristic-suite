
namespace AlgorithmForce.HeuristicSuite
{
    public class EuclideanDistanceComparer : DistanceComparer
    {
        public EuclideanDistanceComparer(Point2DInt32 goal)
            : base(goal)
        {
        }

        public EuclideanDistanceComparer(Point2DInt64 goal)
            : base(goal)
        {
        }
        
        public override double EstimateH(Point2DInt32 a)
        {
            return DistanceHelper.GetEuclideanDistance(a, (this as IHeuristicComparer<Point2DInt32>).Goal);
        }

        public override double EstimateH(Point2DInt64 a)
        {
            return DistanceHelper.GetEuclideanDistance(a, (this as IHeuristicComparer<Point2DInt64>).Goal);
        }
    }
}