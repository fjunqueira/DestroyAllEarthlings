using System.Collections;
using System.Collections.Generic;
using ProgressBar;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class UFOWeapons : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private Laser laser;

        [SerializeField]
        private Transform ufo;

        [SerializeField]
        private Transform beam;

        [SerializeField]
        private Transform charger;

        private void LateUpdate()
        {
            if (Input.GetButton("Fire1"))
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, -ufo.transform.up, out hit, float.MaxValue, layerMask))
                {
                    laser.transform.position = hit.point + (hit.point.normalized * 4);
                    laser.transform.rotation = ufo.transform.rotation;

                    beam.transform.position = transform.position;
                    charger.transform.position = transform.position;
                }
            }
        }
    }
}