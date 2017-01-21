using System.Collections;
using System.Collections.Generic;
using ProgressBar;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class UFOHull : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarBehaviour shipEnergy;

        [SerializeField]
        private Laser laser;

        [SerializeField]
        private UFO ufo;

        [SerializeField]
        private Transform beam;

        [SerializeField]
        private Transform charger;

        [SerializeField]
        private Light hullLight;

        private Material material;

        private Color blue = new Color((float)129 / 255, (float)177 / 255, (float)255 / 255, (float)255 / 255);

        private Color green = new Color((float)75 / 255, (float)255 / 255, (float)51 / 255, (float)255 / 255);

        private float interpolation = 1;

        private void Start()
        {
            material = transform.GetComponent<Renderer>().material;
        }

        public void RotateAround(Vector3 position, Vector3 up, float angle)
        {
            this.transform.RotateAround(position, up, angle);
        }

        // move to laser script
        private void Update()
        {
            float delta = 0;//Input.GetButton("Fire1") ? -Time.deltaTime : Time.deltaTime;

            if (this.laser.laserChargeBeam.activeSelf)
                delta = -Time.deltaTime;
            else if (!this.laser.laserEffects.activeSelf)
                delta = Time.deltaTime;

            interpolation = Mathf.Clamp(interpolation += delta, 0, 1);

            hullLight.spotAngle = Mathf.LerpAngle(5, 70, interpolation);

            var color = Color.Lerp(blue, green, interpolation);

            material.SetColor("_EmissionColor", color);
            hullLight.color = color;
        }

        private void LateUpdate()
        {
            if (Input.GetButton("Fire1"))
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, -ufo.transform.up, out hit))
                {
                    laser.transform.position = hit.point + (hit.point.normalized * 4);
                    laser.transform.rotation = ufo.transform.rotation;

                    beam.transform.position = transform.position;
                    charger.transform.position = transform.position;
                }
            }
        }
    }
}