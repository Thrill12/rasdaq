using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Graphics;

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

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnLoad()
    {
        // TODO: Function to set background color
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        Console.WriteLine("rasdaq started");
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Rendering code here
        RenderSprites();

        // Double-buffering means that there are two areas that OpenGL draws to.
        // In essence: One area is displayed, while the other is being rendered to.
        // Then, when you call SwapBuffers, the two are reversed.
        // A single-buffered context could have issues such as screen tearing.
        SwapBuffers();
    }

    private List<Sprite> sprites = new List<Sprite>();

    internal void LoadSprite(Sprite sprite)
    {
        sprites.Add(sprite);
        Console.WriteLine($"Sprite loaded. Total sprites: {sprites.Count}");
    }

    private void RenderSprites()
    {
        foreach (Sprite sprite in sprites)
        {
            sprite.Render();
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
