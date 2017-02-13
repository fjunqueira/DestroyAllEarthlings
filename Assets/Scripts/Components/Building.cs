using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class Building : Destroyable
    {
        [SerializeField]
        private List<PathFollower> humans;

        public override int EarthlingCount { get { return base.EarthlingCount + humans.Count; } }

        protected override void TriggerEnter(Collider collider)
        {
            var pathfindingObstacle = gameObject.GetComponent<PathfindingObstacle>();
            foreach (var human in humans) human.StartingNodePosition = pathfindingObstacle.BlockingNodePosition;

            base.TriggerEnter(collider);

            StartCoroutine(StartFleeingHumans());

            var children = destructionFx.GetComponentsInChildren<Rigidbody>();

            foreach (var child in children)
                child.AddForceAtPosition((this.transform.up - collider.transform.position).normalized * 10.0f, collider.transform.position, ForceMode.Impulse);

            children.First().OnDestroyAsObservable().Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
        }

        private IEnumerator StartFleeingHumans()
        {
            foreach (var human in humans)
            {
                yield return new WaitForSeconds(Random.Range(0, 0.5f));
                human.gameObject.SetActive(true);
            }
        }
    }
}