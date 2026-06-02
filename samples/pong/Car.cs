using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Transformations;

public class Car : Component
{
    public float speed;
    PhysicsBody? body;

    public override void Start()
    {
        base.Start();

        body = Entity?.GetComponent<PhysicsBody>();

        Log.Info("Car component started");
    }

    Vector2 currentMovement = Vector2.Zero;

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
        Accelerate(deltaTime);
        Turn((float)deltaTime);
    }

    private float _currentSpeed;
    public float maxSpeed = 2000;

    private void Accelerate(double deltaTime)
    {
        if (body == null)
        {
            return;
        }

        if (currentMovement.Y > 0)
        {
            _currentSpeed += speed * (float)deltaTime;
        }
        else if (currentMovement.Y < 0)
        {
            _currentSpeed -= speed * (float)deltaTime;
        }
        else
        {
            _currentSpeed = (float)Lerp.Linear(_currentSpeed, 0, 1 * (float)deltaTime);
        }

        _currentSpeed = Math.Clamp(_currentSpeed, -maxSpeed, maxSpeed);

        Vector2 velocity = Entity.Transform.Forward * _currentSpeed;
        body.MoveOnce(velocity);
    }

    public override void FrameUpdate(double deltaTime)
    {
        base.FrameUpdate(deltaTime);

        Vertical();
        Horizontal();

        Log.Info($"Current movement: {currentMovement}");
    }

    public override void LateUpdate(double deltaTime)
    {
        base.LateUpdate(deltaTime);

        Renderer.Instance.Camera.SetPosition(
            (Vector2)Entity.Transform.position - new Vector2(300, 300)
        );
    }

    public float rotationSpeed = 100;

    private void Turn(float deltaTime)
    {
        // Prevent turning when stopped
        float speedRatio = Math.Abs(_currentSpeed) / maxSpeed;
        if (speedRatio < 0.001f)
        {
            return;
        }

        float turnFactor = 0.5f - speedRatio;

        Entity.Transform.rotation += (float)(
            currentMovement.X * rotationSpeed * deltaTime * turnFactor
        );
    }

    private void Horizontal()
    {
        if (Input.IsKeyDown(Keys.D))
        {
            currentMovement.X = -1;
        }
        else if (Input.IsKeyDown(Keys.A))
        {
            currentMovement.X = 1;
        }
        else
        {
            currentMovement.X = 0;
        }

        // Reverse for car-like movement
        if (_currentSpeed < 0)
        {
            currentMovement.X *= -1;
        }
    }

    private void Vertical()
    {
        if (Input.IsKeyDown(Keys.W))
        {
            currentMovement.Y = 1;
        }
        else if (Input.IsKeyDown(Keys.S))
        {
            currentMovement.Y = -1;
        }
        else
        {
            currentMovement.Y = 0;
        }
    }
}
