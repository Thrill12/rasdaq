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
public class Application
{
    public static Application? Instance { get; private set; }

    private List<World> _worlds = new();
    private GameWindow? _gameWindow;

    static internal GameWindow GameWindow
    {
        get
        {
            return Instance?._gameWindow ?? throw new InvalidOperationException("There is no window created.");
        }
        set
        {
            if (Instance == null) throw new InvalidOperationException("There is no Application instance.");

            Instance._gameWindow = value;
        }
    }

    public static void Initialize(int width, int height, string title)
    {
        if (Instance != null) return;

        Instance = new(width, height, title);
    }

    internal Application(int width, int height, string title)
    {
        GameWindow = new(
            new GameWindowSettings()
            {
                UpdateFrequency = 0
            },
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        );

        GameWindow.VSync = VSyncMode.Off;
        GameWindow.UpdateFrame += OnUpdateFrame;
        GameWindow.Load += OnLoad;
        GameWindow.RenderFrame += OnRenderFrame;
        GameWindow.FramebufferResize += OnFramebufferResize;
    }

    internal void RegisterWorld(World world)
    {
        _worlds.Add(world);
    }

    /// <summary>
    /// Starts the application window.
    /// </summary>
    public static void Run()
    {
        GameWindow.Run();
    }

    private void OnUpdateFrame(FrameEventArgs args)
    {
        if (GameWindow.KeyboardState.IsKeyDown(Keys.Escape))
        {
            GameWindow.Close();
        }

        foreach (World world in _worlds)
        {
            world.GameLoop.Tick(args.Time);
        }
    }

    public static void SetBackgroundColor(Color color)
    {
        GL.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }

    private void OnLoad()
    {
        // Allows rendering png transparency
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Console.WriteLine("rasdaq started");

        Renderer.Instance.Init();

        foreach (World world in _worlds)
        {
            world.Start();
        }
    }

    private void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        Renderer.Instance.Render();

        GameWindow.SwapBuffers();
    }

    private void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        GL.Viewport(0, 0, args.Width, args.Height);
    }
}