using System.Numerics;
using static Alkaid.Core.Extensions.MathRT;
namespace Alkaid.Core.Data {
    public class AABB {
        public Interval Xintv, Yintv, Zintv;
        public AABB() {
            Xintv = new Interval();
            Yintv = new Interval();
            Zintv = new Interval();
        }

        public AABB(Interval x, Interval y, Interval z) {
            Xintv = x;
            Yintv = y;
            Zintv = z;
        }
        public AABB(Vector3 a, Vector3 b) {
            Xintv = new Interval(Min(a.X, b.X), Max(a.X, b.X));
            Yintv = new Interval(Min(a.Y, b.Y), Max(a.Y, b.Y));
            Zintv = new Interval(Min(a.Z, b.Z), Max(a.Z, b.Z));
        }

        public AABB(AABB box1, AABB box2) {
            Xintv = new Interval(box1.Xintv, box2.Xintv);
            Yintv = new Interval(box1.Yintv, box2.Yintv);
            Zintv = new Interval(box1.Zintv, box2.Zintv);
        }
        public AABB Padding() {
            float delta = 0.0001f;
            Interval newX = (Xintv.Size >= delta) ? Xintv : Xintv.Expand(delta);
            Interval newY = (Yintv.Size >= delta) ? Yintv : Yintv.Expand(delta);
            Interval newZ = (Zintv.Size >= delta) ? Zintv : Zintv.Expand(delta);
            return new AABB(newX, newY, newY);
        }
        public Interval Axis(int n) {
            if (n == 1) return Yintv;
            if (n == 2) return Zintv;
            return Xintv;
        }
        public bool Hit(Ray ray, Interval rayIntv) {

            float t0, t1, orig, invD;
            // X
            invD = 1 / ray.Direction.X;
            orig = ray.Origin.X;

            t0 = (Xintv.min - orig) * invD;
            t1 = (Xintv.max - orig) * invD;

            if (invD < 0) (t0, t1) = (t1, t0);

            rayIntv.min = Max(rayIntv.min, t0);
            rayIntv.max = Min(rayIntv.max, t1);

            if (rayIntv.max <= rayIntv.min) return false;

            // Y
            invD = 1 / ray.Direction.Y;
            orig = ray.Origin.Y;

            t0 = (Yintv.min - orig) * invD;
            t1 = (Yintv.max - orig) * invD;

            if (invD < 0) (t0, t1) = (t1, t0);

            rayIntv.min = Max(rayIntv.min, t0);
            rayIntv.max = Min(rayIntv.max, t1);

            if (rayIntv.max <= rayIntv.min) return false;

            // Z
            invD = 1 / ray.Direction.Z;
            orig = ray.Origin.Z;

            t0 = (Zintv.min - orig) * invD;
            t1 = (Zintv.max - orig) * invD;

            if (invD < 0) (t0, t1) = (t1, t0);

            rayIntv.min = Max(rayIntv.min, t0);
            rayIntv.max = Min(rayIntv.max, t1);

            if (rayIntv.max <= rayIntv.min) return false;

            return true;
        }
  
    }
}
