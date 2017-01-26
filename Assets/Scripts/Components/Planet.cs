using System.Linq;
using UnityEngine;
using UniRx;
using System;

namespace DestroyAllEarthlings
{
    public class Planet : MonoBehaviour
    {
        [SerializeField]
        private float radius;

        public float Radius { get { return this.radius; } }
    }
}