using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SpaceCentipedeFromHell
{
    public class PlanetGrid : IPathfindingGrid<PlanetNode>
    {
        private float planetRadius;

        private readonly Dictionary<PlanetNode, IEnumerable<PlanetNode>> planetAdjacencyMap;

        public PlanetGrid(NavigationMesh navMesh, float planetRadius)
        {
            this.planetRadius = planetRadius;

            //Change the navmesh indexing key to the triangle centroid and use the data at the navmesh instead of this
            this.planetAdjacencyMap =
                navMesh.AdjacencyMap.Select(map => new KeyValuePair<PlanetNode, IEnumerable<PlanetNode>>(
                    new PlanetNode(this, map.Key), map.Value.Select(triangle => new PlanetNode(this, triangle)))).ToDictionary();
        }

        public Dictionary<PlanetNode, IEnumerable<PlanetNode>> PlanetAdjacencyMap
        {
            get
            {
                return this.planetAdjacencyMap;
            }
        }

        public int Size
        {
            get
            {
                return this.planetAdjacencyMap.Keys.Count;
            }
        }

        public float GetHeuristic(PlanetNode from, PlanetNode to)
        {
            return this.GetDistance(from, to);
        }

        public float GetPartialPathCost(PlanetNode from, PlanetNode to)
        {
            // TODO: Replace with constants for improved performance
            return this.GetDistance(from, to);
        }

        private float GetDistance(PathfindingNode from, PathfindingNode to)
        {
            return Mathf.Atan2(Vector3.Cross(from.Position.normalized, to.Position.normalized).magnitude, Vector3.Dot(from.Position.normalized, to.Position.normalized)) * this.planetRadius;
        }

        public IEnumerable<PlanetNode> GetNodesAdjacentTo(PlanetNode node)
        {
            return this.planetAdjacencyMap[node];
        }
    }
}