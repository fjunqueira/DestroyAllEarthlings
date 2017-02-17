using NUnit.Framework;

namespace DestroyAllEarthlings.Tests
{
    public class PathfinderTest
    {

        [Test]
        public void AssertPathIsCalculatedCorrectly()
        {
            var grid = new PlaneGrid(1000, 1000, 1);
            grid.GetNodeByIndex(498, 500).RaiseWeight(0.00002f);
            for (int i = 0; i < 3; i++)
            {
                grid.GetNodeByIndex(499 + i, 500).IsWalkable = false;
            }

            var startingNode = grid.GetNodeByIndex(500, 498);
            var endingNode = grid.GetNodeByIndex(500, 502);

            var result = grid.RunAStar(startingNode, endingNode);

            var expectedResult = new PathfindingNode[]
            {
                 grid.GetNodeByIndex(500, 498),
                 grid.GetNodeByIndex(501, 499),
                 grid.GetNodeByIndex(502, 500),
                 grid.GetNodeByIndex(501, 501),
                 grid.GetNodeByIndex(500, 502),
            };

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}