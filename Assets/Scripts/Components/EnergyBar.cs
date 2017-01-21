using ProgressBar;
using System.Collections;
using UnityEngine;
using UniRx;

namespace DestroyAllEarthlings
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarBehaviour BarBehaviour;
        [SerializeField]
        private float UpdateDelay = 2f;

        private void Start()
        {
            BarBehaviour.Value = 100;

            //decrease percentage, decrease light, fire lazer, etc
            Observable.EveryUpdate()
                .Where(x => Input.GetButton("Fire1") && BarBehaviour.Value > 0)
                .Subscribe(_ =>
                {
                    BarBehaviour.Value -= Time.deltaTime;
                });

            //increase percentage, increase lights, etc
            Observable.EveryUpdate()
                .Where(x => !Input.GetButton("Fire1") && BarBehaviour.Value < 100)
                .Subscribe(_ =>
                {
                    BarBehaviour.Value += Time.deltaTime;
                });
        }
    }
}