using UnityEngine;
using UnityEditor;
using UniRx;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace SpaceCentipedeFromHell.EditorExtensions
{
    [CustomEditor(typeof(PlanetComponent))]
    public class PlanetEditor : Editor
    {
        private const int LEFT_MOUSE_BUTTON = 0;

        private static PlanetComponent planet;

        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        private int gridRadius;

        private string gridPath;

        static PlanetEditor()
        {
            var sceneGUIObservable = Observable.FromEvent<SceneView.OnSceneFunc, Event>(
                    h => (view) => h(Event.current), 
                    h => SceneView.onSceneGUIDelegate += h,
                    h => SceneView.onSceneGUIDelegate -= h);

            sceneGUIObservable
                .Where(currentEvent => currentEvent.isKey && currentEvent.keyCode == KeyCode.Space)
                .Subscribe(obj =>
                {
                    var position = (new Vector3(obj.mousePosition.x, obj.mousePosition.y) - origin).normalized;

                    planet.transform.rotation =
                        Quaternion.AngleAxis(position.x, Camera.current.transform.up) *
                        Quaternion.AngleAxis(position.y, Camera.current.transform.right) *
                        planet.transform.rotation;
                });

            sceneGUIObservable
                 .Where(currentEvent => currentEvent.isMouse && currentEvent.button == LEFT_MOUSE_BUTTON && currentEvent.type == EventType.MouseDown)
                 .Select(currentEvent => new
                 {
                     mousePosition = currentEvent.mousePosition,
                     prefab = PrefabUtility.GetPrefabParent(Selection.activeObject) as GameObject
                 })
                 .Where(x => x.prefab != null && x.prefab.GetComponent<IDestroyable>() != null)
                 .Subscribe(data =>
                 {
                     RaycastHit hit;
                     Ray ray = HandleUtility.GUIPointToWorldRay(data.mousePosition);

                     if (Physics.Raycast(ray, out hit) && hit.collider.GetComponent<PlanetComponent>() != null)
                     {
                         var obj = (GameObject)PrefabUtility.InstantiatePrefab(data.prefab);

                         var boxCollider = obj.GetComponent<BoxCollider>();

                         var colliderHeight = boxCollider == null ? 0 : boxCollider.size.y;

                         var centerOffset = 1 + ((colliderHeight / 2) / hit.point.magnitude);

                         obj.transform.position = hit.point * centerOffset;
                         obj.transform.up = hit.normal;
                         obj.transform.parent = planet.transform;
                     }
                 });
        }

        public void OnEnable()
        {
            planet = (PlanetComponent)target;

            gridPath = EditorPrefs.GetString("PathfindingExporter_GridName", gridPath);
            gridRadius = EditorPrefs.GetInt("PathfindingExporter_GridRadius", gridRadius);
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

            var normalizedMesh =
                new MeshNormalizer().Normalize(planet.GetComponent<MeshFilter>().sharedMesh);

            var adjacencyMap = new MeshAdjacencyMap(normalizedMesh);

            var planetGrid = new PlanetGrid(adjacencyMap, gridRadius);

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