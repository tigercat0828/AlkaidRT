using Alkaid.Core.Data;
using Alkaid.Core.IO;
using Alkaid.Core.Primitives;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;

namespace Alkaid.Core;
public class Camera {

    const float DEG2RAD = MathF.PI / 180f;

    public float m_AspectRatio;
    public int m_ImageWidth;
    public int m_ImageHeight { get; private set; }
    public float m_Vfov { get; private set; }
    public Vector3 m_LookFrom;
    public Vector3 m_LookAt;
    public Vector3 m_Center { get; private set; }

    private Vector3 m_pixel00;
    private Vector3 m_deltaU;
    private Vector3 m_deltaV;
    private Vector3 m_Vup;
    private Vector3 U, V, W;
    public Camera() { }
    public Camera(CamOption option) {
        SetOption(option);
    }
    public void SetOption(CamOption option) {
        m_AspectRatio = option.AspectRatio;
        m_ImageWidth = option.ImageWidth;
        m_Vfov = option.Fov;
        m_LookFrom = option.LookFrom;
        m_LookAt = option.LookAt;
        m_Vup = option.Vup;
    }
    public Color RayCast(Ray ray, Scene scene) {
        HitRecord record = new();
        IHitable sph = scene.Items[0];
        if (scene.HitAny(ray, new Interval(0, float.MaxValue), ref record)) {
            Light light = scene.Lights[0];

            Vector3 normal = Normalize(record.Normal);
            Vector3 pixelPos = record.Point;
            Vector3 lightDir = Normalize(pixelPos - light.Position);
            Vector3 viewDir = Normalize(ray.Direction);
            Ray pixelToLightRay = new(pixelPos, -lightDir);
            Vector3 lightReflected = Normalize(Reflect(lightDir, normal));
            Material material = record.Material;
            Color pixelColor = new();

            Color ambient = material.Ka * material.Albedo * light.Intensity;

            float diffStren = Max(Dot(-lightDir, normal), 0);
            Color diffuse = diffStren * material.Kd * material.Albedo * light.Intensity;

            float specStren = Pow(Max(Dot(lightReflected, -viewDir), 0), material.Shineness);
            Color specular = specStren * material.Ks * material.Albedo * light.Intensity;

            int HitID = record.ID;
            if (scene.InShadow(pixelToLightRay, HitID)) {
                pixelColor = ambient;
            }
            else {
                pixelColor = ambient + diffuse + specular;
            }
            float r = material.Reflect;
            if (r > 0) {
                Vector3 viewReflectedDir = Reflect(viewDir, normal);
                Ray viewDirReflectedRay = new(pixelPos + 0.0001f * viewReflectedDir, viewReflectedDir);
                return (1 - r) * pixelColor + r * RayCast(viewDirReflectedRay, scene);
            }
            else {
                return pixelColor.Clamp();
            }

        }
        return Color.Black;
    }
    public Color BackgroundSky(Ray ray) {
        Vector3 uniDir = Normalize(ray.Direction);
        float a = 0.5f * (uniDir.Y + 1.0f);
        return (1.0f - a) * Color.White + a * new Color(0.5f, 0.7f, 1.0f);
    }
    public void Render(Scene scene) { // shot a photo !!
        Console.WriteLine($"Size = {m_ImageWidth} x {m_ImageHeight}");
        RawImage output = new(m_ImageWidth, m_ImageHeight);
        for (int j = 0; j < m_ImageHeight; j++) {
            for (int i = 0; i < m_ImageWidth; i++) {
                //Console.WriteLine($"{i} {j}");
                Vector3 pixelCenter = m_pixel00 + (i * m_deltaU) + (j * m_deltaV);
                Vector3 rayDirection = pixelCenter - m_Center;
                Ray ray = new(m_Center, rayDirection);
                Color pixelColor = 255.99f * RayCast(ray, scene);
                output.SetPixel(i, j, pixelColor);
            }
        }
        output.SaveFile("output.ppm");
    }
    public void Initialize() {
        // Image
        // Calculate the image height, and ensure that it's at least 1.
        m_ImageHeight = (int)(m_ImageWidth / m_AspectRatio);
        m_ImageHeight = (m_ImageHeight < 1) ? 1 : m_ImageHeight;

        m_Center = m_LookFrom;
        // Camera
        float focalLength = (m_LookFrom - m_LookAt).Length();
        float theta = m_Vfov * DEG2RAD;
        float h = theta * Tan(theta / 2);
        //float viewportHeight = 2.0f * h * focalLength;
        //float viewportWidth = viewportHeight * m_ImageWidth / m_ImageHeight;
        float viewportWidth = 2.0f * h * focalLength;
        float viewportHeight = viewportWidth * m_ImageHeight / m_ImageWidth;

        W = Normalize(m_LookFrom - m_LookAt);
        U = Normalize(Cross(m_Vup, W));
        V = Normalize(Cross(W, U));

        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        Vector3 viewportU = viewportWidth * U;
        Vector3 viewportV = viewportHeight * -V;

        // Calculate the horizontal and vertical delta vectors from pixel to pixel.
        m_deltaU = viewportU / m_ImageWidth;
        m_deltaV = viewportV / m_ImageHeight;

        // Calculate the location of the upper left pixel.
        Vector3 viewportUpperLeft = m_Center - (focalLength * W) - viewportU / 2 - viewportV / 2;
        m_pixel00 = viewportUpperLeft + 0.5f * (m_deltaU + m_deltaV);

    }

}
