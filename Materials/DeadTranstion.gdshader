shader_type canvas_item;

uniform vec2 window_size = vec2(320.0, 180.0); // 窗口大小
uniform float grid_size: hint_range(1.0, 100.0, 1.0) = 10.0; // 每个格子的固定大小（正方形）
uniform int step: hint_range(1, 5, 1) = 1; // 控制每行透明度截止值的变化步长（从无到有，必须是整数）
uniform float offset: hint_range(-1.0, 1.0, 0.01) = 0.0; // 控制起始位置的偏移量（-1.0 表示窗口外左侧，1.0 表示窗口外右侧）

void fragment() {
    vec4 col = vec4(0.0, 0.0, 0.0, 1.0); // 基础颜色

    // 计算当前像素在窗口中的坐标
    vec2 pixel_pos = UV * window_size;

    // 计算当前行的索引（从上到下）
    int row_index = int(floor(pixel_pos.y / grid_size));

    // 计算当前行的透明度截止值（从上到下逐行增加可见区域）
    float alpha_cutoff = window_size.x - grid_size * float(row_index) * float(step);

    // 根据偏移量调整透明度截止值
    float offset_x = offset * window_size.x; // 将偏移量从 [-1, 1] 转换为像素值
    alpha_cutoff += offset_x; // 增加偏移量

    // 如果当前像素的 X 坐标大于当前行的透明度截止值，则设置为完全透明
    if (pixel_pos.x > alpha_cutoff) {
        col.a = 0.0;
    }

    COLOR = col;
}