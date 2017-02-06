using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
}
