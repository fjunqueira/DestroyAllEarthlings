using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
namespace DestroyAllEarthlings
{
    public class Human : MonoBehaviour
    {
        [SerializeField]
        private PlanetNavMesh navMesh;


        [SerializeField]
        private Vector3 startingNodePosition;


        private void Start()
        {
            var path = navMesh.FindPath(navMesh.GetNodeByPosition(startingNodePosition)).ToList();

            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (!path.Any()) return;
                
                transform.position = path.Shift().Position;
            }).AddTo(this);
        }
    }
}