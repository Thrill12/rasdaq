using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Graphics;
using rasdaq.Input;
using System.Drawing;

namespace rasdaq;

public class Application : GameWindow
{
    public static Application? Instance { get; private set; }
    public InputManager InputManager { get; private set; }

    public Application(int width, int height, string title)
        : base(
            GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        )
    {
        Instance = this;
            InputManager = new InputManager(this);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    public static void SetBackgroundColor(Color color)
    {
        GL.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }

    public override void Run()
    {
        InputManager.SetEventListeners();
        base.Run();
    }


    protected override void OnLoad()
    {
        // Allows rendering png transparency
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Console.WriteLine("rasdaq started");

        Renderer.Instance.Init();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Rendering code here
        Renderer.Instance.Render();

        // Double-buffering means that there are two areas that OpenGL draws to.
        // In essence: One area is displayed, while the other is being rendered to.
        // Then, when you call SwapBuffers, the two are reversed.
        // A single-buffered context could have issues such as screen tearing.
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
