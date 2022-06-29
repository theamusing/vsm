#version 330 core
out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
    vec4 FragPosLightSpace;
} fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

uniform float c_power;
float ShadowCalculation(vec4 fragPosLightSpace)
{

    // perform perspective divide
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    // transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;
    // get depth of current fragment from light's perspective
    float currentDepth = projCoords.z;
    // calculate bias (based on depth map resolution and slope)
    vec3 normal = normalize(fs_in.Normal);
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    currentDepth-=bias;
    //float currentDepth_second=exp(c_power*currentDepth);
    //currentDepth_second=currentDepth;
    currentDepth=exp(-c_power*currentDepth);

    //EVSM  
    float mean = texture(shadowMap, projCoords.xy).r;
    float variance = texture(shadowMap, projCoords.xy).g;
    float mean_second=texture(shadowMap, projCoords.xy).b;
    float variance_second=texture(shadowMap, projCoords.xy).a;
    variance-=mean*mean;
    variance_second-=mean_second*mean_second;
    float shadow=0.0;
    return mean;
    if(mean*currentDepth<1)
        shadow=1.0f;
    //float shadow1 = 1-variance/(variance+(currentDepth-mean)*(currentDepth -mean));
    //float shadow2=1-variance_second/(variance_second+(currentDepth_second-mean_second)*(currentDepth_second -mean_second));
    
    
    /*
    //reflect to [Minshadow,1]
    const float Minshadow=0.1;
    if (shadow<Minshadow)
        return 0;
    shadow=Minshadow+(1-Minshadow)*shadow;
    */

    //PCF
    /*
    //float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth  ? 1.0 : 0.0;        
        }    
    }

    shadow /= 9.0;
    */

    // keep the shadow at 0.0 when outside the far_plane region of the light's frustum.
    if(projCoords.z > 1.0)
        shadow = 0.0;
        
    return shadow;
}

void main()
{           
    vec3 color = texture(diffuseTexture, fs_in.TexCoords).rgb;
    vec3 normal = normalize(fs_in.Normal);
    vec3 lightColor = vec3(1);
    // ambient
    vec3 ambient = 0.3 * lightColor;
    // diffuse
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 diffuse = diff * lightColor;
    // specular
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = 0.0;
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    spec = pow(max(dot(normal, halfwayDir), 0.0), 64.0);
    vec3 specular = spec * lightColor;    
    // calculate shadow
    float shadow = ShadowCalculation(fs_in.FragPosLightSpace);                      
    vec3 lighting = (ambient + (1.0 - shadow) * (diffuse + specular)) * color;    
    
    FragColor = vec4(lighting, 1.0);
    //FragColor = vec4(shadow);
}