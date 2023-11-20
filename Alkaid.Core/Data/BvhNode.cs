using Alkaid.Core.Extensions;
using Alkaid.Core.Material;
using Alkaid.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkaid.Core.Data; 
public class BvhNode : IHitable {
    public IHitable Left;
    public IHitable Right;
    public AABB Box { get; set; }
    public BvhNode(Scene scene):this(scene.Items, 0, scene.Items.Count)
    {
        
    }
    public BvhNode(List<IHitable> items, int start, int end) {
        int axis = MathRT.RandomNext(0, 3);
        IComparer<IHitable> comparator;
        if (axis == 0) comparator = new XComparator();
        if (axis == 1) comparator = new YComparator();
        else comparator = new ZComparator();

        int itemSpan = end - start;

        if (itemSpan == 1) {
            Left = Right = items[start];
        } 
        else if (itemSpan == 2) {
            if (comparator.Compare(items[start], items[start + 1]) <= 0) {
                Left = items[start];
                Right = items[start + 1];
            }
            else {
                Left = items[start + 1];
                Right = items[start];
            }
        }
        else {
            items.Sort(start, end - start, comparator);
            var mid = start + itemSpan / 2;
            Left = new BvhNode(items, start, mid);
            Right = new BvhNode(items, mid, end);
        }
        Box = new AABB(Left.Box, Right.Box);
    }

    public bool Hit(Ray ray) {
        throw new NotImplementedException();
    }
    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        if (!Box.Hit(ray, interval)) return false;

        bool hitLeft = Left.Hit(ray, interval, ref record);
        bool hitRight = Right.Hit(ray,
            new Interval(interval.min, hitLeft ? record.t : interval.max), ref record);
        return hitLeft || hitRight;
    }

    public int ID => 0;
    public MaterialBase Material { get; set; }
}
public class XComparator : IComparer<IHitable> {
    public int Compare(IHitable? x, IHitable? y) {
        return MathRT.BoxCompare(x, y, 0);

    }
}
public class YComparator : IComparer<IHitable> {
    public int Compare(IHitable? x, IHitable? y) {
        return MathRT.BoxCompare(x, y, 1);

    }
}
public class ZComparator : IComparer<IHitable> {
    public int Compare(IHitable? x, IHitable? y) {
        return MathRT.BoxCompare(x, y, 2);
    }
}