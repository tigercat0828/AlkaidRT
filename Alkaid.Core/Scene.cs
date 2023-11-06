using Alkaid.Core.Data;
using Alkaid.Core.Material;
using Alkaid.Core.Primitives;

namespace Alkaid.Core;
public class Scene  {
    public List<IHitable> Items = new();
    public List<Light> Lights = new();
    private AABB Box = new();

    public int ID => throw new NotImplementedException();

    public MaterialBase Material { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
 
    public Scene() { }

    public Scene(List<IHitable> items) {
        Items = items;

    }

    public Scene(List<IHitable> items, List<Light> lights) {
        Items = items;
        Lights = lights;
    }
    public void AddLight(Light light) {
        Lights.Add(light);
    }
    public void AddItem(IHitable item) {
        Items.Add(item);
        Box = new AABB(Box, item.Box);
    }

    public bool HitAny(Ray ray) {
        foreach (var item in Items) {
            if (item.Hit(ray)) return true;
        }
        return false;
    }

    public bool HitAny(Ray ray, Interval interval, ref HitRecord record) {
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