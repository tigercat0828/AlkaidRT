using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using System.Numerics;

namespace Alkaid.Core.Material {
    public class LambertianMat : MaterialBase {
        Random random = new ();
        Color albedo;

        public LambertianMat(Color albedo) {
            this.albedo = albedo;
        }
        public override bool Scatter(Ray ray, HitRecord record, ref Color attenuation, ref Ray scattered) {
            Vector3 scatterDirection = record.Normal + random.UnitVector();
            if (scatterDirection.NearZero())
                scatterDirection = record.Normal;
            scattered = new Ray(record.Point, scatterDirection);
            attenuation = albedo;

            return true;
        }
    }
}
