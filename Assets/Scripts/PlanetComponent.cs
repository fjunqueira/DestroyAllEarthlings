using UnityEngine;
using Zenject;

namespace SpaceCentipedeFromHell
{
    public class PlanetComponent : MonoBehaviour
    {
        //private NavigationMesh navigationMesh;

        private PlanetGrid grid;

        [Inject]
        public void Construct(PlanetGrid grid)
        {
            //this.navigationMesh = navigationMesh;
            this.grid = grid;
        }
    }
}