shader_type canvas_item;

const vec2 OFFSETS[8] = {
	vec2(-1, -1), vec2(-1, 0), vec2(-1, 1), vec2(0, -1), vec2(0, 1), 
	vec2(1, -1), vec2(1, 0), vec2(1, 1)
};

uniform float hit_effect : hint_range(0,1) = 0.0;
uniform float flash_speed = 30.0;
uniform vec4 flash_color : source_color = vec4(1.0, 0.0, 0.0, 1.0);

uniform float line_thickness = 2.0;
uniform vec4 line_color : source_color = vec4(1.0);
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
	vec4 original_color = texture(TEXTURE, UV);
	vec2 size = line_thickness * SCREEN_PIXEL_SIZE;
	float outline = 0.0;
	for (int i = 0; i < OFFSETS.length(); i++) {
		vec2 coordinate_offset = SCREEN_UV + size * OFFSETS[i];
		outline += texture(screen_texture, coordinate_offset).a;
	}
	outline = sign(outline);
	vec4 c = texture(screen_texture, SCREEN_UV);
	if (c.a > 0.0001) {
		c.rgb /= c.a;
	}
	vec4 final_color = mix(c, line_color, outline - c.a);
	
	if (hit_effect > 0.0) {
		float flash = sin(TIME * flash_speed) * 0.5 + 0.5;
		flash *= final_color.a; 
		vec4 flash_final_color = mix(final_color, flash_color, flash);
		final_color = mix(final_color, flash_final_color, hit_effect);
		final_color.a = final_color.a; 
	}
	COLOR = final_color;
}