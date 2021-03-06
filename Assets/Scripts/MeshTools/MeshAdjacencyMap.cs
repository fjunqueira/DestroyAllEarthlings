﻿using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace DestroyAllEarthlings
{
    /// <summary>
    /// Extracts a navigation mesh given a mesh, only works on triangle meshes
    /// </summary>
    public class MeshAdjacencyMap
    {
        private Dictionary<Triangle, Triangle[]> adjacencyMap;

        public MeshAdjacencyMap(NormalizedMesh normalizedMesh)
        {
            var triangles = normalizedMesh.Triangles.SelectMany(triangleList => triangleList).ChunksOf(3).ToList();

            this.adjacencyMap = triangles.Select(triangle => new KeyValuePair<Triangle, Triangle[]>(
                new Triangle(normalizedMesh.IndexToVertex(triangle)),
                triangles.Where(otherTriangle => HasCommonVertices(otherTriangle, triangle))
                .Select(otherTriangle =>
                    new Triangle(normalizedMesh.IndexToVertex(otherTriangle))).ToArray())).ToDictionary();
        }

        private bool HasCommonVertices(IEnumerable<int> otherTriangle, IEnumerable<int> triangle)
        {
            return otherTriangle != triangle &&
                   (otherTriangle.Contains(triangle.ElementAt(0)) ||
                   otherTriangle.Contains(triangle.ElementAt(1)) ||
                   otherTriangle.Contains(triangle.ElementAt(2)));
        }

        private IEnumerable<Vector3> IndexToVertex(IEnumerable<int> indexes, NormalizedMesh mesh)
        {
            return indexes.Select(verticeIndex => mesh.Vertices.ElementAt(verticeIndex));
        }

        public Dictionary<Triangle, Triangle[]> Map
        {
            get { return this.adjacencyMap; }
        }
    }
}