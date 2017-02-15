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
        public delegate void LaserEnergyChangedHandler(float energy);

        public static event LaserEnergyChangedHandler laserEnergyChanged;

        private float laserEnergy;

        [SerializeField]
        private GameObject laserEffects;

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
        private GameObject laserChargeBeam;

        [SerializeField]
        private GameObject smokeAndSparks;

        private ParticleSystem.EmissionModule laserSparksEmitter;

        private ParticleSystem.EmissionModule laserSmokeEmitter;

        private Coroutine charging;

        private int laserChargeFlag;

        public bool IsActive { get; set; }

        public bool IsCharging { get; set; }

        private void Start()
        {
            laserEffects.SetActive(IsActive = false);

            laserSparksEmitter = laserSparks.emission;
            laserSparksEmitter.enabled = false;

            laserSmokeEmitter = laserSmoke.emission;
            laserSmokeEmitter.enabled = false;

            laserChargeBeam.SetActive(IsCharging = false);
            smokeAndSparks.SetActive(false);
            smokeAndSparks.SetActive(true);

            laserChargeAudio.Stop();
            laserAudio.Stop();
            laserStopAudio.Stop();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1") && charging == null && laserEnergy > 0)
            {
                laserChargeFlag = 0;
                laserChargeAudio.Play();
                laserChargeBeam.SetActive(IsCharging = true);
                charging = StartCoroutine(LaserChargeWait());
            }

            if ((Input.GetButtonUp("Fire1") && (IsActive || IsCharging)) || laserEnergy <= 0)
            {
                if (charging != null) StopCoroutine(charging);
                charging = null;
                laserChargeFlag = 1;
                laserEffects.SetActive(IsActive = false);
                laserSparksEmitter.enabled = false;
                laserSmokeEmitter.enabled = false;
                laserAudio.Stop();
                laserStopAudio.Play();
                laserChargeBeam.SetActive(IsCharging = false);
            }

            this.laserEnergy = Mathf.Clamp(this.laserEnergy + (IsActive ? -1 : 1), 0, 100);
            if (laserEnergyChanged != null) laserEnergyChanged(this.laserEnergy);
        }

        private IEnumerator LaserChargeWait()
        {
            yield return new WaitForSeconds(1.4f);

            if (laserChargeFlag == 0)
            {
                laserEffects.SetActive(IsActive = true);
                laserSparksEmitter.enabled = true;
                laserSmokeEmitter.enabled = true;
                laserAudio.Play();
                laserChargeFlag = 0;
            }

            charging = null;
        }
    }
}