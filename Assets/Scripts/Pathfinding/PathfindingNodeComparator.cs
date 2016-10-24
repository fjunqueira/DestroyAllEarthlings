using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceCentipedeFromHell
{
    public sealed class PathfindingNodeComparator : IComparer<PathfindingNode>
    {
        public int Compare(PathfindingNode n1, PathfindingNode n2)
        {
            if (n1 == n2)
            {
                return 0;
            }

            if (n1 == null || n2 == null)
            {
                return 2;
            }

            if (n1.F < n2.F)
            {
                return -1;
            }

            if (n1.F > n2.F)
            {
                return 1;
            }

            return -2;
        }
    }
}