using System.Linq;
using UnityEngine;
using Zenject;

namespace SpaceCentipedeFromHell
{
    public class PlanetComponent : MonoBehaviour
    {
        private PlanetGrid grid;

        private FacePicker picker;

        private PlanetNode firstNode = null;

        private PlanetNode secondNode = null;

        [Inject]
        public void Construct(PlanetGrid grid, FacePicker facePicker)
        {
            this.picker = facePicker;
            this.grid = grid;
        }

        private bool doonce = true;
        private void Update()
        {
            if (firstNode != null && secondNode != null && doonce)
            {
                Debug.Log("Finding path..");
                var path = this.grid.FindPath(firstNode, secondNode);
                Debug.Log("Found path!");

                foreach (var node in path.Select(x => x as PlanetNode))
                {
                    Debug.Log("Drawing lines");
                    Debug.DrawLine(node.Triangle.A, node.Triangle.B, Color.black, float.PositiveInfinity, false);
                    Debug.DrawLine(node.Triangle.B, node.Triangle.C, Color.black, float.PositiveInfinity, false);
                    Debug.DrawLine(node.Triangle.C, node.Triangle.A, Color.black, float.PositiveInfinity, false);
                }
                doonce = false;
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Picking");
                var triangle = this.picker.Pick(Input.mousePosition);

                if (triangle == null)
                {
                    Debug.Log("Triangle is null");
                    return;
                }

                if (this.firstNode == null)
                {
                    Debug.Log("Setting first node.");
                    this.firstNode = this.grid.PositionIndexing[triangle.Centroid];
                }
                else
                {
                    Debug.Log("Setting second node.");
                    this.secondNode = this.grid.PositionIndexing[triangle.Centroid];
                }
            }
        }
    }
}