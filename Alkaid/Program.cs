using Alkaid.Core;
using Alkaid.Core.Primitive;
using System.Numerics;

//(Camera MainCam, Scene world) = PickScene(DefaultScene.CameraDebug1);
//(Camera MainCam, Scene world) = PickScene(DefaultScene.CameraDebug2);
(Camera MainCam, Scene world) = PickScene(DefaultScene.OcclusionTest);

MainCam.Initialize();

MainCam.Render(world);


(Camera, Scene ) PickScene(DefaultScene sceneID) {
    Scene scene = new();

    CamOption option = new();
  
    Material MatGreen = new(Color.Green);
    Material MatBlue = new(Color.Blue);
    Material MatRed = new(Color.Red);
    Material MatYellow = new(Color.Yellow);
    Material Mat1 = new(new(0.8f, 0.8f, 0.0f));
    Material Mat2 = new(new(0.1f, 0.2f, 0.5f));
    Material Mat3 = new(new(0.5f, 0.5f, 0.5f));
    Material Mat4 = new(new(0.8f, 0.6f, 0.2f));
    Vector3 A = new(0.5f, 0.5f,   0);
    Vector3 B = new(0.5f, -0.5f,  0);
    Vector3 C = new(-0.5f, 0.5f,  0);
    Vector3 D = new(-0.5f, -0.5f, 0);

    switch (sceneID) {
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
            scene.AddItem(new Sphere(new Vector3(0.0f, -100.5f, -1.0f)  , 100.0f, Mat1));
            scene.AddItem(new Sphere(new Vector3(0.0f, 0.0f, -1.0f)     , 0.5f, Mat2));
            scene.AddItem(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f)    , 0.5f, Mat3));
            scene.AddItem(new Sphere(new Vector3(1.0f, 0.0f, -1.0f)     , 0.5f, Mat4));
            break;
        default:
            break;
    }
    Camera camera = new(option);
    return (camera, scene);
}
enum DefaultScene {
    OcclusionTest, CameraDebug1, CameraDebug2
}
