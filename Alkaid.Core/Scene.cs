using Alkaid.Core.Primitive;

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

    public void AddItem(IHitable item) {
        Items.Add(item);
    }

    public bool HitAny(Ray ray) {
        foreach (var item in Items) {
            if (item.Hit(ray)) return true;
        }
        return false;
    }
    public bool HitAny(Ray ray, float tMin, float tMax, ref HitRecord record) {
        HitRecord tempRec = new();
        bool hitAny = false;
        float currentCloset = tMax;

        foreach (var item in Items) {
            
            if (item.Hit(ray, tMin, currentCloset, ref tempRec)) {
                hitAny = true;
                currentCloset = tempRec.t;
                record = tempRec;
            }
        }
        return hitAny;
    }
}