using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace SpaceCentipedeFromHell
{
    /// <summary>
    /// Extracts a navigation mesh given a mesh, only works triangle meshes
    /// </summary>
    public class NavigationMesh
    {
        private Dictionary<Triangle, IEnumerable<Triangle>> adjacencyMap;

        public NavigationMesh(MeshFilter filter, MeshNormalizer normalizer)
        {
            var mesh = filter.sharedMesh;

            var normalizedMesh = normalizer.Normalize(mesh);

            var triangles = normalizedMesh.Triangles.ChunksOf(3).ToList();

            this.adjacencyMap = triangles
                .Select(triangle => new KeyValuePair<Triangle, IEnumerable<Triangle>>(new Triangle(IndexToVertex(triangle, normalizedMesh)),
                triangles.Where(otherTriangle => otherTriangle != triangle &&
                     (otherTriangle.Contains(triangle.ElementAt(0)) ||
                     otherTriangle.Contains(triangle.ElementAt(1)) ||
                     otherTriangle.Contains(triangle.ElementAt(2)))).Select(otherTriangle => new Triangle(IndexToVertex(otherTriangle, normalizedMesh))))).ToDictionary();
        }

        private IEnumerable<Vector3> IndexToVertex(IEnumerable<int> indexes, NormalizedMesh mesh)
        {
            return indexes.Select(verticeIndex => mesh.Vertices.ElementAt(verticeIndex));
        }

        public Dictionary<Triangle, IEnumerable<Triangle>> AdjacencyMap
        {
            get { return this.adjacencyMap; }
        }
    }
}