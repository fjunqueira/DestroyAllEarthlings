using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace SpaceCentipedeFromHell
{
    [Serializable]
    public class Triangle
    {
        public Triangle(IEnumerable<Vector3> vertices)
        {
            this.Vertices = new List<Vector3>(vertices);
        }

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            this.Vertices = new List<Vector3>() { a, b, c };
        }

        public Vector3 Centroid
        {
            get
            {
                return new Vector3((this.C.x + this.A.x + this.B.x) / 3, (this.C.y + this.A.y + this.B.y) / 3, (this.C.z + this.A.z + this.B.z) / 3);
            }
        }

        public List<Vector3> Vertices { get; set; }

        public Vector3 A { get { return this.Vertices.ElementAt(0); } set { this.Vertices.Insert(0, value); } }
        public Vector3 B { get { return this.Vertices.ElementAt(1); } set { this.Vertices.Insert(1, value); } }
        public Vector3 C { get { return this.Vertices.ElementAt(2); } set { this.Vertices.Insert(2, value); } }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Triangle otherTriangle = obj as Triangle;

            if (otherTriangle == null) return false;

            return (this.A == otherTriangle.A) && (this.B == otherTriangle.B) && (this.C == otherTriangle.C);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + this.A.GetHashCode();
                hash = (hash * 23) + this.C.GetHashCode();
                hash = (hash * 23) + this.B.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("A: {0} B: {1} C: {2}", this.A, this.B, this.C);
        }
    }
}