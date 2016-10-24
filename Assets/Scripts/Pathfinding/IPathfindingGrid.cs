using UnityEngine;
using System.Collections;

namespace SpaceCentipedeFromHell
{
    public interface IPathfindingGrid<TNode> where TNode : PathfindingNode
    {
        int Size { get; }

        float GetHeuristic(TNode from, TNode to);

        float GetPartialPathCost(TNode from, TNode to);
    }

    public interface IPathfindingGrid : IPathfindingGrid<PathfindingNode>
    {
    }
}