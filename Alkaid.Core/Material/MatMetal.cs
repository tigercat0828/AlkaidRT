using Alkaid.Core.Data;
using System.Numerics;
using static System.Numerics.Vector3;
using static Alkaid.Core.Extensions.MathRT;
using System.ComponentModel.DataAnnotations;

namespace Alkaid.Core.Material;
public class MatMetal : MaterialBase {

    public Color albedo;
    public float fuzz; // smaller, specular
    public MatMetal() : this(Color.White, 0) { }
    
    public MatMetal(Color albedo, float fuzz) {
        this.albedo = albedo;
        fuzz = Math.Clamp(fuzz, 0, 1);
        this.fuzz = fuzz;
    }

    public override bool Scatter(Ray ray, HitRecord record, ref Color attenuation, ref Ray scattered) {
        Vector3 reflected = Reflect(Normalize(ray.Direction), record.Normal);
        scattered = new Ray(record.Point, reflected + fuzz * RandomUnitVector(), ray.Time);
        attenuation = albedo;
        return (Dot(scattered.Direction, record.Normal)) > 0;
    }
}
