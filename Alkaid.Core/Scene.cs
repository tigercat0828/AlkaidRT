using Alkaid.Core.Data;
using Alkaid.Core.Primitives;

namespace Alkaid.Core;
public class Scene {
    public List<IHitable> Items = new();
    public List<Light> Lights = new();
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
    }

    public bool HitAny(Ray ray) {
        foreach (var item in Items) {
            if (item.Hit(ray)) return true;
        }
        return false;
    }
    public bool InShadow(Ray ray, int id) {
        foreach (var item in Items) {
            if (item.ID == id) continue;
            HitRecord record = new HitRecord();
            if (item.Hit(ray, new Interval(0, float.MaxValue), ref record)) {
                return true;
            }
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