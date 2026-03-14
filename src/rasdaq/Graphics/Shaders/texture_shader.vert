#version 330 core

in vec3 aPosition;
in vec2 aTexture;
in vec4 aColor;

out vec2 TextureCoord;
out vec4 VertColor;

void main()
{
    gl_Position = vec4(aPosition, 1.0);

    TextureCoord = aTexture;
    VertColor = aColor;
}