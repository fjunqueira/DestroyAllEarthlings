using System.Collections.Generic;
using System;

namespace DestroyAllEarthlings
{
    public static class PathfindingGrid
    {
        private static PathfindingNodeComparator comparator = new PathfindingNodeComparator();

        public static PathfindingNode[] FindPath<TNode>(this IPathfindingGrid<TNode> grid, PathfindingNode startingNode, PathfindingNode destinationNode) where TNode : PathfindingNode
        {
            return FindPath(grid, startingNode, destinationNode, grid.Size);
        }

        public static PathfindingNode[] FindPath<TNode>(this IPathfindingGrid<TNode> grid, PathfindingNode startingNode, PathfindingNode destinationNode, int maxIterations) where TNode : PathfindingNode
        {
            grid.RunId = Guid.NewGuid();

            var binaryHeapOpenList = new BinaryHeap<PathfindingNode>(grid.Size, comparator);

            startingNode.ResetInfo(grid.RunId);

            var currentNode = FindNextNode(binaryHeapOpenList, grid, startingNode, destinationNode, maxIterations, 0);

            var orderedPath = new Stack<PathfindingNode>();

            while (currentNode != null)
            {
                orderedPath.Push(currentNode);

                currentNode = currentNode.ParentNode;
            }

            return orderedPath.ToArray();
        }

        private static PathfindingNode FindNextNode<TNode>(BinaryHeap<PathfindingNode> binaryHeapOpenList, IPathfindingGrid<TNode> grid, PathfindingNode currentNode, PathfindingNode destinationNode, int maxIterations, int currentIteration) where TNode : PathfindingNode
        {
            if (destinationNode.WasVisited && destinationNode.RunId == grid.RunId)
            {
                return destinationNode;
            }

            currentNode.WasVisited = true;

            var adjacentNodes = currentNode.GetAdjacentNodes();

            for (int i = 0; i < adjacentNodes.Length; i++)
            {
                var adjacentNode = adjacentNodes[i];

                if (adjacentNode != null && adjacentNode.IsWalkable && (!adjacentNode.WasVisited || adjacentNode.RunId != grid.RunId))
                {
                    if (!adjacentNode.IsOnOpenList || adjacentNode.RunId != grid.RunId)
                    {
                        adjacentNode.RunId = grid.RunId;
                        adjacentNode.ParentNode = currentNode;

                        adjacentNode.G = currentNode.G + grid.GetPartialPathCost((TNode)currentNode, (TNode)adjacentNode);

                        adjacentNode.H = grid.GetHeuristic((TNode)adjacentNode, (TNode)destinationNode);

                        binaryHeapOpenList.Push(adjacentNode);
                        adjacentNode.IsOnOpenList = true;
                    }
                    else if (adjacentNode.IsOnOpenList && adjacentNode.RunId == grid.RunId)
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

            if (binaryHeapOpenList.IsEmpty || currentIteration + 1 > maxIterations)
            {
                return null;
            }

            var nodeWithLowestCostOnHeap = binaryHeapOpenList.Pop();
            nodeWithLowestCostOnHeap.IsOnOpenList = false;

            return FindNextNode(binaryHeapOpenList, grid, nodeWithLowestCostOnHeap, destinationNode, maxIterations, ++currentIteration);
        }

        public static PathfindingNode[] RunDijkstra<TNode>(this IPathfindingGrid<TNode> grid, PathfindingNode startingNode) where TNode : PathfindingNode
        {
            grid.RunId = Guid.NewGuid();

            var binaryHeapOpenList = new BinaryHeap<PathfindingNode>(grid.Size, comparator);

            startingNode.ResetInfo(grid.RunId);

            var currentNode = RunDijkstra(binaryHeapOpenList, grid, startingNode, grid.Size, 0);

            var orderedPath = new Stack<PathfindingNode>();

            while (currentNode != null)
            {
                orderedPath.Push(currentNode);

                currentNode = currentNode.ParentNode;
            }

            return orderedPath.ToArray();
        }

        private static PathfindingNode RunDijkstra<TNode>(BinaryHeap<PathfindingNode> binaryHeapOpenList, IPathfindingGrid<TNode> grid, PathfindingNode currentNode, int maxIterations, int currentIteration) where TNode : PathfindingNode
        {
            currentNode.WasVisited = true;

            if (currentNode.IsDestination) return currentNode;

            var adjacentNodes = currentNode.GetAdjacentNodes();

            for (int i = 0; i < adjacentNodes.Length; i++)
            {
                var adjacentNode = adjacentNodes[i];

                if (adjacentNode != null && adjacentNode.IsWalkable && (!adjacentNode.WasVisited || adjacentNode.RunId != grid.RunId))
                {
                    if (!adjacentNode.IsOnOpenList || adjacentNode.RunId != grid.RunId)
                    {
                        adjacentNode.RunId = grid.RunId;
                        adjacentNode.ParentNode = currentNode;

                        adjacentNode.G = currentNode.G + grid.GetPartialPathCost((TNode)currentNode, (TNode)adjacentNode);

                        adjacentNode.H = 0;

                        binaryHeapOpenList.Push(adjacentNode);
                        adjacentNode.IsOnOpenList = true;
                    }
                    else if (adjacentNode.IsOnOpenList && adjacentNode.RunId == grid.RunId)
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

            if (binaryHeapOpenList.IsEmpty || currentIteration + 1 > maxIterations)
            {
                return null;
            }

            var nodeWithLowestCostOnHeap = binaryHeapOpenList.Pop();
            nodeWithLowestCostOnHeap.IsOnOpenList = false;

            return RunDijkstra(binaryHeapOpenList, grid, nodeWithLowestCostOnHeap, maxIterations, ++currentIteration);
        }
    }
}