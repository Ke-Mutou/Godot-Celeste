shader_type canvas_item;

uniform float hight = 1.0; //设置高度 1=16px，用于确认草的根部
uniform float wind = 0.02; //摇晃程度
uniform float speed = 3.0; //摇晃速度

void fragment() {
    // 获取当前像素的UV坐标
    vec2 uv = UV;
    // 将草的倾斜效果应用于纹理坐标
    uv.x += sin((uv.y + TIME) * speed) * wind * (hight*0.1 - uv.y);
    // 从纹理中获取像素颜色
    vec4 color = texture(TEXTURE, uv);
    // 输出像素颜色
    COLOR = color;
}