using UnityEngine;
using UnityEditor;
using UniRx;
using System.Linq;
using System.Reflection;

namespace DestroyAllEarthlings.EditorExtensions
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : Editor
    {
        private const int LEFT_MOUSE_BUTTON = 1;

        private static Planet planet;

        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        static PlanetEditor()
        {
            var guiObservable = Observable.FromEvent<SceneView.OnSceneFunc, Event>(
                    h => (view) => h(Event.current),
                    h => SceneView.onSceneGUIDelegate += h,
                    h => SceneView.onSceneGUIDelegate -= h);

            var controlKeyDown = guiObservable.Where(currentEvent => currentEvent.isKey && currentEvent.keyCode == KeyCode.LeftControl && currentEvent.type == EventType.KeyDown);

            var controlKeyUp = guiObservable.Where(currentEvent => currentEvent.isKey && currentEvent.keyCode == KeyCode.LeftControl && currentEvent.type == EventType.KeyUp);

            var rightMouseDown = guiObservable.Where(currentEvent => currentEvent.isMouse && currentEvent.button == LEFT_MOUSE_BUTTON && currentEvent.type == EventType.MouseDown);

            guiObservable
                .Where(currentEvent => currentEvent.isKey && currentEvent.keyCode == KeyCode.Space)
                .Subscribe(obj =>
                {
                    var position = (new Vector3(obj.mousePosition.x, obj.mousePosition.y) - origin).normalized;

                    planet.transform.rotation =
                        Quaternion.AngleAxis(position.x, Camera.current.transform.up) *
                        Quaternion.AngleAxis(position.y, Camera.current.transform.right) *
                        planet.transform.rotation;
                });

            controlKeyDown
                 .SelectMany(_ => rightMouseDown.TakeUntil(controlKeyUp))
                 .Select(currentEvent => new
                 {
                     mousePosition = currentEvent.mousePosition,
                     prefab = PrefabUtility.GetPrefabParent(Selection.activeObject) as GameObject
                 })
                 .Where(x => x.prefab != null && x.prefab.GetComponentInChildren<IPlanetElement>() != null)
                 .Subscribe(data =>
                 {
                     RaycastHit hit;
                     Ray ray = HandleUtility.GUIPointToWorldRay(data.mousePosition);

                     if (Physics.Raycast(ray, out hit))
                     {
                         var obj = (GameObject)PrefabUtility.InstantiatePrefab(data.prefab);

                         var boxCollider = obj.GetComponentInChildren<BoxCollider>();

                         var colliderHeight = boxCollider == null ? 0 : boxCollider.size.y;

                         var centerOffset = 1 + ((colliderHeight / 2) / hit.point.magnitude);

                         var triangle = FacePicker.ToTriangle(hit);

                         obj.transform.position = triangle.Centroid * centerOffset;
                         obj.transform.up = hit.normal;
                         obj.transform.parent = planet.transform;

                         foreach (var gravityBody in obj.GetComponentsInChildren<GravityBody>(true))
                             SetNonPublicMember(gravityBody, planet.GetComponent<GravityAttractor>(), "attractor");

                         var pathfindingObstacle = obj.GetComponent<NavMeshElement>();

                         SetNavMeshInfo(pathfindingObstacle, triangle);
                     }
                 });
        }

        private static void SetNavMeshInfo(NavMeshElement pathfindingObstacle, Triangle triangle)
        {
            if (pathfindingObstacle != null)
            {
                var planetNavMesh = planet.GetComponent<PlanetNavMesh>();

                SetNonPublicMember(pathfindingObstacle, triangle.Centroid, "blockingNodePosition");
                SetNonPublicMember(pathfindingObstacle, planetNavMesh, "navMesh");

                foreach (var follower in pathfindingObstacle.GetComponentsInChildren<PathFollower>(true))
                {
                    SetNonPublicMember(follower, planetNavMesh, "navMesh");
                    SetNonPublicMember(follower, triangle.Centroid, "startingNodePosition");
                }
            }
        }

        private static void SetNonPublicMember<T1, T2>(T1 obj, T2 value, string field)
        {
            obj.GetType()
               .GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)
               .SetValue(obj, value);
        }

        public void OnEnable()
        {
            planet = (Planet)target;
        }
    }
}