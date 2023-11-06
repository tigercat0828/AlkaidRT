using Alkaid.Core.Data;
using Alkaid.Core.Material;

namespace Alkaid.Core.Primitives {
    public class Quad : IHitable {
        public int ID => throw new NotImplementedException();
        public MaterialBase Material { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AABB Box { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Hit(Ray ray) {
            throw new NotImplementedException();
        }
        public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
            throw new NotImplementedException();
        }
    }
}
