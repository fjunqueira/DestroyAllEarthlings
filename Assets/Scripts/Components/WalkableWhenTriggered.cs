using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class WalkableWhenTriggered : MonoBehaviour
    {
        [SerializeField]
        private NavMeshElement obstacle;

        [SerializeField]
        private Collider obstacleCollider;

        [SerializeField]
        private string triggerName = "Orbital_Laser_Hit";

        private void Start()
        {
            obstacleCollider
				.OnTriggerEnterAsObservable()
				.Where(collider => collider.transform.name == triggerName)
				.Subscribe(collider => obstacle.IsWalkable = true)
				.AddTo(this);
        }
    }
}