using Alkaid.Core.Primitive;
using System.Numerics;
using static System.Numerics.Vector3;

namespace Alkaid.Core {
    public class Camera {
        public float m_AspectRatio;
        public int m_ImageWidth;
        public int m_ImageHeight { get; private set; }
        public Vector3 m_Position;

        private Vector3 m_pixel00;
        private Vector3 m_deltaU;
        private Vector3 m_deltaV;
        public Camera(CamOption option) {
            m_AspectRatio = option.AspectRatio;
            m_ImageWidth = option.ImageWidth;
            m_Position = option.Position;
        }
        public Color RayCast(Ray ray, Scene scene) {
            HitRecord record = new();

            if (scene.HitAny(ray, new Interval(0, float.MaxValue), ref record)) {
                return record.Material.Albedo;
            }
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
                    Vector3 rayDirection = pixelCenter - m_Position;
                    Ray ray = new(m_Position, rayDirection);
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

            // Camera
            float focalLength = 1.0f;
            float viewportHeight = 2.0f;
            float viewportWidth = viewportHeight * (m_ImageWidth / (float)m_ImageHeight);
    

            // Calculate the vectors across the horizontal and down the vertical viewport edges.
            Vector3 viewportU = new(viewportWidth, 0, 0);
            Vector3 viewportV = new(0, -viewportHeight, 0);

            // Calculate the horizontal and vertical delta vectors from pixel to pixel.
            m_deltaU = viewportU / m_ImageWidth;
            m_deltaV = viewportV / m_ImageHeight;

            // Calculate the location of the upper left pixel.
            Vector3 viewportUpperLeft = m_Position - new Vector3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
            m_pixel00 = viewportUpperLeft + 0.5f * (m_deltaU + m_deltaV);

        }

    }
}
