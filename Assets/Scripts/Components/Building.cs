using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class Building : Destroyable
    {
        [SerializeField]
        private List<PathFollower> humans;

        [SerializeField]
        private List<AudioClip> destructionSounds;

        public override int EarthlingCount { get { return base.EarthlingCount + humans.Count; } }

        protected override void TriggerEnter(Collider collider)
        {
            base.TriggerEnter(collider);

            StartCoroutine(PlayDestructionSounds());

            StartCoroutine(StartFleeingHumans());

            var children = destructionFx.GetComponentsInChildren<Rigidbody>();

            foreach (var child in children)
                child.AddForceAtPosition((this.transform.up - collider.transform.position).normalized * 10.0f, collider.transform.position, ForceMode.Impulse);

            children.First().OnDestroyAsObservable().Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
        }

        private IEnumerator StartFleeingHumans()
        {
            foreach (var human in humans)
            {
                yield return new WaitForSeconds(Random.Range(0, 0.5f));
                human.gameObject.SetActive(true);
                human.transform.parent = null;
            }
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
    }
}