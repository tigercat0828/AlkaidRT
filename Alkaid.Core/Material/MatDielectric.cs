using Alkaid.Core.Data;
using System.Numerics;
using static Alkaid.Core.Extensions.MathR;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Material {

    public class MatDielectric : MaterialBase {
        public float ir;

        public MatDielectric(float ir) {
            this.ir = ir;
        }

        public override bool Scatter(Ray ray, HitRecord record, ref Color attenuation, ref Ray scattered) {
            attenuation = new Color(1.0f, 1.0f, 1.0f);
            float refraction_ratio = record.FrontFace ? (1.0f / ir) : ir;

            Vector3 unit_direction = Normalize(ray.Direction);
            Vector3 refracted = Refract(unit_direction, record.Normal, refraction_ratio);
            scattered = new Ray(record.Point, refracted);
            return true;
        }
    }
}
