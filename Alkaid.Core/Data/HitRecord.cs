using System.Numerics;

namespace Alkaid.Core.Data {
    public class HitRecord {
        public float t;
        public Vector3 Point;
        public Vector3 Normal;
        public Material Material = new();
        public int ID;
    }
}