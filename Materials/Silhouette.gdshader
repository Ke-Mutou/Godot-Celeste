shader_type canvas_item;

instance uniform vec4 color: source_color = vec4(0.0, 0.0, 1.0, 1.0); // 默认为蓝色，带透明度
instance uniform float opacity: hint_range(0.0, 1.0) = 1.0; // 透明度变量，默认为1.0（完全不透明）
uniform sampler2D screen_texture: hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
    vec2 uv = SCREEN_UV;

    // 获取当前像素的颜色
    vec4 current_pixel = texture(screen_texture, uv);

    // 如果当前像素不透明，将颜色替换为指定颜色，并应用透明度
    if (current_pixel.a > 0.0) {
        COLOR = vec4(color.rgb, color.a * opacity); // 应用透明度
    } else {
        COLOR = current_pixel; // 保持当前像素的颜色
    }
}