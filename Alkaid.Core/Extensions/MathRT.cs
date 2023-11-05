using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Extensions {
    public static class MathRT {
        public static Vector3 Refract(Vector3 uv, Vector3 n, float etai_over_etat) {
            float cos_theta = Min(Dot(-uv, n), 1.0f);
            Vector3 r_out_perp = etai_over_etat * (uv + cos_theta * n);
            Vector3 r_out_parallel = -Sqrt(Abs(1.0f - r_out_perp.LengthSquared())) * n;
            return r_out_perp + r_out_parallel;
        }

    }
}
