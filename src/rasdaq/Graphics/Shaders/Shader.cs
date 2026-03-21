using OpenTK.Graphics.OpenGL4;

namespace rasdaq.Graphics.Shaders;

public class Shader : IDisposable
{
    int Handle;

    public Shader(string? vertexPath = null, string? fragmentPath = null)
    {
        vertexPath ??= Common.TEXTURE_SHADER;
        fragmentPath ??= Common.TEXTURE_SHADER_FRAG;

        LoadShaderFiles(vertexPath, fragmentPath, out int VertexShader, out int FragmentShader);

        CompileShader(VertexShader, FragmentShader);
        LinkShader(VertexShader, FragmentShader);
        CleanupShader(VertexShader, FragmentShader);
    }

    public static Shader FromSource(string vertexSource, string fragmentSource)
    {
        Shader shader = new();
        LoadShaderFilesDirectly(vertexSource, fragmentSource, out int VertexShader, out int FragmentShader);
        CompileShader(VertexShader, FragmentShader);
        shader.LinkShader(VertexShader, FragmentShader);
        shader.CleanupShader(VertexShader, FragmentShader);
        return shader;
    }

    protected Shader() { }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    private bool _disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            GL.DeleteProgram(Handle);
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Shader()
    {
        if (!_disposedValue)
        {
            Console.WriteLine("Shader not disposed properly.");
        }
    }

    private void CleanupShader(int VertexShader, int FragmentShader)
    {
        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(VertexShader);
        GL.DeleteShader(FragmentShader);
    }

    private static void LoadShaderFiles(
        string vertexName,
        string fragmentName,
        out int VertexShader,
        out int FragmentShader
        )
    {
        string vertexSource = LoadEmbeddedShader(vertexName);
        string fragmentSource = LoadEmbeddedShader(fragmentName);

        Console.WriteLine(vertexSource);

        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, vertexSource);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, fragmentSource);
    }

    private static string LoadEmbeddedShader(string filename)
    {
        var assembly = typeof(Shader).Assembly;
        string resourceName = $"rasdaq.Graphics.Shaders.{filename}";

        using Stream stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Embedded shader not found: {resourceName}");
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }

    private static void LoadShaderFilesDirectly(
        string vertex,
        string fragment,
        out int VertexShader,
        out int FragmentShader
        )
    {
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, vertex);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, fragment);
    }

    private void LinkShader(int VertexShader, int FragmentShader)
    {
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);
        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }
    }

    private static void CompileShader(int VertexShader, int FragmentShader)
    {
        GL.CompileShader(VertexShader);
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(FragmentShader);

        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }

    public int GetUniformLocation(string uniformName)
    {
        return GL.GetUniformLocation(Handle, uniformName);
    }

    public void SetUniform(string uniformName, float val)
    {
        Use();
        int location = GetUniformLocation(uniformName);
        if (location == -1)
        {
            return;
        }

        GL.Uniform1(location, val);
    }

    public void SetUniform(string uniformName, OpenTK.Mathematics.Vector2 val)
    {
        Use();
        int location = GetUniformLocation(uniformName);
        if (location == -1)
        {
            return;
        }

        GL.Uniform2(location, val.X, val.Y);
    }

    public void SetUniform(string uniformName, OpenTK.Mathematics.Vector3 val)
    {
        Use();
        int location = GetUniformLocation(uniformName);
        if (location == -1)
        {
            return;
        }

        GL.Uniform3(location, ref val);
    }
    public void SetUniform(string uniformName, OpenTK.Mathematics.Vector4 val)
    {
        Use();
        int location = GetUniformLocation(uniformName);
        if (location == -1)
        {
            return;
        }

        GL.Uniform4(location, ref val);
    }

    public void SetUniform(string uniformName, OpenTK.Mathematics.Matrix4 val, bool transpose = false)
    {
        Use();
        int location = GetUniformLocation(uniformName);
        if (location == -1)
        {
            return;
        }

        GL.UniformMatrix4(location, transpose, ref val);
    }
}
