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
        world = BuildScene();

        CamOption option = new() {
            AspectRatio = 16 / 9f,
            hFov = 90,
            //LookFrom = new Vector3(-3, 8f, 1.5f),
            //LookAt = new Vector3(-3, 0f, -1.5f),
            LookFrom = new Vector3(0, 0, 1),
            LookAt = new Vector3(0, 0, -1f),
            DefocusAngle = 0.0f,
            FocusDistance = 20f,
            SampleNum = 10,
            ImageWidth = 800
        };
        MainCam = new Camera(option);

        
        MainCam.SetRenderer(new RtiowkRenderer());
        MainCam.MaxDepth = 50;
        //MainCam.SetRenderer(new PhongRenderer());
        //MainCam.MaxDepth = 1;
        
        
        MainCam.Initialize();
        output = MainCam.Render(world);
    }
    private Scene BuildScene() {
        // 
        Scene scene = new();
        PhongMat MatRed = new(Color.Red);
        PhongMat MatBlue = new(Color.Blue);
      
        Sphere sphere1 = new(center: new(0, 0, -1), 0.5f, MatRed);
        Sphere sphere2 = new(center: new(0, -100.5f, -1), 100.0f, MatBlue);
        scene.AddItem(sphere1);
        scene.AddItem(sphere2);

        Light light = new Light(new(0, 8, 0), Color.White);
        scene.AddLight(light);
        return scene;
    }
}
