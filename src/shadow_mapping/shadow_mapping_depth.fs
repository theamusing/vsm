#version 330 core
out vec4 FragColor;
uniform float pos_power;
uniform float neg_power;
uniform float amout;
void main()
{             
    //gl_FragDepth = gl_FragCoord.z;
    float depth = gl_FragCoord.z;
    depth=2.0*depth-1.0;
    float k1=exp(pos_power*depth)/amout;
    float k2=exp(-neg_power*depth)/amout;
    FragColor = vec4(k1,k1*k1,k2,k2*k2);
}