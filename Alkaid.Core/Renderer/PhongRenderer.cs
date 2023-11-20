using Alkaid.Core.Data;
using Alkaid.Core.Material;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;

namespace Alkaid.Core.Renderer;
public class PhongRenderer : RendererBase {
    // if output is weird, try add one light
    public override Color RayColor(Ray ray, Scene scene, int depth) {
        if (depth == 0)
            return Color.None;
        depth--;
        HitRecord record = new();
        if (scene.Hit(ray, new Interval(0, float.MaxValue), ref record)) {

            Vector3 normal = Normalize(record.Normal);
            Vector3 pixelPos = record.Point;

            // TODO : fix multiple light
            //foreach(Light light in scene.Lights) {
            Light light = scene.Lights[0];
            Vector3 viewDir = Normalize(ray.Direction);
            PhongMat material = (PhongMat)record.Material;

            Color pixelColor = new();
            Vector3 lightDir = Normalize(pixelPos - light.Position);
            Ray pixelToLightRay = new(pixelPos, -lightDir);
            Vector3 lightReflected = Normalize(Reflect(lightDir, normal));
            // ambient
            Color ambient = material.Ka * material.Albedo * light.Intensity;

            // diffuse
            float diffStren = Max(Dot(-lightDir, normal), 0);
            Color diffuse = diffStren * material.Kd * material.Albedo * light.Intensity;

            // specular
            float specStren = Pow(Max(Dot(lightReflected, -viewDir), 0), material.Shineness);
            Color specular = specStren * material.Ks * material.Albedo * light.Intensity;

            int HitID = record.ID;
            if (IsInShadow(pixelToLightRay, scene, HitID)) {
                pixelColor = ambient;
            }
            else {
                pixelColor = ambient + diffuse + specular;
            }
            float r = material.Reflect;
            if (r > 0) {
                Vector3 viewReflectedDir = Reflect(viewDir, normal);
                Ray viewDirReflectedRay = new(pixelPos + 0.000001f * viewReflectedDir, viewReflectedDir);
                return ((1 - r) * pixelColor).Clamp() + (r * RayColor(viewDirReflectedRay, scene, depth)).Clamp();
            }
            else {
                return pixelColor.Clamp();
            }
            //}

        }
        return BackgroundSky(ray); // background !
    }
    private bool IsInShadow(Ray ray, Scene scene, int id) {
        var Items = scene.Items;
        foreach (var item in Items) {
            if (item.ID == id)
                continue;
            HitRecord record = new HitRecord();
            if (item.Hit(ray, new Interval(0, float.MaxValue), ref record)) {
                return true;
            }
        }
        return false;
    }
    public Color BackgroundSky(Ray ray) {
        Vector3 uniDir = Normalize(ray.Direction);
        float a = 0.5f * (uniDir.Y + 1.0f);
        return (1.0f - a) * Color.White + a * new Color(0.5f, 0.7f, 1.0f);
    }
}
