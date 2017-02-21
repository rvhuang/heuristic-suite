# 8-Puzzle Example

### Overview

The example demonstrates how to solve 8-puzzle with A\* algorithm and to apply the engine to the puzzle. The initial (L) and goal (R) board states are shown below: 

![From](http://www.8puzzle.com/images/8_puzzle_start_state_a.png)
![Goal](http://www.8puzzle.com/images/8_puzzle_goal_state_a.png)

#### Define Board State 

In this puzzle, the step is defined with `BoardState` class.

```cs

    public class BoardState : IStep<BoardState>, 
        INextStepFactory<BoardState, BoardState>, IEquatable<BoardState>
    { 
        private readonly Point2DInt32[] _positions;
        
        // Code omitted 
    }
```

Square positions on the board are stored in an array of [Point2DInt32](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/Point2DInt32.cs) structure, where array index is the square number and index zero is the empty square. A board with numbers [2, 8, 3], [1, 6, 4] and [7, (empty), 5] is equivalent to the array in following code snippet:

```cs

    new[]
    {
        new Point2DInt32(1, 2), // empty square 
        new Point2DInt32(0, 1), // square 1
        new Point2DInt32(0, 0), // square 2
        new Point2DInt32(2, 0), // square 3
        new Point2DInt32(2, 1), // square 4
        new Point2DInt32(2, 2), // square 5
        new Point2DInt32(1, 1), // square 6
        new Point2DInt32(0, 2), // square 7
        new Point2DInt32(1, 0), // square 8
    }
```

#### Compare Board States

Checking the equality of two board states is easy. An out-of-the-box class [SequenceEqualityComparer(T)](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/SequenceEqualityComparer.cs) is designed for determining whether the two collections is equivalent. In the example, the class is used to check the equality of two `Point2DInt32` arrays.

```cs

    public bool Equals(BoardState other) // Member of BoardState
    {
        if (other == null) return false;

        return SequenceEqualityComparer<Point2DInt32>.Default
            .Equals(this._positions, other._positions);
    }
```

In order to compare two board states to determine the closer one to the goal, we define the [BoardStateComparer](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.Example.EightPuzzle/BoardStateComparer.cs) class with following comparison method: 

```cs

    private readonly IReadOnlyList<Point2DInt32> goal;
    
    public override int Compare(BoardState a, BoardState b) // Member of BoardStateComparer
    {
        if (a != null && b == null)
            return -1;

        if (a == null && b != null)
            return 1;

        if (a == null && b == null)
            return 0;

        // Use Manhattan Distance to compare each of position with goal.
        var scoreA = goal.Select((p, i) => p.GetManhattanDistance(a.Positions[i])).Sum();
        var scoreB = goal.Select((p, i) => p.GetManhattanDistance(b.Positions[i])).Sum();
            
        return DistanceHelper.Int32Comparer.Compare(scoreA, scoreB);
    }
```

#### Get Next Steps From Current

To get next board states from current state, we need to exchange the position of empty square, and the position of square that the empty square will move to. Therefore, we implement following methods in `BoardState` class:

```cs

    public IEnumerable<BoardState> GetNextSteps() // INextStepFactory implementation
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

    private BoardState CreateNextStep(int offsetX, int offsetY)
    {
        var array = _positions.ToArray(); // create a copy
        var emptyPos = _positions[0].Add(offsetX, offsetY);

        Swap(array, 0, Array.IndexOf(array, emptyPos));

        return new BoardState(array);
    }

    private static void Swap(Point2DInt32[] array, int indexA, int indexB)
    {
        // Code omitted.
    }
```

#### Execute The Engine

Now we can execute the engine and enumerate each step of the solution.

```cs

    var comparer = new BoardStateComparer(goal.Positions);
    var aStar = new AStar<BoardState>();

    foreach (var step in aStar.Execute(initial, goal, comparer).Reverse().Enumerate())
    {
        Console.WriteLine("Step {0}:", step.Depth);
        // TODO: Print the board detail
    }

```

