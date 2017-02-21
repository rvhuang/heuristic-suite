# Heuristic Suite

### Overview

**Heuristic Suite** is an experimental implementation of A\* algorithm in generic programming. The project is aimed to define an engine from abstract perspective that can be applied to any puzzle, as long as the puzzle can be resolved with the algorithm. The implementation takes advantages of built-in .Net Framework comparison mechanism such as [IComparer(T)](https://msdn.microsoft.com/en-us/library/8ehhxeaf.aspx) and [IEqualityComparer(T)](https://msdn.microsoft.com/en-us/library/ms132151.aspx) to create great compatibility with .Net development.

### Basic Guide

In order to apply the engine to the puzzle, following implemenations are needed:

1. **The type of step in the puzzle.** The type will be required to implement [IStep(TKey)](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/IStep.cs) interface where `Key` is the property that the engine checks the equality between two steps.

2. **The method to compare steps.** Steps are compared to each other by `Key` property to determine which has better score. This can be done by implementing [IComparable(TKey)](https://msdn.microsoft.com/en-us/library/4d7sx9hd.aspx) or providing [IComparer(TKey)](https://msdn.microsoft.com/en-us/library/8ehhxeaf.aspx) instance.

3. **The method to get next steps from current step.** Next step information can be given by implementing [INextStepFactory(TKey, TStep)](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/IStep.cs) interface, or providing [NextStepFactory](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/AStar.cs#L29) delegate.

Now we can [Execute](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/AStar.cs#L87) the engine with the `from` and `goal` steps to get the solution. If the solution exists, you can [Enumerate](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/StepExtensions.cs#L7) each of steps, or [Reverse](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/StepExtensions.cs#L19) them before the enumeration starts.

### Examples

* Path Finding ([AlgorithmForce.Example.PathFinding](https://github.com/rvhuang/heuristic-suite/tree/master/AlgorithmForce.Example.PathFinding))

    The example demonstrates the most common and traditional puzzle that use A\* algorithm to solve.

* 8-Puzzle ([AlgorithmForce.Example.EightPuzzle](https://github.com/rvhuang/heuristic-suite/tree/master/AlgorithmForce.Example.EightPuzzle))

    The example demonstrates how to solve the 8-puzzle with A\* algorithm. Following figures show the initial and goal steps of the puzzle:

    ![From](http://www.8puzzle.com/images/8_puzzle_start_state_a.png)
    ![Goal](http://www.8puzzle.com/images/8_puzzle_goal_state_a.png)

More examples will be added in future.

### Advanced Options

By changing [HeuristicFunctionPreference](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/AStar.cs#L53) property, we can give different priority to G(x) function, which is based on [Depth](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/IStep.cs#L11) and H(x) function, which is based on [Key](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/IStep.cs#L7) to adjust the heuristic function. The default value is `Average` which gives both functions same priority.

More advanced options will be added in future version.

### Platform

The project currently targets .Net Core only.
