using System.Numerics;

namespace Alkaid.Core;
public struct Ray {

    public Vector3 Origin { get; private set; }
    public Vector3 Direction { get; private set; }

    public Ray(Vector3 origin, Vector3 direction) {
        Origin = origin;
        Direction = direction;
    }
    public readonly Vector3 At(float t) {
        return Origin + t * Direction;
    }
}
