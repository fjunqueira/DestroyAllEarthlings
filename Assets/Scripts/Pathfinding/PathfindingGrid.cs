using System.Collections.Generic;
using System;

namespace SpaceCentipedeFromHell
{
    public static class PathfindingGrid
    {
        private static Guid runId;
        private static PathfindingNodeComparator comparator = new PathfindingNodeComparator();

        public static PathfindingNode[] FindPath<TNode>(this IPathfindingGrid<TNode> grid, PathfindingNode startingNode, PathfindingNode destinationNode) where TNode : PathfindingNode
        {
            return FindPath(grid, startingNode, destinationNode, grid.Size);
        }

        public static PathfindingNode[] FindPath<TNode>(this IPathfindingGrid<TNode> grid, PathfindingNode startingNode, PathfindingNode destinationNode, int nodeSearchLimit) where TNode : PathfindingNode
        {
            runId = Guid.NewGuid();

            var binaryHeapOpenList = new BinaryHeap<PathfindingNode>(grid.Size, comparator);

            startingNode.ResetInfo(runId);

            var currentNode = FindNextNode(binaryHeapOpenList, grid, startingNode, destinationNode, nodeSearchLimit, 0);

            var orderedPath = new Stack<PathfindingNode>();

            while (currentNode != null)
            {
                orderedPath.Push(currentNode);

                currentNode = currentNode.ParentNode;
            }

            return orderedPath.ToArray();
        }

        private static PathfindingNode FindNextNode<TNode>(BinaryHeap<PathfindingNode> binaryHeapOpenList, IPathfindingGrid<TNode> grid, PathfindingNode currentNode, PathfindingNode destinationNode, int nodeSearchLimit, int currentVisited) where TNode : PathfindingNode
        {
            if (destinationNode.WasVisited && destinationNode.RunId == runId)
            {
                return destinationNode;
            }

            currentNode.WasVisited = true;

            var adjacentNodes = currentNode.GetAdjacentNodes();

            for (int i = 0; i < adjacentNodes.Length; i++)
            {
                var adjacentNode = adjacentNodes[i];

                if (adjacentNode != null && adjacentNode.IsWalkable && (!adjacentNode.WasVisited || adjacentNode.RunId != runId))
                {
                    if (!adjacentNode.IsOnOpenList || adjacentNode.RunId != runId)
                    {
                        adjacentNode.RunId = runId;
                        adjacentNode.ParentNode = currentNode;

                        adjacentNode.G = currentNode.G + grid.GetPartialPathCost((TNode)currentNode, (TNode)adjacentNode);

                        adjacentNode.H = grid.GetHeuristic((TNode)adjacentNode, (TNode)destinationNode);

                        binaryHeapOpenList.Push(adjacentNode);
                        adjacentNode.IsOnOpenList = true;
                    }
                    else if (adjacentNode.IsOnOpenList && adjacentNode.RunId == runId)
                    {
                        var g = currentNode.G + grid.GetPartialPathCost((TNode)currentNode, (TNode)adjacentNode);

                        if (g < adjacentNode.G)
                        {
                            binaryHeapOpenList.Remove(adjacentNode);

                            adjacentNode.ParentNode = currentNode;
                            adjacentNode.G = g;

                            binaryHeapOpenList.Push(adjacentNode);
                        }
                    }
                }
            }

            if (binaryHeapOpenList.IsEmpty || currentVisited + 1 > nodeSearchLimit)
            {
                return null;
            }

            var nodeWithLowestCostOnHeap = binaryHeapOpenList.Pop();
            nodeWithLowestCostOnHeap.IsOnOpenList = false;

            return FindNextNode(binaryHeapOpenList, grid, nodeWithLowestCostOnHeap, destinationNode, nodeSearchLimit, ++currentVisited);
        }
    }
}