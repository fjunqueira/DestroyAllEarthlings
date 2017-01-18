using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace DestroyAllEarthlings.Tests
{
    public static class TestAdjacencyMapBuilder
    {
        public static MeshAdjacencyMap BuildAdjacencyMap()
        {
            var sphere = GeodesicSphere.InitializeSphere(2);

            var mesh = new Mesh()
            {
                vertices = sphere.ToArray(),
                triangles = Enumerable.Range(0, sphere.Count).ToArray()
            };

            var gameObject = new GameObject();
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            return new MeshAdjacencyMap(new MeshNormalizer().Normalize(meshFilter.sharedMesh));
        }

		public static IOrderedEnumerable<IGrouping<int, KeyValuePair<Triangle, Triangle[]>>> GetSizes(MeshAdjacencyMap adjacencyMap)
        {
            return adjacencyMap.Map.GroupBy(x => x.Value.Count()).OrderByDescending(x => x.Key);
		}
    }
}