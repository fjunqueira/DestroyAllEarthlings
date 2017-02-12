using System.Collections;
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
            var planetGrid = Resources.Load("EarthGrid") as TextAsset;
            
            using (var stream = new MemoryStream(planetGrid.bytes))
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
            return this.grid.GetNodeByPosition(position);
        }
    }
}