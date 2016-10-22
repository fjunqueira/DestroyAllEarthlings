using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MeshNormalizer
{
    /// <summary>
    /// Mesh normalizing functions
    /// </summary>
    /// <param name="mesh">A mesh containing the vertices and triangles</param>
    /// <returns>The normalized mesh</returns>
    public NormalizedMesh Normalize(Mesh mesh)
    {
        //Get a list of vertices and it's indexes
        var vertexIndex = mesh.vertices.Zip(Enumerable.Range(0, mesh.vertexCount), (vertex, index) => new { vertex, index }).ToList();

        var vertexGroup =
            //Group all replicated vertices
            vertexIndex.GroupBy(x => x.vertex)
            //Assign a new index to each
            .Zip(Enumerable.Range(0, mesh.vertexCount), (group, index) => new { group, index })
            //Returns a projection containing the vertex, the new index, and all the old indexes (to be replaced)
            .Select(x => new VertexGroup { Vertex = x.group.Key, Index = x.index, Replaced = x.group.Select(y => y.index) }).ToList();

        //Creates a new vertex list, with no replicated vertices
        var newVertexList = vertexGroup.Select(x => x.Vertex).ToList();

        //Creates a new triangle list, using the new indexes
        var newTrianglesList = this.UpdateTriangleList(mesh.triangles, vertexGroup);

        var normalizedMesh = new NormalizedMesh()
        {
            Vertices = newVertexList.ToArray(),
            Triangles = newTrianglesList.ToArray()
        };
        //Create tests for this using a geodesic sphere
        return normalizedMesh;
    }

    private List<int> UpdateTriangleList(int[] triangles, IEnumerable<VertexGroup> vertexGroup)
    {
        return triangles.Select(id =>
        {
            foreach (var group in vertexGroup) if (group.Replaced.Contains(id)) return group.Index;
            return id;
        }).ToList();
    }

    private class VertexGroup
    {
        public Vector3 Vertex { get; set; }
        public int Index { get; set; }
        public IEnumerable<int> Replaced { get; set; }
    }
}

public class NormalizedMesh
{
    public Vector3[] Vertices { get; set; }
    public int[] Triangles { get; set; }
}