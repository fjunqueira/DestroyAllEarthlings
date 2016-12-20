using System.Linq;
using UnityEngine;
using System;

namespace SpaceCentipedeFromHell
{
    [Serializable]
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
            return this.PlanetGrid.GetNodesAdjacentTo(this);
        }
        public override int GetHashCode()
        {
            return this.Triangle.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var otherNode = obj as PlanetNode;

            if (otherNode == null) return false;

            return this.Triangle.Equals(otherNode.Triangle);
        }
    }
}