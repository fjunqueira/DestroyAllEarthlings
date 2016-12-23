using UnityEngine;
using UnityEditor;
using UniRx;
using System.Linq;
using System.Reflection;

namespace SpaceCentipedeFromHell.EditorExtensions
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : Editor
    {
        private const int LEFT_MOUSE_BUTTON = 0;

        private static Planet planet;

        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

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
                 .Where(x => x.prefab != null && x.prefab.GetComponent<Destroyable>() != null)
                 .Subscribe(data =>
                 {
                     RaycastHit hit;
                     Ray ray = HandleUtility.GUIPointToWorldRay(data.mousePosition);

                     if (Physics.Raycast(ray, out hit) && hit.collider.GetComponent<Planet>() != null)
                     {
                         var obj = (GameObject)PrefabUtility.InstantiatePrefab(data.prefab);

                         var boxCollider = obj.GetComponentInChildren<BoxCollider>();

                         var colliderHeight = boxCollider == null ? 0 : boxCollider.size.y;

                         var centerOffset = 1 + ((colliderHeight / 2) / hit.point.magnitude);

                         obj.transform.position = FacePicker.ToTriangle(hit).Centroid * centerOffset;
                         obj.transform.up = hit.normal;
                         obj.transform.parent = planet.transform;

                         foreach (var gravityBody in obj.GetComponentsInChildren<GravityBody>(true))
                         {
                             gravityBody
                                .GetType()
                                .GetField("attractor", BindingFlags.Instance | BindingFlags.NonPublic)
                                .SetValue(gravityBody, hit.collider.GetComponent<GravityAttractor>());
                         }
                     }
                 });
        }

        public void OnEnable()
        {
            planet = (Planet)target;
        }
    }
}