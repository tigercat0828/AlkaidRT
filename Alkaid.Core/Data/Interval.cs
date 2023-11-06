using Alkaid.Core.Extensions;
using System.Drawing;

namespace Alkaid.Core.Data;
public class Interval {
    public float min;
    public float max;
    public Interval()
    {
        max = float.MinValue; 
        min = float.MaxValue;
    }
    public Interval(float min, float max) {
        this.min = min;
        this.max = max;
    }
    public Interval(Interval a, Interval b) {
        min = MathRT.Min(a.min, b.min);
        max = MathRT.Max(a.max, b.max);
    }
    public bool Surrounds(float x) => x > min && x < max;
    public bool Contains(float x) => x >= min && x <= max;
    public float Size => max - min;
    public Interval Expand(float delta) {
        float padding = delta / 2;
        return new Interval(min -padding, max + padding);
    }
}
