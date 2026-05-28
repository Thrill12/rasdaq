using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Resources;
using rasdaq.Transformations;
using OpenTK.Audio.OpenAL;
using rasdaq.Audio;
using rasdaq.Core.ECS;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Resources;

namespace pong;

internal class Soldier : Component
{
    public override void Start()
    {
        base.Start();

        Entity?.AddComponent(new AudioSource());

        Audio audio1 = ResourceManager.Load<Audio>("assets/test.wav");
        Entity?.GetComponent<AudioSource>()?.AttachAudio(audio1);

        Log.Info($"Audio initialized on {Thread.CurrentThread.Name}");

        Input.OnKeyDownEvent.Add(Keys.W, () =>
        {
            Log.Info("User pressed W based on an event");
        });

        Input.OnKeyUpEvent.Add(Keys.B, () =>
        {
            Log.Info("Hello");
        });
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        Log.Info($"Audio initialized on {Thread.CurrentThread.Name}");
    }

    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);
        Log.Info($"Audio initialized on {Thread.CurrentThread.Name}");


        if (Input.IsKeyPressed(Keys.V))
        {
            Log.Info("V pressed");
        }

        PhysicsBody? body = Entity?.GetComponent<PhysicsBody>();

        if (Input.IsKeyDown(Keys.W))
        {
            body?.MoveOnce(new Vector2(0, 100));
        }

        if (Input.IsKeyDown(Keys.S))
        {
            body?.MoveOnce(new Vector2(0, -100));
        }
        if (Input.IsKeyDown(Keys.A))
        {
            body?.MoveOnce(new Vector2(-100, 0));
        }
        if (Input.IsKeyDown(Keys.D))
        {
            body?.MoveOnce(new Vector2(100, 0));
        }
        if (Input.IsKeyDown(Keys.H))
        {
            // track camera to entity
            var x = Entity?.Transform.position.X - (Application.WindowSize?.X / 2) ?? 0;
            var y = Entity?.Transform.position.Y - (Application.WindowSize?.Y / 2) ?? 0;
            Renderer.Instance.Camera.SetPosition(x, y);
        }
    }
}

internal class Enemy : Component
{
    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);

        if (Input.IsKeyDown(Keys.I))
        {
            // track camera to entity
            var x = Entity?.Transform.position.X - (Application.WindowSize?.X / 2) ?? 0;
            var y = Entity?.Transform.position.Y - (Application.WindowSize?.Y / 2) ?? 0;
            Renderer.Instance.Camera.SetPosition(x, y);
            if (Input.IsKeyDown(Keys.W))
            {
                ALContext context = ALC.GetCurrentContext();
                Log.Info($"CurrentContext = {context == ALContext.Null}");
                Log.Info($"W is held down, playing AudioSource! {Entity?.GetComponent<AudioSource>() == null}");
                Entity?.GetComponent<AudioSource>()?.Play();
            }
        }
    }
