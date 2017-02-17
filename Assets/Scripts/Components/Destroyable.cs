using System.Linq;
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DestroyAllEarthlings
{
    public class Destroyable : MonoBehaviour, IPlanetElement
    {
        public delegate void DestroyedEventHandler(int earthlingCount);

        public static event DestroyedEventHandler destroyed;

        [SerializeField]
        private int earthlingCount = 10;

        [SerializeField]
        protected TextMesh pointsMesh;

        [SerializeField]
        protected Collider destroyableCollider;

        [SerializeField]
        protected GameObject destructionFx;

        [SerializeField]
        protected string triggerName = "Orbital_Laser_Hit";

        public virtual int EarthlingCount { get { return this.earthlingCount; } }

        private void Start()
        {
            pointsMesh.text = earthlingCount.ToString();

            destroyableCollider.OnTriggerEnterAsObservable()
                .Where(collider => collider.transform.name == triggerName)
                .Subscribe(collider => TriggerEnter(collider))
                .AddTo(this);
        }

        protected virtual void TriggerEnter(Collider collider)
        {
            if (destroyed != null) destroyed(this.earthlingCount);

            this.ShowPoints();

            destroyableCollider.gameObject.SetActive(false);
            destructionFx.SetActive(true);
        }

        private void ShowPoints()
        {
            if (earthlingCount == 0) return;
            
            var randomScale = GetScale();
            pointsMesh.transform.position = pointsMesh.transform.position * 1.1f;
            pointsMesh.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            pointsMesh.gameObject.SetActive(true);
        }

        private float GetScale()
        {
            if (earthlingCount < 2) return 1.0f;
            if (earthlingCount < 5) return 2.0f;
            if (earthlingCount < 8) return 2.5f;
            if (earthlingCount < 15) return 3.0f;

            return 4.0f;
        }
    }
}