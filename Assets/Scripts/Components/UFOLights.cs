using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DestroyAllEarthlings
{
    public class UFOLights : MonoBehaviour
    {
        [SerializeField]
        private Laser laser;

        [SerializeField]
        private Light hullLight;

        [SerializeField]
        private Material material;

        private Color blue = new Color((float)129 / 255, (float)177 / 255, (float)255 / 255, (float)255 / 255);

        private Color green = new Color((float)75 / 255, (float)255 / 255, (float)51 / 255, (float)255 / 255);

        private float interpolation = 1;

        private void Update()
        {
            float delta = 0;

            if (laser.IsCharging) delta = -Time.deltaTime;
            else if (!laser.IsActive) delta = Time.deltaTime;

            interpolation = Mathf.Clamp(interpolation += delta, 0, 1);

            hullLight.spotAngle = Mathf.LerpAngle(5, 100, interpolation);

            var color = Color.Lerp(blue, green, interpolation);

            material.SetColor("_EmissionColor", color);
            hullLight.color = color;
        }
    }
}