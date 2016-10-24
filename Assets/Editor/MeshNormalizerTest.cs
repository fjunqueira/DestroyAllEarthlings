using UnityEngine;
using NUnit.Framework;
using System.Linq;

namespace SpaceCentipedeFromHell.Tests
{
    public class MeshNormalizerTest
    {

        [Test]
        public void NormalizedGeodesicSphereTest1()
        {
            var sphere = GeodesicSphere.InitializeSphere(2);

            var mesh = new Mesh()
            {
                vertices = sphere.ToArray(),
                triangles = Enumerable.Range(0, sphere.Count).ToArray()
            };

            var normalizedMesh = new MeshNormalizer().Normalize(mesh);

            Assert.AreEqual(162, normalizedMesh.Vertices.Count(), "A geodesic sphere created by 2 subdivisions must have 642 vertices");
            Assert.AreEqual(mesh.triangles.Count(), normalizedMesh.Triangles.Count(), "The number of faces should stay the same");
        }

        [Test]
        public void NormalizedGeodesicSphereTest2()
        {
            var sphere = GeodesicSphere.InitializeSphere(3);

            var mesh = new Mesh()
            {
                vertices = sphere.ToArray(),
                triangles = Enumerable.Range(0, sphere.Count).ToArray()
            };

            var normalizedMesh = new MeshNormalizer().Normalize(mesh);

            Assert.AreEqual(642, normalizedMesh.Vertices.Count(), "A geodesic sphere created by 3 subdivisions must have 642 vertices");
            Assert.AreEqual(mesh.triangles.Count(), normalizedMesh.Triangles.Count(), "The number of faces should stay the same");
        }
    }
}