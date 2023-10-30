using Alkaid.Core.Data;
using Alkaid.Core.Material;

namespace Alkaid.Core.Primitives
{
    public class Quad : IHitable {
        public int ID => throw new NotImplementedException();
        MaterialBase IHitable.Material { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        bool IHitable.Hit(Ray ray) {
            throw new NotImplementedException();
        }

        bool IHitable.Hit(Ray ray, Interval interval, ref HitRecord record) {
            throw new NotImplementedException();
        }
    }
}
