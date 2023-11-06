using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using System.Numerics;
using System.Runtime.InteropServices;
using static Alkaid.Core.Extensions.MathRT;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Material;

public class MatDielectric : MaterialBase {
    public float refractndex;

    public MatDielectric(float refractIndex) {
        this.refractndex = refractIndex;
    }
    public override bool Scatter(Ray ray, HitRecord record, ref Color attenuation, ref Ray scattered) {
        attenuation = new Color(1.0f, 1.0f, 1.0f);
        float refractionRatio = record.FrontFace ? (1.0f / refractndex) : refractndex;

        Vector3 unitDirection = Normalize(ray.Direction);

        float cosTheta = Math.Min(Dot(-unitDirection, record.Normal), 1.0f);
        float sinTheta = MathF.Sqrt(1.0f - cosTheta * cosTheta);

        bool cannotRefract = refractionRatio * sinTheta > 1.0f;
        Vector3 direction;

        if(cannotRefract || Reflectance(cosTheta, refractionRatio) > RandomSingle()) {
            direction = Vector3.Reflect(unitDirection, record.Normal);
        }
        else {
            direction = Refract(unitDirection, record.Normal, refractionRatio);
        }

        scattered = new Ray(record.Point, direction);
        return true;
    }
    private static float Reflectance(float cosine, float refidx) {
        // schlick's approximation
        float r = (1-refidx)/(1+refidx);
        r = r * r;
        return r + (1 - r) * MathF.Pow(1 - cosine, 5);
    }
}
