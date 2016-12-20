using NUnit.Framework;
using System.Linq;

namespace SpaceCentipedeFromHell.Tests
{
    public class AdjacencyMapTest
    {
        [Test]
        public void GeodesicSphereAdjacencyMapTest1()
        {
            var adjacencyMap = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            Assert.AreEqual(320, adjacencyMap.Map.Count, "There must be one entry for each of the 320 vertices of the Geodesic Sphere");
        }

        [Test]
        public void GeodesicSphereAdjacencyMapTest2()
        {
            var adjacencyMap = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var sizes = TestAdjacencyMapBuilder.GetSizes(adjacencyMap);

            Assert.AreEqual(260, sizes.First().Count(), "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereAdjacencyMapTest3()
        {
            var adjacencyMap = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var sizes = TestAdjacencyMapBuilder.GetSizes(adjacencyMap);

            Assert.AreEqual(12, sizes.First().Key, "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereAdjacencyMapTest4()
        {
            var adjacencyMap = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var sizes = TestAdjacencyMapBuilder.GetSizes(adjacencyMap);

            Assert.AreEqual(60, sizes.Last().Count(), "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }

        [Test]
        public void GeodesicSphereAdjacencyMapTest5()
        {
            var adjacencyMap = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var sizes = TestAdjacencyMapBuilder.GetSizes(adjacencyMap);

            Assert.AreEqual(11, sizes.Last().Key, "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }
    }
}