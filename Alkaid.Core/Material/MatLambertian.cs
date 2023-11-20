using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using System.Numerics;
using static Alkaid.Core.Extensions.MathRT;

namespace Alkaid.Core.Material {
    public class MatLambertian : MaterialBase {
        Random random = new();
        Color albedo;
        public MatLambertian()
        {
            albedo = Color.White;
        }
        public MatLambertian(Color albedo) {
            this.albedo = albedo;
        }
        public override bool Scatter(Ray ray, HitRecord record, ref Color attenuation, ref Ray scattered) {
            Vector3 scatterDirection = record.Normal + random.UnitVector();
            if (NearZero(scatterDirection))
                scatterDirection = record.Normal;
            scattered = new Ray(record.Point, scatterDirection, ray.Time);
            attenuation = albedo;

            return true;
        }
    }
}
