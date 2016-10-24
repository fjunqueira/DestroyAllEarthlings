using SpaceCentipedeFromHell;
using UnityEngine;

namespace SpaceCentipedeFromHell.Tests
{
    public class SquareNode : PathfindingNode
    {
        private Vector3 position;
        private int columnIndex;
        private int rowIndex;
        private PlaneGrid parentGrid;

        public SquareNode(Vector3 position, int rowIndex, int columnIndex, PlaneGrid parentGrid)
        {
            this.position = position;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.parentGrid = parentGrid;
            this.IsWalkable = true;
        }

        public int RowIndex
        {
            get
            {
                return this.rowIndex;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return this.columnIndex;
            }
        }

        public override Vector3 Position
        {
            get
            {
                return this.position;
            }
        }

        public override PathfindingNode[] GetAdjacentNodes()
        {
            return new PathfindingNode[]
            {
                this.parentGrid.GetNodeByIndex(this.rowIndex - 1, this.columnIndex - 1),
                this.parentGrid.GetNodeByIndex(this.rowIndex - 1, this.columnIndex),
                this.parentGrid.GetNodeByIndex(this.rowIndex - 1, this.columnIndex + 1),
                this.parentGrid.GetNodeByIndex(this.rowIndex, this.columnIndex - 1),
                this.parentGrid.GetNodeByIndex(this.rowIndex, this.columnIndex + 1),
                this.parentGrid.GetNodeByIndex(this.rowIndex + 1, this.columnIndex - 1),
                this.parentGrid.GetNodeByIndex(this.rowIndex + 1, this.columnIndex),
                this.parentGrid.GetNodeByIndex(this.rowIndex + 1, this.columnIndex + 1),
            };
        }
    }
}