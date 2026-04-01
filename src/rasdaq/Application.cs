using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq;

/// <summary>
/// Main rasdaq application class.
/// </summary>
public sealed class Application
{
    /// <summary>
    /// Application instance singleton.
    /// </summary>
    public static Application? Instance { get; private set; }

    internal List<World> worlds = new();
    internal GameWindow _gameWindow;

    public Application(int width, int height, string title)
    {
        Instance = this;

        _gameWindow = new(
            new GameWindowSettings()
            {
                UpdateFrequency = 0
            },
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        );

        _gameWindow.VSync = VSyncMode.Off;
        _gameWindow.UpdateFrame += OnUpdateFrame;
        _gameWindow.Load += OnLoad;
        _gameWindow.RenderFrame += OnRenderFrame;
        _gameWindow.FramebufferResize += OnFramebufferResize;
    }

    /// <summary>
    /// Starts the application window.
    /// </summary>
    public void Run()
    {
        _gameWindow.Run();
    }

    void OnUpdateFrame(FrameEventArgs args)
    {
        if (_gameWindow.KeyboardState.IsKeyDown(Keys.Escape))
        {
            _gameWindow.Close();
        }

        foreach (World world in worlds)
        {
            world.GameLoop.Tick(args.Time);
        }
    }

    public static void SetBackgroundColor(Color color)
    {
        GL.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }

    void OnLoad()
    {
        // Allows rendering png transparency
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Console.WriteLine("rasdaq started");

        Renderer.Instance.Init();

        foreach (World world in worlds)
        {
            world.Start();
        }
    }

    void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        Renderer.Instance.Render();

        _gameWindow.SwapBuffers();
    }

    void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        GL.Viewport(0, 0, args.Width, args.Height);
    }
}
