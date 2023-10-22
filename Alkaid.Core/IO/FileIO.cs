using Alkaid.Core;
using Alkaid.Core.Data;
using Alkaid.Core.Primitives;
using System.Numerics;

public static class FileIO {
    public static void WritePPM(string filename, uint[] pixels, int width, int height) {

        using StreamWriter writer = new(filename);
        // Write the PPM header
        writer.WriteLine("P3");                 // P6 format for binary PPM
        writer.WriteLine($"{width} {height}");  // Width, height
        writer.WriteLine("255");                // Maximum color value

        for (int i = 0; i < pixels.Length; i++) {
            uint pixel = pixels[i];
            byte b = (byte)((pixel >> 0) & 0xFF);
            byte g = (byte)((pixel >> 8) & 0xFF);
            byte r = (byte)((pixel >> 16) & 0xFF);
            writer.WriteLine($"{r,3} {g,3} {b,3}");
        }
    }

    public static (Camera, Scene) Parse(string filename) {

        Scene scene = new();
        CamOption option = new();
        PhongMat currMat = new();
        // Parse the input lines
        string[] textLines = File.ReadAllLines(filename);
        foreach (string line in textLines) {

            string[] tokens = line.Split(' ');

            string type = tokens[0];

            switch (type) {
                case "M":
                    Color albedo = new(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    float ka = float.Parse(tokens[4]);
                    float kd = float.Parse(tokens[5]);
                    float ks = float.Parse(tokens[6]);
                    float shininess = float.Parse(tokens[7]);
                    float reflect = float.Parse(tokens[8]);
                    currMat = new PhongMat(albedo, ka, kd, ks, shininess, reflect);
                    Console.WriteLine($"M : {currMat}");
                    break;
                case "S":
                    Vector3 center = new(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    float radius = float.Parse(tokens[4]);
                    Sphere sphere = new(center, radius, currMat);
                    scene.Items.Add(sphere);
                    Console.WriteLine($"S : {sphere}");
                    break;
                case "T":
                    Vector3 pos1 = new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    Vector3 pos2 = new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6]));
                    Vector3 pos3 = new Vector3(float.Parse(tokens[7]), float.Parse(tokens[8]), float.Parse(tokens[9]));
                    Triangle triangle = new(pos1, pos2, pos3, currMat);
                    scene.Items.Add(triangle);
                    Console.WriteLine($"T : {triangle.pos1}, {triangle.pos2}, {triangle.pos3}");
                    break;
                case "E":
                    Vector3 position = new(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    option.LookFrom = position;
                    Console.WriteLine($"E : {option.LookFrom}");
                    break;
                case "F":
                    option.Fov = float.Parse(tokens[1]);
                    Console.WriteLine($"F : {option.Fov}");
                    break;
                case "R":
                    int width = int.Parse(tokens[1]);
                    int height = int.Parse(tokens[2]);
                    option.AspectRatio = width / (float)height;
                    option.ImageWidth = width;
                    Console.WriteLine($"R {option.ImageWidth} x {height}");
                    break;
                case "V":
                    Vector3 viewDir = new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    Vector3 Vup = new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6]));
                    option.LookAt = option.LookFrom + viewDir;
                    Console.WriteLine($"V : {viewDir} , {Vup}");
                    break;
                case "L":
                    Vector3 lightPos = new(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    Light light = new(lightPos);
                    scene.AddLight(light);
                    Console.WriteLine($"L : {light.Position}");
                    break;
                default:
                    Console.WriteLine("Unknown input type: " + type);
                    break;
            }

        }
        Camera camera = new(option);
        Console.WriteLine("Parse Complete");
        return (camera, scene);
    }
}
