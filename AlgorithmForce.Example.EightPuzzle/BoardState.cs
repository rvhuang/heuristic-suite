using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmForce.Example.EightPuzzle
{
    using HeuristicSuite;

    public class BoardState : IStep<Point2DInt32[]>, INextStepFactory<Point2DInt32[], BoardState>
    {
        #region Fields

        public const int BoardSize = 3;

        private readonly Point2DInt32[] _positions;
        private readonly int _hashCode;

        #endregion

        #region Properties

        public IReadOnlyList<Point2DInt32> Positions
        {
            get { return this._positions; }
        }

        #endregion

        #region IStep<BoardStatus> Properties

        int IStep<Point2DInt32[]>.Depth
        {
            get; set;
        }

        IStep<Point2DInt32[]> IStep<Point2DInt32[]>.PreviousStep
        {
            get; set;
        }

        bool IStep<Point2DInt32[]>.IsValidStep
        {
            get { return true; }
        }

        Point2DInt32[] IStep<Point2DInt32[]>.Key
        {
            get { return this._positions.ToArray(); }
        }

        #endregion

        #region Constructor

        public BoardState(IReadOnlyList<Point2DInt32> positions)
        {
            this._positions = VerifyPositions(positions);
            this._hashCode = SequenceEqualityComparer<Point2DInt32>.Default.GetHashCode(positions);
        }

        private BoardState(Point2DInt32[] positions)
        {
            this._positions = positions;
            this._hashCode = SequenceEqualityComparer<Point2DInt32>.Default.GetHashCode(positions);
        }

        #endregion

        #region Methods

        public IEnumerable<BoardState> GetNextSteps()
        {
            if (this._positions[0].X > 0)
                yield return CreateNextStep(-1, 0);

            if (this._positions[0].Y > 0)
                yield return CreateNextStep(0, -1);

            if (this._positions[0].X + 1 < BoardSize)
                yield return CreateNextStep(1, 0);

            if (this._positions[0].Y + 1 < BoardSize)
                yield return CreateNextStep(0, 1);
        }

        public bool Equals(BoardState other)
        {
            if (other == null) return false;

            return SequenceEqualityComparer<Point2DInt32>.Default.Equals(this._positions, other._positions);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BoardState);
        }

        public override int GetHashCode()
        {
            return this._hashCode;
        }

        public override string ToString()
        {
            var rows = new int[BoardSize][];
            var sb = new StringBuilder();

            for (var r = 0; r < rows.Length; r++)
                rows[r] = new int[BoardSize];

            for (var i = 0; i < _positions.Length; i++)
            {
                var pos = _positions[i];
                rows[pos.Y][pos.X] = i;
            }

            foreach (var row in rows)
                sb.AppendLine(string.Join("\t", row.Select(c => c == 0 ? "_" : Convert.ToString(c))));

            return sb.ToString();
        }

        #endregion

        #region Others

        private BoardState CreateNextStep(int offsetX, int offsetY)
        {
            var array = _positions.ToArray(); // create a copy
            var emptyPos = _positions[0].Add(offsetX, offsetY);

            Swap(array, 0, Array.IndexOf(array, emptyPos));

            return new BoardState(array);
        }

        public static void Swap(Point2DInt32[] array, int indexA, int indexB)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            
            var temp = array[indexA];

            array[indexA] = array[indexB];
            array[indexB] = temp;
        }

        public static Point2DInt32[] VerifyPositions(IReadOnlyList<Point2DInt32> positions)
        {
            if (positions == null)
                throw new ArgumentNullException("positions");

            if (positions.Count != BoardSize * BoardSize)
                throw new ArgumentException(string.Format("Number of elements must be {0}.", BoardSize * BoardSize), "positions");

            var set = new HashSet<Point2DInt32>();

            foreach (var pos in positions)
            {
                if (pos.X < 0 || pos.X >= BoardSize)
                    throw new ArgumentException(string.Format("X must be between zero and {0} (inclusive).", BoardSize - 1), "positions");

                if (pos.Y < 0 || pos.Y >= BoardSize)
                    throw new ArgumentException(string.Format("Y must be between zero and {0} (inclusive).", BoardSize - 1), "positions");

                if (!set.Add(pos))
                    throw new ArgumentException("One or more elements are not unique.", "positions");
            }
            return positions.ToArray();
        }

        #endregion
    }
}
