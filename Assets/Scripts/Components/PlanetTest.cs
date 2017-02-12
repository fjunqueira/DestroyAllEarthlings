using System.Linq;
using UnityEngine;
using UniRx;
using System;

namespace DestroyAllEarthlings
{
    public class PlanetTest : MonoBehaviour
    {
        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        public void Start()
        {
            var navMesh = this.GetComponent<PlanetNavMesh>();

            var facePicker = new FacePicker(Camera.main);

            var everyUpdate = Observable.EveryUpdate();

            everyUpdate
                .Where(_ => Input.GetButtonDown("Fire1"))
                .Select(_ =>
                {
                    var face = facePicker.Pick(Input.mousePosition);
                    Debug.Log("Picked Face " + face.ToString());
                    return face;
                })
                .Where(triangle => triangle != null)
                .Select(triangle =>
                {
                    var node = navMesh.GetNodeByPosition(Quaternion.Inverse(this.transform.rotation) * triangle.Centroid);
                    Debug.Log("Picked Node " + node.ToString());
                    return node;
                }).Where(node => node != null)
                .Buffer(2)
                .Subscribe(nodes =>
                {
                    var path = navMesh.FindPath(nodes.First(), nodes.Last());

                    Debug.Log("Path length: " + path.Count());

                    foreach (var node in path.Select(x => x as PlanetNode))
                    {
                        Debug.DrawLine(this.transform.rotation * node.Triangle.A, this.transform.rotation * node.Triangle.B, Color.black, 10, true);
                        Debug.DrawLine(this.transform.rotation * node.Triangle.B, this.transform.rotation * node.Triangle.C, Color.black, 10, true);
                        Debug.DrawLine(this.transform.rotation * node.Triangle.C, this.transform.rotation * node.Triangle.A, Color.black, 10, true);
                    }
                });

            everyUpdate
                .Where(_ => Input.GetButton("Fire2"))
                .Select(_ => (Input.mousePosition - origin).normalized)
                .Subscribe(position =>
                {
                    transform.rotation = Quaternion.AngleAxis(-position.x, Vector3.up) *
                                         Quaternion.AngleAxis(position.y, Vector3.right) *
                                         transform.rotation;
                });
        }
    }
}