using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.Example.EightPuzzle
{
    using HeuristicSuite;

    public class BoardStateComparer : Comparer<IReadOnlyList<Point2DInt32>>
    {
        private readonly IReadOnlyList<Point2DInt32> goal;

        public IReadOnlyList<Point2DInt32> GoalPositions
        {
            get { return this.goal; }
        }

        public BoardStateComparer(IReadOnlyList<Point2DInt32> positions)
        {
            this.goal = BoardState.VerifyPositions(positions);
        }

        public override int Compare(IReadOnlyList<Point2DInt32> a, IReadOnlyList<Point2DInt32> b)
        {
            if (a != null && b == null)
                return -1;

            if (a == null && b != null)
                return 1;

            if (a == null && b == null)
                return 0;

            // Use Manhattan Distance to compare each of position with goal.
            var scoreA = goal.Select((p, i) => p.GetManhattanDistance(a[i])).Sum();
            var scoreB = goal.Select((p, i) => p.GetManhattanDistance(b[i])).Sum();

            return DistanceHelper.Int32Comparer.Compare(scoreA, scoreB);
        }
    }
}