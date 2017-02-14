using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

namespace DestroyAllEarthlings
{
    public class StopMovingWhenTriggered : MonoBehaviour
    {
        [SerializeField]
        private PathFollower follower;

        [SerializeField]
        private Collider followerCollider;

        [SerializeField]
        private string triggerName = "Orbital_Laser_Hit";

        private void Start()
        {
            followerCollider
                .OnTriggerEnterAsObservable()
                .Where(collider => collider.transform.name == triggerName)
                .Subscribe(collider => follower.KeepMoving = false)
                .AddTo(this);
        }
    }
}