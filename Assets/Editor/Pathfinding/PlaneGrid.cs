using SpaceCentipedeFromHell;
using System;
using UnityEngine;

namespace SpaceCentipedeFromHell.Tests
{
    public class PlaneGrid : IPathfindingGrid<SquareNode>
    {
        private SquareNode[,] grid;
        private int size;
        private float squareSize;
        private float halfSquareSize;
        private int columns;
        private int rows;

        public PlaneGrid(int rows, int columns, float squareSize)
        {
            this.rows = rows;
            this.columns = columns;
            this.size = rows * columns;
            this.squareSize = squareSize;
            this.halfSquareSize = squareSize / 2;
            this.grid = new SquareNode[columns, rows];
            this.AddSquaresToGrid(rows, columns);
        }

        private enum SearchType
        {
            Row,
            Column
        }

        public int Size
        {
            get { return this.size; }
        }

        public float GetHeuristic(SquareNode from, SquareNode to)
        {
            var rowMovementAmmount = Math.Abs(to.RowIndex - from.RowIndex);
            var columnMovementAmmount = Math.Abs(to.ColumnIndex - from.ColumnIndex);

            return (rowMovementAmmount + columnMovementAmmount) * 10;
        }

        // TODO: Fix cost calculation
        public float GetPartialPathCost(SquareNode from, SquareNode to)
        {
            var adjacentNodes = from.GetAdjacentNodes();

            int i;
            for (i = 0; i < adjacentNodes.Length; i++)
            {
                if (to == adjacentNodes[i])
                {
                    break;
                }
            }

            if (i == 0 || i == 2 || i == 5 || i == 7)
            {
                return 14 + to.Weight;
            }

            return 10 + to.Weight;
        }

        public SquareNode GetNodeByPosition(Vector3 position)
        {
            int column = this.MatrixBinarySearch(position.z, SearchType.Row);
            int row = this.MatrixBinarySearch(position.x, SearchType.Column);

            if (row < 0 || column < 0)
            {
                return null;
            }

            return this.grid[column, row];
        }

        public SquareNode GetNodeByIndex(int row, int column)
        {
            if ((row >= 0 && row <= this.rows) && (column >= 0 && column <= this.columns))
            {
                return this.grid[column, row];
            }

            return null;
        }

        private int MatrixBinarySearch(float position, SearchType searchType)
        {
            var currentSearchPosition = 0f;

            var min = 0;
            var mid = 0;

            var max = searchType == SearchType.Column ? this.columns : this.rows;

            while (max >= min)
            {
                mid = (min + max) / 2;

                currentSearchPosition = searchType == SearchType.Column ? this.grid[mid, this.rows - 1].Position.x : this.grid[this.columns - 1, mid].Position.z;

                var intervalBeginning = currentSearchPosition - this.halfSquareSize;
                var intervalEnding = currentSearchPosition + this.halfSquareSize;

                if (position <= intervalEnding && position >= intervalBeginning)
                {
                    return mid;
                }

                if (position <= intervalBeginning)
                {
                    max = mid - 1;
                }

                if (position >= intervalEnding)
                {
                    min = mid + 1;
                }
            }

            return -1;
        }

        private void AddSquaresToGrid(int rows, int columns)
        {
            var verticalSpacing = 0f;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    this.grid[x, y] = new SquareNode(new Vector3(x * this.squareSize, 0, verticalSpacing), y, x, this);
                }

                verticalSpacing += this.squareSize;
            }
        }
    }
}