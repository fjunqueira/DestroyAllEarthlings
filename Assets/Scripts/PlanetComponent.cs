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
                var path = this.grid.FindPath(firstNode, secondNode);

                foreach (var node in path.Select(x => x as PlanetNode))
                {
                    Debug.DrawLine(node.Triangle.A, node.Triangle.B, Color.black, 10, false);
                    Debug.DrawLine(node.Triangle.B, node.Triangle.C, Color.black, 10, false);
                    Debug.DrawLine(node.Triangle.C, node.Triangle.A, Color.black, 10, false);
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

                Debug.DrawLine(triangle.A, triangle.B, Color.black, 10, false);
                Debug.DrawLine(triangle.B, triangle.C, Color.black, 10, false);
                Debug.DrawLine(triangle.C, triangle.A, Color.black, 10, false);

                if (this.firstNode == null)
                {
                    Debug.Log("Setting first node.");
                    this.firstNode = this.grid.PositionIndexing[triangle.Centroid];
                }
                else if (this.secondNode == null)
                {
                    Debug.Log("Setting second node.");
                    this.secondNode = this.grid.PositionIndexing[triangle.Centroid];
                }
                else
                {
                    this.firstNode = this.grid.PositionIndexing[triangle.Centroid];
                    this.secondNode = null;
                    this.doonce = true;
                }
            }
        }
    }
}