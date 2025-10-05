import ChaosGraphics.TransparentScreenSampling;
import ChaosGraphics.Instancing;

void fs_Glass(
	out vec4 color : COLOR0
) {
	color = vec4(0.5);
}

Pass Glass {
	VertexShader = vs_transformPosition, vs_transformNormal;
	FragmentShader = fs_Glass;
}