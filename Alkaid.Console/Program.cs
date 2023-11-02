using Alkaid.Core;
using Alkaid.Core.IO;
using Alkaid.Core.Renderer;
using System.Diagnostics;

RawImage output;
(Camera MainCam, Scene world) = FileIO.Parse("./Assets/hw3_input.txt");
MainCam.SetRenderer(new PhongRenderer());
MainCam.DefocusAngle = 1.0f;
MainCam.FocusDistance = 20f;
MainCam.Initialize();
Console.WriteLine("-----------------------------------------------------");


Stopwatch stopwatch = new();

stopwatch.Start();
output = MainCam.Render(world);
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
stopwatch.Stop();

Console.WriteLine($"Time taken : {stopwatch.ElapsedMilliseconds} ms");
stopwatch.Reset();
