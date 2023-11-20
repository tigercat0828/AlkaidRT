using Alkaid.Core.Data;
using System.Numerics;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Renderer;

// the Ray Tracing in One Weekend Ray Tracer
public class RtiowkRenderer : RendererBase {

    Random random = new();
    public override Color RayColor(Ray ray, Scene scene, int depth) {
        if (depth <= 0) {
            return Color.None;
        }
        HitRecord record = new();
        // 0.0001f -> fix acne
        if (scene.Hit(ray, new Interval(0.0001f, float.MaxValue), ref record)) {
            Ray scattered = new Ray();
            Color attenuation = new Color();
            if (record.Material.Scatter(ray, record, ref attenuation, ref scattered)) {
                return attenuation * RayColor(scattered, scene, depth - 1);
            }
            return Color.None;
        }
        return SkyColor(ray);
    }

    Color bottom = new Color(0.5f, 0.7f, 1.0f);
    private Color SkyColor(Ray ray) {

        Vector3 unitDirection = Normalize(ray.Direction);
        float t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * Color.White + t * bottom;
    }
}
