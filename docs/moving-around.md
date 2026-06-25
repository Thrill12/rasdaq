# Moving around

Now that we have entities in our world, we would like to move them around. This 
can involve two aspects:
1. Moving them around visually
2. Using player input to trigger their movement (amongst other things)


Each entity has a `Transform` object property that stores its position in the world.
This is stored in a `Vector3` property called `position` with the `Transform`.

The `Vector3` is a struct that stores the `X`, `Y`, and `Z` coordinates of the entity. 
While the `X` and `Y` coordinates correspond to a position on our 2D screen, the `Z` 
coordinate represents the layer in which the object exists. That is, it determines,
when objects overlap, which sprite overlaps another.

> [!NOTE]
> The Z coordinate must be between 0 and 1000, otherwise the object won't appear. All
> these coordinates are `double`s, so they can even be a decimal number.

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

We would instead want the object to move smoothly that distance. This is where we use `PhysicsBody`
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
<!-- TODO This is wrong, 200 is the speed, not distance every tick. Fix here and below and above etc. -->
The star will now move at 200 pixels to the right every **physics tick**.

> [!TIP]
> A physics tick is its own unit of time used by our game engine for physics operations
> This allows us to do physics operations periodically but independently of the framerate.

#### Moving a given distance
To move the star a specific distance at a specific velocity, we can use the `MoveDistance` method.
```c#
Vector2 starDistanceVelocity = new(50, 0);

// set the independent velocity to zero otherwise this would add to the starDistanceVelocity
// for the purpose of this demo, we just want the star to move by MoveDistance
starBody.Velocity = Vector2.Zero;
starBody.MoveDistance(velocity: starDistanceVelocity, distance: 200, thisDirectionOnly: false);
```

Now, you will notice the star moves to the right over 4 seconds, and then stops.

There is one parameter here that might sound confusing: `thisDirectionOnly`. This will be broken down
in this next section.

#### Moving a given distance in a particular direction only
Now if we want to make sure the star covers the required distance in the direction provided by the velocity, 
we set the `thisDirectionOnly` to true.

The difference here is that it doesn't count any component of distance travelled in another direction.

So if you were to have another "velocity" on the star in the opposite direction, of lets say (40, 0) then for
each second, it will only count 10 units of distance covered.

#### Moving once
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
