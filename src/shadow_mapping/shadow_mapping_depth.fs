#version 330 core
out vec4 FragColor;
uniform float c_power;
void main()
{             

    //gl_FragDepth = gl_FragCoord.z;
    highp float depth = gl_FragCoord.z;
    highp float k1=exp(c_power*depth);
    highp float k2=-exp(-c_power*depth);
    FragColor = vec4(k1,k1*k1,k2,k2*k2);
}