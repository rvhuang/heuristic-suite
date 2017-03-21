using System;

namespace AlgorithmForce.HeuristicSuite
{
    public struct Point2DInt32 : IEquatable<Point2DInt32>
    {
        #region Fields

        public static readonly Point2DInt32 Zero = new Point2DInt32();

        private readonly int x;
        private readonly int y;

        #endregion

        #region Properties

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        #endregion

        #region Constructor

        public Point2DInt32(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Methods

        public bool Equals(Point2DInt32 other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point2DInt32 ? this.Equals((Point2DInt32)obj) : false;
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

        public Point2DInt32 Add(int offsetX, int offsetY)
        {
            return new Point2DInt32(this.x + offsetX, this.y + offsetY);
        }

        #endregion

        #region Operators

        public static bool operator ==(Point2DInt32 a, Point2DInt32 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point2DInt32 a, Point2DInt32 b)
        {
            return !a.Equals(b);
        }

        #endregion
    }
}