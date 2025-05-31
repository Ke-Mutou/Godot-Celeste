shader_type canvas_item;

uniform float radius = 10.0; // 透明区域的半径（以像素为单位）
uniform vec2 center_pos = vec2(20, 90); // 透明区域的中心点，默认为屏幕中心
uniform vec2 window_size = vec2(320.0, 180.0); // 纹理的尺寸（以像素为单位）

void fragment() {
    vec2 uv = UV; // 当前像素的UV坐标
    vec2 pixel_pos = uv * window_size; // 当前像素的像素位置
    float distance = length(pixel_pos - center_pos); // 计算当前像素点到中心点的距离（以像素为单位）
    float alpha = step(radius, distance); // 如果距离大于半径，则透明
    vec4 base_color = vec4(0.0, 0.0, 0.0, 1.0); // 设置非透明区域的颜色为黑色
    COLOR = vec4(base_color.rgb, base_color.a * alpha); // 设置透明度
}