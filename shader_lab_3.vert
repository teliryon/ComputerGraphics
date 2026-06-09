#version 330 core

in vec3 aPosition;
in vec3 aColor;
in vec2 vTex;

out vec3 ourColor;
out vec2 texCoord;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
    ourColor = aColor;
    texCoord = vec2(vTex.x, 1.0 - vTex.y);
}