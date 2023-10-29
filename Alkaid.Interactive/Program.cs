using Alkaid.Core;
using Alkaid.Core.Render;
using Alkaid.RayTracing.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;



(Camera MainCam, Scene world) = FileIO.Parse("./Assets/hw3_input.txt");
MainCam.DefocusAngle = 1.0f;
MainCam.SetRenderer(new PhongRenderer());

MainCam.FocusDistance = 20f; // 8 / 6 / 4 /  20 /40 /60
MainCam.Initialize();
var output = MainCam.Render(world);
output.SaveFile("focus20cm.ppm");
Console.WriteLine("focus 20 done");


MainCam.FocusDistance = 40f; // 8 / 6 / 4 /  20 /40 /60
MainCam.Initialize();
output = MainCam.Render(world);
output.SaveFile("focus40cm.ppm");
Console.WriteLine("focus 40 done");


MainCam.FocusDistance = 60f; // 8 / 6 / 4 /  20 /40 /60
MainCam.Initialize();
output = MainCam.Render(world);
output.SaveFile("focus60cm.ppm");
Console.WriteLine("focus 60 done");

//using (Game game = new Game(800, 600, "LearnOpenTK")) {
//game.Run();
//}
//public class Game : GameWindow {
//    static float[] vertices = {
//    -0.5f, -0.5f, 0.0f, //Bottom-left vertex
//     0.5f, -0.5f, 0.0f, //Bottom-right vertex
//     0.0f,  0.5f, 0.0f  //Top vertex
//    };
//    static string VertexShaderSource = @"
//    #version 330 core
//    layout (location = 0)
//    in vec3 aPosition;
//    void main(){
//        gl_Position = vec4(aPosition, 1.0);
//    }";

//    static string FragmentShaderSource = @"
//    #version 330 core
//    out vec4 FragColor;
//    void main(){
//        FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
//    }";


//    public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }
//    protected override void OnUpdateFrame(FrameEventArgs e) {
//        base.OnUpdateFrame(e);

//        if (KeyboardState.IsKeyDown(Keys.Escape)) {
//            Close();
//        }
//    }
//    protected override void OnLoad() {
//        base.OnLoad();

//        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

//        Code goes here
//        int VertexBufferObject = GL.GenBuffer();
//        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
//        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
//        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
//        GL.DeleteBuffer(VertexBufferObject);
//    }
//    protected override void OnRenderFrame(FrameEventArgs e) {
//        base.OnRenderFrame(e);

//        GL.Clear(ClearBufferMask.ColorBufferBit);

//        Code goes here.

//        SwapBuffers();
//    }
//    protected override void OnResize(ResizeEventArgs e) {
//        base.OnResize(e);

//        GL.Viewport(0, 0, e.Width, e.Height);
//    }
//}
