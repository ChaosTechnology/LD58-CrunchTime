import ChaosGraphics.Sprite;

vec2 dimensions;

void fs_box(
	vec2 inTex : TEXCOORD0,
	out vec4 outColor : COLOR0
) {
	inTex = (inTex - vec2(0.5)) * 2;
	vec2 delta = vec2(1.0) - abs(inTex);
	delta *= dimensions;
	
	float value = all(greaterThan(delta, vec2(0.1))) && any(lessThan(delta, vec2(0.2))) ? 1.0 : 0.0;
	outColor = vec4(value, value, value, 1.0);
}

Pass Dialog {
	Enable(Blend, false);
	Enable(CullFace, false);
	Enable(DepthTest, false);
	DepthMask(false);
	VertexShader = vs_createSprite;
	FragmentShader = fs_box;
}
