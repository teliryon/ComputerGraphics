#version 330 core

in vec3 aPosition;
in vec3 aColor;
in vec2 aTexCoord;

out vec3 ourColor;
out vec2 texCoord;

uniform mat4 transform;

void main()
{
    gl_Position = transform * vec4(aPosition, 1.0);
    ourColor = aColor;
    texCoord = vec2(aTexCoord.x, 1.0 - aTexCoord.y);
}