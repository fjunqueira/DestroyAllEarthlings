using UnityEngine;
using System.Collections;
using System;

namespace SpaceCentipedeFromHell
{
    public class FacePicker
    {
        private Camera camera;
        private MeshCollider collider;

        public FacePicker(Camera camera, MeshCollider collider)
        {
            this.camera = camera;
            this.collider = collider;
        }

        ///<summary>
        ///Returns the triangle under the mouse position on the collider or null, when none was found
        ///</summary>
        public Triangle Pick(Vector3 mousePosition)
        {
            RaycastHit hit;

            if (!Physics.Raycast(camera.ScreenPointToRay(mousePosition), out hit))
                return null;

            var meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return null;

            var mesh = meshCollider.sharedMesh;
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

			// If the mesh for the renderer is normalized the same way it's done for the navmesh
			// Instead of this we could simply navmesh.AdjacencyMap.ElementAt(hit.triangleIndex * 3), i think
            var p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
            var p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
            var p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

            var hitTransform = hit.collider.transform;

            p0 = hitTransform.TransformPoint(p0);
            p1 = hitTransform.TransformPoint(p1);
            p2 = hitTransform.TransformPoint(p2);

            return new Triangle(p0, p1, p2);
        }
    }
}