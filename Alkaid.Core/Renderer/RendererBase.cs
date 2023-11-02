using Alkaid.Core.Data;

namespace Alkaid.Core.Renderer;
public abstract class RendererBase {
    public abstract Color RayColor(Ray ray, Scene scene, int depth);
}
