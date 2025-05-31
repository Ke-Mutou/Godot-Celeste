shader_type fog;

uniform float falloff = 0.0;
uniform float radius = 1.0;
uniform float flatness = 1.0;

uniform vec2 uv_dir = vec2(0,0);

uniform sampler2D primary_noise_tex : repeat_enable;
uniform float primary_noise_scale = 1.0;

uniform sampler2D seconary_noise_tex : repeat_enable;
uniform float secondary_noise_scale = 1.0;

float beersPowder(float l) {
	float beersValue = pow(E, -l);
	float powderValue = 1.0 - pow(E, (-l * 2.0));

	return max(20.0, (log(powderValue/beersValue) / log(10.0)));

	//return (powderValue / beersValue);
}

void fog() {
	vec2 move_uv = uv_dir * TIME;

	float primary_noise = texture(primary_noise_tex, WORLD_POSITION.xz*primary_noise_scale + move_uv).r;
	float second_noise = texture(seconary_noise_tex, WORLD_POSITION.xz*secondary_noise_scale + (move_uv*vec2(.2,.2))).r;

	float noise = primary_noise*second_noise;

	float sdf = - SDF;
	float l = length(WORLD_POSITION - OBJECT_POSITION);

	DENSITY = step(-radius, l);
	DENSITY *= mix(1.0, noise, UVW.y*flatness);

	DENSITY *= smoothstep(0.0, falloff, sdf);

	float holding = beersPowder(l);

	ALBEDO = vec3(holding,holding,holding);



	// Called once for every froxel that is touched by an axis-aligned bounding box
	// of the associated FogVolume. This means that froxels that just barely touch
	// a given FogVolume will still be used.
}
