shader_type canvas_item;

uniform float y_scale = 0.5;
uniform sampler2D noise_tex;
uniform float speed = 0.25;
uniform float strength = 0.0125;
uniform vec4 tint_color: source_color = vec4(0.192, 0.235, 0.443, 1.0);

uniform float blur_radius = 0.5;
const float MATRIX = 2.3999632;
const int ITERATIONS = 200;

vec3 blur(sampler2D tex, vec2 uv, float radius)
{
    mat2 rot = mat2(vec2(cos(MATRIX), sin(MATRIX)), vec2(-sin(MATRIX), cos(MATRIX)));
    vec3 acc = vec3(0), div = acc;
    float r = 1.;
    vec2 vangle = vec2(0.0,radius*.01 / sqrt(float(ITERATIONS)));
    
    for (int j = 0; j < ITERATIONS; j++)
    {  
        r += 1. / r;
        vangle = rot * vangle;
        vec3 col = texture(tex, uv + (r-1.) * vangle).xyz; 
        col = col * col *1.8;
        vec3 bokeh = pow(col, vec3(4));
        acc += col * bokeh;
        div += bokeh;
    }
    return acc / div;
}

void fragment() {
    vec2 uv = vec2(UV.x, 1. - UV.y * (1. / y_scale));
    vec4 base_color = texture(TEXTURE, uv); // 获取当前像素的颜色

    if (base_color.a > 0.0) { // 如果当前像素不透明
        float noise = texture(noise_tex, fract(uv + TIME * speed)).r;
        vec4 color = vec4(blur(TEXTURE, uv - noise * strength + strength / 2., blur_radius), base_color.a);
        COLOR = color * tint_color;
    } else { // 如果当前像素透明，保持透明
        discard; // 直接丢弃该像素，保持透明
    }
}