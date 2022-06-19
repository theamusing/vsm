#version 330 core
out vec4 FragColor;
void main()
{             

    //gl_FragDepth = gl_FragCoord.z;
    highp float depth = gl_FragCoord.z;
    FragColor = vec4(depth,depth*depth,0,0);
}