using System.Numerics;

namespace Alkaid.Core {
    public class Light {
        public Light(Vector3 position, Color intensity) {
            Position = position;
            Intensity = intensity;
        }
        public Light(Vector3 position) : this(position, Color.White) { }

        public Vector3 Position { get; set; }
        public Color Intensity { get; set; }
        public override string ToString() {
            return $"{Position}, {Intensity}";
        }

    }
}
