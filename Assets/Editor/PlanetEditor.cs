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
        private int gridRadius;

        private string gridPath;

        private PlanetComponent planet;

        public void OnEnable()
        {
            this.planet = (PlanetComponent)target;

            gridPath = EditorPrefs.GetString("PathfindingExporter_GridName", gridPath);
            gridRadius = EditorPrefs.GetInt("PathfindingExporter_GridRadius", gridRadius);

            SceneView.onSceneGUIDelegate = Update;
        }

        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        bool pressing = false;

        public void Update(SceneView sceneview)
        {
            var currentEvent = Event.current;
            // var position = (new Vector3(currentEvent.mousePosition.x, currentEvent.mousePosition.y) - origin).normalized;

            // planet.transform.rotation = Quaternion.AngleAxis(-position.x, Vector3.up) *
            //      Quaternion.AngleAxis(position.y, Vector3.right) *
            //      planet.transform.rotation;
            if (currentEvent.isMouse && currentEvent.clickCount == 1)
            {
                GameObject obj;

                Object prefab = PrefabUtility.GetPrefabParent(Selection.activeObject);

                if (prefab != null)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        obj.transform.position = hit.point;
                        obj.transform.up = hit.normal;
                        //hit.normal
                    }
                }
            }
        }

        public void OnDisable()
        {
            EditorPrefs.SetString("PathfindingExporter_GridName", gridPath);
            EditorPrefs.SetInt("PathfindingExporter_GridRadius", gridRadius);
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
            gridRadius = EditorGUILayout.IntField(gridRadius);
            GUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            GridName();
            GridRadius();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Export Grid")) ExportGrid();

            GUILayout.EndHorizontal();
        }

        public void ExportGrid()
        {
            Debug.Log("Exporting, this may take a while...");

            var navigationMesh = new NavigationMesh(
                planet.GetComponent<MeshFilter>(), new MeshNormalizer());

            var planetGrid = new PlanetGrid(
                navigationMesh, gridRadius);

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

                Debug.Log("Exported successfully!");
            }
        }
    }
}