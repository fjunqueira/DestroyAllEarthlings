using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class UFOCamera : MonoBehaviour
    {
        [SerializeField]
        private UFO ufo;

        private float angle = 180;

		private bool startRotatingS = false;
		
		private bool startRotatingW = false;

        private void Start()
        {
            //neededRotation = Quaternion.AngleAxis(180, ufo.transform.up);
            //transform.RotateAround(ufo.transform.position, ufo.transform.up, 180);
        }

        private void Update()
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * 100f);

			if (Input.GetKey(KeyCode.S)) startRotatingS = true;
			if (Input.GetKey(KeyCode.W)) startRotatingW = true;

            if (!startRotatingW && startRotatingS && angle < 180)
            {
                transform.RotateAround(ufo.transform.position, ufo.transform.up, 10);
                angle += 10;
            }
			else startRotatingS = false;

			if (!startRotatingS && startRotatingW && angle > 0)
            {
                transform.RotateAround(ufo.transform.position, ufo.transform.up, -10);
                angle -= 10;
            }
			else startRotatingW = false;

			Debug.Log(angle);
        }
    }
}