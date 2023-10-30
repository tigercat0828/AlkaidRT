using Alkaid.Core.Data;

namespace Alkaid.Core.Material;

public abstract class MaterialBase
{
    public abstract bool Scatter(Ray ray, HitRecord record, ref Color attenuation, ref Ray scattered);
}
