using Alkaid.Core;
using Alkaid.Core.Data;
using Alkaid.Core.Extensions;
using Alkaid.Core.IO;
using Alkaid.Core.Material;
using Alkaid.Core.Primitives;
using Alkaid.Core.Renderer;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using Color = Alkaid.Core.Data.Color;
using static Alkaid.Core.Extensions.MathRT;
using Microsoft.VisualBasic.FileIO;

namespace Alkaid.WPF;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    RawImage output;
    Camera MainCam;
    Scene world;

    Bitmap bitmap;
    public MainWindow() {
        InitializeComponent();
    }
    private async void RenderBtn_Click(object sender, RoutedEventArgs e) {
        c_RenderBtn.IsEnabled = false;
        await Task.Run(() => RenderScene());

        bitmap = Utility.RawImageToBitmap(output);
        Utility.UpdateImageBox(c_RenderImgBox, bitmap);
        c_RenderBtn.IsEnabled = true;
    }
    private void RenderScene() {
        //world = BuildSceneMetal();
        world = BuildSceneLambertian();
        //world = BuildSceneGlassBall();
        //world = BuildComplexScene();

        CamOption option = new() {
            AspectRatio = 16 / 9f,
            //hFov = 90,
            //LookFrom = new Vector3(0, 0, 1),
            //LookAt = new Vector3(0, 0, -1f),
            hFov = 40,
            LookFrom = new Vector3(13, 2, 3),
            LookAt = new Vector3(0, 0, 0),
            DefocusAngle = 0.0f,
            FocusDistance = 20f,
            SampleNum = 50,
            ImageWidth = 800
        };
        MainCam = new Camera(option);

        MainCam.SetRenderer(new RtiowkRenderer());
        MainCam.MaxDepth = 50;

        MainCam.Initialize();
        output = MainCam.Render(world);
        output.SaveFile("output.ppm");
    }
    private Scene BuildSceneMetal() {
        Scene scene = new();
        MatLambertian matGreen = new(Color.Green);
        MatMetal matMetal1 = new(0.7f * Color.White, 0);
        MatMetal matMetal2 = new(0.7f * Color.White, 0.3f);
        MatMetal matMetal3 = new(0.7f * Color.White, 0.8f);
        Sphere sphere1 = new Sphere(new(1, 0, -1),0.5f, matMetal1);
        Sphere sphere2 = new Sphere(new(0, 0, -1), 0.5f, matMetal2);
        Sphere sphere3 = new Sphere(new(-1, 0, -1), 0.5f, matMetal3);
        Sphere groundSphere = new(center: new(0, -100.5f, -1), 100.0f, matGreen); // ground ball
        scene.AddItem(groundSphere);
        scene.AddItem(sphere1);
        scene.AddItem(sphere2);
        scene.AddItem(sphere3);
        return scene;
    }
    private Scene BuildSceneGlassBall() {
        Scene scene = new();
        MatLambertian matGreen = new(Color.Green);
        MatDielectric MatGlass = new(1.5f);
        Sphere outer = new Sphere(new(0, 0, -1), 0.5f, MatGlass);
        Sphere inner = new Sphere(new(0, 0, -1), -0.4f, MatGlass);
        Sphere groundSphere = new(center: new(0, -100.5f, -1), 100.0f, matGreen); // ground ball
        scene.AddItem(outer);
        scene.AddItem(inner);
        scene.AddItem(groundSphere);
        return scene;
    }
    private Scene BuildSceneLambertian() {
        Scene scene = new();
        MatLambertian matGreen = new(Color.Green);
        MatLambertian matLabertian1 = new(Color.Magenta);
        MatLambertian matLabertian2 = new(Color.Blue);
        MatLambertian matLabertian3 = new(Color.Red);
        Sphere sphere1 = new Sphere(new(1, 0, -1), 0.5f, matLabertian1);
        Sphere sphere2 = new Sphere(new(0, 0, -1), 0.5f, matLabertian2);
        Sphere sphere3 = new Sphere(new(-1, 0, -1), 0.5f, matLabertian3);
        Sphere groundSphere = new(center: new(0, -100.5f, -1), 100.0f, matGreen); // ground ball
        scene.AddItem(groundSphere);
        scene.AddItem(sphere1);
        scene.AddItem(sphere2);
        scene.AddItem(sphere3);
        return scene;
    }
    private Scene BuildSceneForPhongRenderer() {
        Scene scene = new();
        MatLambertian MatGreen = new(Color.Green);
        MatDielectric MatDielect1 = new(1.5f);
        MatMetal MatMetalWhite = new(0.7f * Color.White,0);
        // MatDielectric MatDielect2 = new(1.5f);
        Sphere sphere1 = new(center: new(0, -100.5f, -1), 100.0f, MatGreen); // ground ball
        Sphere sphere4 = new(center: new(0, 0, -1), 0.5f, MatDielect1);
        scene.AddItem(sphere1);
        scene.AddItem(sphere4);

        Light light = new Light(new(0, 8, 0), Color.White);
        scene.AddLight(light);
        return scene;
    }
    private Scene BuildComplexScene() {
        Scene scene = new();

        MatLambertian ground_material = new(0.5f * Color.White);
        scene.AddItem(new Sphere(new(0, -1000, 0), 1000, ground_material));

        for (int a = -11; a < 11; a++) {
            for (int b = -11; b < 11; b++) {
                float choose_mat = MathRT.RandomSingle();
                Vector3 Center = new(a + 0.9f * MathRT.RandomSingle(), 0.2f, b + 0.9f * MathRT.RandomSingle());

                if ((Center - new Vector3(4, 0.2f, 0)).Length() > 0.9f) {
                    MaterialBase sphere_material;

                    if (choose_mat < 0.8) {
                        // diffuse
                        Color albedo = RandomColor();
                        sphere_material = new MatLambertian(albedo);
                        scene.AddItem(new Sphere(Center, 0.2f, sphere_material));
                    }
                    else if (choose_mat < 0.95) {
                        // metal
                        Color albedo = RandomSingle(0.5f, 1.0f) * Color.White;
                        float fuzz = RandomSingle(0.0f, 0.5f);
                        sphere_material = new MatMetal(albedo, fuzz);
                        scene.AddItem(new Sphere(Center, 0.2f, sphere_material));
                    }
                    else {
                        // glass
                        sphere_material = new MatDielectric(1.5f);
                        scene.AddItem(new Sphere(Center, 0.2f, sphere_material));
                    }
                }
            }
        }
        MatDielectric dielectric = new(1.5f);
        MatLambertian lambertian = new(new Color(0.4f, 0.2f, 0.1f));
        MatMetal metal = new(new Color(0.7f, 0.6f, 0.5f), 0.05f);
        Sphere sphere1 = new(new Vector3(0, 1, 0), 1, dielectric);
        Sphere sphere2 = new(new Vector3(-4, 1, 0), 1, lambertian);
        Sphere sphere3 = new(new Vector3(4, 1, 0), 1, metal);
        scene.AddItem(sphere1);
        scene.AddItem(sphere2);
        scene.AddItem(sphere3);
        return scene;
    }
}


