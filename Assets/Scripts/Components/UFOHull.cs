using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class UFOHull : MonoBehaviour
    {
        [SerializeField]
        private Transform laser;

        [SerializeField]
        private UFO ufo;

        [SerializeField]
        private Transform beam;

        [SerializeField]
        private Light hullLight;

        private Material material;

        private Color blue = new Color((float)129 / 255, (float)177 / 255, (float)255 / 255, (float)255 / 255);

        private Color green = new Color((float)75 / 255, (float)255 / 255, (float)51 / 255, (float)255 / 255);

        private void Start()
        {
            material = transform.GetComponent<Renderer>().material;
        }

        public void RotateAround(Vector3 position, Vector3 up, float angle)
        {
            this.transform.RotateAround(position, up, angle);
        }

        private void Update()
        {
            hullLight.spotAngle += (Input.GetButton("Fire1") ? -Time.deltaTime : Time.deltaTime) * 100;

            hullLight.spotAngle = Mathf.Clamp(hullLight.spotAngle, 5, 70);

            var color = Color.Lerp(blue, green, hullLight.spotAngle / 70);

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
                }
            }
        }
    }
}