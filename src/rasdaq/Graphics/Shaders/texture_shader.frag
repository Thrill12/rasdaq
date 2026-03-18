#version 330 core

in vec2 TextureCoord;
in vec4 VertColor;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, TextureCoord) * VertColor;
}