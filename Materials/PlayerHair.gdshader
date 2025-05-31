shader_type canvas_item;

uniform vec3 line_colour: source_color = vec3(0.0); // 描边颜色为黑色
uniform int line_thickness: hint_range(1, 10) = 1; // 描边大小，最小为1
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

// 替换白色为特定红色
uniform vec3 from1 : source_color = vec3(1.0, 1.0, 1.0); // 白色
uniform vec3 to1 : source_color = vec3(0.6745, 0.1961, 0.1961); // 特定红色 (172, 50, 50)
uniform float threshold1 : hint_range(0.0, 1.0, 0.01) = 0.1; // 白色替换的阈值

// 替换灰色为另一种红色
uniform vec3 from2 : source_color = vec3(0.5255, 0.5255, 0.5255); // 灰色 (134, 134, 134)
uniform vec3 to2 : source_color = vec3(0.3529, 0.102, 0.102); // 另一种红色 (90, 26, 26)
uniform float threshold2 : hint_range(0.0, 1.0, 0.01) = 0.1; // 灰色替换的阈值

void fragment() {
    vec2 uv = SCREEN_UV;
    vec2 pixel_size = SCREEN_PIXEL_SIZE;

    // 获取当前像素的颜色
    vec4 current_pixel = texture(screen_texture, uv);

    // 如果当前像素完全透明，则检查周围像素
    if (current_pixel.a == 0.0) {
        bool should_outline = false;

        // 遍历上下左右的像素，范围由 line_thickness 决定
        for (int i = 1; i <= line_thickness; i++) {
            // 上
            vec2 up_uv = uv + vec2(0.0, pixel_size.y * float(i));
            vec4 up_pixel = texture(screen_texture, up_uv);

            // 下
            vec2 down_uv = uv - vec2(0.0, pixel_size.y * float(i));
            vec4 down_pixel = texture(screen_texture, down_uv);

            // 左
            vec2 left_uv = uv - vec2(pixel_size.x * float(i), 0.0);
            vec4 left_pixel = texture(screen_texture, left_uv);

            // 右
            vec2 right_uv = uv + vec2(pixel_size.x * float(i), 0.0);
            vec4 right_pixel = texture(screen_texture, right_uv);

            // 如果上下左右的像素中有不透明的像素，则标记描黑
            if (up_pixel.a > 0.0 || down_pixel.a > 0.0 || left_pixel.a > 0.0 || right_pixel.a > 0.0) {
                should_outline = true;
                break; // 找到不透明像素后退出循环
            }
        }

        // 如果需要描黑，则设置描边颜色
        if (should_outline) {
            COLOR = vec4(line_colour, 1.0);
        } else {
            COLOR = current_pixel; // 保持当前像素的颜色
        }
    } else {
        // 如果当前像素不透明，先进行颜色替换
        vec3 d1 = abs(current_pixel.rgb - from1); // 计算与白色的距离
        float m1 = (d1.r + d1.g + d1.b) / 3.0; // 平均距离

        vec3 d2 = abs(current_pixel.rgb - from2); // 计算与灰色的距离
        float m2 = (d2.r + d2.g + d2.b) / 3.0; // 平均距离

        // 如果当前像素接近白色，则替换为特定红色
        if (m1 < threshold1) {
            COLOR.rgb = to1;
        }
        // 如果当前像素接近灰色，则替换为另一种红色
        else if (m2 < threshold2) {
            COLOR.rgb = to2;
        }
        // 如果当前像素既不接近白色也不接近灰色，直接输出当前像素的颜色
        else {
            COLOR = current_pixel;
        }
    }
}