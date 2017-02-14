using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;
using UniRx.Triggers;

namespace DestroyAllEarthlings
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField]
        private PlanetNavMesh navMesh;

        [SerializeField]
        private Vector3 startingNodePosition;

        [SerializeField]
        private bool keepMoving = true;

        public bool KeepMoving
        {
            get { return this.keepMoving; }
            set { this.keepMoving = value; }
        }

        public void Start()
        {
            var path = navMesh.FindPath(navMesh.GetNodeByPosition(startingNodePosition)).ToList();

            if (!path.Any() || path.Count == 1)
            {
                Debug.Log("Could happen, but shouldn't");
                return;
            }

            var navigation = new CurrentNavigation(path);

            Observable.EveryUpdate().Subscribe(_ =>
                {
                    if (!KeepMoving) return;

                    navigation.Interpolation += Mathf.Clamp(Time.deltaTime, 0, 1);

                    transform.position = Vector3.Lerp(navigation.CurrentPosition, navigation.NextPosition, navigation.Interpolation) * 1.01f;

                    transform.LookAt(navigation.NextPosition * 1.01f, navigation.Current.Triangle.FaceNormal);

                    if (navigation.ReachedWayPoint)
                        if (navigation.Arrived) KeepMoving = false; else navigation.MoveNext();

                }).AddTo(this);
        }

        public static Vector3 GetNextPosition(PlanetNode current, PlanetNode target)
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
                return Vector3.zero;
            }
        }

        private class CurrentNavigation
        {
            public CurrentNavigation(List<PathfindingNode> path)
            {
                Path = path;
                Current = path.Shift() as PlanetNode;
                Target = path.Shift() as PlanetNode;
                CurrentPosition = Current.Position;
                NextPosition = GetNextPosition(Current, Target);
                Interpolation = 0.0f;
            }

            public List<PathfindingNode> Path { get; set; }
            public PlanetNode Current { get; set; }
            public PlanetNode Target { get; set; }
            public Vector3 CurrentPosition { get; set; }
            public Vector3 NextPosition { get; set; }
            public float Interpolation { get; set; }

            public bool ReachedWayPoint { get { return Interpolation >= 1.0f; } }

            public bool Arrived { get { return !Path.Any(); } }

            public void MoveNext()
            {
                Interpolation = 0;
                Current = Target;
                Target = Path.Shift() as PlanetNode;
                CurrentPosition = NextPosition;
                NextPosition = Path.Any() ? GetNextPosition(Current, Target) : Target.Position;
            }
        }
    }
}