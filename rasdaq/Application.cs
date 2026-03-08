using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

internal class Application : GameWindow
{
    public Application(int width, int height, string title)
        : base(
            GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        )
    {
        Console.WriteLine("Application started");
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }
}
