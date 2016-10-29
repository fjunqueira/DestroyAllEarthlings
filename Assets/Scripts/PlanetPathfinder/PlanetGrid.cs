using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace SpaceCentipedeFromHell
{
    public class PlanetGrid : IPathfindingGrid<PlanetNode>
    {
        private float planetRadius;

        private Dictionary<PlanetNode, IEnumerable<PlanetNode>> planetAdjacencyMap = new Dictionary<PlanetNode, IEnumerable<PlanetNode>>();

        public PlanetGrid(NavigationMesh navMesh, float planetRadius)
        {
            this.planetRadius = planetRadius;

            var keys = navMesh.AdjacencyMap.Keys.Select(key => new PlanetNode(this, key)).ToList();

            // This is needed because each node reference must be unique for the pathfinder
            var values = navMesh.AdjacencyMap.Values.Select(adjacentTriangles =>
                keys.Where(planetNode => adjacentTriangles.Contains(planetNode.Triangle))).ToList();

            for (int i = 0; i < keys.Count(); i++) this.planetAdjacencyMap.Add(keys[i], values[i]);
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