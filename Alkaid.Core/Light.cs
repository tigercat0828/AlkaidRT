using System.Numerics;

namespace Alkaid.Core {
    public class Light {
        public Light(Vector3 position, Color intensity) {
            Position = position;
            Intensity = intensity;
        }
        public Vector3 Position { get; set; }
        public Color Intensity { get; set; }

    }
}
