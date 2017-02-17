using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    [RequireComponent(typeof(Collider))]
    public class GravityAttractor : MonoBehaviour
    {
        [SerializeField]
        private float gravity = 40.0f;

        [SerializeField]
        private float radius = 105.0f;

        public void Attract(Rigidbody body)
        {
            var force = (body.position - this.transform.position).normalized * -gravity;

            body.drag = body.position.magnitude <= this.radius + 1 ? 0.1f : 1.0f;

            body.AddForce(force, ForceMode.Acceleration);
        }
    }
}