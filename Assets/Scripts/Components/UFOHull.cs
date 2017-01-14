using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class UFOHull : MonoBehaviour
    {
        public void RotateAround(Vector3 position, Vector3 up, float angle)
        {
            this.transform.RotateAround(position, up, angle);
        }
    }
}