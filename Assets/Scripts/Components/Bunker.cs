using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(PathfindingObstacle))]
    public class Bunker : MonoBehaviour, IPlanetElement
    {
        private void Start()
        {
            this.GetComponent<PathfindingObstacle>().GetNode().IsDestination = true;
        }
    }
}