using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SpaceCentipedeFromHell.Tests
{
    public static class TestNavMeshBuilder
    {
        public static NavigationMesh BuildNavMesh()
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

            return new NavigationMesh(meshFilter, new MeshNormalizer());
        }

		public static IOrderedEnumerable<IGrouping<int, KeyValuePair<Triangle, IEnumerable<Triangle>>>> GetSizes(NavigationMesh navMesh)
        {
            return navMesh.AdjacencyMap.GroupBy(x => x.Value.Count()).OrderByDescending(x => x.Key);
		}
    }
}