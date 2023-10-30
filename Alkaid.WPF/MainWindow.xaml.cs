using Alkaid.Core;
using Alkaid.Core.Data;
using Alkaid.Core.IO;
using Alkaid.Core.Primitives;
using Alkaid.Core.Render;
using Alkaid.RayTracing.Core;
using System.Drawing;
using System.Numerics;
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
        (MainCam, world) = FileIO.Parse("./Assets/hw2_input.txt");
    }

    private void RenderBtn_Click(object sender, RoutedEventArgs e) {

        //Scene world = BuildScene();
        //CamOption option = new() {
        //    AspectRatio = 16 / 9f,
        //    Fov = 90,
        //    //LookFrom = new Vector3(-3, 1.5f, 3f),
        //    //LookAt = new Vector3(-3, 1.5f, 0),
        //    LookFrom = new Vector3(-3, 8f, 1.5f),
        //    LookAt = new Vector3(-3, 0f, -1.5f),
        //    ImageWidth = 1600
        //};
        //Camera MainCam = new (option);
        //MainCam.FocusDistance = 6f; // 8 / 6 / 4  
        //MainCam.DefocusAngle = 1.0f;
        //MainCam.SetRenderer(new PhongRenderer());
        //MainCam.Initialize();
        //output = MainCam.Render(world);

        (MainCam, world) = FileIO.Parse("./Assets/hw3_input.txt");
        MainCam.DefocusAngle = 1.0f;
        MainCam.SetRenderer(new PhongRenderer());
        
        MainCam.FocusDistance = 20f; // 8 / 6 / 4 /  20 /40 /60
        MainCam.Initialize();
        output = MainCam.Render(world);
        //output.SaveFile("focus20cm.ppm");
        
        //MainCam.FocusDistance = 40f; // 8 / 6 / 4 /  20 /40 /60
        //MainCam.Initialize();
        //output = MainCam.Render(world);
        //output.SaveFile("focus40cm.ppm");
        
        //MainCam.FocusDistance = 60f; // 8 / 6 / 4 /  20 /40 /60
        //MainCam.Initialize();
        //output = MainCam.Render(world);
        //output.SaveFile("focus60cm.ppm");


        bitmap = Utility.RawImageToBitmap(output);
        Utility.UpdateImageBox(RenderImgBox, bitmap);
    }
    private Scene BuildScene() {

        Scene scene = new();
        PhongMat MatRed = new(Color.Red);
        PhongMat MatBlue = new(Color.Blue);
        PhongMat MatGreen = new(Color.Green);
        PhongMat MatYellow = new(Color.Yellow);

        int planeLen = 50;
        Vector3 A = new(planeLen, 0, planeLen);
        Vector3 B = new(planeLen, 0, -planeLen);
        Vector3 C = new(-planeLen, 0, -planeLen);
        Vector3 D = new(-planeLen, 0, planeLen);
        Triangle tri1 = new(A, B, C, MatGreen);
        Triangle tri2 = new(C, D, A, MatGreen);
        Sphere sphere1 = new(new(-1, 1.0f, -1), 1, MatRed);
        Sphere sphere2 = new(new(-2, 1.0f, -3), 1, MatBlue);
        Sphere sphere3 = new(new(-3, 1.0f, -5), 1, MatYellow);
        Light light = new Light(new(0, 8, 0), Color.White);

        scene.AddItem(tri1);
        scene.AddItem(tri2);
        scene.AddItem(sphere1);
        scene.AddItem(sphere2);
        scene.AddItem(sphere3);
        scene.AddLight(light);
        return scene;
    }
}
