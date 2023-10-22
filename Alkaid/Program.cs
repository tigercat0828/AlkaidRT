using Alkaid.Core;
using Alkaid.Core.Render;

(Camera MainCam, Scene world) = FileIO.Parse("./Assets/hw2_input.txt");

MainCam.SetRenderer(new PhongRenderer());
MainCam.Initialize();
MainCam.Render(world);

