using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace DestroyAllEarthlings.Tests
{
    ///<summary>
    /// This class has the same tests as the NavigationMeshTest, but here we are testing the indexed data
    ///</summary>
    public class PlanetGridTest
    {
        public static List<IGrouping<int, KeyValuePair<PlanetNode, PlanetNode[]>>> GetSizes(PlanetGrid navMesh)
        {
            return navMesh.AdjacencyIndexing.GroupBy(x => x.Value.Count()).ToList().OrderByDescending(x => x.Key).ToList();
        }

        [Test]
        public void GeodesicPlanetGrid1()
        {
            var navMesh = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var grid = new PlanetGrid(navMesh, 1);

            Assert.AreEqual(320, grid.AdjacencyIndexing.Count, "There must be one entry for each of the 320 vertices of the Geodesic Sphere");
        }

        [Test]
        public void GeodesicPlanetGrid2()
        {
            var navMesh = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(260, sizes.First().Count(), "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicPlanetGrid3()
        {
            var navMesh = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(12, sizes.First().Key, "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicPlanetGrid4()
        {
            var navMesh = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(60, sizes.Last().Count(), "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }

        [Test]
        public void GeodesicPlanetGrid5()
        {
            var navMesh = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(11, sizes.Last().Key, "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }

        [Test]
        public void FindPathOnGeodesicSphereSurface1()
        {
            var navMesh = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var grid = new PlanetGrid(navMesh, 1);

            var startingNode = grid.AdjacencyIndexing.ElementAt(0).Key;
            var endingNode = grid.AdjacencyIndexing.ElementAt(100).Key;

            var path = grid.RunAStar(startingNode, endingNode);

            Assert.IsNotNull(path);
            Assert.IsNotEmpty(path);
        }


        [Test]
        public void FindPathOnGeodesicSphereSurface2()
        {
            var navMesh = TestAdjacencyMapBuilder.BuildAdjacencyMap();

            var grid = new PlanetGrid(navMesh, 1);

            var startingNode = grid.AdjacencyIndexing.ElementAt(0).Key;
            var endingNode = grid.AdjacencyIndexing.ElementAt(100).Key;

            var path = grid.RunAStar(startingNode, endingNode);

            Assert.AreEqual(startingNode, path.First(), "The first node in the path should be the starting node");
            Assert.AreEqual(endingNode, path.Last(), "The last node in the path should be the ending node");
        }
    }
}