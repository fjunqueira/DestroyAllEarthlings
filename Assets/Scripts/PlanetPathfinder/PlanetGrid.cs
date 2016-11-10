﻿using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace SpaceCentipedeFromHell
{
    public class PlanetGrid : IPathfindingGrid<PlanetNode>
    {
        private float planetRadius;

        private Dictionary<PlanetNode, PlanetNode[]> adjacencyIndexing = new Dictionary<PlanetNode, PlanetNode[]>();

        private Dictionary<Vector3, PlanetNode> positionIndexing = new Dictionary<Vector3, PlanetNode>();

        public PlanetGrid(NavigationMesh navMesh, float planetRadius)
        {
            this.planetRadius = planetRadius;

            var keys = navMesh.AdjacencyMap.Keys.Select(key => new PlanetNode(this, key)).ToArray();

            // This is needed because each node reference must be unique for the pathfinder
            var values = navMesh.AdjacencyMap.Values.Select(adjacentTriangles =>
                keys.Where(planetNode => adjacentTriangles.Contains(planetNode.Triangle)).ToArray()).ToArray();

            for (int i = 0; i < keys.Count(); i++) this.adjacencyIndexing.Add(keys[i], values[i]);

            this.positionIndexing = keys.ToDictionary(x => x.Position);

            this.Size = keys.Count();
        }

        public Guid RunId { get; set; }

        public Dictionary<PlanetNode, PlanetNode[]> AdjacencyIndexing { get { return this.adjacencyIndexing; } }

        public Dictionary<Vector3, PlanetNode> PositionIndexing { get { return this.positionIndexing; } }

        public int Size { get; private set; }

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

        public PlanetNode[] GetNodesAdjacentTo(PlanetNode node)
        {
            return this.adjacencyIndexing[node];
        }
    }
}