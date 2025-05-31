shader_type canvas_item;

uniform float brightness : hint_range(-1.0, 1.0) = 0.0; // 亮度调节参数

void fragment() {
    // 获取纹理颜色
    vec4 tex_color = texture(TEXTURE, UV);

    // 调整亮度
    vec3 adjusted_color = tex_color.rgb + vec3(brightness);

    // 确保颜色值在合法范围内
    adjusted_color = clamp(adjusted_color, 0.0, 1.0);

    // 输出最终颜色
    COLOR = vec4(adjusted_color, tex_color.a);
}