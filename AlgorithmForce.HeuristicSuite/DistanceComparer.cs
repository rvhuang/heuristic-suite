using System.Collections.Generic;

namespace AlgorithmForce.HeuristicSuite
{
    public abstract class DistanceComparer : IHeuristicComparer<Point2DInt32>, IHeuristicComparer<Point2DInt64>
    {
        #region Fields

        private readonly Point2DInt32 goal2dInt32;
        private readonly Point2DInt64 goal2dInt64;

        #endregion

        #region Properties

        Point2DInt64 IHeuristicComparer<Point2DInt64>.Goal
        {
            get { return goal2dInt64; }
        }

        Point2DInt32 IHeuristicComparer<Point2DInt32>.Goal
        {
            get { return goal2dInt32; }
        }

        public HeuristicFunctionPreference Preference { get; set; }

        #endregion

        #region Constructors

        protected DistanceComparer(Point2DInt32 goal)
        {
            this.goal2dInt32 = goal;
            this.Preference = HeuristicFunctionPreference.Average;
        }

        protected DistanceComparer(Point2DInt64 goal)
        {
            this.goal2dInt64 = goal;
            this.Preference = HeuristicFunctionPreference.Average;
        }

        #endregion

        #region To Be Implemented

        public abstract double EstimateH(Point2DInt32 a);

        public abstract double EstimateH(Point2DInt64 a);

        #endregion

        #region IComparer(T) Methods

        public int Compare(Point2DInt32 a, Point2DInt32 b)
        {
            return Comparer<double>.Default.Compare(this.EstimateH(a), this.EstimateH(b));
        }

        public int Compare(Point2DInt64 a, Point2DInt64 b)
        {
            return Comparer<double>.Default.Compare(this.EstimateH(a), this.EstimateH(b));
        }

        public int Compare(IStep<Point2DInt32> a, IStep<Point2DInt32> b)
        {
            return this.Compare(a, b, this.Preference);
        }

        public int Compare(IStep<Point2DInt64> a, IStep<Point2DInt64> b)
        {
            return this.Compare(a, b, this.Preference);
        }

        #endregion
    }
}