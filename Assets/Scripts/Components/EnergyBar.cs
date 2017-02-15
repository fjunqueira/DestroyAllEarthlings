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
            Laser.laserEnergyChanged += (energy => barBehaviour.Value = energy);
        }
    }
}