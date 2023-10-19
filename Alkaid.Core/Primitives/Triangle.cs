using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;

namespace Alkaid.Core.Primitives {
    public class Triangle : IHitable {
        public int ID { get; }
        public Vector3 pos1;
        public Vector3 pos2;
        public Vector3 pos3;

        public Vector3 Corner;
        public Vector3 U;
        public Vector3 V;
        public Vector3 Normal;
        private Vector3 W; // to determin alpha, beta
        private float D;    // D = Ax+By+Cz
        public Material Material { get; set; }
   

        public Triangle(Vector3 pos1, Vector3 pos2, Vector3 pos3, Material material) {
            this.pos1 = pos1;
            this.pos2 = pos2;
            this.pos3 = pos3;
            Corner = pos1;
            U = pos2 - pos1;
            V = pos3 - pos1;
            Material = material;
            Vector3 n = Cross(U, V);
            Normal = Normalize(n);
            D = Dot(Normal, Corner); // Ax+By+Cz = N dot (x,y,z)
            W = n / Dot(n, n);
            ID = GetHashCode();
        }
        public Triangle(Vector3 pos1, Vector3 pos2, Vector3 pos3) : this(pos1, pos2, pos3, new()) { }

        public bool Hit(Ray ray) {
            float denom = Dot(Normal, ray.Direction);
            if (Abs(denom) < 1e-8f) return false; // parallel to the plane
            float t = (D - Dot(Normal, ray.Origin)) / denom;

            Vector3 intersection = ray.At(t);
            Vector3 PlanarHitVector = intersection - Corner;
            float alpha = Dot(W, Cross(PlanarHitVector, V));
            float beta = Dot(W, Cross(U, PlanarHitVector));

            if (!IsInterior(alpha, beta)) return false;

            return true;
        }
        public bool Hit(Ray ray, Interval interval, ref HitRecord record) {

            float denom = Dot(Normal, ray.Direction);
            if (Abs(denom) < 1e-8f) return false; // parallel to the plane
            float t = (D - Dot(Normal, ray.Origin)) / denom;
            // occlusion here
            if (!interval.Contains(t)) return false;

            Vector3 intersect = ray.At(t);
            Vector3 PlanarHitVector = intersect - Corner;
            float alpha = Dot(W, Cross(PlanarHitVector, V));
            float beta = Dot(W, Cross(U, PlanarHitVector));

            if (!IsInterior(alpha, beta)) return false;

            record.t = t;
            record.Point = intersect;
            record.Normal = Normal;
            record.Material = Material;
            record.ID = ID;
            return true;
        }
        bool IsInterior(float a, float b) {

            if ((a < 0) || (1 < a) || (b < 0) || (1 < b))
                return false;

            if (a + b > 1) return false;

            return true;
        }

    }
}
