using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SpaceCentipedeFromHell
{
    public class PlanetNode : PathfindingNode
    {
        private Triangle triangle;

        public PlanetGrid PlanetGrid { get; private set; }

        public PlanetNode(PlanetGrid grid, Triangle triangle)
        {
            this.triangle = triangle;
            this.PlanetGrid = grid;
        }

        public override Vector3 Position
        {
            get
            {
                return this.triangle.Centroid;
            }
        }

        public override PathfindingNode[] GetAdjacentNodes()
        {
            return this.PlanetGrid.GetNodesAdjacentTo(this).ToArray();
        }

        public override int GetHashCode()
        {
            return this.triangle.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            PlanetNode otherNode = obj as PlanetNode;

            if (otherNode == null) return false;

            return this.triangle.Equals(otherNode.triangle);
        }
    }
}