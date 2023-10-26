using Alkaid.Core.Data;

namespace Alkaid.Core.Render {
    public abstract class RendererBase {
        public abstract Color RayColor(Ray ray, Scene scene, int depth);
    }
}
