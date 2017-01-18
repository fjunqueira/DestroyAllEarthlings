using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Planet))]
    public class GravityAttractor : MonoBehaviour
    {
        [SerializeField]
        private float gravity = 40.0f;

        private Planet planet;

        private void Start()
        {
            this.planet = this.GetComponent<Planet>();
        }

        public void Attract(Rigidbody body)
        {
            var force = (body.position - this.transform.position).normalized * -gravity;

            body.drag = body.position.magnitude <= this.planet.Radius + 1 ? 0.1f : 1.0f;

            body.AddForce(force, ForceMode.Acceleration);
        }
    }
}