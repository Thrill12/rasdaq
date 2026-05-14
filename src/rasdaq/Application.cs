using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Input;
using rasdaq.Logging;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq;

/// <summary>
/// Main rasdaq application class.
/// </summary>
public sealed class Application : IDisposable
{
    public static Application? Instance { get; private set; }

    public InputManager InputManager { get; private set; }

    private List<World> _worlds = new();
    private GameWindow _gameWindow;

    /// <summary>
    /// Main entry point of a rasdaq-based application.
    /// 
    /// When creating an application, it is recommended to implement "using" pattern:
    /// <code>
    ///     using Application app = new(800, 600, "rasdaq");
    /// </code>
    /// </summary>
    /// <param name="width">Width of window in pixels</param>
    /// <param name="height">Height of window in pixels</param>
    /// <param name="title">Title of window</param>
    /// <exception cref="InvalidOperationException"></exception>
    public Application(int width, int height, string title)
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
    }

    internal void RegisterWorld(World world)
    {
        _worlds.Add(world);
    }

    /// <summary>
    /// Starts the application window. You must call this in order to start the game.
    /// </summary>
    public void Run()
    {
        InputManager.SetEventListeners();
        _gameWindow.Run();
    }

    private void OnUpdateFrame(FrameEventArgs args)
    {
        if (_gameWindow.KeyboardState.IsKeyDown(Keys.Escape))
        {
            _gameWindow.Close();
        }

        foreach (World world in _worlds)
        {
            world.GameLoop.Tick(args.Time);
        }
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

        Log.Debug("rasdaq started");

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

        _gameWindow.SwapBuffers();
    }

    private void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        GL.Viewport(0, 0, args.Width, args.Height);
    }

    /// <summary>
    /// Cleans up resources. Used once the Application instance is removed.
    /// </summary>
    public void Dispose()
    {
        _gameWindow.Dispose();
        Instance = null;
    }
}