using Alkaid.Core;
using Alkaid.Core.Data;
using Alkaid.Core.IO;
using Alkaid.Core.Material;
using Alkaid.Core.Primitives;
using Alkaid.Core.Renderer;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using Color = Alkaid.Core.Data.Color;

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
        world = BuildSceneMetal();
        world = BuildSceneLambertian();

        CamOption option = new() {
            AspectRatio = 16 / 9f,
            hFov = 90,
            //LookFrom = new Vector3(-3, 8f, 1.5f),
            //LookAt = new Vector3(-3, 0f, -1.5f),
            LookFrom = new Vector3(0, 0, 1),
            LookAt = new Vector3(0, 0, -1f),
            DefocusAngle = 0.0f,
            FocusDistance = 20f,
            SampleNum = 50,
            ImageWidth = 1600
        };
        MainCam = new Camera(option);

        MainCam.SetRenderer(new RtiowkRenderer());
        MainCam.MaxDepth = 50;

        MainCam.Initialize();
        output = MainCam.Render(world);
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
    private Scene BuildScene() {
        // 
        Scene scene = new();
        MatLambertian MatRed = new(Color.Red);
        MatLambertian MatBlue = new(Color.Blue);
        MatLambertian MatGreen = new(Color.Green);
        MatLambertian MatYellow = new(Color.Yellow);
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
}

/*
        Scene scene = new();
        MatLambertian MatRed = new(Color.Red);
        MatLambertian MatBlue = new(Color.Blue);
        MatLambertian MatGreen = new(Color.Green);
        MatLambertian MatYellow = new(Color.Yellow);
        MatDielectric MatDielect1 = new(1.5f);
        MatMetal MatMetalWhite = new(0.7f * Color.White);
        // MatDielectric MatDielect2 = new(1.5f);
        Sphere sphere1 = new(center: new(0, -100.5f, -1), 100.0f, MatGreen); // ground ball
        Sphere sphere3 = new(center: new(1, 0, -1), 0.5f, MatRed);
        Sphere sphere2 = new(center: new(0, 0, -1), 0.5f, MatMetalWhite);
        Sphere sphere4 = new(center: new(-1, 0, -1), 0.5f, MatBlue);
        scene.AddItem(sphere1);
        scene.AddItem(sphere2);
        scene.AddItem(sphere3);
        scene.AddItem(sphere4);

        Light light = new Light(new(0, 8, 0), Color.White);
        scene.AddLight(light);
        return scene;

*/
