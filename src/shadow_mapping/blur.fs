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
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    // perform perspective divide
    vec3 projCoords = FragPosLightSpace.xyz / FragPosLightSpace.w;
    // transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;
    if(type==0)//x_direction
    {
        for(int i=-range;i<=range;i++)
        {
            float x = texture(shadowMap,projCoords.xy+i*vec2(1,0)*texelSize).r;

            float y = texture(shadowMap,projCoords.xy+i*vec2(1,0)*texelSize).g;
            float d = y-x*x;
            mean+=x;
            variance+=y;
            //variance+=x*x;
        }
        mean/=float(2*range+1);
        variance/=float(2*range+1);
    }
    else//y_direction
    {
        for(int i=-range;i<=range;i++)
        {
            float x = texture(shadowMap,projCoords.xy+i*vec2(0,1)*texelSize).r;
            mean+=x;
            variance+=texture(shadowMap,projCoords.xy+i*vec2(0,1)*texelSize).g;
        }
        mean/=float(2*range+1);
        variance/=float(2*range+1);
    }
    FragColor = vec4(mean,variance,0,0);
    //FragColor = vec4(0.5,0.25,0,0);
}