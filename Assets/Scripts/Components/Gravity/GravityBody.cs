using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class GravityBody : MonoBehaviour
    {
        [SerializeField]
        private GravityAttractor attractor;

        private Rigidbody body;

        private void Start()
        {
            this.body = this.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            this.attractor.Attract(this.body);
        }
    }
}