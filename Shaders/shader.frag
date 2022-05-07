#version 330 core
in vec2 fUv;

//A uniform of the type sampler2D will have the storage value of our texture.
uniform sampler2D uTexture0;
//uniform float uHue;
uniform float uAlpha;
uniform vec4 uTint;

out vec4 FragColor;

vec4 HueShift (in vec4 c, in float Shift)
{
    vec3 Color = vec3(c);

    vec3 P = vec3(0.55735)*dot(vec3(0.55735),Color);
    
    vec3 U = Color-P;
    
    vec3 V = cross(vec3(0.55735),U);    

    Color = U*cos(Shift*6.2832) + V*sin(Shift*6.2832) + P;
    
    return vec4(Color,c.a);
}

vec4 PureTint(in vec4 c, in vec4 tint)
{
    
    //vec3 Color = vec3(c);
    //
    //if (Color == vec3(1))
    //{
    //    c = c * tint;
    //    return c;
    //}
    //return c;
    c = c * tint;
    return (c * tint);
}

vec4 biLinearTex2D(in vec2 fragCoord)
{
	vec2 uv = fUv;	
	vec2 res = vec2(200.0);
    vec2 st = uv * res - 0.5;

    vec2 iuv = floor( st );
    vec2 fuv = fract( st );

    vec4 a = texture( uTexture0, (iuv+vec2(0.5,0.5))/res );
    vec4 b = texture( uTexture0, (iuv+vec2(1.5,0.5))/res );
    vec4 c = texture( uTexture0, (iuv+vec2(0.5,1.5))/res );
    vec4 d = texture( uTexture0, (iuv+vec2(1.5,1.5))/res );

    return mix(
        mix( a, b, fuv.x),
        mix( c, d, fuv.x), fuv.y
    );
    
}

void main()
{
    //vec4 col = PureTint(texture(uTexture0, fUv), uTint);
    vec4 col = PureTint(biLinearTex2D(fUv), uTint);
    FragColor = vec4(col.r, col.g, col.b, col.a * uAlpha);
        
}