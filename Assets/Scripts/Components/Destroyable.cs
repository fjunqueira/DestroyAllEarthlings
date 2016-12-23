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

        private void Start()
        {
            StartCoroutine(TestCoroutine());

            // building.OnCollisionEnterAsObservable()
            //     .Where(x => x.transform.GetComponent<Player>() != null)
            //     .Subscribe(_ =>
            //     {
            //         building.gameObject.SetActive(false);
            //         destructionFx.SetActive(true);

            //         destructionFx.OnDestroyAsObservable().Subscribe(__ => Destroy(this.gameObject));
            //     });
        }

        IEnumerator TestCoroutine()
        {
            yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();

            building.gameObject.SetActive(false);

            destructionFx.SetActive(true);

            var children = destructionFx.GetComponentsInChildren<Rigidbody>();

            foreach (var child in children)
            {
                child.AddForceAtPosition(this.transform.up.normalized * 10.0f, this.transform.position, ForceMode.Impulse);
            }
        }
    }
}