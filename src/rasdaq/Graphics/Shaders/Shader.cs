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
        string vertexPath,
        string fragmentPath,
        out int VertexShader,
        out int FragmentShader
    )
    {
        string VertexShaderSource = File.ReadAllText(vertexPath);
        string FragmentShaderSource = File.ReadAllText(fragmentPath);

        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);
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
}
