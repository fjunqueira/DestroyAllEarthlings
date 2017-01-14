using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class UFOCamera : MonoBehaviour
    {
        [SerializeField]
        private UFO ufo;

        [SerializeField]
        private UFOHull hull;

        [SerializeField]
        private float turningSpeed = 2f;

        private float angle = 180;

        private bool startRotatingS = true;

        private bool startRotatingW = false;

        private float interpolation = 1;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                startRotatingS = startRotatingW;
                startRotatingW = !startRotatingW;
            }

            var rotatingToFront = !startRotatingW && startRotatingS && interpolation < 1;
            var rotatingToBack = !startRotatingS && startRotatingW && interpolation > 0;

            if (rotatingToBack || rotatingToFront)
            {
                var delta = turningSpeed * (rotatingToFront ? Time.deltaTime : -Time.deltaTime);

                interpolation = Mathf.Clamp(interpolation += delta, 0, 1);

                var nextAngle = Mathf.LerpAngle(0, 180, interpolation) - angle;

                transform.RotateAround(ufo.transform.position, ufo.transform.up, nextAngle);
                hull.RotateAround(ufo.transform.position, ufo.transform.up, nextAngle);
                angle += nextAngle;
            }
        }
    }
}