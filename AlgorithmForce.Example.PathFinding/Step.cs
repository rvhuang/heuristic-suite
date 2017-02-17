using System;
using System.Collections.Generic;

namespace AlgorithmForce.Example.PathFinding
{
    using HeuristicSuite;

    class Step : IStep<Point2DInt64, Step>
    {
        #region Fields

        private readonly Point2DInt64 pos;
        private readonly Point2DInt64 max;

        private readonly long step;

        #endregion

        #region Properties

        public Point2DInt64 Position
        {
            get { return this.pos; }
        }
        
        public bool IsValidStep
        {
            get { return this.pos.X < this.max.X && this.pos.Y < this.max.Y && this.pos.X >= 0 && this.pos.Y >= 0; }
        }

        Point2DInt64 IStep<Point2DInt64, Step>.Key
        {
            get { return this.pos; }
        }

        int IStep<Point2DInt64, Step>.Depth
        {
            get; set;
        }

        IStep<Point2DInt64, Step> IStep<Point2DInt64, Step>.PreviousStep
        {
            get; set;
        }

        #endregion

        #region Constructor

        public Step(Point2DInt64 pos, Point2DInt64 max, long step)
        {
            this.pos = pos;
            this.max = max;
            this.step = step;
        }

        #endregion

        #region Methods

        public IEnumerable<Step> GetNextSteps()
        {
            return new[]
            {
                new Step(new Point2DInt64(this.pos.X - this.step, this.pos.Y), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X + this.step, this.pos.Y), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X, this.pos.Y - this.step), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X, this.pos.Y + this.step), this.max, this.step),
                /*
                new Step(new Point2DInt64(this.pos.X + this.step, this.pos.Y + this.step), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X - this.step, this.pos.Y - this.step), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X + this.step, this.pos.Y - this.step), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X - this.step, this.pos.Y + this.step), this.max, this.step) */
            };
        }

        public bool Equals(Step other)
        {
            return this.pos.Equals(other.pos);
        }

        public override int GetHashCode()
        {
            return this.pos.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.pos.Equals(obj);
        }

        public override string ToString()
        {
            return string.Format("Step: {0}", this.pos.ToString());
        }

        #endregion
    }
}
