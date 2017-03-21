using System;

namespace AlgorithmForce.HeuristicSuite
{
    public struct Point2DInt64 : IEquatable<Point2DInt64>
    {
        #region Fields

        public static readonly Point2DInt64 Zero = new Point2DInt64();

        private readonly long x;
        private readonly long y;

        #endregion

        #region Properties

        public long X
        {
            get { return x; }
        }

        public long Y
        {
            get { return y; }
        }

        #endregion

        #region Constructor

        public Point2DInt64(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Methods

        public bool Equals(Point2DInt64 other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point2DInt64 ? this.Equals((Point2DInt64)obj) : false;
        }

        public override int GetHashCode()
        {
            // Prevent XOR commutative property
            return DistanceHelper.Int64EqualityComparer.GetHashCode(this.x ^ (2L - this.y));
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", this.x, this.y);
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, "[{0}, {1}]", this.x, this.y);
        }

        #endregion

        #region Others

        public Point2DInt64 Add(long offsetX, long offsetY)
        {
            return new Point2DInt64(this.x + offsetX, this.y + offsetY);
        }

        #endregion

        #region Operators

        public static bool operator ==(Point2DInt64 a, Point2DInt64 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point2DInt64 a, Point2DInt64 b)
        {
            return !a.Equals(b);
        }

        #endregion
    }
}