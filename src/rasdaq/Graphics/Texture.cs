using OpenTK.Graphics.OpenGL4;
using StbiSharp;
using System.Runtime.InteropServices;

namespace rasdaq.Graphics;

public class Texture
{
    private int _handle;

    protected Texture() { }

    internal Texture(string path)
    {
        _handle = GL.GenTexture();
        Use();

        Stbi.SetFlipVerticallyOnLoad(true);

        using FileStream stream = File.OpenRead(path);
        using MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        StbiImage image = Stbi.LoadFromMemory(memoryStream, 4);
        byte[] pixelData = image.Data.ToArray();

        // OpenGL expects a static memory location for pixel data. This 
        // allocates that memory, instead of relying on C# automatic memory handling.
        GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);

        try
        {
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                handle.AddrOfPinnedObject()
            );
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        finally
        {
            handle.Free();
        }
    }

    internal void Use()
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
}