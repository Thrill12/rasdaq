namespace rasdaq.Core.ECS;

internal class Utils
{
    internal static void EnsureStartableStart(Component component)
    {
        if (!component.Started)
        {
            component.Start();
            component.Started = true;
        }
    }

    internal static void EnsureStartableStart(Entity entity)
    {
        if (!entity.Started)
        {
            entity.Start();
            entity.Started = true;
        }
    }

    internal static void EnsureStartableStart(World world)
    {
        if (!world.Started)
        {
            world.Start();
            world.Started = true;
        }
    }
}
