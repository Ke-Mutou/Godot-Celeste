shader_type canvas_item;

uniform vec2 circle_center = vec2(16.0, 16.0); // 圆心位置，默认为(16, 16)，因为ColorRect是32x32
instance uniform float outer_radius = 1.5; // 大椭圆的半径，单位为像素，默认值调整为15
instance uniform float radius_difference = 0.2; // 大椭圆和小椭圆的半径差，默认为2像素
uniform vec2 ellipse_ratio = vec2(1.0, 0.5); // 椭圆的水平和垂直缩放比例，默认为水平1:1，垂直1:0.5
instance uniform float opacity = 0.6;
uniform vec3 color: source_color = vec3(1.0, 1.0, 1.0);

void fragment() {
    vec2 uv = UV * vec2(32.0, 32.0); // 将UV坐标映射到像素坐标，因为ColorRect是32x32
    vec2 offset_uv = uv - circle_center; // 计算相对于圆心的偏移

    // 应用椭圆的缩放比例
    offset_uv.x *= ellipse_ratio.x;
    offset_uv.y *= ellipse_ratio.y;

    float dist = length(offset_uv); // 计算当前点到圆心的距离

    float inner_radius = outer_radius - radius_difference; // 小椭圆的半径

    // 判断当前点是否在大椭圆和小椭圆之间的环形区域内
    if (dist > outer_radius || dist < inner_radius) {
        discard; // 如果不在环形区域内，丢弃该像素，使其透明
    }

    // 如果在环形区域内，设置为白色
    COLOR = vec4(color, opacity); // 白色
}