using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshElement))]
    public class WalkableWhenTriggered : MonoBehaviour
    {
        [SerializeField]
        private Collider obstacleCollider;

        [SerializeField]
        private string triggerName = "Orbital_Laser_Hit";

        private void Start()
        {
            obstacleCollider
                .OnTriggerEnterAsObservable()
                .Where(collider => collider.transform.name == triggerName)
                .Subscribe(collider => this.GetComponent<NavMeshElement>().IsWalkable = true)
                .AddTo(this);
        }
    }
}