using Alkaid.Core;
using Alkaid.Core.IO;
using Alkaid.Core.Render;
using System.Windows;
using Color = Alkaid.Core.Data.Color;
using System.Drawing;
using System.Numerics;
using Alkaid.Core.Primitives;
using Alkaid.Core.Data;
using System.Linq;
using System.Windows.Documents;

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
        //(MainCam, world) = FileIO.Parse("./Assets/hw2_input.txt");
    }

    private void RenderBtn_Click(object sender, RoutedEventArgs e) {
        world = BuildScene();
        CamOption option = new() {
            AspectRatio = 16 / 9f,
            Fov = 90,
            LookFrom = new Vector3(0, 8, .1f),
            LookAt = new Vector3(0, 2, 0),
            ImageWidth = 800
        };
        MainCam = new Camera(option);
        MainCam.SetRenderer(new PhongRenderer());
        MainCam.Initialize();
        output =  MainCam.Render(world);

        bitmap = Utility.RawImageToBitmap(output);
        Utility.UpdateImageBox(RenderImgBox, bitmap);
    }
    private Scene BuildScene() {
        Scene scene = new();
        PhongMat MatRed = new(Color.Red);
        PhongMat MatBlue = new(Color.Blue);
        PhongMat MatGreen = new(Color.Green);
        Vector3 A = new(4, 0, 4);
        Vector3 B = new(4, 0, -4);
        Vector3 C = new(-4, 0, -4);
        Vector3 D = new(-4, 0, 4);
        Triangle tri1 = new(A, B, C,MatGreen);
        Triangle tri2 = new(C, D, A, MatGreen);
        Sphere sphere1 = new (new(2,2,0),1,MatRed);
        Sphere sphere2 = new (new(-2, 2, 0), 1, MatBlue);
        Light light = new Light(new(0,8,0),Color.White);
        scene.AddItem(tri1);
        scene.AddItem(tri2);
        scene.AddItem(sphere1);
        scene.AddItem(sphere2);
        scene.AddLight(light);
        return scene;
    }
}
