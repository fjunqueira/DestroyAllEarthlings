using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private void Update()
    {
        transform.LookAt(cam.transform, cam.transform.up);
		transform.forward = -transform.forward;
    }
}
