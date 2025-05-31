shader_type canvas_item;

// 替换白色为特定红色
instance uniform vec3 from1 : source_color = vec3(1.0, 1.0, 1.0); // 白色
instance uniform vec3 to1 : source_color = vec3(0.6745, 0.1961, 0.1961); // 特定红色 (172, 50, 50)
instance uniform float threshold1 : hint_range(0.0, 1.0, 0.01) = 0.1; // 白色替换的阈值

// 替换灰色为另一种红色
uniform vec3 from2 : source_color = vec3(0.5255, 0.5255, 0.5255); // 灰色 (134, 134, 134)
uniform vec3 to2 : source_color = vec3(0.3529, 0.102, 0.102); // 另一种红色 (90, 26, 26)
uniform float threshold2 : hint_range(0.0, 1.0, 0.01) = 0.1; // 灰色替换的阈值

void fragment() {
    vec3 d1 = abs(COLOR.rgb - from1); // 计算与白色的距离
    float m1 = (d1.r + d1.g + d1.b) / 3.0; // 平均距离

    vec3 d2 = abs(COLOR.rgb - from2); // 计算与灰色的距离
    float m2 = (d2.r + d2.g + d2.b) / 3.0; // 平均距离

    // 如果当前像素接近白色，则替换为特定红色
    if (m1 < threshold1) {
        COLOR.rgb = to1;
    }
    // 如果当前像素接近灰色，则替换为另一种红色
    else if (m2 < threshold2) {
        COLOR.rgb = to2;
    }
}