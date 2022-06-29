#version 330 core
out vec4 FragColor;
uniform float c_power;

void main()
{             
    //gl_FragDepth = gl_FragCoord.z;
    float depth = gl_FragCoord.z;
    depth=2.0*depth-1.0;
    float k1=exp(c_power*depth);
    float k2=depth;
    FragColor = vec4(k1,k2,depth,c_power);
}