﻿using System.Collections.Generic;

namespace DestroyAllEarthlings
{
    public sealed class PathfindingNodeComparator<TNode> : IComparer<TNode> where TNode : PathfindingNode
    {
        public int Compare(TNode n1, TNode n2)
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