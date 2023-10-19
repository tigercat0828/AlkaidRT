using Alkaid.Core;
using Alkaid.Core.Data;
using Alkaid.Core.Primitives;
using System.Numerics;

//(Camera MainCam, Scene world) = PickScene(DefaultScene.CameraDebug1);
//(Camera MainCam, Scene world) = PickScene(DefaultScene.CameraDebug2);
//(Camera MainCam, Scene world) = PickScene(DefaultScene.RandomSphere);

(Camera MainCam, Scene world) = PickScene(DefaultScene.FileLoad);

MainCam.Initialize();

MainCam.Render(world);


(Camera, Scene) PickScene(DefaultScene sceneID) {
    Camera camera = new();
    CamOption option = new();
    Scene scene = new();

    Material MatGreen = new(Color.Green);
    Material MatBlue = new(Color.Blue);
    Material MatRed = new(Color.Red);
    Material MatYellow = new(Color.Yellow);
    Material Mat1 = new(new(0.8f, 0.8f, 0.0f));
    Material Mat2 = new(new(0.1f, 0.2f, 0.5f));
    Material Mat3 = new(new(0.5f, 0.5f, 0.5f));
    Material Mat4 = new(new(0.8f, 0.6f, 0.2f));
    Vector3 A = new(0.5f, 0.5f, 0);
    Vector3 B = new(0.5f, -0.5f, 0);
    Vector3 C = new(-0.5f, 0.5f, 0);
    Vector3 D = new(-0.5f, -0.5f, 0);

    switch (sceneID) {
        case DefaultScene.FileLoad:
            (camera, scene) = FileIO.Parse("./Assets/hw2_input.txt");
            
            break;
        case DefaultScene.OcclusionTest:

            option = new() {
                AspectRatio = 16 / 9f,
                ImageWidth = 1600,
                LookFrom = new Vector3(-2, 2, 1),
                LookAt = new Vector3(0, 0, 0),
                Fov = 50f,
                Vup = Vector3.UnitY
            };
            scene.AddItem(new Sphere(new Vector3(0, -100.5f, 0), 100, MatGreen));
            scene.AddItem(new Sphere(new Vector3(0, 0, 0), 0.5f, MatRed));
            scene.AddItem(new Triangle(A, B, C, MatBlue));
            scene.AddItem(new Triangle(C, D, B, MatYellow));
            break;
        case DefaultScene.CameraDebug1:
            option = new() {
                AspectRatio = 16 / 9f,
                ImageWidth = 1600,
                LookFrom = new Vector3(0, 0, 0),
                LookAt = new Vector3(0, 0, -1),
                Fov = 90f,
                Vup = Vector3.UnitY
            };
            camera.SetOption(option);
            float R = MathF.Cos(MathF.PI / 4f);
            scene.AddItem(new Sphere(new Vector3(-R, 0, -1), R, MatBlue));
            scene.AddItem(new Sphere(new Vector3(R, 0, -1), R, MatRed));
            break;
        case DefaultScene.CameraDebug2:
            option = new() {
                AspectRatio = 16 / 9f,
                ImageWidth = 1600,
                LookFrom = new Vector3(-2, 2, 1),
                LookAt = new Vector3(0, 0, -1),
                Fov = 90f,
                Vup = Vector3.UnitY
            };
            camera.SetOption(option);
            scene.AddItem(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, Mat1));
            scene.AddItem(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, Mat2));
            scene.AddItem(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, Mat3));
            scene.AddItem(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, Mat4));
            break;
        case DefaultScene.RandomSphere:
            option = new() {
                AspectRatio = 16 / 9f,
                ImageWidth = 1600,
                LookFrom = new Vector3(10, 10, 10),
                LookAt = new Vector3(0, 0, 0),
                Fov = 50f,
                Vup = Vector3.UnitY
            };
            camera.SetOption(option);
            Random random = new();
            for (int i = 0; i < 30; i++) {
                float r = random.NextSingle();
                float g = random.NextSingle();
                float b = random.NextSingle();
                float x = random.Next(-10, 10);
                float y = random.Next(-10, 10);
                float z = random.Next(-10, 10);
                Color albedo = new(r, g, b);
                Material material = new(albedo);
                Vector3 position = new(x, y, z);
                Sphere sphere = new(position, 0.5f, material);
                scene.AddItem(sphere);
            }
            break;
        default:
            break;
    }
    if (sceneID != DefaultScene.FileLoad) { 
        camera.SetOption(option);
    }
    Light light = new Light(new(5.0f, 5.0f, -5.0f));
    scene.AddLight(light);
    return (camera, scene);
}
enum DefaultScene {
    FileLoad, OcclusionTest, CameraDebug1, CameraDebug2, RandomSphere
}
