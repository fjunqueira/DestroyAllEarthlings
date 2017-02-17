using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class NavMeshElement : MonoBehaviour
    {
        [SerializeField]
        private Vector3 blockingNodePosition;

        [SerializeField]
        private PlanetNavMesh navMesh;

        [SerializeField]
        private bool walkable = false;

        [SerializeField]
        private bool destination = false;

        private PlanetNode node;

        public bool IsWalkable
        {
            get { return this.walkable; }
            set
            {
                this.walkable = value;
                this.node.IsWalkable = value;
            }
        }

        public bool IsDestination
        {
            get { return this.destination; }
            set
            {
                this.destination = value;
                this.node.IsDestination = value;
            }
        }

        public void Start()
        {
            node = navMesh.GetNodeByPosition(blockingNodePosition) as PlanetNode;
            node.IsWalkable = IsWalkable;
            node.IsDestination = IsDestination;
        }
    }
}