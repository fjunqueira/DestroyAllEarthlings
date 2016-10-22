using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GeodesicSphere
{
    private static void Subdivide(Vector3 v1, Vector3 v2, Vector3 v3, List<Vector3> spherePoints, int depth)
    {
        if (depth == 0)
        {
            spherePoints.Add(v1);
            spherePoints.Add(v2);
            spherePoints.Add(v3);
            return;
        }

        var v12 = (v1 + v2).normalized;
        var v23 = (v2 + v3).normalized;
        var v31 = (v3 + v1).normalized;

        Subdivide(v1, v12, v31, spherePoints, depth - 1);
        Subdivide(v2, v23, v12, spherePoints, depth - 1);
        Subdivide(v3, v31, v23, spherePoints, depth - 1);
        Subdivide(v12, v23, v31, spherePoints, depth - 1);
    }

    public static List<Vector3> InitializeSphere(int depth)
    {
        float X = 0.525731112119133606f;
        float Z = 0.850650808352039932f;

        var vertices = new Vector3[12]
        {
            new Vector3(-X, 0.0f, Z),  new Vector3( X, 0.0f, Z ),  new Vector3( -X, 0.0f, -Z ),  new Vector3( X, 0.0f, -Z ),
            new Vector3( 0.0f, Z, X ),  new Vector3( 0.0f, Z, -X ),  new Vector3( 0.0f, -Z, X ),  new Vector3( 0.0f, -Z, -X ),
            new Vector3( Z, X, 0.0f ),  new Vector3( -Z, X, 0.0f),  new Vector3( Z, -X, 0.0f ),  new Vector3( -Z, -X, 0.0f )
        };

        var triangles = new int[20][]
        {
            new int[] { 0, 4, 1}, new int[] { 0, 9, 4 }, new int[] { 9, 5, 4 }, new int[] { 4, 5, 8 }, new int[] { 4, 8, 1 },
            new int[] { 8, 10, 1 }, new int[] { 8, 3, 10 }, new int[] { 5, 3, 8 }, new int[] { 5, 2, 3 }, new int[] { 2, 7, 3 },
            new int[] { 7, 10, 3 }, new int[] { 7, 6, 10 }, new int[] { 7, 11, 6 }, new int[] { 11, 0, 6 }, new int[] { 0, 1, 6 },
            new int[] { 6, 1, 10 }, new int[] { 9, 0, 11 }, new int[] { 9, 11, 2 }, new int[] { 9, 2, 5 }, new int[] { 7, 2, 11 }
        };

        var spherePoints = new List<Vector3>();

        for (int i = 0; i < 20; i++)
            Subdivide(vertices[triangles[i][0]], vertices[triangles[i][1]], vertices[triangles[i][2]], spherePoints, depth);

        return spherePoints;
    }
}
