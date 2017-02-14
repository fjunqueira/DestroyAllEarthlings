﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class Bunker : MonoBehaviour, IPlanetElement
    {
        public delegate void EscapedEventHandler();

        public static event EscapedEventHandler escaped;

        [SerializeField]
        private NavMeshElement obstacle;

        private void Start()
        {
            obstacle.IsDestination = true;
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