using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DestroyAllEarthlings
{
    public class WaypointManager
    {
        public WaypointManager(List<PathfindingNode> path, Vector3 initialPosition)
        {
            Path = path;
            Current = path.Shift() as PlanetNode;
            Target = path.Shift() as PlanetNode;
            CurrentPosition = initialPosition;
            NextPosition = GetNextPosition(CurrentPosition, Target);
            Interpolation = 0.0f;
        }

        public List<PathfindingNode> Path { get; set; }
        public PlanetNode Current { get; set; }
        public PlanetNode Target { get; set; }
        public Vector3 CurrentPosition { get; set; }
        public Vector3 NextPosition { get; set; }
        public float Interpolation { get; set; }

        public bool ReachedWayPoint { get { return Interpolation >= 1.0f; } }

        public bool Arrived { get { return !Path.Any(); } }

        public void MoveNext()
        {
            Interpolation = 0;
            Current = Target;
            Target = Path.Shift() as PlanetNode;
            CurrentPosition = NextPosition;
            NextPosition = Path.Any() ? GetNextPosition(CurrentPosition, Target) : Target.Position;
        }

        private static Vector3 GetNextPosition(Vector3 currentPosition, PlanetNode target)
        {
            return target.Triangle.Vertices.OrderByDescending(vertice => Vector3.Distance(currentPosition, vertice)).First();
        }
    }
}