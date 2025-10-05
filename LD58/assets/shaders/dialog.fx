import ChaosGraphics.Sprite;

void fs_box(out vec4 outColor : COLOR0)
{
	outColor = vec4(0.0, 0.0, 0.0, 1.0);
}

Pass Dialog {
	Enable(Blend, false);
	Enable(CullFace, false);
	Enable(DepthTest, false);
	DepthMask(false);
	VertexShader = vs_createSprite;
	FragmentShader = fs_box;
}
