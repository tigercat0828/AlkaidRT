using Alkaid.Core;
using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using Alkaid.Core.IO;
using Alkaid.Core.Render;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;

namespace Alkaid.RayTracing.Core;
public class Camera {
    Random random = new();

    const float DEG2RAD = PI / 180f;
    public RendererBase Renderer { get; private set; }
    public float AspectRatio { get; private set; }


    public int SampleNum = 50;
    public int ImageWidth;
    public int ImageHeight { get; private set; }
    public float Vfov { get; private set; }
    public Vector3 m_LookFrom;
    public Vector3 m_LookAt;
    public Vector3 m_Center { get; private set; }


    private Vector3 m_pixel00;
    private Vector3 m_deltaU;
    private Vector3 m_deltaV;
    private Vector3 m_Vup;
    private Vector3 U, V, W;

    public float DefocusAngle = 0;
    public float FocusDistance = 10;
    Vector3 defocusDiskU;  // Defocus disk horizontal radius
    Vector3 defocusDiskV;  // Defocus disk vertical radius

    public Camera() { }
    public Camera(CamOption option) {
        SetOption(option);
    }
    public void SetOption(CamOption option) {
        AspectRatio = option.AspectRatio;
        ImageWidth = option.ImageWidth;
        Vfov = option.Fov;
        m_LookFrom = option.LookFrom;
        m_LookAt = option.LookAt;
        m_Vup = option.Vup;
    }
    public void SetRenderer(RendererBase renderer) {
        Renderer = renderer;
    }
    public RawImage Render(Scene scene) { // shot a photo !!
        if (Renderer == null) {
            Console.WriteLine("Renderer of camera is missing!");
        }
        Console.WriteLine($"Size = {ImageWidth} x {ImageHeight}");
        RawImage output = new(ImageWidth, ImageHeight);

        for (int j = 0; j < ImageHeight; j++) {
            for (int i = 0; i < ImageWidth; i++) {

                Color color = Color.None;
                for (int t = 0; t < SampleNum; t++) {
                    Ray ray = GetRay(i, j);
                    color += Renderer.RayColor(ray, scene, 1);
                }
                color /= SampleNum;
                color *= 255.99f;
                output.SetPixel(i, j, color.Clamp());
                
            }
        }
        return output;
    }
    Vector3 GetSampleOffset() {
        // Returns a random point in the square surrounding a pixel at the origin.
        float px = -0.5f + random.NextSingle();
        float py = -0.5f + random.NextSingle();
        return px * m_deltaU + py * m_deltaV;
    }
    public Ray GetRay(int i, int j) {
        Vector3 pixelCenter = m_pixel00 + i * m_deltaU + j * m_deltaV;
        if (SampleNum > 1) {
            pixelCenter += GetSampleOffset();
        }

        Vector3 rayOrigin = DefocusAngle <= 0 ? m_Center : GetRayFromDisk();
        Vector3 rayDirection = pixelCenter - rayOrigin;

        return new Ray(rayOrigin, rayDirection);
    }
    public void Initialize() {
        // Image
        // Calculate the image height, and ensure that it's at least 1.
        ImageHeight = (int)(ImageWidth / AspectRatio);
        ImageHeight = ImageHeight < 1 ? 1 : ImageHeight;

        m_Center = m_LookFrom;
        // Camera
        //float focalLength = (m_LookFrom - m_LookAt).Length();
        float theta = Vfov * DEG2RAD;
        float h = Tan(theta / 2);
        float viewportWidth = 2.0f * h * FocusDistance;
        float viewportHeight = viewportWidth * ImageHeight / ImageWidth;

        W = Normalize(m_LookFrom - m_LookAt);
        U = Normalize(Cross(m_Vup, W));
        V = Normalize(Cross(W, U));

        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        Vector3 viewportU = viewportWidth * U;
        Vector3 viewportV = viewportHeight * -V;

        // Calculate the horizontal and vertical delta vectors from pixel to pixel.
        m_deltaU = viewportU / ImageWidth;
        m_deltaV = viewportV / ImageHeight;

        // Calculate the location of the upper left pixel.
        Vector3 viewportUpperLeft = m_Center - FocusDistance * W - viewportU / 2 - viewportV / 2;
        m_pixel00 = viewportUpperLeft + 0.5f * (m_deltaU + m_deltaV);

        float defocusRadius = FocusDistance * Tan(DefocusAngle / 2 * DEG2RAD);
        defocusDiskU = U * defocusRadius;
        defocusDiskV = V * defocusRadius;
    }
    Vector3 GetRayFromDisk() {
        // Returns a random point in the camera defocus disk.
        Vector3 p = random.UnitDisk();
        return m_Center + p.X * defocusDiskU + p.Y * defocusDiskV;
    }

}
