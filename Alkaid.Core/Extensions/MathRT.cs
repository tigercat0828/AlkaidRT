using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Extensions {
    public static class MathRT {
        public static Random random = new ();
        public static Vector3 Refract(Vector3 uv, Vector3 n, float etai_over_etat) {
            float cos_theta = Min(Dot(-uv, n), 1.0f);
            Vector3 r_out_perp = etai_over_etat * (uv + cos_theta * n);
            Vector3 r_out_parallel = -Sqrt(Abs(1.0f - r_out_perp.LengthSquared())) * n;
            return r_out_perp + r_out_parallel;
        }
        public static Vector3 RandomUnitVector() {
            while (true) {
                Vector3 tmp = new(random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f);
                if (tmp.LengthSquared() < 1) {
                    return Vector3.Normalize(tmp);
                }
            }
        }
        public static bool NearZero(Vector3 vec) {
            float error = 1e-8f;
            return Abs(vec.X) < error && Abs(vec.Y) < error && Abs(vec.Z) < error;
        }
        public static float NextSingle(float min, float max) {
            float t = random.NextSingle();
            return t * (max - min) + min;
        }
    }
}
