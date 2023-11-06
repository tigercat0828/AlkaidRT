using Alkaid.Core.Data;
using Alkaid.Core.Material;

namespace Alkaid.Core.Primitives;
public interface IHitable {
    public int ID { get; }
    public MaterialBase Material { get; set; }
    public AABB Box { get; set; }
    public bool Hit(Ray ray);

    public bool Hit(Ray ray, Interval interval, ref HitRecord record);
}
