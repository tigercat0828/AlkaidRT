using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using Alkaid.Core.IO;
using Alkaid.Core.Renderer;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;

namespace Alkaid.Core;
public class Camera {
    Random random = new();

    const float DEG2RAD = PI / 180f;
    public RendererBase Renderer { get; private set; }
    public float AspectRatio { get; private set; }
    public int ImageWidth;
    public int ImageHeight { get; private set; }
    public float Hfov { get; private set; }
    public int SampleNum = 1;
    public int MaxDepth = 1;
    public Vector3 LookFrom;
    public Vector3 LookAt;
    public Vector3 m_Center { get; private set; }

    private Vector3 m_pixel00;
    private Vector3 m_deltaU;
    private Vector3 m_deltaV;
    private Vector3 Vup;
    private Vector3 U, V, W;

    // detemine the aperture size
    public float DefocusAngle = 0;
    public float FocusDistance = 10;
    Vector3 defocusDiskU;
    Vector3 defocusDiskV;
    public Camera() { }
    public Camera(CamOption option) {
        SetOption(option);
    }
    public void SetOption(CamOption option) {
        AspectRatio = option.AspectRatio;
        ImageWidth = option.ImageWidth;
        Hfov = option.hFov;
        LookAt = option.LookAt;
        LookFrom = option.LookFrom;
        Vup = option.Vup;
        DefocusAngle = option.DefocusAngle;
        FocusDistance = option.FocusDistance;
        SampleNum = option.SampleNum;
    }
    public void SetRenderer(RendererBase renderer) {
        Renderer = renderer;
    }
    public RawImage Render(Scene scene) { // shot a photo !!
        if (Renderer == null) {
            Console.WriteLine("Renderer of camera is missing!");
        }
        RawImage output = new(ImageWidth, ImageHeight);

        for (int j = 0; j < ImageHeight; j++) {
            for (int i = 0; i < ImageWidth; i++) {

                Color color = Color.None;
                for (int t = 0; t < SampleNum; t++) {
                    Ray ray = GetRay(i, j);
                    color += Renderer.RayColor(ray, scene, MaxDepth);
                }
                color /= SampleNum;

                // gamma correction
                color.R = Sqrt(color.R);
                color.G = Sqrt(color.G);
                color.B = Sqrt(color.B);

                color *= 255.99f;


                output.SetPixel(i, j, color.Clamp());
            }
        }
        return output;
    }

    Vector3 GetSampleOffset() {
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
        ImageHeight = (int)(ImageWidth / AspectRatio);
        ImageHeight = ImageHeight < 1 ? 1 : ImageHeight;

        m_Center = LookFrom;
        float theta = Hfov * DEG2RAD;
        float w = Tan(theta / 2);
        float viewportWidth = 2.0f * w * FocusDistance;
        float viewportHeight = viewportWidth * ImageHeight / ImageWidth;

        W = Normalize(LookFrom - LookAt);
        U = Normalize(Cross(Vup, W));
        V = Normalize(Cross(W, U));

        Vector3 viewportU = viewportWidth * U;
        Vector3 viewportV = viewportHeight * -V;

        m_deltaU = viewportU / ImageWidth;
        m_deltaV = viewportV / ImageHeight;

        Vector3 viewportUpperLeft = m_Center - FocusDistance * W - viewportU / 2 - viewportV / 2;
        m_pixel00 = viewportUpperLeft + 0.5f * (m_deltaU + m_deltaV);

        float defocusRadius = FocusDistance * Tan(DefocusAngle / 2 * DEG2RAD);
        defocusDiskU = U * defocusRadius;
        defocusDiskV = V * defocusRadius;
    }
    Vector3 GetRayFromDisk() {
        Vector3 p = random.UnitDisk();
        return m_Center + p.X * defocusDiskU + p.Y * defocusDiskV;
    }

}
