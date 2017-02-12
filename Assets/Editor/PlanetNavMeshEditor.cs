using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace DestroyAllEarthlings.EditorExtensions
{
    [CustomEditor(typeof(PlanetNavMesh))]
    public class PlanetNavigationMeshEditor : Editor
    {
        private PlanetNavMesh navigationMesh;

        private int meshRadius;

        private Mesh mesh;

        private int[] subGroups;

        private Material[] materials;

        private bool[] walkable;

        public void OnEnable()
        {
            navigationMesh = (PlanetNavMesh)target;

            navigationMesh.MeshName = EditorPrefs.GetString("MeshExporter_MeshName", navigationMesh.MeshName);
            meshRadius = EditorPrefs.GetInt("MeshExporter_MeshRadius", meshRadius);
            mesh = navigationMesh.GetComponent<MeshFilter>().sharedMesh;

            materials = navigationMesh.GetComponent<MeshRenderer>().sharedMaterials;
            subGroups = Enumerable.Range(0, materials.Count()).ToArray();

            walkable = new bool[materials.Length];

            foreach (var group in subGroups)
                walkable[group] = EditorPrefs.GetBool("MeshExporter_Walkable" + group);
        }

        public void OnDisable()
        {
            EditorPrefs.SetString("MeshExporter_MeshName", navigationMesh.MeshName);
            EditorPrefs.SetInt("MeshExporter_MeshRadius", meshRadius);

            foreach (var group in subGroups)
                EditorPrefs.SetBool("MeshExporter_Walkable" + group, walkable[group]);
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

        private void SubMeshes()
        {
            var subMeshes = subGroups.Zip3(walkable, materials, (id, walkable, material) => new { id, walkable, material });

            foreach (var subMesh in subMeshes)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(" " + subMesh.material.name, GUILayout.Width(73));
                walkable[subMesh.id] = EditorGUILayout.Toggle("Is Walkable", walkable[subMesh.id]);
                GUILayout.EndHorizontal();
            }
        }

        public override void OnInspectorGUI()
        {
            Mesh();
            MeshName();
            MeshRadius();
            SubMeshes();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Export Mesh")) ExportMesh();

            GUILayout.EndHorizontal();
        }

        public void ExportMesh()
        {
            Debug.Log("Exporting, this may take a while...");

            var normalizedMesh = new MeshNormalizer().Normalize(mesh);
            var adjacencyMap = new MeshAdjacencyMap(normalizedMesh);
            var planetGrid = new PlanetGrid(adjacencyMap, meshRadius);

            var unwalkableNodes = normalizedMesh.Triangles.Zip(walkable, (triangles, walkable) => new { triangles, walkable })
                .Where(x => !x.walkable).SelectMany(x => x.triangles).ChunksOf(3).Select(vertices => new Triangle(normalizedMesh.IndexToVertex(vertices)));

            foreach (var node in unwalkableNodes) planetGrid.GetNodeByPosition(node.Centroid).IsWalkable = false;

            Debug.Log("Done!");

            using (var stream = new FileStream(string.Format("Assets/Resources/{0}.bytes", navigationMesh.MeshName), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var formatter = Formatter.CreateFormatter();

                formatter.Serialize(stream, planetGrid);
            }
        }
    }
}