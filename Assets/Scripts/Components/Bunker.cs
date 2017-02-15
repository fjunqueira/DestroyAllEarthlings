using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class Bunker : MonoBehaviour, IPlanetElement
    {
        public delegate void EscapedEventHandler();

        public static event EscapedEventHandler escaped;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.transform.name == "Human")
            {
                Destroy(collider.transform.parent.gameObject);
                if (escaped != null) escaped();
            }
        }
    }
}