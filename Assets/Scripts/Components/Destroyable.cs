using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;

namespace SpaceCentipedeFromHell
{
    public class Destroyable : MonoBehaviour
    {
        [SerializeField]
        private Collider building;

        [SerializeField]
        private GameObject destructionFx;

        // private void Start()
        // {
        //     building.OnCollisionEnterAsObservable()
        //         .Select(x =>
        //         {
        //             Debug.Log(x);
        //             return x;
        //         })
        //         .Where(x => x.transform.name == "Orbital_Laser_Hit")
        //         .Subscribe(collision =>
        //         {
        //             building.gameObject.SetActive(false);
        //             destructionFx.SetActive(true);

        //             var children = destructionFx.GetComponentsInChildren<Rigidbody>();

        //             foreach (var child in children)
        //                 child.AddForceAtPosition(collision.impulse * 10.0f, this.transform.position, ForceMode.Impulse);

        //             //destructionFx.OnDestroyAsObservable().Subscribe(__ => Destroy(this.gameObject));
        //         });
        // }

        void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.name == "Orbital_Laser_Hit")
            {
                building.gameObject.SetActive(false);
                destructionFx.SetActive(true);

                var children = destructionFx.GetComponentsInChildren<Rigidbody>();

                foreach (var child in children)
                    child.AddForceAtPosition((this.transform.up - collision.transform.position).normalized * 10.0f, collision.transform.position, ForceMode.Impulse);
            }
        }

        // IEnumerator TestCoroutine()
        // {
        //     yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();

        //     building.gameObject.SetActive(false);

        //     destructionFx.SetActive(true);

        //     var children = destructionFx.GetComponentsInChildren<Rigidbody>();

        //     foreach (var child in children)
        //     {
        //         child.AddForceAtPosition(this.transform.up.normalized * 10.0f, this.transform.position, ForceMode.Impulse);
        //     }
        // }
    }
}