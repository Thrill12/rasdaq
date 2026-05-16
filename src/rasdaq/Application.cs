using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Interfaces;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq;

/// <summary>
/// Main rasdaq application class.
/// </summary>
public class Application : IDisposable
{
    /// <summary>
    /// The <c>Application</c> instance is created when <c>Run</c> is called.
    /// </summary>
    public static Application? Instance { get; private set; }
    public List<World> Worlds => _worlds.Objects;

    internal InputManager? InputManager { get; set; }

    private FlushEnumerable<World> _worlds = new();
    private GameWindow? _gameWindow;

    internal void AddWorld(World world)
    {
        _worlds.Add(world);
    }

    internal void RemoveWorld(World world)
    {
        _worlds.Add(world);
    }

    /// <summary>
    /// Starts the application window.
    /// </summary>
    public void Run(int width, int height, string title)
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("There already exists an application.");
        }

        Instance = this;

        _gameWindow = new(
            new GameWindowSettings()
            {
                UpdateFrequency = 0
            },
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        )
        {
            VSync = VSyncMode.Off
        };
        _gameWindow.UpdateFrame += OnUpdateFrame;
        _gameWindow.Load += OnLoad;
        _gameWindow.RenderFrame += OnRenderFrame;
        _gameWindow.FramebufferResize += OnFramebufferResize;

        InputManager = new InputManager(new GameWindowWrapper(_gameWindow));

        InputManager.SetEventListeners();
        _gameWindow.Run();
    }

    /// <summary>
    /// Sets the background color of the window.
    /// </summary>
    /// <param name="color"></param>
    public static void SetBackgroundColor(Color color)
    {
        GL.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }

    private void OnLoad()
    {
        // Allows rendering png transparency
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Renderer.Instance.Init();

        Init();
        _worlds.FlushPending();

        Start();
        _worlds.FlushPending();

        for (int i = 0; i < Worlds.Count; i++)
        {
            World world = Worlds[i];
            world.Start();
        }
    }

    private void OnUpdateFrame(FrameEventArgs args)
    {
        _worlds.FlushPending();
        for (int i = 0; i < Worlds.Count; i++)
        {
            World world = Worlds[i];
            world.GameLoop.Tick(args.Time);
        }
    }

    private void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        Renderer.Instance.Render();

        _gameWindow?.SwapBuffers();
    }

    private void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        GL.Viewport(0, 0, args.Width, args.Height);
    }

    /// <summary>
    /// Cleans up resources.
    /// </summary>
    public void Dispose()
    {
        _gameWindow?.Dispose();
        Instance = null;
    }

    /// <summary>
    /// Executes before any <c>World</c> start function, once the application window starts.
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    /// Executes before <c>Start</c>, once the application window starts.
    /// </summary>
    public virtual void Init() { }
}
