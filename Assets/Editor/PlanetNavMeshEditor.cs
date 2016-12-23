using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace SpaceCentipedeFromHell.EditorExtensions
{
    [CustomEditor(typeof(PlanetNavMesh))]
    public class PlanetNavigationMeshEditor : Editor
    {
        private PlanetNavMesh navigationMesh;

        private int meshRadius;

        private Mesh mesh;

        public void OnEnable()
        {
            navigationMesh = (PlanetNavMesh)target;

            navigationMesh.MeshName = EditorPrefs.GetString("MeshExporter_MeshName", navigationMesh.MeshName);
            meshRadius = EditorPrefs.GetInt("MeshExporter_MeshRadius", meshRadius);
            mesh = navigationMesh.GetComponent<MeshFilter>().sharedMesh;
        }

        public void OnDisable()
        {
            EditorPrefs.SetString("MeshExporter_MeshName", navigationMesh.MeshName);
            EditorPrefs.SetInt("MeshExporter_MeshRadius", meshRadius);
        }

        private void Mesh()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Mesh             ");
            mesh = EditorGUILayout.ObjectField(mesh, typeof(Mesh), true) as Mesh;
            GUILayout.EndHorizontal();
        }

        private void MeshName()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Mesh Name  ");
            navigationMesh.MeshName = EditorGUILayout.TextField(navigationMesh.MeshName);
            GUILayout.EndHorizontal();
        }

        private void MeshRadius()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Mesh Radius");
            meshRadius = EditorGUILayout.IntField(meshRadius);
            GUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            Mesh();
            MeshName();
            MeshRadius();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Export Mesh")) ExportMesh();

            GUILayout.EndHorizontal();
        }

        public void ExportMesh()
        {
            Debug.Log("Exporting, this may take a while...");

            var normalizedMesh =
                new MeshNormalizer().Normalize(mesh);

            var adjacencyMap = new MeshAdjacencyMap(normalizedMesh);

            var planetGrid = new PlanetGrid(adjacencyMap, meshRadius);

            using (var stream = new FileStream("Assets/Grids/" + navigationMesh.MeshName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = Formatter.CreateFormatter();

                formatter.Serialize(stream, planetGrid);

                Debug.Log("Exported successfully!");
            }
        }
    }
}