using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class UFO : MonoBehaviour
    {
        [SerializeField]
        private Animator idleAnimation;

        private void Start()
        {
            this.idleAnimation.Play("UFOHullIdle");
        }

        private void Update()
        {

            var input = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) input.y = 1;

            if (Input.GetKey(KeyCode.A)) input.x = 1;

            if (Input.GetKey(KeyCode.S)) input.y = -1;

            if (Input.GetKey(KeyCode.D)) input.x = -1;

            if (input == Vector3.zero)
            {
                this.idleAnimation.enabled = true;
            }
            else
            {
                this.idleAnimation.enabled = false;

                input = input.normalized;

                transform.rotation = Quaternion.AngleAxis(input.x, transform.forward) *
                                     Quaternion.AngleAxis(input.y, transform.right) *
                                     transform.rotation;
            }
        }
    }
}