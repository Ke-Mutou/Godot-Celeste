shader_type canvas_item;

instance uniform float force: hint_range(0.0, 1, 0.001) = 0.5;
instance uniform float edge : hint_range(0.0, 0.5, 0.001) = 0.15;
instance uniform float thickness : hint_range(0.0, 0.5, 0.001) = 0.2;
instance uniform float size : hint_range(0.0, 1, 0.001) = 0.5;

uniform sampler2D screen_texture: hint_screen_texture, filter_nearest;

varying vec2 center_uv;

void vertex(){
    center_uv = (SCREEN_MATRIX * CANVAS_MATRIX * MODEL_MATRIX * vec4(0.5, 0.5, 0, 1)).xy;
    center_uv = (center_uv + 1.0) / 2.0;
}

void fragment() {
    vec2 difference_uv = SCREEN_UV - center_uv;
    vec2 object_center_uv = UV - vec2(.5, .5);
    float mask = smoothstep(size - thickness, size - thickness + edge, length(object_center_uv)) *
                 (1.0 - smoothstep(size - edge, size, length(object_center_uv))) * force;
    mask = 1.0 - mask;
    vec2 result = difference_uv * mask + center_uv;
    vec4 sampler = texture(screen_texture, result);
    COLOR = sampler;
}