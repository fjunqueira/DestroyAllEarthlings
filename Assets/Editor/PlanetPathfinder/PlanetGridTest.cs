using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace SpaceCentipedeFromHell.Tests
{
    ///<summary>
    /// This class has the same tests as the NavigationMeshTest, but here we are testing the indexed data
    ///</summary>
    public class PlanetGridTest
    {
        public static IOrderedEnumerable<IGrouping<int, KeyValuePair<PlanetNode, IEnumerable<PlanetNode>>>> GetSizes(PlanetGrid navMesh)
        {
            return navMesh.PlanetAdjacencyMap.GroupBy(x => x.Value.Count()).OrderByDescending(x => x.Key);
        }

        [Test]
        public void GeodesicPlanetGrid1()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var grid = new PlanetGrid(navMesh, 1);

            Assert.AreEqual(320, grid.PlanetAdjacencyMap.Count, "There must be one entry for each of the 320 vertices of the Geodesic Sphere");
        }

        [Test]
        public void GeodesicPlanetGrid2()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(260, sizes.First().Count(), "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicPlanetGrid3()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(12, sizes.First().Key, "260 vertices out of the 320 in the mesh should have 12 neighbouring triangles");
        }

        [Test]
        public void GeodesicPlanetGrid4()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(60, sizes.Last().Count(), "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }

        [Test]
        public void GeodesicPlanetGrid5()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var grid = new PlanetGrid(navMesh, 1);

            var sizes = GetSizes(grid);

            Assert.AreEqual(11, sizes.Last().Key, "60 vertices out of the 320 in the mesh should have 11 neighbouring triangles");
        }

        [Test]
        public void FindPathOnGeodesicSphereSurface()
        {
            var navMesh = TestNavMeshBuilder.BuildNavMesh();

            var grid = new PlanetGrid(navMesh, 1);

            var startingNode = grid.PlanetAdjacencyMap.ElementAt(0).Key;
            var endingNode = grid.PlanetAdjacencyMap.ElementAt(10).Key;

            var path = grid.FindPath(startingNode, endingNode);

            Assert.IsNotNull(path);
            Assert.IsNotEmpty(path);

            Debug.Log(path.Length);
        }
    }
}