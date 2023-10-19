using System.Numerics;

namespace Alkaid.Core.Data {
    public struct CamOption {
        public float AspectRatio = 16f / 9f;
        public int ImageWidth = 400;
        public float Fov = 90f;
        public Vector3 LookAt = new(0, 0, -1);
        public Vector3 LookFrom = new(0, 0, 0);
        public Vector3 Vup = Vector3.UnitY;
        public CamOption() { }

    }
}
