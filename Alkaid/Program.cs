using Alkaid.Core;
using Alkaid.Core.Primitive;
using System.Numerics;

Scene world = new();
Material MatGreen = new(Color.Green);
Material MatBlue = new(Color.Blue);
Material MatRed = new(Color.Red);
Material MatYellow = new(Color.Yellow);

Vector3 A = new(0.5f, 0.5f, -2);
Vector3 B = new(0.5f, -0.5f, -2);
Vector3 C = new(-0.5f, 0.5f, -2);
Vector3 D = new(-0.5f, -0.5f, -2);

world.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100, MatGreen));
world.AddItem(new Sphere(new Vector3(0, 0, -2), 0.5f, MatRed));
world.AddItem(new Triangle(A, B, C, MatBlue));
world.AddItem(new Triangle(C, D, B, MatYellow));

CamOption option = new() {
    AspectRatio = 16 / 9f,
    ImageWidth = 400,
    Position = Vector3.Zero
};
Camera MainCam = new(option);
MainCam.Initialize();
MainCam.Render(world);