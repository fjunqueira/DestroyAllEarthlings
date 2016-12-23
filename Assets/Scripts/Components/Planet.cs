using System.Linq;
using UnityEngine;
using UniRx;
using System;

namespace SpaceCentipedeFromHell
{
    public class Planet : MonoBehaviour
    {
        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        public float Radius { get; set; }

        public void Start()
        {
            var navMesh = this.GetComponent<PlanetNavMesh>();

            var facePicker = new FacePicker(Camera.main);

            var everyUpdate = Observable.EveryUpdate();

            everyUpdate
                .Where(_ => Input.GetButtonDown("Fire1"))
                .Select(_ => facePicker.Pick(Input.mousePosition)).Where(triangle => triangle != null)
                .Select(triangle => navMesh.GetNodeByPosition(Quaternion.Inverse(this.transform.rotation) * triangle.Centroid)).Where(node => node != null)
                .Buffer(2)
                .Subscribe(nodes =>
                {
                    var path = navMesh.FindPath(nodes.First(), nodes.Last());

                    foreach (var node in path.Select(x => x as PlanetNode))
                    {
                        Debug.DrawLine(this.transform.rotation * node.Triangle.A, this.transform.rotation * node.Triangle.B, Color.black, 10, false);
                        Debug.DrawLine(this.transform.rotation * node.Triangle.B, this.transform.rotation * node.Triangle.C, Color.black, 10, false);
                        Debug.DrawLine(this.transform.rotation * node.Triangle.C, this.transform.rotation * node.Triangle.A, Color.black, 10, false);
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