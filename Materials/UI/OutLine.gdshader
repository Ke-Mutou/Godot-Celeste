shader_type canvas_item;

// 描边相关参数
uniform int width: hint_range(0, 5, 1) = 1; // 描边宽度
uniform vec4 outline_color: source_color = vec4(0.0, 0.0, 0.0, 1.0); // 描边颜色
uniform float flickering_speed: hint_range(0.0, 50.0, 1.) = 10.0; // 描边闪烁速度
uniform float minimal_flickering_alpha: hint_range(0.0, 0.9, 0.1) = 0.2; // 描边最小透明度

// 弹簧效果相关参数
uniform float Strength = 1.0; // 振动强度
uniform float Speed = 2.0; // 振动速度
uniform float Smoothness = 0.5; // 平滑度

void vertex() {
    // 弹簧振动效果
    float offset = sin(TIME * Speed) * Strength; // 使用正弦函数模拟弹簧振动
    float smoothFactor = smoothstep(1. - Smoothness, 1., 1. - VERTEX.y); // 平滑过渡因子
    VERTEX.y += offset * smoothFactor; // 只在 Y 轴方向上应用偏移量
}

void fragment() {
    // 获取当前像素的纹理颜色
    vec4 color = texture(TEXTURE, UV);

    // 如果当前像素是透明的，检查周围像素是否为非透明，以实现描边效果
    if (abs(color.a - 0.) < 0.001) {
        float pixel_size_x = 1.0 / float(textureSize(TEXTURE, 0).x);
        float pixel_size_y = 1.0 / float(textureSize(TEXTURE, 0).y);
        vec2 pixel_size = vec2(pixel_size_x, pixel_size_y);

        for (int x = -width; x <= width; x++) {
            for (int y = -width; y <= width; y++) {
                vec2 pixel_offset = vec2(float(x), float(y));
                if (texture(TEXTURE, UV + (pixel_offset * pixel_size)).a > 0.) {
                    vec4 resulting_color = outline_color;
                    float alpha_norm = (sin(TIME * flickering_speed) + 1.2) * 0.5;
                    alpha_norm = (1. - minimal_flickering_alpha) * alpha_norm + minimal_flickering_alpha;
                    resulting_color.a = alpha_norm;
                    COLOR = resulting_color;
                    discard; // 一旦找到非透明像素，直接返回描边颜色
                }
            }
        }
    }

    // 如果当前像素不是透明的，或者周围没有非透明像素，则保持原颜色
    COLOR = color;
}