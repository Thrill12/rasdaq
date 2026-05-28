using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using rasdaq.Audio;
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
    /// The <c>Application</c> instance is created after <c>Run</c> is called.
    /// </summary>
    public static Application Instance
    {
        get => _instance ?? throw new InvalidOperationException("Application not started. Call Run() first.");
        private set => _instance = value;
    }
    private static Application? _instance;
    public static Vector2? WindowSize => Instance._gameWindow?.Size;
    /// <summary>
    /// Return list of instantiated <c>Worlds</c>.
    /// </summary>
    public List<World> Worlds => _worlds.Objects;

    private InputManager? _inputManager;
    internal InputManager InputManager
    {
        get => _inputManager ?? throw new InvalidOperationException("InputManager not started.");
        private set => _inputManager = value;
    }

    private FlushEnumerable<World> _worlds = new();

    private GameWindow? _gameWindow;
    internal GameWindow GameWindow
    {
        get => _gameWindow ?? throw new InvalidOperationException("Game window not found. Call Run() first.");
        private set => _gameWindow = value;
    }

    internal void AddWorld(World world)
    {
        _worlds.Add(world);
    }

    internal void RemoveWorld(World world)
    {
        _worlds.Remove(world);
    }

    internal AudioManager AudioManager { get; private set; }

    private List<World> _worlds = new();
    private GameWindow _gameWindow;

    /// <summary>
    /// Starts the application window.
    /// </summary>
    public void Run(int width, int height, string title)
    {
        if (_instance != null)
        {
            throw new InvalidOperationException("There already exists an application.");
        }

        Instance = this;

        GameWindow = new(
            new GameWindowSettings()
            {
                UpdateFrequency = 0
            },
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        )
        {
            VSync = VSyncMode.Off
        };
        GameWindow.UpdateFrame += OnUpdateFrame;
        GameWindow.Load += OnLoad;
        GameWindow.RenderFrame += OnRenderFrame;
        GameWindow.FramebufferResize += OnFramebufferResize;

        InputManager = new InputManager(new GameWindowWrapper(_gameWindow));

        InputManager.SetEventListeners();
        GameWindow.Run();
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

        // Allows rendering in the correct order based on z value
        GL.Enable(EnableCap.DepthTest);

        Renderer.Instance.Init();
        AudioManager.Initialize();

        Init();

        Start();

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
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Renderer.Instance.Render(WindowSize ?? new Vector2(0, 0));

        GameWindow.SwapBuffers();
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
        _instance = null;
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
