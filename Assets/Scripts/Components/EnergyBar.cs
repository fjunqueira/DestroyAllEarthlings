using ProgressBar;
using System.Collections;
using UnityEngine;
using UniRx;

namespace DestroyAllEarthlings
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarBehaviour barBehaviour;

        private void Start()
        {
            barBehaviour.Value = 100;
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && barBehaviour.Value > 0)
                barBehaviour.Value -= Time.deltaTime;

            if (!Input.GetButton("Fire1") && barBehaviour.Value < 100)
                barBehaviour.Value += Time.deltaTime;
        }
    }
}