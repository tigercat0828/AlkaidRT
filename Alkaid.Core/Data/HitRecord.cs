using System.Numerics;
using Alkaid.Core.Material;

namespace Alkaid.Core.Data
{
    public class HitRecord {
        public float t;
        public Vector3 Point;
        public Vector3 Normal;
        public MaterialBase Material;
        public int ID;
        public bool FrontFace;
        public void SetFaceNormal(Ray ray, Vector3 outwardNormal) {
            // assume that vec is normalized 
            FrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : -outwardNormal;
        }
    }
}