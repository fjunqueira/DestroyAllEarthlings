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

            var navigation = new WaypointManager(path, transform.position);

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
    }
}