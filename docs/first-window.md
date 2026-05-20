# Your first window

The code below shows how you can set up an extremely simple window, with a sprite rendering in the center.

##### **`Program.cs`**
```C#
class Program
{
    private static void Main()
    {
        try
        {
            using Game game = new();

            game.Run(800, 600, "rasdaq");
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
            throw new Exception(ex.Message + "\n Check 'rasdaq.log' for more details. \n" + ex.StackTrace);
        }
    }
}
```

The `Game` class is something we will define later. We create an instance of it, and then we call `Run` with the width, height and window title that we want. We wrap this all in a try catch clause to... catch any errors. Now we can move on!

##### **`Game.cs`**
```C#
using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Resources;

public class Game : Application
{
    public Texture texture;

    public override void Init()
    {
        texture = ResourceManager.Load<Texture>("assets/andrei.png");
    }

    public override void Start()
    {
        World world = new();
        Entity entity = new();
        world.AddEntity(entity);

        Sprite spr = new(1, 1, andrei);
        entity.AddComponent(spr);

        Soldier sold = new();
        entity.AddComponent(sold);
    }
}
```

This might seem like a lot, so let's take this from the start.

You must have a `Game` class, which will be responsible for managing and starting your game. Everything inside this class will have access to initialized rasdaq resources, like the InputManager and the ResourceManager. Outside of `Game`, you will not be able to use these.

The `Init` function runs as an initialization step for your game. Here, you may do anything, but we recommend creating object pools here for use later in the game. This is the perfect time to use the `ResourceManager` to load your files!

The `Start` function runs *after* `Init`, and is likely where you will be creating the bulk of your game. Here, we are creating a world, a player, and adding two components to that player. Keep in mind, you can also do this inside `Init`, as these two functions are essentially the same but with a guaranteed running order.

```C#
World world = new();
Entity entity = new();
world.AddEntity(entity);
```

rasdaq works with worlds and entities. Think of a world as a "Collection" of entities. 
Each world is responsible for its own entities behind the scenes, such as performing their lifecycle events.

Here, we create a new empty world. A universe we could do anything we please with!

But... what's a universe without stars? The next line creates an empty entity. Each entity is a "thing" in a world. To assign an entity to a world, you simply call "AddEntity" on the world you want. Simple!

```C#
public override void Init()
{
    texture = ResourceManager.Load<Texture>("assets/andrei.png");
}
```

The `Init` function handles loading a texture using the `ResourceManager`. It may return any type from any path, as long as it's configured to do so.

> **Note**
> The `ResourceManager` will soon be expanded to allow users to define their own file loaders.

```C#
Sprite spr = new(1, 1, texture);
entity.AddComponent(spr);

Soldier sold = new();
entity.AddComponent(sold);
```

Here, we are creating a new sprite based on the texture we created in the `Init` function.
Each entity may have multiple components - you can even create your own!

Now, if you run your application, you should see your image!