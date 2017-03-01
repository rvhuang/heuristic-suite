using System;
using System.Collections.Generic;

namespace AlgorithmForce.Example.TwoZeroFourEight
{
    using HeuristicSuite;

    public sealed class BoardState : IStep<BoardState>, INextStepFactory<BoardState, BoardState>, 
        IEquatable<BoardState>, IComparable<BoardState>
    {
        #region Fields

        private static readonly SequenceEqualityComparer<int[]> ec = new SequenceEqualityComparer<int[]>(SequenceEqualityComparer<int>.Default);

        private readonly int[][] array;

        #endregion

        #region IStep Properties

        BoardState IStep<BoardState>.Key
        {
            get { return this; }
        }

        bool IStep<BoardState>.IsValidStep
        {
            get { return Helper.HasEmptyPlace(this.array); }
        }

        int IStep<BoardState>.Depth
        {
            get; set;
        }

        IStep<BoardState> IStep<BoardState>.PreviousStep
        {
            get; set;
        }

        #endregion

        #region Constructors

        public BoardState()
        {
            this.array = Helper.InitalizeArray();
        }

        public BoardState(int[][] array)
        {
            if (array == null) throw new ArgumentNullException("array");

            this.array = Helper.Clone(array); // create a copy
        }

        #endregion

        #region IEquatable<BoardState>, IComparable<BoardState>

        public bool Equals(BoardState other)
        {
            return other != null ? ec.Equals(this.array, other.array) : false;
        }

        public int CompareTo(BoardState other)
        {
            if (other == null) return -1;

            // TODO:

            return 0;
        }

        #endregion

        #region Next Step Related

        public IEnumerable<BoardState> GetNextSteps()
        {
            var nextMove = default(BoardState);

            if ((nextMove = this.MoveUp()) != null)
                yield return nextMove;

            if ((nextMove = this.MoveDown()) != null)
                yield return nextMove;

            if ((nextMove = this.MoveLeft()) != null)
                yield return nextMove;

            if ((nextMove = this.MoveRight()) != null)
                yield return nextMove;
        }

        public BoardState MoveUp()
        {
            var copied = Helper.Clone(this.array);
            var modified = false;

            for (var col = 0; col < Helper.BoardSize; col++)
                modified = Helper.MoveUp(copied, col) || modified;

            return modified ? new BoardState(copied) : null;
        }

        public BoardState MoveDown()
        {
            var copied = Helper.Clone(this.array);
            var modified = false;

            for (var col = 0; col < Helper.BoardSize; col++)
                modified = Helper.MoveDown(copied, col) || modified;

            return modified ? new BoardState(copied) : null;
        }

        public BoardState MoveLeft()
        {
            var copied = Helper.Clone(this.array);
            var modified = false;

            for (var row = 0; row < Helper.BoardSize; row++)
                modified = Helper.MoveLeft(copied[row]) || modified;

            return modified ? new BoardState(copied) : null;
        }

        public BoardState MoveRight()
        {
            var copied = Helper.Clone(this.array);
            var modified = false;

            for (var row = 0; row < Helper.BoardSize; row++)
                modified = Helper.MoveRight(copied[row]) || modified;

            return modified ? new BoardState(copied) : null;
        }

        #endregion

        #region Overriding Methods

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BoardState);
        }

        public override int GetHashCode()
        {
            return ec.GetHashCode(this.array);
        }

        public override string ToString()
        {
            return Helper.Print(this.array);
        }

        #endregion
    }
}