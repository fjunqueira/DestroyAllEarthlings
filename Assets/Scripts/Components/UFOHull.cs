using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class UFOHull : MonoBehaviour
    {
        public void SetMovingMotion(Vector3 input, Vector3 up)
        {
            Debug.DrawRay(transform.parent.position, input * 100);

            var test = Quaternion.AngleAxis(90, up) * input;

            Debug.DrawRay(transform.parent.position, test * 100, Color.blue);

			transform.up = up;

            transform.Rotate(test, 20);
            
            // this.transform.rotation = 
			// 	Quaternion.RotateTowards(
			// 		transform.rotation, 
			// 		Quaternion.AngleAxis(50, test), 
			// 		Time.deltaTime * 100.0f);
        }

        public void ResetMovingMotion()
        {
            this.transform.rotation = Quaternion.identity;
        }
    }
}