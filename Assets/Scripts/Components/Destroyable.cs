﻿using System.Linq;
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.UI;

namespace DestroyAllEarthlings
{
    public class Destroyable : MonoBehaviour
    {
        [SerializeField]
        private int humanCount = 10;

        [SerializeField]
        private HudScore hudScore;

        [SerializeField]
        private TextMesh pointsMesh;

        [SerializeField]
        private Collider building;

        [SerializeField]
        private GameObject destructionFx;

        public int HumanCount { get { return this.humanCount; } }

        private void Start()
        {
            pointsMesh.text = humanCount.ToString();

            building.OnTriggerEnterAsObservable()
                .Where(collider => collider.transform.name == "Orbital_Laser_Hit")
                .Subscribe(collision =>
                {
                    ShowPoints();

                    building.gameObject.SetActive(false);
                    destructionFx.SetActive(true);

                    var children = destructionFx.GetComponentsInChildren<Rigidbody>();

                    foreach (var child in children)
                        child.AddForceAtPosition((this.transform.up - collision.transform.position).normalized * 10.0f, collision.transform.position, ForceMode.Impulse);

                    children.First().OnDestroyAsObservable().Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
                });
        }

        private void ShowPoints()
        {
            if (humanCount > 0)
            {
                pointsMesh.transform.position = pointsMesh.transform.position * 1.1f;
                pointsMesh.gameObject.SetActive(true);
                hudScore.RemainingHumans -= humanCount;
            }
        }
    }
}