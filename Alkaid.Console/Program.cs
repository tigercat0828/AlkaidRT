using Alkaid.Core;
using Alkaid.Core.IO;
using Alkaid.Core.Render;

(Camera MainCam, Scene world) = FileIO.Parse("./Assets/hw3_input.txt");
MainCam.DefocusAngle = 1.0f;
MainCam.SetRenderer(new PhongRenderer());

Console.WriteLine("-----------------------------------------------------");

RawImage output;
MainCam.FocusDistance = 20f; 
MainCam.Initialize();
output= MainCam.Render(world);
output.SaveFile("focus20cm.ppm");
Console.WriteLine("focus20cm Done");

MainCam.FocusDistance = 40f; 
MainCam.Initialize();
output = MainCam.Render(world);
output.SaveFile("focus40cm.ppm");
Console.WriteLine("focus40cm Done");

MainCam.FocusDistance = 60f; 
MainCam.Initialize();
output = MainCam.Render(world);
output.SaveFile("focus60cm.ppm");
Console.WriteLine("focus60cm Done");
