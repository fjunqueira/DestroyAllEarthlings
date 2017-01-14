using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class UFO : MonoBehaviour
    {
        [SerializeField]
        private Animator idleAnimation;

        private bool reverseInput = true;

        private void Start()
        {
            this.idleAnimation.Play("UFOHullIdle");
        }

        private void Update()
        {
            var input = Vector3.zero;

            if (Input.GetKeyUp(KeyCode.UpArrow)) reverseInput = !reverseInput;

            if (Input.GetKey(KeyCode.W)) input.y = reverseInput ? 1 : -1;

            if (Input.GetKey(KeyCode.S)) input.y = reverseInput ? -1 : 1;

            if (Input.GetKey(KeyCode.A)) input.x = reverseInput ? 1 : -1;

            if (Input.GetKey(KeyCode.D)) input.x = reverseInput ? -1 : 1;

            if (input != Vector3.zero)
            {
                input = input.normalized;

                transform.rotation = Quaternion.AngleAxis(input.x, transform.forward) *
                                     Quaternion.AngleAxis(input.y, transform.right) *
                                     transform.rotation;
            }
        }
    }
}