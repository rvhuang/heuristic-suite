using System;
using System.Collections.Generic;

namespace AlgorithmForce.Example.PathFinding
{
    using HeuristicSuite;

    class Step : IStep<Point2DInt64>, INextStepFactory<Point2DInt64, Step>
    {
        #region Fields

        private readonly Point2DInt64 _pos;
        private readonly Point2DInt64 _border;

        private readonly long _unit;

        #endregion

        #region Properties

        public Point2DInt64 Position
        {
            get { return this._pos; }
        }
        
        public bool IsValidStep
        {
            get { return this._pos.X < this._border.X && this._pos.Y < this._border.Y && this._pos.X >= 0 && this._pos.Y >= 0; }
        }

        Point2DInt64 IStep<Point2DInt64>.Key
        {
            get { return this._pos; }
        }

        int IStep<Point2DInt64>.Depth
        {
            get; set;
        }

        IStep<Point2DInt64> IStep<Point2DInt64>.PreviousStep
        {
            get; set;
        }

        #endregion

        #region Constructor

        public Step(Point2DInt64 pos, Point2DInt64 border, long unit)
        {
            this._pos = pos;
            this._border = border;
            this._unit = unit;
        }

        #endregion

        #region Methods

        public IEnumerable<Step> GetNextSteps()
        {
            return new[]
            {
                new Step(new Point2DInt64(this._pos.X - this._unit, this._pos.Y), this._border, this._unit),
                new Step(new Point2DInt64(this._pos.X + this._unit, this._pos.Y), this._border, this._unit),
                new Step(new Point2DInt64(this._pos.X, this._pos.Y - this._unit), this._border, this._unit),
                new Step(new Point2DInt64(this._pos.X, this._pos.Y + this._unit), this._border, this._unit),
                /*
                new Step(new Point2DInt64(this.pos.X + this.step, this.pos.Y + this.step), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X - this.step, this.pos.Y - this.step), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X + this.step, this.pos.Y - this.step), this.max, this.step),
                new Step(new Point2DInt64(this.pos.X - this.step, this.pos.Y + this.step), this.max, this.step) */
            };
        }
        
        public override string ToString()
        {
            return string.Format("Step: {0}", this._pos.ToString());
        }

        #endregion
    }
}
