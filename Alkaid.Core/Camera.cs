using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using Alkaid.Core.IO;
using Alkaid.Core.Render;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector3;

namespace Alkaid.Core;
public class Camera {
    Random random = new Random();

    const float DEG2RAD = MathF.PI / 180f;
    public RendererBase Renderer { get; private set; }
    public float m_AspectRatio { get; private set; }
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

    public float defocus_angle = 0;  // Variation angle of rays through each pixel
    public float focus_dist = 10;
    Vector3 defocus_disk_u;  // Defocus disk horizontal radius
    Vector3 defocus_disk_v;  // Defocus disk vertical radius

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
    public void SetRenderer(RendererBase renderer) {
        Renderer = renderer;
    }

    public RawImage Render(Scene scene) { // shot a photo !!
        if (Renderer == null) {
            Console.WriteLine("Renderer of camera is missing!");
        }
        Console.WriteLine($"Size = {m_ImageWidth} x {m_ImageHeight}");
        RawImage output = new(m_ImageWidth, m_ImageHeight);

        for (int j = 0; j < m_ImageHeight; j++) {
            for (int i = 0; i < m_ImageWidth; i++) {
                //Console.WriteLine($"{i} {j}");
                Vector3 pixelCenter = m_pixel00 + (i * m_deltaU) + (j * m_deltaV);
                Vector3 rayDirection = pixelCenter - m_Center;
                // Ray ray = new(m_Center, rayDirection);
                Ray ray = GetRay(i, j);
                Color pixelColor = 255.99f * Renderer.RayColor(ray, scene, 1).Clamp();
                output.SetPixel(i, j, pixelColor);
            }
        }
        return output;
    }
    public Ray GetRay(int i, int j) {
        // Get a randomly-sampled camera ray for the pixel at location i,j, originating from
        // the camera defocus disk.

        Vector3 pixel_center = m_pixel00 + (i * m_deltaU) + (j * m_deltaV);

        Vector3 ray_origin = (defocus_angle <= 0) ? m_Center : defocus_disk_sample();
        Vector3 ray_direction = pixel_center - ray_origin;

        return new Ray(ray_origin, ray_direction);
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
        //float h = theta * Tan(theta / 2);
        float h = Tan(theta / 2);
        //float viewportHeight = 2.0f * h * focalLength;
        //float viewportWidth = viewportHeight * m_ImageWidth / m_ImageHeight;
        //float viewportWidth = 2.0f * h * focalLength;
        float viewportWidth = 2.0f * h * focus_dist;
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
        Vector3 viewportUpperLeft = m_Center - (focus_dist * W) - viewportU / 2 - viewportV / 2;
        m_pixel00 = viewportUpperLeft + 0.5f * (m_deltaU + m_deltaV);


        float defocus_radius = focus_dist * Tan(defocus_angle / 2 * DEG2RAD);
        defocus_disk_u = U * defocus_radius;
        defocus_disk_v = V * defocus_radius;

    }
    Vector3 defocus_disk_sample() {
        // Returns a random point in the camera defocus disk.
        Vector3 p = random.UnitDisk();
        return m_Center + (p.X * defocus_disk_u) + (p.Y * defocus_disk_v);
    }
}
