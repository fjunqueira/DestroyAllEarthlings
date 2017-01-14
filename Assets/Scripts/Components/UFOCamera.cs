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
        private float turningSpeed = 200;

        private float angle = 180;

        private bool startRotatingS = true;

        private bool startRotatingW = false;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                startRotatingS = startRotatingW;
                startRotatingW = !startRotatingW;
            }

            var rotatingToFront = !startRotatingW && startRotatingS && angle < 180;
            var rotatingToBack = !startRotatingS && startRotatingW && angle > 0;

            var nextAngle = Time.deltaTime * turningSpeed * (rotatingToFront ? 1 : -1);

            if (rotatingToFront && angle + nextAngle > 180) nextAngle = 180 - angle;
            if (rotatingToBack && angle + nextAngle < 0) nextAngle = -angle;

            if (rotatingToBack || rotatingToFront)
            {
                transform.RotateAround(ufo.transform.position, ufo.transform.up, nextAngle);
                hull.RotateAround(ufo.transform.position, ufo.transform.up, nextAngle);
                angle += nextAngle;
            }
        }
    }
}