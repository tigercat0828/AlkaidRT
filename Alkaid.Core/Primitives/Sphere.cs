using Alkaid.Core.Data;
using Alkaid.Core.Material;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Primitives;
public class Sphere : IHitable {
    public Vector3 Center { get; set; }
    public float Radius { get; set; }
    public MaterialBase Material { get; set; }
    public int ID { get; }
    public AABB Box {get; set;}

    public Sphere() : this(Vector3.Zero, 1, new PhongMat()) { }
    public Sphere(Vector3 center, float radius) : this(center, radius, new PhongMat()) { }
    public Sphere(Vector3 center, float radius, MaterialBase material) {
        ID = GetHashCode();
        Center = center;
        Radius = radius;
        Material = material;
        Vector3 boxRange = new (radius);
        Box = new AABB(Center - boxRange, Center + boxRange);
    }
    public bool Hit(Ray ray) {
        Vector3 oc = ray.Origin - Center;
        float a = ray.Direction.LengthSquared();
        float halfB = Dot(oc, ray.Direction);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = halfB * halfB - a * c;

        return discriminant >= 0;
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        Vector3 oc = ray.Origin - Center;
        float a = ray.Direction.LengthSquared();
        float halfB = Dot(oc, ray.Direction);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = halfB * halfB - a * c;
        if (discriminant < 0) return false;

        float sqrtd = Sqrt(discriminant);
        // Find the nearest root that lies in the acceptable range.
        float root = (-halfB - sqrtd) / a;
        if (!interval.Surrounds(root)) {
            root = (-halfB + sqrtd) / a;
            if (!interval.Surrounds(root))
                return false;
        }

        record.t = root;
        record.Point = ray.At(record.t);
        Vector3 outwardNormal = (record.Point - Center) / Radius;
        record.SetFaceNormal(ray, outwardNormal);

        record.Material = Material;
        record.ID = ID;
        return true;
    }
    public override string ToString() {
        return $"{Center}, {Radius}";
    }
}
