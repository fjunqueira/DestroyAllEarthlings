using System.Linq;
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;

namespace DestroyAllEarthlings
{
    public class Destroyable : MonoBehaviour
    {
        [SerializeField]
        private Collider building;

        [SerializeField]
        private GameObject destructionFx;

        private void Start()
        {
            building.OnTriggerEnterAsObservable()
                .Where(collider => collider.transform.name == "Orbital_Laser_Hit")
                .Subscribe(collision =>
                {
                    building.gameObject.SetActive(false);
                    destructionFx.SetActive(true);

                    var children = destructionFx.GetComponentsInChildren<Rigidbody>();

                    foreach (var child in children)
                        child.AddForceAtPosition((this.transform.up - collision.transform.position).normalized * 10.0f, collision.transform.position, ForceMode.Impulse);

                    children.First().OnDestroyAsObservable().Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
                });
        }
    }
}