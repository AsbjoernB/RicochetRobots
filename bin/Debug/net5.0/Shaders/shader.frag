#version 330 core
in vec2 fUv;

//A uniform of the type sampler2D will have the storage value of our texture.
uniform sampler2D uTexture0;
uniform float uHue;

out vec4 FragColor;


vec4 hueShift( vec4 color, float hueAdjust )
{

    const vec3  kRGBToYPrime = vec3 (0.299, 0.587, 0.114);
    const vec3  kRGBToI      = vec3 (0.596, -0.275, -0.321);
    const vec3  kRGBToQ      = vec3 (0.212, -0.523, 0.311);

    const vec3  kYIQToR      = vec3 (1.0, 0.956, 0.621);
    const vec3  kYIQToG      = vec3 (1.0, -0.272, -0.647);
    const vec3  kYIQToB      = vec3 (1.0, -1.107, 1.704);

    float   YPrime  = dot (vec3(color), kRGBToYPrime);
    float   I       = dot (vec3(color), kRGBToI);
    float   Q       = dot (vec3(color), kRGBToQ);
    float   hue     = atan (Q, I);
    float   chroma  = sqrt (I * I + Q * Q);

    hue += hueAdjust;

    Q = chroma * sin (hue);
    I = chroma * cos (hue);

    vec3 yIQ = vec3 (YPrime, I, Q);

    return vec4( dot (yIQ, kYIQToR), dot (yIQ, kYIQToG), dot (yIQ, kYIQToB), color.a );

}

void main()
{
    //Here we sample the texture based on the Uv coordinates of the fragment
    FragColor = hueShift(texture(uTexture0, fUv),uHue);
}