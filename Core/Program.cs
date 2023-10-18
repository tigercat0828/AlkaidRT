
using Alkaid.Core;
using Alkaid.Core.Primitive;
using System.Numerics;
using static System.Numerics.Vector3;
public class Program {
    public static void Main(string[] args) {

        // Image

        float aspect_ratio = 16.0f / 9.0f;
        int imageWidth = 400;

        // Calculate the image height, and ensure that it's at least 1.
        int imageHeight = (int)(imageWidth / aspect_ratio);
        imageHeight = (imageHeight < 1) ? 1 : imageHeight;

        // Camera

        float focalLength = 1.0f;
        float viewportHeight = 2.0f;
        float viewportWidth = viewportHeight * (imageWidth / (float)imageHeight);
        Vector3 cameraCenter = new(0, 0, 0);

        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        Vector3 viewportU = new (viewportWidth, 0, 0);
        Vector3 viewportV = new (0, -viewportHeight, 0);

        // Calculate the horizontal and vertical delta vectors from pixel to pixel.
        Vector3 deltaU = viewportU / imageWidth;
        Vector3 deltaV = viewportV / imageHeight;

        // Calculate the location of the upper left pixel.
        Vector3 viewportUpperLeft = cameraCenter - new Vector3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
        Vector3 pixel00 = viewportUpperLeft + 0.5f * (deltaU + deltaV);

        // Render

        Console.WriteLine($"Size = {imageWidth} x { imageHeight}");
        RawImage output = new (imageWidth, imageHeight);
        Scene world = new();
        Material MatGreen = new (Color.Green);
        Material MatBlue = new (Color.Blue);
        Material MatRed = new(Color.Red);
        Material MatYellow = new(Color.Yellow);

        Vector3 A = new (0.5f, 0.5f, -2);
        Vector3 B = new (0.5f, -0.5f, -2);
        Vector3 C = new (-0.5f, 0.5f, -2);
        Vector3 D = new (-0.5f, -0.5f, -2);

        world.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100, MatGreen));
        world.AddItem(new Sphere(new Vector3(0, 0, -2), 0.5f, MatRed));
        world.AddItem(new Triangle(A, B, C, MatBlue));
        world.AddItem(new Triangle(C, D, B, MatYellow) );


        for (int j = 0; j < imageHeight; ++j) {
            for (int i = 0; i < imageWidth; ++i) {
                //Console.WriteLine($"{i} {j}");
                Vector3 pixelCenter = pixel00 + (i * deltaU) + (j * deltaV);
                Vector3 rayDirection = pixelCenter - cameraCenter;
                Ray ray = new(cameraCenter, rayDirection);
                Color pixelColor = 255.99f * RayCast(ray, world);
                output.SetPixel(i, j, pixelColor);
            } 
        }
        output.SaveFile("output.ppm");
    }

    private static Color RayCast(Ray ray, Scene scene) {
        HitRecord record = new();

        if (scene.HitAny(ray,new Interval(0, float.MaxValue),ref record)) {

            return record.Material.Albedo;
        }
        Vector3 unit_direction = Normalize(ray.Direction);
        float a = 0.5f * (unit_direction.Y + 1.0f);
        return (1.0f - a) * Color.White + a * new Color(0.5f, 0.7f, 1.0f);
    }
    
}