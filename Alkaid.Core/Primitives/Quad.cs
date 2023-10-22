using Alkaid.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkaid.Core.Primitives {
    public class Quad : IHitable {
        int IHitable.ID => throw new NotImplementedException();

        PhongMat IHitable.Material { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        bool IHitable.Hit(Ray ray) {
            throw new NotImplementedException();
        }

        bool IHitable.Hit(Ray ray, Interval interval, ref HitRecord record) {
            throw new NotImplementedException();
        }
    }
}
