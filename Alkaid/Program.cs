using Alkaid.Core;

(Camera MainCam, Scene world) = FileIO.Parse("./Assets/hw2_input.txt");

MainCam.Initialize();
MainCam.Render(world);

