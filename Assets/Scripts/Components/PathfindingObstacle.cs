using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class PathfindingObstacle : MonoBehaviour
    {
        [SerializeField]
        private Vector3 blockingNodePosition;

        [SerializeField]
        private PlanetNavMesh navMesh;

        [SerializeField]
        private Collider obstacleCollider;

        [SerializeField]
        private bool walkable = false;

        public void Start()
        {
            navMesh.GetNodeByPosition(blockingNodePosition).IsWalkable = walkable;

            obstacleCollider.OnTriggerEnterAsObservable()
            .Where(collider => collider.transform.name == "Orbital_Laser_Hit")
            .Subscribe(collider =>
            {
                navMesh.GetNodeByPosition(blockingNodePosition).IsWalkable = true;
            }).AddTo(this);
        }

        public Vector3 BlockingNodePosition
        {
            get
            {
                return blockingNodePosition;
            }
        }

        public PlanetNode GetNode()
        {
            return navMesh.GetNodeByPosition(blockingNodePosition) as PlanetNode;
        }
    }
}