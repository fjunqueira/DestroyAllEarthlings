using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;
using System;

namespace SpaceCentipedeFromHell
{
    public class PlanetComponent : MonoBehaviour
    {
        private static Vector3 origin = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);

        [Inject]
        public void Construct(PlanetGrid grid, FacePicker facePicker)
        {
            var everyUpdate = Observable.EveryUpdate();

            everyUpdate
                .Where(_ => Input.GetButtonDown("Fire1"))
                .Select(_ => facePicker.Pick(Input.mousePosition))
                .Where(triangle => triangle != null)
                .Buffer(2)
                .Subscribe(triangles =>
                {
                    var startingNode = grid.PositionIndexing[triangles.First().Centroid];
                    var endingNode = grid.PositionIndexing[triangles.Last().Centroid];

                    var path = grid.FindPath(startingNode, endingNode);

                    foreach (var node in path.Select(x => x as PlanetNode))
                    {
                        Debug.DrawLine(node.Triangle.A, node.Triangle.B, Color.black, 10, false);
                        Debug.DrawLine(node.Triangle.B, node.Triangle.C, Color.black, 10, false);
                        Debug.DrawLine(node.Triangle.C, node.Triangle.A, Color.black, 10, false);
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