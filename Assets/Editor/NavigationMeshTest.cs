using NUnit.Framework;
using System.Linq;

namespace SpaceCentipedeFromHell.Tests
{
    public class NavigationMeshTest
    {
        [Test]
        public void GeodesicSphereNavMeshTest1()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            Assert.AreEqual(320, navMesh.AdjacencyMap.Count, "There must be one entry for each of the 320 vertices of the Geodesic Sphere");
        }

        [Test]
        public void GeodesicSphereNavMeshTest2()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var sizes = TestNavMeshBuilder.GetSizes(navMesh);

            Assert.AreEqual(260, sizes.First().Count(), "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereNavMeshTest3()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var sizes = TestNavMeshBuilder.GetSizes(navMesh);

            Assert.AreEqual(12, sizes.First().Key, "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereNavMeshTest4()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var sizes = TestNavMeshBuilder.GetSizes(navMesh);

            Assert.AreEqual(60, sizes.Last().Count(), "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereNavMeshTest5()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var sizes = TestNavMeshBuilder.GetSizes(navMesh);

            Assert.AreEqual(11, sizes.Last().Key, "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }
    }
}