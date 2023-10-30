using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Numerics.Vector3;
namespace Alkaid.Core.Renderer;

// the Ray Tracing in One Weekend Ray Tracer
public class RtiowkRenderer : RendererBase {

    Random random = new ();
    public override Color RayColor(Ray ray, Scene scene, int depth) {
        if(depth <= 0) {
            return Color.None;
        }
        HitRecord record = new ();
        // 0.0001f -> fix acne
        if (scene.HitAny(ray, new Interval(0.0001f, float.MaxValue), ref record) ){
            // Vector3 direction = random.InUnitHemisphere(hitRecord.Normal);
            Vector3 direction = record.Normal + random.UnitVector();
            return 0.3f * RayColor(new Ray(record.Point, direction), scene, depth-1);
        }
        return Sky(ray);
    }
    
    Color bottom = new Color(0.5f, 0.7f, 1.0f);
    //Color bottom = new Color(1.0f, 0.7f, 0.3f);
    private Color Sky(Ray ray) {
        
        Vector3 unitDirection = Normalize(ray.Direction);
        float t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * Color.White + t * bottom;
    }
}
