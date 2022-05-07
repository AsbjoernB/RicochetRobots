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

void main()
{
    //vec4 col = HueShift(texture(uTexture0, fUv),uHue);
    //FragColor = vec4(col.r, col.g, col.b, col.a * uAlpha);
    vec4 col = PureTint(texture(uTexture0, fUv), uTint);
    FragColor = vec4(col.r, col.g, col.b, col.a * uAlpha);
        
}