using System.Numerics;
namespace Alkaid.Core.Data {
    public class HitRecord {
        public float t;
        public Vector3 Point;
        public Vector3 Normal;
        public PhongMat Material = new();
        public int ID;
        public bool FrontFace;
        public void SetFaceNormal(Ray ray, Vector3 outwardNormal) {
            // assume that vec is normalized 
            FrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : -outwardNormal;
        }
    }
}