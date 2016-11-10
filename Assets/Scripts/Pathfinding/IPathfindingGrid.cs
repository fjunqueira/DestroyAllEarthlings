using System;

namespace SpaceCentipedeFromHell
{
    public interface IPathfindingGrid<TNode> where TNode : PathfindingNode
    {
        Guid RunId { get; set; }

        int Size { get; }

        float GetHeuristic(TNode from, TNode to);

        float GetPartialPathCost(TNode from, TNode to);
    }

    public interface IPathfindingGrid : IPathfindingGrid<PathfindingNode>
    {
    }
}