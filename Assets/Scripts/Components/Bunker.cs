using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(PathfindingObstacle))]
    public class Bunker : MonoBehaviour, IPlanetElement
    {
        public delegate void EscapedEventHandler();

        public static event EscapedEventHandler escaped;

        [SerializeField]
        private PathfindingObstacle obstacle;

        private void Start()
        {
            obstacle.GetNode().IsDestination = true;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.transform.name == "Human")
            {
                Destroy(collider.gameObject);
                if (escaped != null) escaped();
                Debug.Log("A human just escaped");
            }
        }
    }
}