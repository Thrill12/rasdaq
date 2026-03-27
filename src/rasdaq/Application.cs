using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Graphics;
using System.Drawing;

namespace rasdaq;

public class Application : GameWindow
{
    public static Application? Instance { get; private set; }

    public Application(int width, int height, string title)
        : base(
            GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        )
    {
        Instance = this;
    }

    // GAME LOOP ACTIONS
    public Action<double> Update;
    public Action<double> FrameUpdate;
    public Action<double> LateUpdate;

    private double _msPerUpdate = 0.01f;
    private double _lag = 0.0f;
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        _lag += args.Time;

        // TODO: Spiral of death?
        // When lag keeps increasing, the game can't catch up and
        // will at some point explode?

        // PROCESS INPUT HERE

        while (_lag >= _msPerUpdate)
        {
            Update?.Invoke(_msPerUpdate);

            _lag -= _msPerUpdate;
        }

        FrameUpdate?.Invoke(args.Time);

        LateUpdate?.Invoke(args.Time);
    }

    public static void SetBackgroundColor(Color color)
    {
        GL.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }

    protected override void OnLoad()
    {
        // Allows rendering png transparency
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Console.WriteLine("rasdaq started");

        Renderer.Instance.Init();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        Renderer.Instance.Render();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        base.OnFramebufferResize(args);

        GL.Viewport(0, 0, args.Width, args.Height);
    }
}
