﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(MeshFilter))]
    public class PlanetNavMesh : MonoBehaviour
    {
        private PlanetGrid grid;

        public string MeshName { get; set; }

        private void Awake()
        {
            using (var stream = new FileStream("Assets/Grids/" + this.MeshName, FileMode.Open))
            {
                var formatter = Formatter.CreateFormatter();
                this.grid = formatter.Deserialize(stream) as PlanetGrid;
            }
        }

        public PathfindingNode[] FindPath(PathfindingNode startingNode)
        {
            return this.grid.RunDijkstra(startingNode);
        }

        public PathfindingNode GetNodeByPosition(Vector3 position)
        {
            Debug.Log(this.grid);
            return this.grid.GetNodeByPosition(position);
        }
    }
}