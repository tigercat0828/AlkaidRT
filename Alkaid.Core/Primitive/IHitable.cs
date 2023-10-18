using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkaid.Core.Primitive;
public interface IHitable {
    public Material Material { get; set; }
    public bool Hit(Ray ray);

    // may lead to bug
    public bool Hit(Ray ray, Interval interval, ref HitRecord record);
}
