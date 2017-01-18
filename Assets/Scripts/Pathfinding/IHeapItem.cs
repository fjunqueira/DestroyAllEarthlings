using UnityEngine;
using System.Collections;

namespace DestroyAllEarthlings
{
    public interface IHeapItem
    {
        int HeapIndex { get; set; }
    }
}