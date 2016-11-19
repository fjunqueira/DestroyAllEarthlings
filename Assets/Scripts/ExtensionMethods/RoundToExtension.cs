using UnityEngine;
using System.Collections;
using System;

public static class RoundToExtension
{

    public static Vector3 RoundTo(this Vector3 vector, int decimals)
    {
        return new Vector3(
			Mathf.Round(vector.x * 100f) / 100f, 
			Mathf.Round(vector.y * 100f) / 100f, 
			Mathf.Round(vector.z * 100f) / 100f);
    }
}
