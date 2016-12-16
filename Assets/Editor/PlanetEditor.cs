using UnityEngine;
using UnityEditor;
using UniRx;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System;

namespace SpaceCentipedeFromHell.EditorExtensions
{
    [CustomEditor(typeof(PlanetComponent))]
    public class PlanetEditor : Editor
    {
        private int gridRadius;

        private string gridPath;

        private PlanetComponent planet;

        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        private bool IsRightButton(Event currentEvent)
        {
            return currentEvent.isMouse && currentEvent.button == 1;
        }

        public void OnEnable()
        {
            //Debug.Log("OnEnable!");
            this.planet = (PlanetComponent)target;

            gridPath = EditorPrefs.GetString("PathfindingExporter_GridName", gridPath);
            gridRadius = EditorPrefs.GetInt("PathfindingExporter_GridRadius", gridRadius);

            var sceneGUIObservable = Observable.FromEvent<SceneView.OnSceneFunc, Event>(
                h => (view) => h(Event.current),
                h => SceneView.onSceneGUIDelegate += h,
                h => SceneView.onSceneGUIDelegate -= h);

            sceneGUIObservable
                .Where(x => x.isKey && x.keyCode == KeyCode.Space)
                .Subscribe(obj =>
                {
                    var position = (new Vector3(obj.mousePosition.x, obj.mousePosition.y) - origin).normalized;

                    Debug.Log("Position was computed");

                    planet.transform.rotation =
                        Quaternion.AngleAxis(position.x, Camera.current.transform.up) *
                        Quaternion.AngleAxis(position.y, Camera.current.transform.right) *
                        planet.transform.rotation;
                });

            sceneGUIObservable
                .Where(currentEvent => currentEvent.isMouse && currentEvent.button == 0 && currentEvent.type == EventType.MouseDown)
                .Select(currentEvent => new
                {
                    currentEvent = currentEvent,
                    prefab = PrefabUtility.GetPrefabParent(Selection.activeObject)
                }).Where(x => x.prefab != null)
                .Subscribe(data =>
                {
                    RaycastHit hit;
                    Ray ray = HandleUtility.GUIPointToWorldRay(data.currentEvent.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        var obj = (GameObject)PrefabUtility.InstantiatePrefab(data.prefab);
                        obj.transform.position = hit.point;
                        obj.transform.up = hit.normal;
                    }
                });
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