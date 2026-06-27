public static class Lerp
{
    /// <summary>
    /// Linearly interpolate between two values.
    /// </summary>
    public static double Linear(double a, double b, double t)
    {
        return a + ((b - a) * t);
    }

    public static float Linear(float a, float b, float t)
    {
        return a + ((b - a) * t);
    }

    public static Vector3 Linear(Vector3 a, Vector3 b, float t)
    {
        return a + ((b - a) * t);
    }
}
