shader_type canvas_item;

instance uniform float amplitude = 5.0; // 抖动的幅度（像素单位）
instance uniform float frequency = 2.0; // 抖动的频率
instance uniform float randomness = 1.0; // 随机性强度

void vertex() {
    // 获取当前时间
    float time = TIME;

    // 生成随机抖动的偏移量
    vec2 offset = vec2(
        sin(time * frequency + randomness * sin(time * 0.5)) * amplitude, // X轴方向的抖动
        cos(time * frequency + randomness * cos(time * 0.5)) * amplitude  // Y轴方向的抖动
    );

    // 将偏移量应用到顶点位置
    VERTEX += offset;
}