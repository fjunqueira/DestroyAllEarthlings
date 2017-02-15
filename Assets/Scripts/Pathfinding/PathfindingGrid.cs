using System.Collections.Generic;
using System;

namespace DestroyAllEarthlings
{
    public static class PathfindingGrid
    {
        public static TNode[] RunAStar<TNode>(this IPathfindingGrid<TNode> grid, TNode startingNode, TNode destinationNode) where TNode : PathfindingNode
        {
            grid.InitializeExecution(startingNode);

            var binaryHeapOpenList = new BinaryHeap<TNode>(grid.Size, new PathfindingNodeComparator<TNode>());

            var result = RunAStar(binaryHeapOpenList, grid, startingNode, destinationNode);

            return result.PathAsArray();
        }

        public static TNode[] RunDijkstra<TNode>(this IPathfindingGrid<TNode> grid, TNode startingNode) where TNode : PathfindingNode
        {
            grid.InitializeExecution(startingNode);

            var binaryHeapOpenList = new BinaryHeap<TNode>(grid.Size, new PathfindingNodeComparator<TNode>());

            var result = RunDijkstra(binaryHeapOpenList, grid, startingNode);

            return result.PathAsArray();
        }

        private static void InitializeExecution<TNode>(this IPathfindingGrid<TNode> grid, PathfindingNode startingNode) where TNode : PathfindingNode
        {
            grid.RunId = Guid.NewGuid();

            startingNode.ResetInfo(grid.RunId);
        }

        private static TNode[] PathAsArray<TNode>(this TNode node) where TNode : PathfindingNode
        {
            var orderedPath = new Stack<TNode>();

            while (node != null)
            {
                orderedPath.Push(node);

                node = node.ParentNode as TNode;
            }

            return orderedPath.ToArray();
        }

        private static TNode RunAStar<TNode>(BinaryHeap<TNode> binaryHeapOpenList, IPathfindingGrid<TNode> grid, TNode currentNode, TNode destinationNode) where TNode : PathfindingNode
        {
            if (destinationNode.WasVisited && destinationNode.RunId == grid.RunId) return destinationNode;

            var nodeWithLowestCostOnHeap = StepAlgorithm(binaryHeapOpenList, grid, currentNode, destinationNode,
                (current, destination) => grid.GetHeuristic(current, destination));

            return RunAStar(binaryHeapOpenList, grid, nodeWithLowestCostOnHeap, destinationNode);
        }

        private static TNode RunDijkstra<TNode>(BinaryHeap<TNode> binaryHeapOpenList, IPathfindingGrid<TNode> grid, TNode currentNode) where TNode : PathfindingNode
        {
            if (currentNode.IsDestination) return currentNode;

            var nodeWithLowestCostOnHeap = StepAlgorithm(binaryHeapOpenList, grid, currentNode, null, (current, destination) => 0);

            return RunDijkstra(binaryHeapOpenList, grid, nodeWithLowestCostOnHeap);
        }

        private static TNode StepAlgorithm<TNode>(
            BinaryHeap<TNode> binaryHeapOpenList,
            IPathfindingGrid<TNode> grid,
            TNode currentNode,
            TNode destinationNode,
            Func<TNode, TNode, float> heuristic) where TNode : PathfindingNode
        {
            currentNode.WasVisited = true;

            var adjacentNodes = currentNode.GetAdjacentNodes();

            for (int i = 0; i < adjacentNodes.Length; i++)
            {
                var adjacentNode = adjacentNodes[i] as TNode;

                if (adjacentNode != null && adjacentNode.IsWalkable && (!adjacentNode.WasVisited || adjacentNode.RunId != grid.RunId))
                {
                    if (!adjacentNode.IsOnOpenList || adjacentNode.RunId != grid.RunId)
                    {
                        adjacentNode.RunId = grid.RunId;
                        adjacentNode.ParentNode = currentNode;

                        adjacentNode.G = currentNode.G + grid.GetPartialPathCost(currentNode, (TNode)adjacentNode);

                        adjacentNode.H = heuristic(adjacentNode, destinationNode);

                        binaryHeapOpenList.Push(adjacentNode);
                        adjacentNode.IsOnOpenList = true;
                    }
                    else if (adjacentNode.IsOnOpenList && adjacentNode.RunId == grid.RunId)
                    {
                        var g = currentNode.G + grid.GetPartialPathCost(currentNode, (TNode)adjacentNode);

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

            if (binaryHeapOpenList.IsEmpty) return null;

            var nodeWithLowestCostOnHeap = binaryHeapOpenList.Pop();
            nodeWithLowestCostOnHeap.IsOnOpenList = false;

            return nodeWithLowestCostOnHeap;
        }
    }
}