using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;
using Alkaid.Core.Data;
namespace Alkaid.Core.Extensions {
    public static class MathRT {
        private static readonly Random random = new();
        public static Vector3 Refract(Vector3 rin, Vector3 normal, float etai_over_etat) {
            float cosTheta = Min(Dot(-rin, normal), 1.0f);
            Vector3 routPerp = etai_over_etat * (rin + cosTheta * normal);
            Vector3 routPara = -Sqrt(Abs(1.0f - routPerp.LengthSquared())) * normal;
            return routPerp + routPara;
        }

        public static bool NearZero(Vector3 vec) {
            float error = 1e-8f;
            return Abs(vec.X) < error && Abs(vec.Y) < error && Abs(vec.Z) < error;
        }
        public static float RandomSingle(float min, float max) {
            float t = random.NextSingle();
            return t * (max - min) + min;
        }
        public static float RandomSingle() {
            return random.NextSingle();
        }
        public static Vector3 RandomUnitVector() {
            while (true) {
                float x = random.NextSingle() * 2.0f - 1.0f;
                float y = random.NextSingle() * 2.0f - 1.0f;
                float z = random.NextSingle() * 2.0f - 1.0f;
                Vector3 tmp = new(x, y, z);
                if (tmp.LengthSquared() < 1) {
                    return Normalize(tmp);
                }
            }
        }
        public static Color RandomColor() {
            float x = random.NextSingle() * 2.0f;
            float y = random.NextSingle() * 2.0f;
            float z = random.NextSingle() * 2.0f;
            return new Color(x, y, z);
        }
        public static float Min(float a, float b) {
            if (a < b) return a;
            return b;
        }
        public static float Max(float a, float b) {
            if(a > b) return a;
            return b;
        }
    }
}
