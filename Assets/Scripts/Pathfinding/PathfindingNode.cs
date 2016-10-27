using UnityEngine;
using System;

namespace SpaceCentipedeFromHell
{
    public abstract class PathfindingNode
    {
        public PathfindingNode()
        {
            this.IsWalkable = true;
        }

        private float weight;

        public float Weight
        {
            get
            {
                return this.weight;
            }
        }

        public bool IsWalkable { get; set; }

        public abstract Vector3 Position { get; }

        internal bool WasVisited { get; set; }

        internal bool IsOnOpenList { get; set; }

        internal float F
        {
            get
            {
                return this.G + this.H;
            }
        }

        internal float G { get; set; }

        internal float H { get; set; }

        internal Guid RunId { get; set; }

        internal PathfindingNode ParentNode { get; set; }

        public void RaiseWeight(float raise)
        {
            this.weight += raise;
        }

        public void DecreaseWeight(float decrease)
        {
            this.weight -= decrease;
        }

        public abstract PathfindingNode[] GetAdjacentNodes();

        internal void ResetInfo(Guid newRunId)
        {
            this.RunId = newRunId;
            this.H = 0;
            this.G = 0;
            this.ParentNode = null;
            this.IsOnOpenList = false;
        }
    }
}