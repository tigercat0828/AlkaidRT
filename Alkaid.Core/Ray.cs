using System.Numerics;

namespace Alkaid.Core;
public struct Ray {

    public Vector3 Origin { get; private set; }
    public Vector3 Direction { get; private set; }
    public float Time { get; set; }
    public Ray(Vector3 origin, Vector3 direction, float time = 0.0f) {
        Origin = origin;
        Direction = direction;
        Time = time;
    }
    public Ray(Vector3 origin, Vector3 direction): this(origin, direction, 0.0f) {

    }
    public readonly Vector3 At(float t) {
        return Origin + t * Direction;
    }
}
