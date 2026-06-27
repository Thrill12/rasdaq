#version 330 core

in vec3 aPosition;
in vec4 aColor;

out vec4 VertColor;

uniform mat4 transform;
uniform mat4 projection;
uniform mat4 view;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
    VertColor = aColor;
}