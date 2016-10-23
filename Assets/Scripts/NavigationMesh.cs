using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SpaceCentipedeFromHell
{
    /// <summary>
    /// Extracts a navigation mesh given a mesh, only works triangle meshes
    /// </summary>
    public class NavigationMesh
    {

        private List<KeyValuePair<Triangle, IEnumerable<Triangle>>> adjacencyMap;

        public NavigationMesh(MeshFilter meshFilter, MeshNormalizer normalizer)
        {
            var mesh = meshFilter.mesh;

            var normalizedMesh = normalizer.Normalize(mesh);

            var triangles = normalizedMesh.Triangles.ChunksOf(3).ToList();

            this.adjacencyMap = triangles
                .Select(triangle => new KeyValuePair<Triangle, IEnumerable<Triangle>>(new Triangle(triangle),
                triangles.Where(otherTriangle => otherTriangle != triangle &&
                     (otherTriangle.Contains(triangle.ElementAt(0)) ||
                     otherTriangle.Contains(triangle.ElementAt(1)) ||
                     otherTriangle.Contains(triangle.ElementAt(2)))).Select(otherTriangle => new Triangle(otherTriangle)))).ToList();

            Debug.Log("Map count: " + adjacencyMap.Count);
        }

        private class Triangle
        {
            public Triangle(IEnumerable<int> vertices)
            {
                this.Vertices = new List<int>(vertices);
            }

            public Triangle(int a, int b, int c)
            {
                this.Vertices = new List<int>() { a, b, c };
            }

            public List<int> Vertices { get; set; }

            public int A { get { return this.Vertices.ElementAt(0); } set { this.Vertices.Insert(0, value); } }
            public int B { get { return this.Vertices.ElementAt(1); } set { this.Vertices.Insert(1, value); } }
            public int C { get { return this.Vertices.ElementAt(2); } set { this.Vertices.Insert(2, value); } }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                Triangle otherTriangle = obj as Triangle;

                if (otherTriangle == null) return false;

                return (this.A == otherTriangle.A) && (this.B == otherTriangle.B) && (this.C == otherTriangle.C);
            }

            public override int GetHashCode()
            {
                return A ^ B ^ C;
            }

            public override string ToString()
            {
                return string.Format("A: {0} B: {1} C: {2}", this.A, this.B, this.C);
            }
        }
    }
}