# Heuristic Suite

### Overview

**Heuristic Suite** is an experimental implementation of A* algorithm in generic programming. The project is aimed to define a unified engine from abstract perspective for various scenarios that can use A* algorithm to solve the puzzle.

### Quick Start

Following information will be required to utilize the engine:

1. **The type of step in the puzzle**

   The type will be required to implement [IStep(TKey)](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/IStep.cs) interface. `Key` is the property used to check the equality, and compare between two steps.

2. **The method to compare steps**

   Steps will be compared with each other by `Key` property to determine which has better score. This can be done by implementing [IComparable(TKey)](https://msdn.microsoft.com/en-us/library/4d7sx9hd.aspx) or providing [IComparer(TKey)](https://msdn.microsoft.com/en-us/library/8ehhxeaf.aspx) instance.

3. **The method to get next steps from current step** 

   Next step information can be given by implementing [INextStepFactory(TKey, TStep)](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/IStep.cs) interface, or providing [NextStepFactory](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/AStar.cs#L29) delegate.

Now we can call [Execute](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/AStar.cs#L87) method with the `from` and `goal` steps to get the solution. If the solution exists, you can [Enumerate](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/StepExtensions.cs#L7) each of steps, or [Reverse](https://github.com/rvhuang/heuristic-suite/blob/master/AlgorithmForce.HeuristicSuite/StepExtensions.cs#L19) them before the enumeration starts.

### Examples

* Path Finding ([AlgorithmForce.Example.PathFinding](https://github.com/rvhuang/heuristic-suite/tree/master/AlgorithmForce.Example.PathFinding))

   The example shows the most common and traditional puzzle that use A* algorithm to solve.

* 8-Puzzle ([AlgorithmForce.Example.EightPuzzle](https://github.com/rvhuang/heuristic-suite/tree/master/AlgorithmForce.Example.EightPuzzle))

   The example shows how to solve the 8-puzzle with A* algorithm, as following figures show:
   
   ![From](http://www.8puzzle.com/images/8_puzzle_start_state_a.png)
   ![Goal](http://www.8puzzle.com/images/8_puzzle_goal_state_a.png)

### Platform

The project currently targets .Net Core only.
