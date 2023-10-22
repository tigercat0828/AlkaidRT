using Alkaid.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Alkaid.Core.Render {
    public class CheckHitRenderer : RendererBase {
        public override Color RayColor(Ray ray, Scene scene, int depth) {
            
            HitRecord record = new();
            if(scene.HitAny(ray, new Interval(0,float.MaxValue), ref record)){
                return 0.5f * (Color)(Vector3.One + record.Normal);
            }
            return Color.Black;
        }
    }
}
