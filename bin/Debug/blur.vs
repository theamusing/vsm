#version 330 core
layout (location = 0) in vec3 aPos;

uniform mat4 lightSpaceMatrix;
uniform mat4 model;

out vec4 FragPosLightSpace;

void main()
{
    gl_Position = lightSpaceMatrix * model * vec4(aPos, 1.0);
    FragPosLightSpace = lightSpaceMatrix * vec4(vec3(model * vec4(aPos, 1.0)), 1.0);
}