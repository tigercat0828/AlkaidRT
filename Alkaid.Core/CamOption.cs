using System.Numerics;

namespace Alkaid.Core {
    public struct CamOption {
        public Vector3 Position = Vector3.Zero;
        public float AspectRatio = 16f / 9f;
        public int ImageWidth = 800;
        public CamOption() { }

        public CamOption(float aspectRatio, int imageWidth) {
            AspectRatio = aspectRatio;
            ImageWidth = imageWidth;
        }
    }
}
