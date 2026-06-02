public static class Lerp
{
    /// <summary>
    /// Linearly interpolate between two values.
    /// </summary>
    public static double Linear(double a, double b, double t)
    {
        return a + ((b - a) * t);
    }
}
