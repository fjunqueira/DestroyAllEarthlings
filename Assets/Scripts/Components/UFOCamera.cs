﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
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

        private float interpolation = 1;

        private float direction = 1;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow)) direction = -direction;

            var delta = direction * turningSpeed * Time.deltaTime;

            interpolation = Mathf.Clamp(interpolation += delta, 0, 1);

            var nextAngle = Mathf.LerpAngle(0, 180, interpolation) - angle;

            transform.RotateAround(ufo.transform.position, ufo.transform.up, nextAngle);
            hull.RotateAround(ufo.transform.position, ufo.transform.up, nextAngle);
            angle += nextAngle;
        }
    }
}