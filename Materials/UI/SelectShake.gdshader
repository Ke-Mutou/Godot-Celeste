shader_type canvas_item;

uniform float amplitude = 0.2; // 飘动的最大幅度
uniform float frequency = 2.0; // 飘动的频率
uniform float speed = 1.0; // 飘动的速度

void fragment() {
    vec2 uv = UV;
    vec4 color = texture(TEXTURE, uv);

    // 计算当前像素距离顶部的比例（从顶部到底部逐渐增大）
    float distance_from_top = uv.y;

    // 只有底部三分之一的部分会飘动
    if (distance_from_top > 2.0 / 3.0) {
        // 计算飘动的偏移量，偏移量从底部到顶部逐渐减小
        float offset = amplitude * (distance_from_top - 2.0 / 3.0) * sin(TIME * speed + uv.y * frequency);

        // 应用偏移量，仅在X轴方向上偏移
        uv.x += offset;
    }

    // 采样纹理并输出颜色
    COLOR = texture(TEXTURE, uv);
}