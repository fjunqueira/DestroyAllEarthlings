using UnityEngine;
using System.Collections;
using Zenject;

namespace SpaceCentipedeFromHell
{
    public class PlanetFacade : MonoBehaviour
    {
        private NavigationMesh navigationMesh;

        public void Start()
        {
            Debug.Log("Hello!");
        }

        [Inject]
        public void Construct(NavigationMesh navigationMesh)
        {
            this.navigationMesh = navigationMesh;
        }
    }
}