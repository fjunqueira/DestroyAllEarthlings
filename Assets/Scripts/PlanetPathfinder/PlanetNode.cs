using System.Linq;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class PlanetNode : PathfindingNode
    {
        public PlanetNode(PlanetGrid grid, Triangle triangle)
        {
            this.Triangle = triangle;
            this.PlanetGrid = grid;
        }

        public Triangle Triangle { get; private set; }

        public PlanetGrid PlanetGrid { get; private set; }

        public override Vector3 Position { get { return this.Triangle.Centroid; } }

        public override PathfindingNode[] GetAdjacentNodes()
        {
            return this.PlanetGrid.GetNodesAdjacentTo(this).ToArray();
        }
    }
}