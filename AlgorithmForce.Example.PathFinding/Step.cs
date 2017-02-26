using System;
using System.Collections.Generic;

namespace AlgorithmForce.Example.PathFinding
{
    using HeuristicSuite;

    class Step : IStep<Point2DInt32>, INextStepFactory<Point2DInt32, Step>
    {
        #region Fields

        private readonly Point2DInt32 _pos;
        private readonly Point2DInt32 _border;

        private readonly int _unit;

        #endregion

        #region Properties

        public Point2DInt32 Position
        {
            get { return this._pos; }
        }
        
        public bool IsValidStep
        {
            get { return this._pos.X < this._border.X && this._pos.Y < this._border.Y && this._pos.X >= 0 && this._pos.Y >= 0; }
        }

        Point2DInt32 IStep<Point2DInt32>.Key
        {
            get { return this._pos; }
        }

        int IStep<Point2DInt32>.Depth
        {
            get; set;
        }

        IStep<Point2DInt32> IStep<Point2DInt32>.PreviousStep
        {
            get; set;
        }

        #endregion

        #region Constructor

        public Step(Point2DInt32 pos, Point2DInt32 border, int unit)
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
                new Step(new Point2DInt32(this._pos.X - this._unit, this._pos.Y), this._border, this._unit),
                new Step(new Point2DInt32(this._pos.X + this._unit, this._pos.Y), this._border, this._unit),
                new Step(new Point2DInt32(this._pos.X, this._pos.Y - this._unit), this._border, this._unit),
                new Step(new Point2DInt32(this._pos.X, this._pos.Y + this._unit), this._border, this._unit),
                /*
                new Step(new Point2DInt32(this.pos.X + this.step, this.pos.Y + this.step), this.max, this.step),
                new Step(new Point2DInt32(this.pos.X - this.step, this.pos.Y - this.step), this.max, this.step),
                new Step(new Point2DInt32(this.pos.X + this.step, this.pos.Y - this.step), this.max, this.step),
                new Step(new Point2DInt32(this.pos.X - this.step, this.pos.Y + this.step), this.max, this.step) */
            };
        }
        
        public override string ToString()
        {
            return string.Format("Step: {0}", this._pos.ToString());
        }

        #endregion
    }
}
