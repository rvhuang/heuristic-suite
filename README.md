# Heuristic Suite

### Overview

**Heuristic Suite** is an experimental implementation of a series of heuristic algorithms in generic programming. The project is aimed to provide an infrastructure that can be applied to any puzzle - as long as the puzzle can be resolved with the algorithm.

Implemented algorithms:

* A\*
* Iterative Deepening A\* (IDA\*)
* Best First Search
* Recursive Best First Search (RBFS)

The implementation takes advantages of built-in .NET Framework comparison mechanism such as [IComparer(T)](https://msdn.microsoft.com/en-us/library/8ehhxeaf.aspx) and [IEqualityComparer(T)](https://msdn.microsoft.com/en-us/library/ms132151.aspx) to create great compatibility with [.NET Standard](https://github.com/dotnet/standard) and entire .NET ecosystem. Source code can be easily brought to other platforms such as Unity and Xamarin. 


### Solving Puzzle with Algorithm 

In order to apply the algorithm to puzzle, following implementations are needed:

1. **The type of step in the puzzle.** The type will be required to implement [IStep(TKey)](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/IStep.cs) interface where `Key` is the property referred by the engine to check the equality between two.

2. **The method to compare steps.** Steps are compared to each other by `Key` property to determine which has better score. The comparison can be done by implementing [IComparable(TKey)](https://msdn.microsoft.com/en-us/library/4d7sx9hd.aspx) or providing [IComparer(TKey)](https://msdn.microsoft.com/en-us/library/8ehhxeaf.aspx) instance.

3. **The method to get next steps from current step.** Next step information can be given by implementing `INextStepFactory(TKey, TStep)` interface, or providing `NextStepFactory` delegate.

The algorithm can now be run by invoking `Execute` method with `from` and `goal` steps as parameters. If the solution exists, each of steps can be obtained by calling `Enumerate` method. Steps can be `Reverse`d before the enumeration starts.

### Examples

* Path Finding ([AlgorithmForce.Example.PathFinding](https://github.com/rvhuang/heuristic-suite/tree/master/AlgorithmForce.Example.PathFinding))

    The example demonstrates the most common and traditional puzzle that use heuristic algorithms to solve.

* 8-Puzzle ([AlgorithmForce.Example.EightPuzzle](https://github.com/rvhuang/heuristic-suite/tree/master/AlgorithmForce.Example.EightPuzzle))

    The example demonstrates how to solve the 8-puzzle with heuristic algorithm. The initial and goal steps of the puzzle are shown below:

    ![From](http://www.8puzzle.com/images/8_puzzle_start_state_a.png)
    ![Goal](http://www.8puzzle.com/images/8_puzzle_goal_state_a.png)

* Coins Flipping Puzzle ([AlgorithmForce.Example.CoinsFlipping](https://github.com/rvhuang/heuristic-suite/tree/master/AlgorithmForce.Example.CoinsFlipping))

    This example demonstrates how the game AI solves the coins flipping puzzle. In the puzzle, only a pair of adjacent coins is allowed to be flipped over at same time. All ten coins that are head is the goal. The puzzle is inspired by [brilliant.org](https://brilliant.org/practice/flipping-pairs/?chapter=introduction-to-joy).

More examples will be added in future.

### Platform

The project currently targets .NET Core and .NET Framework 4.5 or above.
