using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class Human : Destroyable
    {
        protected override void TriggerEnter(Collider collider)
        {
            base.TriggerEnter(collider);
            Destroy(gameObject, 3.0f);
        }
    }
}