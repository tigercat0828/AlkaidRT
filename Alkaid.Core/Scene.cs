using Alkaid.Core.Data;
using Alkaid.Core.Material;
using Alkaid.Core.Primitives;

namespace Alkaid.Core;
public class Scene : IHitable {
    public List<IHitable> Items = new();
    public List<Light> Lights = new();
    public AABB Box { get; set; }
    public int ID => 0;
    public MaterialBase Material { get; set; }

    public Scene() {
        Box = new AABB();
    }

    public Scene(List<IHitable> items) {
        Items = items;
        Box = new AABB();
    }

    public Scene(List<IHitable> items, List<Light> lights) {
        Items = items;
        Lights = lights;
        Box = new AABB();
    }
    public void AddLight(Light light) {
        Lights.Add(light);
    }
    public void AddItem(IHitable item) {
        Items.Add(item);
        Box = new AABB(Box, item.Box);
    }

    public bool Hit(Ray ray) {
        foreach (var item in Items) {
            if (item.Hit(ray)) return true;
        }
        return false;
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        HitRecord tempRec = new();
        bool hitAny = false;
        float currentCloset = interval.max;

        foreach (var item in Items) {

            if (item.Hit(ray, new Interval(interval.min, currentCloset), ref tempRec)) {
                hitAny = true;
                currentCloset = tempRec.t;
                record = tempRec;
            }
        }
        return hitAny;
    }

    
}