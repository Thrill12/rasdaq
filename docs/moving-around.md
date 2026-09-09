# Moving around

Now that we have entities in our world, we would like to move them around. This 
can involve two aspects:
1. Moving them around visually
2. Using player input to trigger their movement (amongst other ways)


Each entity has a `Transform` property, which holds attributes like position, rotation, and scale.

We'll begin our tutorial by focusing on `position`, which is stored in a `Vector3` property.

The `Vector3` is a struct that stores the `X`, `Y`, and `Z` coordinates of the entity. 
While the `X` and `Y` coordinates correspond to a position on our 2D screen, the `Z` 
coordinate represents the layer in which the object exists. That is, it determines,
when objects overlap, which sprite overlaps another.

> [!NOTE]
> The Z coordinate must be between 0 and 1000, otherwise the object won't appear. All
> these coordinates are `double`, so they can even be a decimal number.

When creating an Entity, you can pass in a `Vector3` as a spawn position:
```c#
Vector3 topStarPosition = new(10, 10, 1);
Entity topStar = new(topStarPosition);

Entity defaultStar = new();

... // adding sprite components and adding them to the world
```
Here, `topStar` spawns at coordinates `(x: 10, y: 10)` and is in "layer" 1. On the other 
hand, `defaultStar` appears at `(x: 0, y: 0, z: 0)`, which is the default spawn if not set. 
This will be the bottom left corner of the screen, at layer 0.

Now, time to move these entities around! We can do them in two ways:
1. Setting the position of the entity
2. Attaching a `PhysicsBody` component to let physics properties move them around

### Setting the position of the entity
This is relatively straightforward:
```c#
topStar.position = new Vector3(20, 10, 1);
```
This will change the position of `topStar` to `x: 20` from `x: 10`. However, this will simply
"teleport" the object there.

We would instead want the object to smoothly move that distance. This is where we use `PhysicsBody`
component, which will deal with the visual movement based on given parameters.

### Using `PhysicsBody`
This is a component that handles the physics operations for an Entity. We may just want to set a
velocity and let this component handle the actual movement. So lets do that.
```c#
// moving the star at a velocity of (x: 200, y: 0) per second
Vector2 starVelocity = new(200, 0);

PhysicsBody starBody = new();
topStar.AddComponent(starBody);

starBody.Velocity = starVelocity
```
The star will now move at 200 pixels per millisecond. The actual distance it moves here
would be determined by the time the **physics tick** takes (since distance is a product
of speed and time).

> [!TIP]
> A physics tick is its own unit of time used by our game engine for physics operations
> This allows us to do physics operations periodically but independently of the framerate.
>
> The tick is timed in milliseconds, hence the speed being mentioned in pixels per millisecond.

#### Moving a given distance
To move the star a specific distance at a specific velocity, we can use the `MoveDistance` method.
```c#
Vector2 starDistanceVelocity = new(30, 0);

// set the independent velocity to zero otherwise this would add to the starDistanceVelocity
// for the purpose of this demo, we just want the star to move by MoveDistance
starBody.Velocity = Vector2.Zero;
starBody.MoveDistance(velocity: starDistanceVelocity, distance: 200, thisDirectionOnly: false);
```

Now, you will notice the star moves to the right until it covers that distance, and then stops.

There is one parameter here that might sound confusing: `thisDirectionOnly`. This will be broken down
in this next section.

#### Moving a given distance in a particular direction only
Things get complicated if the star is being pushed by more than one force at the same time. Let's say
we have another "velocity" on the star in a perpendicular direction, in `(0, 40)`. How does the game calculate
when the star has covered 200 pixels? This is determined by `thisDirectionOnly`.

If we want to make sure the star covers the required distance only in the direction provided by the velocity, 
we set the `thisDirectionOnly` to true.

The difference here is that it doesn't count any component of distance travelled in another direction.

So if `thisDirectionOnly` is set to true, it will only count 30 units of distance covered per unit time. 
On the other hand, if `thisDirectionOnly` is set to false, the distance counted as covered would be 50 units 
(30 horizontal and 40 from vertical velocity, and so a diagonal distance of 50 units).

#### Moving once / using player input to trigger movement
If you want to move a specific velocity for one physics tick, you can call the `MoveOnce` method. This is ideal for
input-based movement, as you can call this everytime the user presses a key.
```c#
// creating a component so that we can use its Update method
class StarComponent : Component
{
    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        PhysicsBody? body = Entity?.GetComponent<PhysicsBody>();

        if (Input.IsKeyDown(Keys.W))
        {
            body?.MoveOnce(new Vector2(0, 100));
        }
        if (Input.IsKeyDown(Keys.S))
        {
            body?.MoveOnce(new Vector2(0, -100));
        }
    }
}

...

// somewhere in application start, we add our custom component to our desired entity
StarComponent starComponent = new()
topStar.AddComponent(starComponent);
```

Here, when the Update method (which is called every physics tick on all components) is called, and the `W` key is pressed
by the user, it will call `MoveOnce` on the `PhysicsBody` component of the entity it's attached to (in this case it will be `topStar`).

So in simple words, everytime `W` is pressed, the entity is moved once at (0, 100) velocity, allowing for input-based movement!

### Moving the camera
Now that we have learned about movement, let us learn how to move the camera so it tracks `topStar` such that it
doesn't disappear from the screen.

This is fairly straightforward. All we have to do is get the instance of `Camera`, which is a property 
of the singleton `Renderer`, and set its coordinates in the world.

Note that the coordinates we set here is of the **bottom left corner** of the camera.

```
    +---------------------------------+
    |                                 |
    |                                 |
    |             Window              |
    |                                 |
    |                                 |
    +---------------------------------+
  (x, y)                
    ^
    |
Camera coordinates
```

> [!NOTE]
> This feature will soon be changed to use center of the camera window as coordinates

```c#
// somewhere in a frame update loop
...
// getting entity's x/y position... 
// and then offsetting its coordinates by half of window size...
// so the entity is in the center of camera window
var cameraX = Entity?.Transform.position.X - (Application.WindowSize?.X / 2) ?? 0;
var cameraY = Entity?.Transform.position.Y - (Application.WindowSize?.Y / 2) ?? 0;
Renderer.Instance.Camera.SetPosition(x, y);
```

Now when moving around you will notice the entity doesn't appear to move, which is due to the camera tracking
its position.
