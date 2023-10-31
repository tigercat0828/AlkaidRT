using Alkaid.Core.Data;
using System.Numerics;
using static Alkaid.Core.Extensions.MathR;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Material;
public class MatMetal : MaterialBase {

    public Color albedo;

    public MatMetal(Color albedo) {
        this.albedo = albedo;
    }
  
    public override bool Scatter(Ray ray, HitRecord record, ref Color attenuation, ref Ray scattered) {
        Vector3 reflected = Vector3.Reflect(Normalize(ray.Direction), record.Normal);
        scattered = new Ray(record.Point, reflected);
        attenuation = albedo;
        return true;
    }
}
