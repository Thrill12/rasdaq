using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Input;
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
    private Entity soldier;

    private List<World> _worlds = new();
    private GameWindow _gameWindow;

    /// <summary>
    /// Main entry point of a rasdaq-based application.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="title"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public Application(int width, int height, string title, Entity soldier)
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
        this.soldier = soldier;
    }

    internal void RegisterWorld(World world)
    {
        _worlds.Add(world);
    }

    /// <summary>
    /// Starts the application window.
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

        if (_gameWindow.KeyboardState.IsKeyDown(Keys.W))
        {
            soldier.Transform.MoveOnce(new OpenTK.Mathematics.Vector2(0, 100));
        }

        if (_gameWindow.KeyboardState.IsKeyDown(Keys.S))
        {
            soldier.Transform.MoveOnce(new OpenTK.Mathematics.Vector2(0, -100));
        }
        if (_gameWindow.KeyboardState.IsKeyDown(Keys.A))
        {
            soldier.Transform.MoveOnce(new OpenTK.Mathematics.Vector2(-100, 0));
        }
        if (_gameWindow.KeyboardState.IsKeyDown(Keys.D))
        {
            soldier.Transform.MoveOnce(new OpenTK.Mathematics.Vector2(100, 0));
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

        Renderer.Instance.Render(args.Time);

        _gameWindow.SwapBuffers();
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
        _gameWindow.Dispose();
        Instance = null;
    }
}