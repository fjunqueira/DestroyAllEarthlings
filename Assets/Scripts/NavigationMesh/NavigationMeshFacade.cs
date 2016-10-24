﻿using UnityEngine;
using Zenject;

namespace SpaceCentipedeFromHell
{
    public class NavigationMeshFacade : MonoBehaviour
    {
        private NavigationMesh navigationMesh;

        [Inject]
        public void Construct(NavigationMesh navigationMesh)
        {
            this.navigationMesh = navigationMesh;
        }
    }
}