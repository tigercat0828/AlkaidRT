using Alkaid.Core.Material;
using Alkaid.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkaid.Core.Data {
    public class BvhNode : IHitable {
        public IHitable Left;
        public IHitable Right;
        public AABB Box { get; set; }
        public BvhNode(Scene scene)
        {
            
        }


        public bool Hit(Ray ray) {
            throw new NotImplementedException();
        }
        public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
            throw new NotImplementedException();
        }


        public int ID => 0;
        public MaterialBase Material { get; set; }
    }
}
