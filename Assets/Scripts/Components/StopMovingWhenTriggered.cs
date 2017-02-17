using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PathFollower))]
    public class StopMovingWhenTriggered : MonoBehaviour
    {
        [SerializeField]
        private Collider followerCollider;

        [SerializeField]
        private string triggerName = "Orbital_Laser_Hit";

        private void Start()
        {
            followerCollider
                .OnTriggerEnterAsObservable()
                .Where(collider => collider.transform.name == triggerName)
                .Subscribe(collider => this.GetComponent<PathFollower>().KeepMoving = false)
                .AddTo(this);
        }
    }
}