using Silk.NET.GLFW;
using Silk.NET.OpenGL;

class Program {
    public unsafe static int Main(string[] args) {
        var GLFW = Glfw.GetApi();
        
        if (!GLFW.Init()) {
            Console.WriteLine("Failed to initialize GLFW");
            return -1;
        }
        GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
        GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 5);
        GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
        var window = GLFW.CreateWindow(400, 400, "GLFW.NET", null, null);
        if (window == null) {
            Console.WriteLine("Failed to create GLFW window");
            GLFW.Terminate();
            return -1;
        }
        GLFW.MakeContextCurrent(window);
        
        var Gl = GL.GetApi(,);
        while (!GLFW.WindowShouldClose(window)) {
            // Render here
            Gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            // Swap front and back buffers
            GLFW.SwapBuffers(window);

            // Poll for and process events
            GLFW.PollEvents();
        }

        GLFW.Terminate();
        return 0;
    }
}