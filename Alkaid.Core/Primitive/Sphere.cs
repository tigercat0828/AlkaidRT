﻿using System.Numerics;
using static System.Numerics.Vector3;
using static System.MathF;
namespace Alkaid.Core.Primitive; 
public class Sphere : IHitable {

    public Vector3 Center { get; set; }
    public float Radius { get; set; }
    public Material Material { get; set; }
    public Sphere() : this(Vector3.Zero, 1, new()) { }

    public Sphere(Vector3 center, float radius) : this(center, radius, new()) { }

    public Sphere(Vector3 center, float radius, Material material) {
        Center = center;
        Radius = radius;
        Material = material;
    }

    public bool Hit(Ray ray) {
        Vector3 oc = ray.Origin - Center;
        float a = ray.Direction.LengthSquared();
        float halfB = Dot(oc, ray.Direction);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = halfB * halfB - a * c;

        return discriminant >= 0;
    }

    public bool Hit(Ray ray, float tMin, float tMax,ref HitRecord record) {
        Vector3 oc = ray.Origin - Center;
        float a = ray.Direction.LengthSquared();
        float halfB = Dot(oc, ray.Direction);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = halfB * halfB - a * c;
        if (discriminant < 0) return false;
        
        float sqrtd = Sqrt(discriminant);
        // Find the nearest root that lies in the acceptable range.
        float root = (-halfB - sqrtd) / a;
        if (root <= tMin || tMax <= root) {
            root = (-halfB + sqrtd) / a;
            if (root <= tMin || tMax <= root)
                return false;
        }

        record.t = root;
        record.Point = ray.At(record.t);
        record.Normal = (record.Point - Center) / Radius;
        record.Material = Material;
        return true;
    }
}
