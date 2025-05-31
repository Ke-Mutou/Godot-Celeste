shader_type canvas_item;

// 顶点着色器的参数
uniform float Strength = 1.; // 振动强度
uniform float Speed = 2.; // 振动速度
uniform float Smoothness = 0.5; // 平滑度

// 片段着色器的参数
uniform int width:hint_range(0, 5, 1);
uniform vec4 outline_color:source_color;
uniform float flickering_speed:hint_range(0.0, 50.0, 1.);
uniform float minimal_flickering_alpha:hint_range(0.0, 0.9, 0.1);

void vertex() {
    // 计算弹簧振动的偏移量
    float offset = sin(TIME * Speed) * Strength; // 使用正弦函数模拟弹簧振动
    float smoothFactor = smoothstep(1. - Smoothness, 1., 1. - VERTEX.y); // 平滑过渡因子
    VERTEX.y += offset * smoothFactor; // 只在 Y 轴方向上应用偏移量
}

void fragment() {
    float pixel_size_x = 1.0 / float(textureSize(TEXTURE, 0).x);
    float pixel_size_y = 1.0 / float(textureSize(TEXTURE, 0).y);
    vec2 pixel_size = vec2(pixel_size_x, pixel_size_y);

    // 检查当前像素是否为透明
    if (texture(TEXTURE, UV).a == 0.) {
        // 检查周围像素是否有非透明像素
        for (int x = -width; x <= width; x++) {
            for (int y = -width; y <= width; y++) {
                vec2 pixel_offset = vec2(float(x), float(y));
                if (texture(TEXTURE, UV + (pixel_offset * pixel_size)).a > 0.) {
                    // 应用轮廓颜色并添加闪烁效果
                    vec4 resulting_color = outline_color;
                    float alpha_norm = (sin(TIME * flickering_speed) + 1.2) * 0.5;
                    alpha_norm = (1. - minimal_flickering_alpha) * alpha_norm + minimal_flickering_alpha;
                    resulting_color.a = alpha_norm;
                    COLOR = resulting_color;
                    break; // 确保只设置一次颜色
                }
            }
        }
    } else {
        // 如果当前像素不透明，直接使用原始纹理颜色
        COLOR = texture(TEXTURE, UV);
    }
}