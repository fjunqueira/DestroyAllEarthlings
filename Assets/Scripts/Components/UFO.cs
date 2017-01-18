using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class UFO : MonoBehaviour
    {
        private bool reverseInput = false;

        [SerializeField]
        private float speed = 50;

        private void Update()
        {
            var input = Vector3.zero;

            if (Input.GetKeyUp(KeyCode.UpArrow)) reverseInput = !reverseInput;

            if (Input.GetKey(KeyCode.W)) input.y = 1;

            if (Input.GetKey(KeyCode.S)) input.y = -1;

            if (Input.GetKey(KeyCode.A)) input.x = 1;

            if (Input.GetKey(KeyCode.D)) input.x = -1;

            if (input != Vector3.zero)
            {
                input = input.normalized;

                transform.rotation = Quaternion.AngleAxis(speed * Time.deltaTime * (!reverseInput ? input.x : -input.x), transform.forward) *
                                     Quaternion.AngleAxis(speed * Time.deltaTime * (!reverseInput ? input.y : -input.y), transform.right) *
                                     transform.rotation;
            }
        }
    }
}