namespace Alkaid.Core;
public class Interval {
    public float min;
    public float max;
    public Interval(float min, float max) {
        this.min = min;
        this.max = max;
    }
    public Interval() : this(0, 0) { }
    public bool Surrounds(float x) => x > min && x < max;
    public bool Contains(float x) => x >= min && x <= max;
}
