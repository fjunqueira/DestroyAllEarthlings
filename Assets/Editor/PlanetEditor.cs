using UnityEngine;
using UnityEditor;
using UniRx;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using SpaceCentipedeFromHell;

namespace SpaceCentipedeFromHell.EditorExtensions
{
    [CustomEditor(typeof(PlanetComponent))]
    public class PlanetEditor : Editor
    {
        private int planetRadius;

        private string gridPath;

        private PlanetComponent planet;

        private string status = "Ready!";

        public void OnEnable()
        {
            this.planet = (PlanetComponent)target;

            gridPath = EditorPrefs.GetString("PathfindingExporter_GridName", gridPath);
            planetRadius = EditorPrefs.GetInt("PathfindingExporter_PlanetRadius", planetRadius);
        }

        public void OnDisable()
        {
            EditorPrefs.SetString("PathfindingExporter_GridName", gridPath);
            EditorPrefs.SetInt("PathfindingExporter_PlanetRadius", planetRadius);
        }

        private void GridName()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Name   ");
            gridPath = EditorGUILayout.TextField(gridPath);
            GUILayout.EndHorizontal();
        }

        private void GridRadius()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Radius ");
            planetRadius = EditorGUILayout.IntField(planetRadius);
            GUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            GridName();
            GridRadius();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Export Grid"))
            {
                var navigationMesh = new NavigationMesh(
                    planet.GetComponent<MeshFilter>(), new MeshNormalizer());

                var planetGrid = new PlanetGrid(
                    navigationMesh, planetRadius);

                using (var stream = new FileStream(gridPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var surrogateSelector = new SurrogateSelector();

                    surrogateSelector.AddSurrogate(typeof(Vector3),
                                    new StreamingContext(StreamingContextStates.All),
                                    new Vector3SerializationSurrogate());

                    var formatter = new BinaryFormatter()
                    {
                        SurrogateSelector = surrogateSelector
                    };

                    formatter.Serialize(stream, planetGrid);

                    this.status = "Exported successfully!";
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(this.status);
        }
    }
}