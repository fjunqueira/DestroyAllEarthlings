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
        private Collider destroyableCollider;

        [SerializeField]
        protected GameObject destructionFx;

        [SerializeField]
        private List<AudioClip> destructionSounds;

        public virtual int EarthlingCount { get { return this.earthlingCount; } }

        private void Start()
        {
            pointsMesh.text = earthlingCount.ToString();

            destroyableCollider.OnTriggerEnterAsObservable()
            .Where(collider => collider.transform.name == "Orbital_Laser_Hit")
            .Subscribe(collider =>
            {
                TriggerEnter(collider);
            });
        }

        protected virtual void TriggerEnter(Collider collider)
        {
            if (destroyed != null) destroyed(this.EarthlingCount);

            this.ShowPoints();

            StartCoroutine(PlayDestructionSounds());

            destroyableCollider.gameObject.SetActive(false);
            destructionFx.SetActive(true);
        }

        private IEnumerator PlayDestructionSounds()
        {
            foreach (var sound in this.destructionSounds)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(0, 0.3f));
                var audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                audioSource.clip = sound;
                audioSource.volume = 0.1f;
                audioSource.Play();
            }
        }

        private void ShowPoints()
        {
            pointsMesh.transform.position = pointsMesh.transform.position * 1.1f;
            pointsMesh.gameObject.SetActive(true);
        }
    }
}