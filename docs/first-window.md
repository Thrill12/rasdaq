# Your first window

The code below shows how you can set up an extremely simple window, with a sprite rendering in the center.

```C#
using Application app = new(800, 600, "My First Window");

World world = new();
Entity entity = new();
world.AddEntity(entity);

Texture texture = ResourceManager.Load<Texture>("assets/star.png");
Sprite sprite = new(1, 1, texture);
entity.AddComponent(sprite);

app.Run();
```

This may look quite complicated, but let's break it down line by line.

```C#
using Application app = new(800, 600, "My First Window");
```

Each application built with rasdaq must be initialized. You pass in width/height and a window name to the Application constructor, and your app is technically ready to launch!
Nothing will happen yet as we've not started the app, nor have we added anything so let's change that!

```C#
World world = new();
Entity entity = new();
world.AddEntity(entity);
```

rasdaq works with worlds and entities. Think of a world as a "Collection" of entities. 
Each world is responsible for its own entities behind the scenes, but the player will still be able to interact with any world.

Here, we create a new empty world. A universe we could do anything we please with!

But... what's a universe without stars? The next line creates an empty entity. Each entity is a "thing" in a world. To assign an entity to a world, you simply call "AddEntity" on the world you want. Simple!

```C#
Texture texture = ResourceManager.Load<Texture>("assets/star.png");
Sprite sprite = new(1, 1, texture);
entity.AddComponent(sprite);
```

The next three lines handle loading a texture, creating a sprite component, and adding that component to the entity.
Each entity may have multiple components - you can even create your own! We'll come back to that later though.

The last piece of our puzzle:

```C#
app.run();
```

Make sure to start your application **after** you initialize everything - you won't see anything otherwise!