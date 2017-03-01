using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmForce.Example.TwoZeroFourEight
{
    using HeuristicSuite;

    public static class Helper
    {
        public const int BoardSize = 4;

        private static readonly Random rand = new Random(Environment.TickCount);

        #region Main

        public static int[][] InitalizeArray()
        {
            var board = Enumerable.Repeat(BoardSize, BoardSize).Select(size => new int[size]).ToArray();

            PutNumberOnArray(board, 2);
            PutNumberOnArray(board, 2);

            return board;
        }

        public static Point2DInt32? PutNumberOnArray(int[][] board, int? number = null)
        {
            if (CountPlaces(board) == BoardSize * BoardSize) // no empty place to put new number
                return null;

            var pos = new Point2DInt32(rand.Next(BoardSize), rand.Next(BoardSize));

            while (board[pos.Y][pos.X] != 0)
                pos = new Point2DInt32(rand.Next(BoardSize), rand.Next(BoardSize));

            if (number == null)
                number = rand.Next(4) != 1 ? 2 : 4; // 3/4 chance to get 2, 1/4 chance to get 4

            board[pos.Y][pos.X] = number.Value;

            return pos;
        }

        #endregion

        #region Moves

        public static bool MoveUp(int[][] array, int col)
        {
            return MoveLeft(new ColumnWrapper<int>(array, col));
        }

        public static bool MoveDown(int[][] array, int col)
        {
            return MoveRight(new ColumnWrapper<int>(array, col));
        }

        public static bool MoveLeft(IList<int> row)
        {
            // Phase 1: merge numbers
            var col = -1;
            var length = row.Count;
            var modified = false;

            for (var y = 0; y < length; y++)
            {
                if (row[y] == 0)
                    continue;

                if (col == -1)
                {
                    col = y; // remember current col
                    continue;
                }
                if (col != -1 && row[col] != row[y])
                {
                    col = y; // update
                    continue;
                }
                if (col != -1 && row[col] == row[y])
                {
                    row[col] += row[y]; // merge same numbers
                    row[y] = 0;
                    col = -1; // reset
                    modified = true;
                }
            }
            // Phase 2: move numbers
            for (var i = 0; i < length * length; i++)
            {
                var y = i % length;

                if (y == length - 1) continue;
                if (row[y] == 0 && row[y + 1] != 0) // current is empty and next is not 
                {
                    row[y] = row[y + 1]; // move next to current
                    row[y + 1] = 0;
                    modified = true;
                }
            }
            return modified;
        }

        public static bool MoveRight(IList<int> row)
        {
            // Phase 1: merge numbers
            var col = -1;
            var length = row.Count;
            var modified = false;

            for (var y = length - 1; y >= 0; y--)
            {
                if (row[y] == 0)
                    continue;

                if (col == -1)
                {
                    col = y; // remember current col
                    continue;
                }
                if (col != -1 && row[col] != row[y])
                {
                    col = y; // update
                    continue;
                }
                if (col != -1 && row[col] == row[y])
                {
                    row[col] += row[y]; // merge same numbers
                    row[y] = 0;
                    col = -1; // reset
                    modified = true;
                }
            }
            // Phase 2: move numbers
            for (var i = length * length - 1; i >= 0; i--)
            {
                var y = i % length;

                if (y == 0) continue;
                if (row[y] == 0 && row[y - 1] != 0) // current is empty and next is not 
                {
                    row[y] = row[y - 1]; // move next to current
                    row[y - 1] = 0;
                    modified = true;
                }
            }
            return modified;
        }

        #endregion

        #region Others

        public static int CountPlaces(int[][] array)
        {
            return array.SelectMany(row => row).Count(num => num != 0);
        }

        public static int GetLargestNumber(int[][] array)
        {
            return array.SelectMany(row => row).Max();
        }

        public static bool HasEmptyPlace(int[][] array)
        {
            return array.SelectMany(row => row).Contains(0);
        }

        public static int[][] Clone(int[][] array)
        {
            return array.Select(row => row.ToArray()).ToArray();
        }

        public static string Print(int[][] array)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < array.Length; y++)
            {
                for (var x = 0; x < array[y].Length; x++)
                    sb.AppendFormat(" {0} ", array[y][x] == 0 ? "_" : Convert.ToString(array[y][x]));

                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion
    }
}
