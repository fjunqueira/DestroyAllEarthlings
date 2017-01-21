using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using ProgressBar;

namespace DestroyAllEarthlings
{
    public class Laser : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarBehaviour shipEnergy;

        [SerializeField]
        public GameObject laserEffects;

        [SerializeField]
        private ParticleSystem laserSparks;

        [SerializeField]
        private ParticleSystem laserSmoke;

        [SerializeField]
        private AudioSource laserChargeAudio;

        [SerializeField]
        private AudioSource laserAudio;

        [SerializeField]
        private AudioSource laserStopAudio;

        [SerializeField]
        public GameObject laserChargeBeam;

        [SerializeField]
        private GameObject smokeAndSparks;

        private ParticleSystem.EmissionModule laserSparksEmitter;

        private ParticleSystem.EmissionModule laserSmokeEmitter;

        private Coroutine charging;

        private int laserChargeFlag;

        private void Start()
        {
            // Reset and stop all effects and audio
            laserEffects.SetActive(false);

            laserSparksEmitter = laserSparks.emission;
            laserSparksEmitter.enabled = false;

            laserSmokeEmitter = laserSmoke.emission;
            laserSmokeEmitter.enabled = false;

            laserChargeBeam.SetActive(false);
            smokeAndSparks.SetActive(false);
            smokeAndSparks.SetActive(true);

            laserChargeAudio.Stop();
            laserAudio.Stop();
            laserStopAudio.Stop();
        }

        private void Update()
        {
            // Fire laser when left mouse button is pressed
            if (Input.GetButtonDown("Fire1") && charging == null && shipEnergy.Value > 0)
            {
                laserChargeFlag = 0;
                laserChargeAudio.Play();
                laserChargeBeam.SetActive(true);
                charging = StartCoroutine(LaserChargeWait());
            }

            // Stop laser if left mouse button is released
            if (Input.GetButtonUp("Fire1") || shipEnergy.Value <= 0)
            {
                if (charging != null) StopCoroutine(charging);
                charging = null;
                laserChargeFlag = 1;
                laserEffects.SetActive(false);
                laserSparksEmitter.enabled = false;
                laserSmokeEmitter.enabled = false;
                laserAudio.Stop();
                laserStopAudio.Play();
                laserChargeBeam.SetActive(false);
            }

            this.shipEnergy.Value += laserEffects.activeSelf ? -1 : 1;
        }

        private IEnumerator LaserChargeWait()
        {
            // Wait for laser to charge
            yield return new WaitForSeconds(1.4f);

            if (laserChargeFlag == 0)
            {
                laserEffects.SetActive(true);
                laserSparksEmitter.enabled = true;
                laserSmokeEmitter.enabled = true;
                laserAudio.Play();
                laserChargeFlag = 0;
            }

            charging = null;
        }
    }
}