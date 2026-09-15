using OpenTK.Audio.OpenAL;
using rasdaq;
using rasdaq.Audio;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Resources;
using rasdaq.Transformations;

namespace pong;

internal class Soldier : Component
{
    AudioSource _audioSource;

    public override void Start()
    {
        base.Start();

        Audio audio1 = ResourceManager.Load<Audio>("assets/laser.wav");
        AudioSource source = new();
        // source.Handle = audio1.Handle;
        source.SetAudio(audio1);
        _audioSource = source;
        Entity.AddComponent(source);
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);

        if (Input.IsKeyPressed(Keys.V))
        {
            Entity.Transform.RotateFromPoint(new Vector2(800, 200), 90);
        }

        if (Input.IsKeyPressed(Keys.Space))
        {
            _audioSource.Play();
        }

        if (Input.IsKeyPressed(Keys.LeftShift))
        {
            _audioSource.Stop();
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
            }
        }
    }
}
