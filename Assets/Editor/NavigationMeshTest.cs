using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using SpaceCentipedeFromHell;

namespace SpaceCentipedeFromHell.Tests
{
    public class NavigationMeshTest
    {
        private NavigationMesh BuildNavMesh()
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

        private IOrderedEnumerable<IGrouping<int, KeyValuePair<Triangle, IEnumerable<Triangle>>>> GetSizes(NavigationMesh navMesh)
        {
            return navMesh.AdjacencyMap.GroupBy(x => x.Value.Count()).OrderByDescending(x => x.Key);
        }

        [Test]
        public void GeodesicSphereNavMeshTest1()
        {
            var navMesh = BuildNavMesh();

            var sizes = GetSizes(navMesh);

            Assert.AreEqual(320, navMesh.AdjacencyMap.Count, "There must be one entry for each of the 320 vertices of the Geodesic Sphere");
        }

        [Test]
        public void GeodesicSphereNavMeshTest2()
        {
            var navMesh = BuildNavMesh();

            var sizes = GetSizes(navMesh);

            Assert.AreEqual(260, sizes.First().Count(), "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereNavMeshTest3()
        {
            var navMesh = BuildNavMesh();

            var sizes = GetSizes(navMesh);

            Assert.AreEqual(12, sizes.First().Key, "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereNavMeshTest4()
        {
            var navMesh = BuildNavMesh();

            var sizes = GetSizes(navMesh);

            Assert.AreEqual(60, sizes.Last().Count(), "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereNavMeshTest5()
        {
            var navMesh = BuildNavMesh();

            var sizes = GetSizes(navMesh);

            Assert.AreEqual(11, sizes.Last().Key, "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }
    }
}