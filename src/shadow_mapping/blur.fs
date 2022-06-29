#version 330 core
out vec4 FragColor;

uniform sampler2D shadowMap;
uniform int type;

in vec4 FragPosLightSpace;

void main()
{             
    int range = 5;
    //gl_FragDepth = gl_FragCoord.z;
    float mean=0;
    float variance=0;
    float mean_second=0;
    float variance_second=0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    // perform perspective divide
    vec3 projCoords = FragPosLightSpace.xyz / FragPosLightSpace.w;
    // transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;
    if(type==0)//x_direction
    {
        for(int i=-range;i<=range;i++)
        {
            mean+=texture(shadowMap,projCoords.xy+i*vec2(1,0)*texelSize).r;
            variance+=texture(shadowMap,projCoords.xy+i*vec2(1,0)*texelSize).g;
            mean_second+=texture(shadowMap,projCoords.xy+i*vec2(1,0)*texelSize).b;
            variance_second+=texture(shadowMap,projCoords.xy+i*vec2(1,0)*texelSize).a;
        }
        mean/=float(2*range+1);
        variance/=float(2*range+1);
        mean_second/=float(2*range+1);
        variance_second/=float(2*range+1);
    }
    else//y_direction
    {
        for(int i=-range;i<=range;i++)
        {
            mean+=texture(shadowMap,projCoords.xy+i*vec2(0,1)*texelSize).r;
            variance+=texture(shadowMap,projCoords.xy+i*vec2(0,1)*texelSize).g;
            mean_second+=texture(shadowMap,projCoords.xy+i*vec2(0,1)*texelSize).b;
            variance_second+=texture(shadowMap,projCoords.xy+i*vec2(0,1)*texelSize).a;
        }
        mean/=float(2*range+1);
        variance/=float(2*range+1);
        mean_second/=float(2*range+1);
        variance_second/=float(2*range+1);
    }
    FragColor = vec4(mean,variance,mean_second,variance_second);
}