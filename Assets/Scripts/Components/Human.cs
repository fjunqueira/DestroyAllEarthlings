using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

namespace DestroyAllEarthlings
{
    public class Human : MonoBehaviour
    {
        [SerializeField]
        private PlanetNavMesh navMesh;


        [SerializeField]
        private Vector3 startingNodePosition;


        private void Start()
        {
            var path = navMesh.FindPath(navMesh.GetNodeByPosition(startingNodePosition)).ToList();

            foreach (var node in path.Select(x => x as PlanetNode))
            {
                Debug.DrawLine(navMesh.transform.rotation * node.Triangle.A, navMesh.transform.rotation * node.Triangle.B, Color.black, 10, true);
                Debug.DrawLine(navMesh.transform.rotation * node.Triangle.B, navMesh.transform.rotation * node.Triangle.C, Color.black, 10, true);
                Debug.DrawLine(navMesh.transform.rotation * node.Triangle.C, navMesh.transform.rotation * node.Triangle.A, Color.black, 10, true);
            }

            if (!path.Any())
            {
                Debug.Log("No where to run No where to hide No where to go!");
                return;
            }

            if (path.Count == 1)
            {
                Debug.Log("Already at destination");
                return;
            }

            var current = (path.Shift() as PlanetNode);
            var target = (path.Shift() as PlanetNode);

            var currentPosition = current.Position;
            var nextPosition = GetNextPosition(current, target);

            var interpolation = 0.0f;

            Observable.EveryUpdate().Subscribe(_ =>
            {
                interpolation += Mathf.Clamp(Time.deltaTime, 0, 1);

                transform.position = Vector3.Lerp(currentPosition, nextPosition, interpolation);

                transform.LookAt(nextPosition, interpolation > 0.5f ? target.Triangle.FaceNormal : current.Triangle.FaceNormal);

                if (interpolation >= 1.0f)
                {
                    if (!path.Any())
                    {
                        Destroy(this.gameObject);
                        return;
                    }
                    else
                    {
                        interpolation = 0;
                        current = target;
                        target = path.Shift() as PlanetNode;
                        currentPosition = nextPosition;
                        nextPosition = GetNextPosition(current, target);
                    }
                }
            }).AddTo(this);
        }

        private Vector3 GetNextPosition(PlanetNode current, PlanetNode target)
        {
            if (current.Triangle.A == target.Triangle.A)
                return target.Triangle.A;
            else if (current.Triangle.A == target.Triangle.B)
                return target.Triangle.B;
            else if (current.Triangle.A == target.Triangle.C)
                return target.Triangle.C;
            else if (current.Triangle.B == target.Triangle.A)
                return target.Triangle.A;
            else if (current.Triangle.B == target.Triangle.B)
                return target.Triangle.B;
            else if (current.Triangle.B == target.Triangle.C)
                return target.Triangle.C;
            else if (current.Triangle.C == target.Triangle.A)
                return target.Triangle.A;
            else if (current.Triangle.C == target.Triangle.B)
                return target.Triangle.B;
            else if (current.Triangle.C == target.Triangle.C)
                return target.Triangle.C;
            else
            {
                Debug.Log("Something went wrong: Human.GetNextPosition");
                Destroy(this.gameObject);
                return Vector3.zero;
            }
        }
    }
}