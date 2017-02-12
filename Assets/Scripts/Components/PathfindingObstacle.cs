using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class PathfindingObstacle : MonoBehaviour
    {
        [SerializeField]
        private Vector3 blockingNodePosition;

        [SerializeField]
        private PlanetNavMesh navMesh;

        public PlanetNode GetNode()
        {
            return navMesh.GetNodeByPosition(blockingNodePosition) as PlanetNode;
        }

        private void OnDestroy()
        {
            navMesh.GetNodeByPosition(blockingNodePosition).IsWalkable = true;
        }
    }
}